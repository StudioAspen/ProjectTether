using System;
using System.Collections.Generic;
using _Scripts.Runtime.Combat;
using _Scripts.Runtime.Combat.States.CombatManagerStates;
using _Scripts.Runtime.Combat.States.ContextData;
using _Scripts.Runtime.Entities.Scripts;
using _Scripts.Runtime.Entities.Scripts.Combat;
using _Scripts.Runtime.Math;
using _Scripts.Runtime.Tile_System.Scripts;
using _Scripts.Runtime.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

//TODO: NEED COROUTINES when we are past the mvp(?)
namespace _Scripts.Runtime.Managers
{
    /*will not be a traditional manager because it does not
    need to be static. EncounterManager will be static and the one
    to send the data over to CombatManager. CombatManager will exist
    in the battle scene only.
    */
    public class CombatManager : MonoBehaviour, ICombatFunctionProvider
    {
        private const float ArbitraryOffset = 10;
        private const int TileNum = 19;
        private UnitDataSO[,] initializerData;

        private bool toggledManualEnemy;
        
        private Encounter encounter; 
        [SerializeField] private Transform tilesParent;
        private InputSystem_Actions Input { get; set; }
        private TileController[] TileControllers { get; }= new TileController[TileNum];

        private Dictionary<Vector3Int, int> TileCubeCoords { get; }= new Dictionary<Vector3Int, int>();
        
        //TODO: update turn order to match the initiative proposal in the doc  - we will have to create a new class 
        private List<UnitController> TurnOrder { get; }= new List<UnitController>();
        private List<UnitController> DeadUnits {get; }= new List<UnitController>();
        private int _deadEnemies;
        
        //TODO: add an enum for this if we ever have more than just enemy/ally turns?
        private readonly BattlePhase[] _phases = new BattlePhase[2];
        private BattlePhase _currentPhase;  
        [SerializeField] private RangeDisplay rangeDisplay; 
        
        #region miscStateManagementVariables
        
        //need totals for defeat/win checks
        private int _totalAllies { get; set; }
        private int _totalEnemies { get; set; }
        
        //for easier targeting calculation
        private List<AllyUnitController> PlayerUnits { get; } = new List<AllyUnitController>();
        private int CurrentUnitTurn { get; set; }
        private Vector3Int SelectedTile { get; set; }
        private Vector3Int _currentTile { get; set; } = new Vector3Int(0, 0, 0);
        private CombatActions _receivedAction { get; set; }
        private int ActionSelection { get; set; }

        /// <summary>
        /// statuses associated with player actions (e.g., overclocking, attacking, defending too much, etc.)
        /// </summary>
        [SerializeField] private List<StatusSO> _actionStatuses;
        
        #endregion

        //TODO: sub to each unit themselves 
        //TODO: figure out what to do when a unit dies 
        public static event Action<Unit[]> unitsDead;
        //animation/tile update handled per unit at the instant they move. 
        public static event Action<UnitController> unitMoved;
        //we may want sounds when the cursor moves around 
        public static event Action<Vector3> hoverTileChanged;  
        public static event Action<BattlePhase, UnitController> battlePhaseChanged;
        public static event Action<TileController> examinedTile;
        public static event Action<UnitController> examinedUnit;
        public static event Action exitedExamine;
        
        public static event Action finishedBattle; 
        

        private void Awake()
        {
            Input = InputManager.Instance.Actions;
        }

        //must occur after OnEnable bc other classes will subscribe OnEnable
        void Start()
        {
            encounter = EncounterManager.Instance.GetEncounter();
            initializerData = EncounterManager.Instance.GetInitializerData();
            SortTiles(tilesParent.GetComponentsInChildren<TileController>());
            GenerateCoords();
            CreateObjects(); 
            TurnOrder.Sort((a,b) => b.GetData().Speed.CompareTo(a.GetData().Speed));
            rangeDisplay.Initialize(TileControllers);
            
            _phases[0] = new PlayerPhase(new CombatContext(TileControllers, TileCubeCoords, Input, PlayerUnits, this));
            _phases[1] = new EnemyPhase(new CombatContext(TileControllers, TileCubeCoords, Input, PlayerUnits, this));
            CombatUI.PlayerAction += HandleAction;
            CombatUI.PlayerSelectiveAction += HandleAction;
            CombatUI.ToggledManualEnemy += ToggleManualEnemy;
            ChangeTurn(); 
        }

        private void OnDisable()
        {
            CombatUI.PlayerAction -= HandleAction;
            CombatUI.PlayerSelectiveAction -= HandleAction;
            Input.TileSelect.Disable();
           TurnOrder.Clear();
            PlayerUnits.Clear();
        }

        /*
        void Update()
        {
           currentPhase?.Update();
        }
        */

        //Correct order is not guaranteed by GetComponentsInChildren
        private void SortTiles(TileController[] tiles)
        {
            foreach (TileController tileController in tiles)
            {
                TileControllers[tileController.Num()] = tileController;
            } 
        }
        
        #region setupRelatedStuff


        void CreateObjects()
        {
            for (int tile = 0; tile < encounter.TotalTiles(); tile++)
            {
                if (encounter.UnitCountAtTile(tile) < 1) 
                    continue; 
                for (int unit = 0; unit < encounter.MaxUnitsPerTile(); unit++)
                {
                    if (unit > encounter.UnitCountAtTile(tile) - 1)
                        break;
                    GameObject newTempObject = Instantiate(encounter[tile,unit], TileControllers[tile].Position(), Quaternion.Euler(-90f,0,0));
                    //set up controllers after object instantiation so objects don't override each other's data
                    //the newly cloned object does not share the same reference as the original prefab, so there is no overriding
                    if (initializerData[tile, unit].Faction == Faction.Ally)
                    {
                        TileControllers[tile].AddUnit(newTempObject.GetComponent<AllyUnitController>());
                        _totalAllies++;
                        PlayerUnits.Add((AllyUnitController)TileControllers[tile].PeekUnit());
                    }
                    else if (initializerData[tile, unit].Faction == Faction.Enemy)
                    {
                        TileControllers[tile].AddUnit(newTempObject.GetComponent<EnemyUnitController>());
                        _totalEnemies++;
                    }
                    else 
                        Debug.Log("neutral units not yet implemented");
//                    Debug.Log($"{tile}: {unit}, {tileControllers[tile].UnitCount()}");
                    TileControllers[tile].GetUnitAt(unit).Initialize(initializerData[tile,unit]);
                    TileControllers[tile].GetUnitAt(unit).SetTile(TileControllers[tile].tileCoordinate);
                    TurnOrder.Add(TileControllers[tile].GetUnitAt(unit));
                    TileControllers[tile].GetUnitAt(unit).SetTile(TileControllers[tile].tileCoordinate);
                    Debug.Log(TileControllers[tile].UnitControllers[unit].GetData().Name);
                }
                TileControllers[tile].RepositionUnits(ArbitraryOffset);
            }
        }

        
        /// <summary>
        ///Starts at tile 0 and spirals outwards to get the cube coords for every tile.
        ///Coords are for determining proper tile selection when the user moves across the field 
        /// </summary>
        //For reference, tile 18 should be (2,0,-2) 
        //3r(r+1)+1=tiles formula for generic implementation if additional rings are added.
        void GenerateCoords()
        {
            int tile = 0;
            Vector3Int currentPos = new Vector3Int(0, 0, 0);
            TileCubeCoords.Add(currentPos, tile);
            TileControllers[tile].tileCoordinate = currentPos;
            for (int ring = 1; ring <= 2; ring++)
            {
                currentPos += CubeCoordDirections.NE.Vector();
                tile++;
                TileCubeCoords.Add(currentPos, tile);
                TileControllers[tile].tileCoordinate = currentPos;
                for (int southEasts = ring - 1; southEasts > 0; southEasts--)
                {
                    currentPos += CubeCoordDirections.SE.Vector();
                    tile++;
                    TileCubeCoords.Add(currentPos, tile);
                    TileControllers[tile].tileCoordinate = currentPos;
                }
                for (int direction = (int)CubeCoordDirections.S; direction < Enum.GetNames(typeof(CubeCoordDirections)).Length; direction++)
                {
                    for (int times = ring; times > 0; times--)
                    {
                        currentPos += ((CubeCoordDirections)direction).Vector();
                        tile++;
                        TileCubeCoords.Add(currentPos, tile);
                        TileControllers[tile].tileCoordinate = currentPos;
                    }
                }
            }
        } 

        //debug tool
        void ValidateData()
        {
            for (int tile = 0; tile < encounter.TotalTiles(); tile++)
            {
                if (encounter.UnitCountAtTile(tile) < 1) 
                    continue; 
                for (int unit = 0; unit < encounter.MaxUnitsPerTile(); unit++)
                {
                    if (unit > encounter.UnitCountAtTile(tile) - 1)
                        break;
                    Debug.Log($"{TileControllers[tile].GetUnitAt(unit).GetData().Name}:  {TileControllers[tile].GetUnitAt(unit).GetData().Speed}");
                    
                }
            }
        }
        #endregion
        
        //dead are kept because lazy deletion. Also, there may or may not be a revive feature, so their order being kept is good.
        //I am also not sure if deletion is better because deletion would require searching and result in the entire list shifting. 
        //TODO: turn system needs to match gcc. Currently only sorts by unit speed and has the fastest unit go first
        public void ChangeTurn()
        {
            if (DeadUnits.Count >= (_totalAllies + _totalEnemies))
            {
                Debug.Log("All units dead.");
                return;
            }
            while (TurnOrder[CurrentUnitTurn].GetData().IsDead)
                CurrentUnitTurn = (CurrentUnitTurn + 1)%TurnOrder.Count;
            if (_currentPhase != null)
            {
                _currentPhase.Exit();
                CurrentUnitTurn = (CurrentUnitTurn + 1)%TurnOrder.Count;
            }
            if (toggledManualEnemy || TurnOrder[CurrentUnitTurn].GetData().Faction == Faction.Ally)
                _currentPhase = _phases[0];
            else if (TurnOrder[CurrentUnitTurn].GetData().Faction==Faction.Enemy)
                _currentPhase = _phases[1];
            else
                return;
            TurnOrder[CurrentUnitTurn].ResetValues();
            _currentPhase.Enter();
            battlePhaseChanged?.Invoke(_currentPhase, TurnOrder[CurrentUnitTurn]);
        } 
        
        //functions for camera/ui movement/whatever 
        public void MoveTileSelector(CubeCoordDirections direction)
        {
            Vector3Int projectedTile = _currentTile + direction.Vector(); 
            if(TileCubeCoords.TryGetValue(projectedTile, out _))
            {
                _currentTile = projectedTile;
                hoverTileChanged?.Invoke(TileControllers[TileCubeCoords[_currentTile]].Position());
            }
        }
        
        /*
        the player's selectTileState (pushed by this function) will tell this manager when to
        execute the SelectTile function.
        I opted for states because the player may undo actions.
        Usually, the first requirement after selecting an action
        is selecting a tile.
        */
        /// <summary>
        /// Handles actions that do not require a separate ui panel. Connected to CombatUI.
        /// </summary>
        /// <param name="action"></param>
        private void HandleAction(CombatActions action)
        {
            _receivedAction = action;
            UnitController currUnit = TurnOrder[CurrentUnitTurn];
            switch(action)
            {
                case CombatActions.Attack:
                    _currentPhase.PushState();
                    rangeDisplay.DisplayAttackRange(TileControllers, currUnit);
                    return;
                case CombatActions.Defend:
                    currUnit.GetData().Defend();
                    FinishSelection();
                    return;
                case CombatActions.Move:
                    _currentPhase.PushState();
                    rangeDisplay.DisplayMoveRange(currUnit.TileCoords, TileControllers, 1, GetCurrentUnit().GetData().Faction); //currently only adjacent tiles
                    return;
                case CombatActions.View:
                    _currentPhase.PushState();
                    return;
                case CombatActions.Overclock:
                    if(currUnit.GetData().Faction == Faction.Ally)
                        Overclock(currUnit);
                    return;
                default:
                    Debug.Log($"Unknown action: {action}");
                    return;
            }
        }

        /// <summary>
        /// Handles player actions that requires a new ui panel 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="selection"></param>
        private void HandleAction(CombatActions action, int selection)
        {
            _receivedAction = action;
            ActionSelection = selection;
            UnitController currUnit = TurnOrder[CurrentUnitTurn];
            switch (action)
            {
                case CombatActions.Ability:
                    _currentPhase.PushState();
                    rangeDisplay.DisplayAbilityRange(TileControllers, currUnit, currUnit.GetData().Moves[selection]);
                    return;
                case CombatActions.Item:
                    Debug.Log($"unknown action: {action}" );
                    return;
                default:
                    Debug.Log($"Unknown action: {action}");
                    break;
            }
//            rangeDisplay.DisplayRange();
        }
        
        public TileController GetCurrTileController()
        {
            return TileControllers[TileCubeCoords[TurnOrder[CurrentUnitTurn].TileCoords]];
        }
        
        private void ResetCurrentTile()
        {
            SelectedTile = TurnOrder[CurrentUnitTurn].TileCoords;
            _currentTile = SelectedTile;
            hoverTileChanged?.Invoke(TileControllers[TileCubeCoords[_currentTile]].Position());
        }
        
        //attack is basic attack with no ability selection. 
        //attacks do not target individual enemies and hit every enemy in a tile (per the gdd)
        public void SelectTile(InputAction.CallbackContext context)
        {
            SelectedTile = _currentTile;
            TileController selectedTileController = TileControllers[TileCubeCoords[SelectedTile]];
            UnitController currentUnit = TurnOrder[CurrentUnitTurn];
            switch (_receivedAction)
            {
                case CombatActions.Attack:
                    if (currentUnit.AttackReachable(selectedTileController)) 
                        HandleSelectAttack(currentUnit);
                    break;
                case CombatActions.Move:
                    if (selectedTileController.IsMoveable(currentUnit.TileCoords,1, currentUnit.GetData().Faction) && !currentUnit.HasMoved)
                        HandleSelectMove(selectedTileController, currentUnit); 
                    return;
                case CombatActions.Ability:
                    if(currentUnit.AbilityReachable(currentUnit.GetData().Moves[ActionSelection],selectedTileController))
                        HandleSelectAbility(currentUnit); 
                    break;
                case CombatActions.Item:
                    Debug.Log("items are not implemented in mvp");
                    break;
                case CombatActions.View:
                    ExamineTile();
                    _currentPhase.PushState();
                    break;
                default:
                    Debug.Log("Unknown action");
                    break;
            }
        }
        
        private void HandleSelectAttack(UnitController currentUnit)
        {
            foreach (UnitController enemy in TileControllers[TileCubeCoords[SelectedTile]].UnitControllers) 
                CombatFormulas.Damage(currentUnit, currentUnit.GetData().DefaultAttackTypes(), enemy);  
            rangeDisplay.HideRange();
            FinishSelection();
        }

        private void HandleSelectAbility(UnitController currentUnit)
        {
            TileController epicenter =  TileControllers[TileCubeCoords[SelectedTile]];
            AbilitySO ability = currentUnit.GetData().Moves[ActionSelection];
            rangeDisplay.HideRange();           
            //epicenter hit 
            foreach (UnitController enemy in TileControllers[TileCubeCoords[SelectedTile]].UnitControllers)
                CombatFormulas.AbilityDamage(currentUnit, enemy, ability );    
            
            //hit the area surrounding the epicenter
            if (ability.AOE > 0)
            {
                foreach (CubeCoordDirections direction in Enum.GetValues(typeof(CubeCoordDirections)))
                {
                    Vector3Int affectedTile = epicenter.tileCoordinate;
                    for (int tilesFromEpicenter = 0; tilesFromEpicenter < ability.AOE; tilesFromEpicenter++)
                    {
                        affectedTile += direction.Vector();
                        if (TileCubeCoords.TryGetValue(affectedTile, out _))
                        {
                            foreach (UnitController enemy in TileControllers[TileCubeCoords[SelectedTile]].UnitControllers)
                                CombatFormulas.AbilityDamage(currentUnit, enemy, ability );    
                        }
                        else
                            break;
                    }
                }
            }
            FinishSelection();
        }
        
        private void HandleSelectMove(TileController selectedTileController, UnitController currentUnit)
        {
            selectedTileController.AddUnit(GetCurrTileController().RemoveUnit(currentUnit));
            currentUnit.TryMove(selectedTileController.Position(),selectedTileController.tileCoordinate);
            selectedTileController.RepositionUnits(ArbitraryOffset); //maybe more efficient to call here than in AddUnit bc of the CreateObjects function 
            RedoSelection();
        }

        private void Overclock(UnitController unit)
        {
            ((AllyUnit)unit.GetData()).Overclock();
            Debug.Log("overclocking");
            RedoSelection();
        }

        public TileController GetTileController(Vector3Int pos)
        {
            return TileControllers[TileCubeCoords[pos]];
        }

        private void FinishSelection()
        { 
            ChangeTurn();
            ResetCurrentTile();
            battlePhaseChanged?.Invoke(_currentPhase, TurnOrder[CurrentUnitTurn]);
        }

        public void RedoSelection()
        {
            Debug.Log("redo selection");
            rangeDisplay.HideRange();
            ResetCurrentTile();
            _currentPhase.Exit();
            battlePhaseChanged?.Invoke(_currentPhase, TurnOrder[CurrentUnitTurn]);
        }

        private void ExamineTile()
        {
           examinedTile?.Invoke(GetTileController(SelectedTile)); 
        }

        public void ExamineUnit(UnitController unitController)
        {
            examinedUnit?.Invoke(unitController);
        }

        public void CheckCombatOver()
        {
            if (_deadEnemies >= _totalEnemies)
            {
                foreach (AllyUnitController auc in PlayerUnits)
                {
                   ((AllyUnit)auc.GetData()).Underclock();
                }
                finishedBattle?.Invoke();
            }
        }

        public void ExitExamine() { exitedExamine?.Invoke(); }
        public UnitController GetCurrentUnit() { return TurnOrder[CurrentUnitTurn]; }
        public Vector3Int GetSelectedTile() { return _currentTile; }

        private void ToggleManualEnemy(bool b) { toggledManualEnemy = b; }
    }
}
