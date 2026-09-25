namespace _Scripts.Runtime.Managers
{
    public abstract class ActionState : IState
    {
        protected CombatManager CombatManager { get; private set; }
        protected BattlePhase BattlePhase { get; private set; }

        protected ActionState(CombatManager combatManager, BattlePhase battlePhase)
        {
           CombatManager = combatManager; 
           BattlePhase = battlePhase; 
        }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit(); 
    }
}