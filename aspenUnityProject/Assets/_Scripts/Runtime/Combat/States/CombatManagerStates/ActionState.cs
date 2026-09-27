using _Scripts.Runtime.Managers;

namespace _Scripts.Runtime.Combat.States.CombatManagerStates
{
    public abstract class ActionState
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