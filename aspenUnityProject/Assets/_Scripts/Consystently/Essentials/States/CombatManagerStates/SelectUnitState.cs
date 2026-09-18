namespace Consystently.Essentials
{
    public class SelectUnitState : ActionState 
    {
        public SelectUnitState(CombatManager combatManager, PlayerPhase battlePhase) : base(combatManager, battlePhase) {}
        public override void Enter()
        {
            //tileSelect's input also works 
            CombatManager.Input.Enable(); 
        }

        public override void Update()
        {
        }

        public override void Exit()
        {
            
        }
    }
}