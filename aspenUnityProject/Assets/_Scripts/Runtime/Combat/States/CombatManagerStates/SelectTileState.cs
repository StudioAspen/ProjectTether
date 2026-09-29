using _Scripts.Runtime.Combat.States.ContextData;
using _Scripts.Runtime.Managers;
using _Scripts.Runtime.Math;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

namespace _Scripts.Runtime.Combat.States.CombatManagerStates
{
    //the state when the player is selecting a tile for an action (e.g., attacking, viewing, etc.)
    public class SelectTileState : ActionState
    {
        public SelectTileState(CombatContext combatContext, PlayerPhase battlePhase) : base(combatContext, battlePhase) {}
        
       
        public override void Enter()
        {
            CombatContext.Input.TileSelect.Enable();
            CombatContext.Input.TileSelect.Confirm.performed += CombatContext.CombatFunctionProvider.SelectTile;
            CombatContext.Input.TileSelect.Exit.performed += ((PlayerPhase)BattlePhase).PopState;
            CombatContext.Input.TileSelect.Move.started += OnMove;
        }

        public override void Update()
        {
            /*
            Vector2 move = CombatManager.Input.TileSelect.Move.ReadValue<Vector2>();
            if (move != Vector2.zero)
            {
                CombatManager.MoveTileSelector(move.GetDirection());
            }
            */
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            Vector2 move = context.ReadValue<Vector2>();
            CombatContext.CombatFunctionProvider.MoveTileSelector(move.GetDirection());
        }

        public override void Exit()
        {
            CombatContext.Input.TileSelect.Confirm.performed -= CombatContext.CombatFunctionProvider.SelectTile;
            CombatContext.Input.TileSelect.Exit.performed -= ((PlayerPhase)BattlePhase).PopState;
            CombatContext.Input.TileSelect.Move.started -= OnMove;
            CombatContext.Input.TileSelect.Disable();
        }
    }
}