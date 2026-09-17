namespace Consystently.Essentials
{
    public abstract class ActionState : IState
    {
        protected CombatManager CombatManager { get; private set; }
        protected BattleState BattleState { get; private set; }

        protected ActionState(CombatManager combatManager, BattleState battleState)
        {
           CombatManager = combatManager; 
           BattleState = battleState; 
        }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit(); 
    }
}