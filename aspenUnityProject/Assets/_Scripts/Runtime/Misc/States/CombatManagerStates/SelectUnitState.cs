using System.Collections.Generic;
using TileSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Runtime.Managers
{
    public class SelectUnitState : ActionState 
    {
        public SelectUnitState(CombatManager combatManager, PlayerPhase battlePhase) : base(combatManager, battlePhase) {}
        
      
        //trying to generalize a formula for the positions is difficult, so I am using 4 dictionaries 
        private List<Dictionary<Vector2, int>> dictionaries = new List<Dictionary<Vector2, int>>()
        {
            new Dictionary<Vector2, int>()
            {
                { new Vector2(0, 1), 0 },
            },
            new Dictionary<Vector2, int>()
            {
                { new Vector2(0, 1), 0 },
                { new Vector2(0, -1), 1 },
            },
            new Dictionary<Vector2, int>()
            {
                { new Vector2(0, 1), 0 },
                { new Vector2(1, 0), 1 },
                { new Vector2(-1, 0), 2 },
            },
            new Dictionary<Vector2, int>()
            {
                { new Vector2(0, 1), 0 },
                { new Vector2(1, 0), 1 },
                { new Vector2(0, -1), 2 }, 
                { new Vector2(-1, 0), 3 },
            }
        };

        private Vector2 initialPos;
        
        
        public override void Enter()
        {
            //tileSelect's input also works for this case
            CombatManager.Input.Enable();
            if(CombatManager.GetTileController(CombatManager.SelectedTile).UnitCount() > 0)
                CombatManager.Input.TileSelect.Move.started += OnMove;
            //use confirm if we want to display details on unit select specifically
            //CombatManager.Input.TileSelect.Confirm.performed += 
            CombatManager.Input.TileSelect.Exit.performed += ((PlayerPhase)BattlePhase).PopState;
        }

        public override void Update()
        {
        }

        public override void Exit()
        {
            Debug.Log("exiting");
            CombatManager.ExitExamine(); 
            if(CombatManager.GetTileController(CombatManager.SelectedTile).UnitCount() > 0)
                CombatManager.Input.TileSelect.Move.started -= OnMove;
            CombatManager.Input.TileSelect.Exit.performed -= ((PlayerPhase)BattlePhase).PopState;
            CombatManager.Input.Disable();
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            initialPos = new Vector2(0, 0);
            Vector2 move = context.ReadValue<Vector2>();
            Vector2 tryMove = initialPos + move;
            TileController currTileController = CombatManager.GetTileController(CombatManager.SelectedTile);
            if (dictionaries[currTileController.UnitCount() - 1].ContainsKey(tryMove)){
                initialPos = tryMove;
                CombatManager.ExamineUnit(
                    currTileController.GetUnitAt(dictionaries[currTileController.UnitCount() - 1][initialPos]));
            }
        }
    }
}