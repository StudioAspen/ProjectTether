using System;
using System.Collections.Generic;
using _Scripts.Consystently.UI;
using _Scripts.Runtime.Combat;
using _Scripts.Runtime.Combat.Abilities;
using _Scripts.Runtime.Combat.Abilities;
using _Scripts.Runtime.Combat.Mechanics;
using _Scripts.Runtime.Combat.States.CombatManagerStates;
using _Scripts.Runtime.Combat.States.ContextData;
using _Scripts.Runtime.Combat.Statuses;
using _Scripts.Runtime.Combat.Statuses;
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
        private InputSystem_Actions _input;
        private EncounterSO _currentEncounter;

        private bool toggledManualEnemy;
        
        [SerializeField] private Transform tilesParent;
        private TileController[] TileControllers { get; }= new TileController[CombatTileCoordinates.TileCount];

        /// <summary>
        /// Vector3Int -> TileControllers index (index is equal to desired count order of tiles)
        /// </summary>
        private Dictionary<Vector3Int, int> TileCubeCoords { get; }= new Dictionary<Vector3Int, int>();
        private TurnQueue TurnOrder;
        
        //no use for now
        private List<UnitController> _deadUnitsThisTurn = new List<UnitController>();
        
        private readonly BattlePhase[] _phases = new BattlePhase[2];
        private BattlePhase _currentPhase;  
        [SerializeField] private RangeDisplay rangeDisplay;

        public BattleUnitsList BattleUnitsList { get; private set; } = new BattleUnitsList();
       
        /// <summary>
        /// unit who is currently taking their turn 
        /// </summary>
        private UnitController CurrentUnit { get; set; }
        
        private Vector3Int SelectedTile { get; set; }
        
        /// <summary>
        /// tracks position of cursor (cursor is mainly cosmetic) 
        /// </summary>
        private Vector3Int _currentTile { get; set; } = new Vector3Int(0, 0, 0);
        private CombatActions _receivedAction { get; set; }

        private bool _combatOver { get; set; } = false;
        
        /// <summary>
        /// int used for knowing which thing is selected from a submenu
        /// </summary>
        private int ActionSelection { get; set; }

        //no clue how to connect the Overclock status differently 
        /// <summary>
        /// statuses associated with player actions (e.g., overclocking, attacking, defending too much, etc.)
        /// </summary>
        [SerializeField] private List<StatusSO> _actionStatuses = new List<StatusSO>();

        private Dictionary<string, StatusSO> _actionStatusDic = new Dictionary<string, StatusSO>();

        //TODO: sub to each unit themselves 
        //TODO: figure out what to do when a unit dies 
        public static event Action<List<UnitController>> unitsDead;
        //animation/tile update handled per unit at the instant they move. 
        public static event Action<UnitController> unitMoved;
        //we may want sounds when the cursor moves around 

        // Notify the camera system when a unit performs an action:
        public static event Action<UnitController> unitActionStarted;
        public static event Action<Vector3> hoverTileChanged;  
        public static event Action<BattlePhase, UnitController> battlePhaseChanged;
        
        public static event Action<TileController> examinedTile;
        public static event Action<UnitController> examinedUnit;
        public static event Action exitedExamine;
        
        /// <summary>
        /// true = win, false = lose
        /// </summary>
        public static event Action<bool> finishedBattle;

        // For now, until there's a better way
        #region UI_References
        AbilitySelectionPanel _abilitySelectionPanel;
        #endregion

        private void Awake()
        {
            _input = InputManager.Instance.Actions;
            _currentEncounter = GameSession.Instance.CurrentEncounterData;
            _abilitySelectionPanel = GameObject.Find("AbilitySelectionPanel").GetComponent<AbilitySelectionPanel>();
        }

        //must occur after OnEnable bc other classes will subscribe OnEnable
        private void Start()
        {
            SortTiles(tilesParent.GetComponentsInChildren<TileController>());
            GenerateCoords();
            CreateObjects();
            
            TurnOrder = new TurnQueue(BattleUnitsList.AllUnits); 
            rangeDisplay.Initialize(TileControllers);
            
            _phases[0] = new PlayerPhase(new CombatContext(TileControllers, TileCubeCoords, _input, _playerUnits, this));
            _phases[1] = new EnemyPhase(new CombatContext(TileControllers, TileCubeCoords, _input, _playerUnits, this));
            BattleMenuHE.PlayerWorldAction += HandleWorldAction;
            BattleMenuHE.PlayerUIAction += HandleUIAction;
            BattleMenuHE.ToggledManualEnemy += ToggleManualEnemy;
            _input.NoUICombatActions.Examine.performed += ExamineTile;
            
            foreach (StatusSO status in _actionStatuses)
                _actionStatusDic.Add(status.StatusId, status);
            ChangeTurn();
        }

        private void OnDestroy()
        {
            BattleMenuHE.PlayerWorldAction -= HandleWorldAction;
            BattleMenuHE.PlayerUIAction -= HandleUIAction;
            BattleMenuHE.ToggledManualEnemy -= ToggleManualEnemy;
            _input.NoUICombatActions.Examine.performed -= ExamineTile;
            
            _input.NoUICombatActions.Disable(); 
        }

        private void Update()
        {
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
                    tileController.AddUnit(unitController);
                    
                    unitController.Initialize(unitData);
                    unitController.SetTile(tileController.tileCoordinate);
                    BattleUnitsList.AddUnit(unitController);
                    unitController.OnDeath += HandleDeath;
                }
                
                tileController.RepositionUnits();
            }
        }

        
        void GenerateCoords()
        {
            IReadOnlyList<Vector3Int> coordinates = CombatTileCoordinates.GetAll();
            for (int tile = 0; tile < coordinates.Count; tile++)
            {
                Vector3Int coordinate = coordinates[tile];
                TileCubeCoords.Add(coordinate, tile);
                TileControllers[tile].tileCoordinate = coordinate;
            }
        } 
       
        /// <summary>
        /// should be called somehow whenever an action that should end a turn is performed by any unit
        /// (e.g., attacking, using an ability, defending)
        /// </summary>
        public void ChangeTurn()
        {
            if (_combatOver)
            {
                Debug.Log("combat over");
                if(_currentPhase != null)
                    _currentPhase.Exit();
                _currentPhase = null;
                return;
            }
            if (_currentPhase != null)
            {
                _currentPhase.Exit();
                _input.NoUICombatActions.Disable(); 
                TurnOrder.Consume(); //dequeue only after turn ends to keep portrait of current unit
            }
            CurrentUnit = TurnOrder.GetCurrentTurn().CurrentUnit;
            CurrentUnit.GetData().TriggerStatuses();
            while (CurrentUnit.GetData().Stunned)
            {
               TurnOrder.Consume(); 
               CurrentUnit = TurnOrder.GetCurrentTurn().CurrentUnit;
               CurrentUnit.GetData().TriggerStatuses();
               CurrentUnit.ResetValues();
            }
            
            if (toggledManualEnemy || CurrentUnit.GetData().Faction == Faction.Ally)
            {
                //manual enemy treats enemy phase like an ally phase
                _currentPhase = _phases[0];
            }
            else if (CurrentUnit.GetData().Faction == Faction.Enemy)
            {
                _currentPhase = _phases[1];

                //debugging code for enemy phase 
                /*
                TurnOrder.Consume();
                CurrentUnit = TurnOrder.GetCurrentTurn().CurrentUnit;
                while (CurrentUnit.GetData().Faction == Faction.Enemy)
                {
                    TurnOrder.Consume();
                    CurrentUnit = TurnOrder.GetCurrentTurn().CurrentUnit;
                }
                */
            }
            else
                return;

            _deadUnitsThisTurn.Clear();
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
        private void HandleWorldAction(CombatActions action)
        {
            _receivedAction = action;
            UnitController currUnit = CurrentUnit;
            Debug.Log(CurrentUnit);
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
                    rangeDisplay.DisplayMoveRange(currUnit.TileCoords, TileControllers, 1, CurrentUnit.GetData().Faction); //currently only adjacent tiles
                    return;
                case CombatActions.View:
                    _currentPhase.PushState();
                    return;
                case CombatActions.Overclock:
                    if(currUnit.GetData().Faction == Faction.Ally)
                        Overclock(currUnit);
                    return;
                default:
                    Debug.Log($"Action Not Implemented: {action}");
                    return;
            }
        }

        /// <summary>
        /// Handles player actions that requires a new ui panel 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="selection"></param>
        private void HandleUIAction(CombatActions action)
        {
            _receivedAction = action;
            UnitController currUnit = CurrentUnit;

            switch (action)
            {
                case CombatActions.Ability:
                    _currentPhase.PushState();
                    _abilitySelectionPanel.Intialize(currentUnit: currUnit, rangeDisplay: rangeDisplay, tileController: TileControllers);
                    return;
                case CombatActions.Item:
                    Debug.Log($"Action Not Implemented: {action}" );
                    return;
                default:
                    Debug.Log($"Action Not Implemented: {action}");
                    break;
            }
//            rangeDisplay.DisplayRange();
        }
        
        public TileController GetCurrTileController()
        {
            return TileControllers[TileCubeCoords[CurrentUnit.TileCoords]];
        }
        
        /// <summary>
        /// Sets cursor position to position of the current unit 
        /// </summary>
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
                    AbilityContext ac = new AbilityContext
                    {
                        Caster = currentUnit, 
                        Target = selectedTileController,
                        AllUnitInfo = BattleUnitsList
                    };
                    if(currentUnit.GetData().Moves[ActionSelection].IsReachable(ac.Caster, ac.Target))
                        HandleSelectAbility(ac); 
                    break;
                case CombatActions.Item:
                    Debug.Log("items are not implemented in mvp");
                    break;
                case CombatActions.View:
                    _currentPhase.PushState();
                    break;
                default:
                    Debug.Log("Unknown action");
                    break;
            }
        }
        
        private void HandleSelectAttack(UnitController currentUnit)
        {
            // Camera Event: Focus on the attacking unit when an attack begins.
            unitActionStarted?.Invoke(currentUnit);
            foreach (UnitController enemy in TileControllers[TileCubeCoords[SelectedTile]].UnitControllers)
            {
                CombatFormulas.Damage(currentUnit, currentUnit.GetData().DefaultAttackTypes(), enemy);
            }
            rangeDisplay.HideRange();
            FinishSelection();
        }

        
        //TODO: move to ability class execute function
        private void HandleSelectAbility(AbilityContext ac)
        {
            // Camera Event: Focus on the acting unit when an ability is used.
            unitActionStarted?.Invoke(CurrentUnit);
            AbilitySO ability = CurrentUnit.GetData().Moves[ActionSelection];
            rangeDisplay.HideRange();
            ability.Cast(ac);
            //epicenter hit 
            FinishSelection();
        }
        
        private void HandleSelectMove(TileController selectedTileController, UnitController currentUnit)
        {
            // Camera Event: Focus on the moving unit when movement begins.
            unitActionStarted?.Invoke(currentUnit);
            selectedTileController.AddUnit(GetCurrTileController().RemoveUnit(currentUnit));
            currentUnit.TryMove(selectedTileController.Position(),selectedTileController.tileCoordinate);
            selectedTileController.RepositionUnits(); //maybe more efficient to call here than in AddUnit bc of the CreateObjects function 
            RedoSelection();
        }

        /// <summary>
        /// adds the actual overclock status to the player unit
        /// </summary>
        /// <param name="unit"></param>
        private void Overclock(UnitController unit)
        {
            AllyUnit au = (AllyUnit)unit.GetData();
           // au.AddStatus(_actionStatusDic["overclock"]);
            au.Overclock();
           //if(au.OverclockLevel == OverclockTiers.Extreme)
           //   au.AddStatus(_actionStatusDic["burnout_0"]);
            Debug.Log("overclocking");
            RedoSelection();
        }

        public TileController GetTileController(Vector3Int pos)
        {
            return TileControllers[TileCubeCoords[pos]];
        }

        private void FinishSelection()
        {
            ResetCurrentTile();
            ChangeTurn();
        }

        public void RedoSelection()
        {
            Debug.Log("redo selection");
            rangeDisplay.HideRange();
            ResetCurrentTile();
            _currentPhase.Exit();
            battlePhaseChanged?.Invoke(_currentPhase, CurrentUnit); //calls this to make the UI pop up again even when turn is same
        }

        private void ExamineTile(InputAction.CallbackContext context)
        {
           HandleWorldAction(CombatActions.View);
           examinedTile?.Invoke(GetTileController(SelectedTile)); 
        }

        public void ExamineUnit(UnitController unitController)
        {
            examinedUnit?.Invoke(unitController);
        }

        //todo: probably enqueue death effects 
        private void HandleDeath(UnitController uc)
        {
                /*
                foreach (var status in CurrentUnit.GetData().Statuses.Values)
                {
                    if (status.StatusSOData.TriggerOnKill)
                    {
                        status.Trigger(CurrentUnit.GetData());
                    }
                }
                */
            Debug.Log($"{uc.GetData().Name} has died");
            BattleUnitsList.KillUnit(uc);
            CheckCombatOver();
        }


        /// <summary>
        /// returns true if battle is over, else false 
        /// </summary>
        /// <returns></returns>
        private void CheckCombatOver()
        {
            if (BattleUnitsList.DeadAllies >= BattleUnitsList.Allies.Count)
            {
               finishedBattle?.Invoke(false);
               _combatOver = true;
               return;
            }
            if (BattleUnitsList.DeadEnemies >= BattleUnitsList.Enemies.Count)
            {
                foreach (AllyUnitController auc in BattleUnitsList.Allies)
                {
                    if(!((AllyUnit)auc.GetData()).CheckOverclock())
                       ((AllyUnit)auc.GetData()).Underclock();
                }
                finishedBattle?.Invoke(true);
                _combatOver = true;
                return;
            }
            _combatOver = false;
        }

        public void ExitExamine()
        {
            exitedExamine?.Invoke();
            Debug.Log("e");
        }

        public UnitController GetCurrentUnit() { return CurrentUnit; }
        public Vector3Int GetSelectedTile() { return _currentTile; }

        private void ToggleManualEnemy(bool b) { toggledManualEnemy = b; }
    }
}
