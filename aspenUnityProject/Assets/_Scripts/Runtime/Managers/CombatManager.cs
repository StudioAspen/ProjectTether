using System;
using System.Collections.Generic;
using _Scripts.Runtime.Combat;
using _Scripts.Runtime.Combat.Mechanics;
using _Scripts.Runtime.Combat.States.CombatManagerStates;
using _Scripts.Runtime.Combat.States.ContextData;
using _Scripts.Runtime.Entities.Scripts.Combat;
using _Scripts.Runtime.Math;
using _Scripts.Runtime.Persistent_Data;
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
    //TODO: break this down 
    public class CombatManager : MonoBehaviour, ICombatFunctionProvider
    {
        private const float ArbitraryOffset = 10;
        private const int TileNum = 19;
        
        private InputSystem_Actions _input;
        private EncounterSO _currentEncounter;

        private bool toggledManualEnemy;
        
        [SerializeField] private Transform tilesParent;
        private TileController[] TileControllers { get; }= new TileController[TileNum];

        /// <summary>
        /// Vector3Int -> TileControllers index (index is equal to desired count order of tiles)
        /// </summary>
        private Dictionary<Vector3Int, int> TileCubeCoords { get; }= new Dictionary<Vector3Int, int>();
        
        //TODO: update turn order to match the initiative proposal in the doc  - we will have to create a new class 
        private TurnQueue TurnOrder { get; } = new TurnQueue();
        private bool _currentlyInTurn;
        private List<UnitController> DeadUnits {get; }= new List<UnitController>();
        private int _deadEnemies;
        private int _deadAllies;
        private List<UnitController> _deadUnitsThisTurn = new List<UnitController>();
        
        private readonly BattlePhase[] _phases = new BattlePhase[2];
        private BattlePhase _currentPhase;  
        [SerializeField] private RangeDisplay rangeDisplay; 
        
        #region miscStateManagementVariables
        
        //need totals for defeat/win checks
        private int _totalAllies { get; set; }
        private int _totalEnemies { get; set; }
        
        //for easier targeting calculation
        private List<AllyUnitController> PlayerUnits { get; } = new List<AllyUnitController>();
        private UnitController CurrentUnit { get; set; }
        private Vector3Int SelectedTile { get; set; }
        private Vector3Int _currentTile { get; set; } = new Vector3Int(0, 0, 0);
        private CombatActions _receivedAction { get; set; }
        private int ActionSelection { get; set; }

        //no clue how to connect the Overclock status differently 
        /// <summary>
        /// statuses associated with player actions (e.g., overclocking, attacking, defending too much, etc.)
        /// </summary>
        [SerializeField] private List<StatusSO> _actionStatuses = new List<StatusSO>();

        private Dictionary<string, StatusSO> _actionStatusDic = new Dictionary<string, StatusSO>();
        
        #endregion

        //TODO: sub to each unit themselves 
        //TODO: figure out what to do when a unit dies 
        public static event Action<List<UnitController>> unitsDead;
        //animation/tile update handled per unit at the instant they move. 
        public static event Action<UnitController> unitMoved;
        //we may want sounds when the cursor moves around 
        public static event Action<Vector3> hoverTileChanged;  
        public static event Action<BattlePhase, UnitController> battlePhaseChanged;
        public static event Action<TileController> examinedTile;
        public static event Action<UnitController> examinedUnit;
        public static event Action exitedExamine;
        
        /// <summary>
        /// true = win, false = lose
        /// </summary>
        public static event Action<bool> finishedBattle; 
        
        private void Awake()
        {
            _input = InputManager.Instance.Actions;
            _currentEncounter = PersistentData.CurrentEncounterData;
        }

        //must occur after OnEnable bc other classes will subscribe OnEnable
        private void Start()
        {
            SortTiles(tilesParent.GetComponentsInChildren<TileController>());
            GenerateCoords();
            CreateObjects(); 
            
            rangeDisplay.Initialize(TileControllers);
            
            _phases[0] = new PlayerPhase(new CombatContext(TileControllers, TileCubeCoords, _input, PlayerUnits, this));
            _phases[1] = new EnemyPhase(new CombatContext(TileControllers, TileCubeCoords, _input, PlayerUnits, this));
            CombatUI.PlayerAction += HandleAction;
            CombatUI.PlayerSelectiveAction += HandleAction;
            CombatUI.ToggledManualEnemy += ToggleManualEnemy;
            foreach (StatusSO status in _actionStatuses)
                _actionStatusDic.Add(status.StatusId, status);
        }

        private void OnDestroy()
        {
            CombatUI.PlayerAction -= HandleAction;
            CombatUI.PlayerSelectiveAction -= HandleAction;
            _input.TileSelect.Disable();
            TurnOrder.Clear();
            PlayerUnits.Clear();
        }

        //TODO: allow player to choose between allies when they have tied initiative?
        private void Update()
        {
            if (TurnOrder.UpdateInitiatives() && !_currentlyInTurn)
                ChangeTurn();
        }

        //Correct order is not guaranteed by GetComponentsInChildren
        private void SortTiles(TileController[] tiles)
        {
            foreach (TileController tileController in tiles)
            {
                TileControllers[tileController.Num()] = tileController;
            } 
        }
        
        private void CreateObjects()
        {
            if (_currentEncounter == null)
            {
                Debug.LogWarning($"CombatManager: Encounter is null, can't create objects");
                return;
            }
            
            for (int tileIndex = 0; tileIndex < _currentEncounter.Tiles.Count; tileIndex++)
            {
                if (_currentEncounter.Tiles[tileIndex].Units.Count == 0) 
                    continue;

                TileController tileController = TileControllers[tileIndex];
                
                foreach (UnitDataSO unitData in _currentEncounter.Tiles[tileIndex].Units)
                {
                    if (unitData.Model == null)
                    {
                        Debug.LogWarning($"UnitData {unitData.name} is missing model prefab");
                        continue;
                    }
                    
                    UnitController unitController = Instantiate(unitData.Model, TileControllers[tileIndex].Position(), Quaternion.Euler(-90f,0,0));
                    if (unitData.Faction == Faction.Ally)
                    {
                        tileController.AddUnit(unitController);
                        PlayerUnits.Add(unitController as AllyUnitController);
                        
                        _totalAllies++;
                    }
                    else if (unitData.Faction == Faction.Enemy)
                    {
                        tileController.AddUnit(unitController);
                        
                        _totalEnemies++;
                    }
                    else
                    {
                        Debug.Log($"Neutral units not yet implemented");
                    }
                    
                    unitController.Initialize(unitData);
                    unitController.SetTile(tileController.tileCoordinate);
                    TurnOrder.Add(unitController);
                }
                
                tileController.RepositionUnits(ArbitraryOffset);
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
       
        public void ChangeTurn()
        {
            if (DeadUnits.Count >= (_totalAllies + _totalEnemies))
            {
                Debug.Log("All units dead.");
                _currentPhase = null;
                return;
            }
            if (_currentPhase != null)
            {
                _currentPhase.Exit();
            }
            CurrentUnit = TurnOrder.GetCappedUnits()[0];
            _currentlyInTurn = true;
            if (toggledManualEnemy || CurrentUnit.GetData().Faction == Faction.Ally)
                _currentPhase = _phases[0];
            else if (CurrentUnit.GetData().Faction==Faction.Enemy)
                _currentPhase = _phases[1];
            else
                return;
            CurrentUnit.ResetValues();
            _currentPhase.Enter();
            battlePhaseChanged?.Invoke(_currentPhase, CurrentUnit);
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
        /// Actions that require tile selection lead to SelectTile
        /// </summary>
        /// <param name="action"></param>
        private void HandleAction(CombatActions action)
        {
            _receivedAction = action;
            UnitController currUnit = CurrentUnit;
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
            UnitController currUnit = CurrentUnit;
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
            return TileControllers[TileCubeCoords[CurrentUnit.TileCoords]];
        }
        
        private void ResetCurrentTile()
        {
            SelectedTile = CurrentUnit.TileCoords;
            _currentTile = SelectedTile;
            hoverTileChanged?.Invoke(TileControllers[TileCubeCoords[_currentTile]].Position());
        }
        
        //attack is basic attack with no ability selection. 
        //attacks do not target individual enemies and hit every enemy in a tile (per the gdd)
        public void SelectTile(InputAction.CallbackContext context)
        {
            SelectedTile = _currentTile;
            TileController selectedTileController = TileControllers[TileCubeCoords[SelectedTile]];
            //possible temporary use of CurrentUnit - CurrentUnit might be changed later
            UnitController currentUnit = CurrentUnit;
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
            {
                CombatFormulas.Damage(currentUnit, currentUnit.GetData().DefaultAttackTypes(), enemy);
                CheckDead(enemy);
            }

            RemoveDead();
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
            {
                CombatFormulas.AbilityDamage(currentUnit, enemy, ability);
                CheckDead(enemy);
            }

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
                            foreach (UnitController enemy in TileControllers[TileCubeCoords[SelectedTile]]
                                         .UnitControllers)
                            {
                                CombatFormulas.AbilityDamage(currentUnit, enemy, ability);
                                CheckDead(enemy);
                            }
                        }
                        else
                            break;
                    }
                }
                RemoveDead();
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

        /// <summary>
        /// adds the actual overclock status to the player unit
        /// </summary>
        /// <param name="unit"></param>
        private void Overclock(UnitController unit)
        {
            AllyUnit au = (AllyUnit)unit.GetData();
            au.AddStatus(_actionStatusDic["overclock"]);
            au.Overclock();
            if(au.OverclockLevel == OverclockTiers.Extreme)
                au.AddStatus(_actionStatusDic["burnout_0"]);
            Debug.Log("overclocking");
            RedoSelection();
        }

        public TileController GetTileController(Vector3Int pos)
        {
            return TileControllers[TileCubeCoords[pos]];
        }

        private void FinishSelection()
        {
            Consume();
            ResetCurrentTile();
            battlePhaseChanged?.Invoke(_currentPhase, CurrentUnit);
        }
        
        public void Consume()
        {
            TurnOrder.ConsumeUnit(CurrentUnit);
            _currentlyInTurn = false;           
        }

        public void RedoSelection()
        {
            Debug.Log("redo selection");
            rangeDisplay.HideRange();
            ResetCurrentTile();
            _currentPhase.Exit();
            battlePhaseChanged?.Invoke(_currentPhase, CurrentUnit);
        }

        private void ExamineTile()
        {
           examinedTile?.Invoke(GetTileController(SelectedTile)); 
        }

        public void ExamineUnit(UnitController unitController)
        {
            examinedUnit?.Invoke(unitController);
        }

        /// <summary>
        /// Must be used in every attack function after the damage calculations
        /// </summary>
        /// <param name="unit"></param>
        public void CheckDead(UnitController unit)
        {
            _deadUnitsThisTurn.Clear();
            Unit u = unit.GetData();
            if (u.HealthRemaining < 0)
            {
                u.SetDead(true);
                if (u.Faction == Faction.Ally)
                    _deadAllies++;
                else
                    _deadEnemies++;  //neutral units do not exist, apparently
                DeadUnits.Add(unit);
                _deadUnitsThisTurn.Add(unit);
                Debug.Log(u.Name);
            }
            unitsDead?.Invoke(_deadUnitsThisTurn);
            CheckCombatOver();
        }

        /// <summary>
        /// should be called checkdead() after the for loop 
        /// </summary>
        public void RemoveDead()
        {
         //  foreach(UnitController u in _deadUnitsThisTurn)  
          //     TileControllers[TileCubeCoords[u.TileCoords]].RemoveUnit(u);
        }

        private void CheckCombatOver()
        {
            if (_deadAllies > PlayerUnits.Count)
            {
               finishedBattle?.Invoke(false);
               return;
            }
            if (_deadEnemies >= _totalEnemies)
            {
                foreach (AllyUnitController auc in PlayerUnits)
                {
                    if(!((AllyUnit)auc.GetData()).CheckOverclock())
                       ((AllyUnit)auc.GetData()).Underclock();
                }
                finishedBattle?.Invoke(true);
            }
        }

        public void ExitExamine() { exitedExamine?.Invoke(); }
        public UnitController GetCurrentUnit() { return CurrentUnit; }
        public Vector3Int GetSelectedTile() { return _currentTile; }

        private void ToggleManualEnemy(bool b) { toggledManualEnemy = b; }
    }
}
