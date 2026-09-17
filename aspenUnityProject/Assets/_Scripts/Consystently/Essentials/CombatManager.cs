using System;
using System.Collections.Generic;
using _Scripts.Runtime.Misc;
using Consystently.Essentials.Math;
using Tether.CharacterSystems;
using TileSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

//TODO: NEED COROUTINES when we are past the mvp(?)
namespace Consystently.Essentials
{
    /*will not be a traditional manager because it does not
    need to be static. EncounterManager will be static and the one
    to send the data over to CombatManager. CombatManager will exist
    in the battle scene only. The UIManager will need a reference to the CombatManager
    */
    public class CombatManager : MonoBehaviour
    {
        private const float ArbitraryOffset = 10;
        private const int TileNum = 19;
        private UnitDataSO[,] initializerData;
        
        //no use in mvp
        //private TileSO[] tiles;
        private Encounter encounter; 
        [SerializeField] private Transform tilesParent;
        public InputSystem_Actions Input { get; private set; }
        public TileController[] tileControllers { get; private set; }= new TileController[TileNum];

        public Dictionary<Vector3Int, int> TileCubeCoords { get; private set; }= new Dictionary<Vector3Int, int>();
        
        //make sure it has references and not copies of the objects, so changes are reflected
        //TODO: update turn order to match the initiative proposal in the doc  - we will have to create a new class
        public List<UnitController> TurnOrder { get; private set; }= new List<UnitController>();
        public List<UnitController> DeadUnits {get; private set;}= new List<UnitController>();
        
        //TODO: add an enum for this if we ever have more than just enemy/ally turns 
        private readonly BattlePhase[] phases = new BattlePhase[2];
        private BattlePhase currentPhase;  
        [SerializeField] private RangeDisplay rangeDisplay; 
        
        #region miscStateManagementVariables
        
        //need totals for defeat/win checks
        private int TotalAllies { get; set; }
        private int TotalEnemies { get; set; }
        
        //for easier targeting calculation
        public List<AllyUnitController> PlayerUnits { get; private set; } = new List<AllyUnitController>();
        public int CurrentUnitTurn { get; private set; }
        private Vector3Int SelectedTile { get; set; }
        private Vector3Int CurrentTile { get; set; } = new Vector3Int(0, 0, 0);
        private CombatActions ReceivedAction { get; set; }
        public int ActionSelection { get; private set; }
        
        #endregion

        //TODO: sub to each unit themselves 
        //TODO: implement the proper response to unit death. For each unit that dies, add them to an array. 
        public static event Action<Unit[]> unitsDead;
        //animation/tile update handled per unit at the instant they move. Perhaps also camera class  
        public static event Action<UnitController> unitMoved;
        //we may want sounds when the cursor moves around 
        public static event Action<Vector3> hoverTileChanged;  
        public static event Action<BattlePhase, UnitController> battlePhaseChanged;

        private void Awake()
        {
            Input = new InputSystem_Actions();
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
            rangeDisplay.Initialize(tileControllers);
            
            phases[0] = new PlayerPhase(this);
            phases[1] = new EnemyPhase(this);
            CombatUI.PlayerAction += HandleAction;
            CombatUI.PlayerSelectiveAction += HandleAction;
            ChangeTurn(); 
        }

        private void OnDisable()
        {
            CombatUI.PlayerAction -= HandleAction;
            CombatUI.PlayerSelectiveAction -= HandleAction;
            Input.Disable();
            foreach (TileController tc in tileControllers)
            {
                foreach (UnitController uc in tc.UnitControllers)
                {
                    uc.OnUnitMove -= UnitHasMoved;
                }
            }
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
                tileControllers[tileController.Num()] = tileController;
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
                    GameObject newTempObject = Instantiate(encounter[tile,unit], tileControllers[tile].Position(), Quaternion.Euler(-90f,0,0));
                    //set up controllers after object instantiation so objects don't override each other's data
                    //the newly cloned object does not share the same reference as the original prefab, so there is no overriding
                    if (initializerData[tile, unit].Faction == Faction.Ally)
                    {
                        tileControllers[tile].AddUnit(newTempObject.GetComponent<AllyUnitController>());
                        TotalAllies++;
                        PlayerUnits.Add((AllyUnitController)tileControllers[tile].PeekUnit());
                    }
                    else if (initializerData[tile, unit].Faction == Faction.Enemy)
                    {
                        tileControllers[tile].AddUnit(newTempObject.GetComponent<EnemyUnitController>());
                        TotalEnemies++;
                    }
                    else 
                        Debug.Log("neutral units not yet implemented");
//                    Debug.Log($"{tile}: {unit}, {tileControllers[tile].UnitCount()}");
                    tileControllers[tile].GetUnitAt(unit).Initialize(initializerData[tile,unit]);
                    tileControllers[tile].GetUnitAt(unit).SetTile(tileControllers[tile].tileCoordinate);
                    TurnOrder.Add(tileControllers[tile].GetUnitAt(unit));
                    tileControllers[tile].GetUnitAt(unit).SetTile(tileControllers[tile].tileCoordinate);
                    tileControllers[tile].GetUnitAt(unit).OnUnitMove += UnitHasMoved;
                }
                tileControllers[tile].RepositionUnits(ArbitraryOffset);
            }
        }

        //starts at tile 0 and spirals outwards to get the cube coords for every tile 
        //coords are for determining proper tile selection when the user moves across the field 
        //3r(r+1)+1=tiles formula for generic implementation if additional rings are added
        //for reference, tile 18 should be (2,0,-2) 
        void GenerateCoords()
        {
            int tile = 0;
            Vector3Int currentPos = new Vector3Int(0, 0, 0);
            //           Debug.Log($"tile: {tile}, {currentPos}");
            TileCubeCoords.Add(currentPos, tile);
            tileControllers[tile].tileCoordinate = currentPos;
            for (int ring = 1; ring <= 2; ring++)
            {
                currentPos += CubeCoordDirections.NE.Vector();
                tile++;
//               Debug.Log($"tile: {tile}, {currentPos}");
                TileCubeCoords.Add(currentPos, tile);
                tileControllers[tile].tileCoordinate = currentPos;
                for (int southEasts = ring - 1; southEasts > 0; southEasts--)
                {
//                   currentPos += directions[(int)CubeCoordDirections.SE];
                    currentPos += CubeCoordDirections.SE.Vector();
                    tile++;
                    //                 Debug.Log($"tile: {tile}, {currentPos}");
                    TileCubeCoords.Add(currentPos, tile);
                    tileControllers[tile].tileCoordinate = currentPos;
                }
                for (int direction = (int)CubeCoordDirections.S; direction < Enum.GetNames(typeof(CubeCoordDirections)).Length; direction++)
                {
                    for (int times = ring; times > 0; times--)
                    {
                        currentPos += ((CubeCoordDirections)direction).Vector();
                        tile++;
                        //                    Debug.Log($"tile: {tile}, {currentPos}");
                        TileCubeCoords.Add(currentPos, tile);
                        //                   Debug.Log($"tileControllers size: {tileControllers.Length}");
                        tileControllers[tile].tileCoordinate = currentPos;
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
                    Debug.Log($"{tileControllers[tile].GetUnitAt(unit).GetData().Name}:  {tileControllers[tile].GetUnitAt(unit).GetData().Speed}");
                    
                }
            }
        }
        #endregion
        
        //dead are kept because lazy deletion. Also, there may or may not be a revive feature, so their order being kept is good.
        //I am also not sure if deletion is better because deletion would require searching and result in the entire list shifting. 
        public void ChangeTurn()
        {
            if (DeadUnits.Count >= (TotalAllies + TotalEnemies))
            {
                Debug.Log("All units dead.");
                return;
            }
            while (TurnOrder[CurrentUnitTurn].GetData().IsDead)
                CurrentUnitTurn = (CurrentUnitTurn + 1)%TurnOrder.Count;
            if (currentPhase != null)
            {
                currentPhase.Exit();
                CurrentUnitTurn = (CurrentUnitTurn + 1)%TurnOrder.Count;
            }
            if (TurnOrder[CurrentUnitTurn].GetData().Faction == Faction.Ally)
                currentPhase = phases[0];
            else if (TurnOrder[CurrentUnitTurn].GetData().Faction==Faction.Enemy)
                currentPhase = phases[1];
            else
                return;
            TurnOrder[CurrentUnitTurn].ResetValues();
            currentPhase.Enter();
            battlePhaseChanged?.Invoke(currentPhase, TurnOrder[CurrentUnitTurn]);
        } 
        
        //functions for camera/ui movement/whatever 
        public void MoveTileSelector(CubeCoordDirections direction)
        {
            Vector3Int projectedTile = CurrentTile + direction.Vector(); 
            if(TileCubeCoords.TryGetValue(projectedTile, out _))
            {
                CurrentTile = projectedTile;
                hoverTileChanged?.Invoke(tileControllers[TileCubeCoords[CurrentTile]].Position());
            }
        }
        
        /*
        the player's selectTileState (pushed by this function) will tell this manager when to
        execute the SelectTile function.
        I opted for states because the player may undo actions.
        Usually, the first requirement after selecting an action
        is selecting a tile.
        */
        private void HandleAction(CombatActions action)
        {
            ReceivedAction = action;
            UnitController currUnit = TurnOrder[CurrentUnitTurn];
            switch(action)
            {
                case CombatActions.Attack:
                    currentPhase.PushState();
                    rangeDisplay.DisplayAttackRange(tileControllers, currUnit);
                    return;
                case CombatActions.Defend:
                    currUnit.GetData().Defend();
                    FinishSelection();
                    return;
                case CombatActions.Move:
                    currentPhase.PushState();
                    rangeDisplay.DisplayMoveRange(currUnit.TileCoords, tileControllers, 1, Faction.Ally); //currently only adjacent tiles
                    return;
                case CombatActions.View:
                    currentPhase.PushState();
                    return;
                default:
                    Debug.Log($"Unknown action: {action}");
                    return;
            }
        }

        //for when action requires selection like with abilities/items
        private void HandleAction(CombatActions action, int selection)
        {
            ReceivedAction = action;
            ActionSelection = selection;
            UnitController currUnit = TurnOrder[CurrentUnitTurn];
            switch (action)
            {
                case CombatActions.Ability:
                    currentPhase.PushState();
                    rangeDisplay.DisplayAbilityRange(tileControllers, currUnit, currUnit.GetData().Moves[selection]);
                    return;
                case CombatActions.Item:
                    Debug.Log($"unknown action: {action}" );
                    return;
            }
//            rangeDisplay.DisplayRange();
        }
        
        public TileController GetCurrTileController()
        {
            return tileControllers[TileCubeCoords[TurnOrder[CurrentUnitTurn].TileCoords]];
        }
        
        public void ResetCurrentTile()
        {
            SelectedTile = TurnOrder[CurrentUnitTurn].TileCoords;
            CurrentTile = SelectedTile;
            hoverTileChanged?.Invoke(tileControllers[TileCubeCoords[CurrentTile]].Position());
        }
        

        //attack is basic attack with no ability selection. 
        //attacks do not target individual enemies and hit every enemy in a tile
        //TODO: for abilities, we may need a new function when we want added functionality
        //TODO: fix redoSelection, fix unitControllers not changing the tile 
        //TODO: break into functions
        public void SelectTile(InputAction.CallbackContext context)
        {
            SelectedTile = CurrentTile;
            TileController selectedTileController = tileControllers[TileCubeCoords[SelectedTile]];
            if (ReceivedAction == CombatActions.View) 
                return;
            UnitController currentUnit = TurnOrder[CurrentUnitTurn];
            switch (ReceivedAction)
            {
                case CombatActions.Attack:
                    if (currentUnit.AttackReachable(selectedTileController)) 
                        HandleSelectAttack(currentUnit);
                    break;
                case CombatActions.Move:
                    if (selectedTileController.IsMoveable(currentUnit.TileCoords,1, Faction.Ally) && !currentUnit.HasMoved)
                        HandleSelectMove(selectedTileController, currentUnit); 
                    return;
                case CombatActions.Ability:
                    if(currentUnit.AbilityReachable(currentUnit.GetData().Moves[ActionSelection],selectedTileController))
                        HandleSelectAbility(currentUnit); 
                    break;
                case CombatActions.Item:
                    Debug.Log("items are not implemented in mvp");
                    break;
                default:
                    Debug.Log("Unknown action");
                    break;
            }
        }
        
        private void HandleSelectAttack(UnitController currentUnit)
        {
            foreach (UnitController enemy in tileControllers[TileCubeCoords[SelectedTile]].UnitControllers) 
                CombatFormulas.Damage(currentUnit, currentUnit.GetData().DefaultAttackTypes(), enemy);  
            rangeDisplay.HideRange();
            FinishSelection();
        }

        private void HandleSelectAbility(UnitController currentUnit)
        {
            TileController epicenter =  tileControllers[TileCubeCoords[SelectedTile]];
            AbilitySO ability = currentUnit.GetData().Moves[ActionSelection];
            rangeDisplay.HideRange();           
            //epicenter hit 
            foreach (UnitController enemy in tileControllers[TileCubeCoords[SelectedTile]].UnitControllers)
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
                            foreach (UnitController enemy in tileControllers[TileCubeCoords[SelectedTile]].UnitControllers)
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
            rangeDisplay.HideRange();
            RedoSelection();
        }

        public TileController GetTileController(Vector3Int pos)
        {
            return tileControllers[TileCubeCoords[pos]];
        }
        
        private void UnitHasMoved(UnitController unitController)
        {
            Debug.Log("unit has moved");
            unitMoved?.Invoke(unitController);
        }

        private void FinishSelection()
        { 
            ChangeTurn();
            ResetCurrentTile();
            battlePhaseChanged?.Invoke(currentPhase, TurnOrder[CurrentUnitTurn]);
        }

        public void RedoSelection()
        {
            Debug.Log("redo selection");
            rangeDisplay.HideRange();
            ResetCurrentTile();
            currentPhase.Exit();
            battlePhaseChanged?.Invoke(currentPhase, TurnOrder[CurrentUnitTurn]);
        }

        public UnitController GetCurrentUnit()
        {
            return TurnOrder[CurrentUnitTurn]; 
        }

    }
}