using _Scripts.Runtime.Managers;

namespace _Scripts.Runtime.Combat.States.CombatManagerStates
{
    public abstract class BattlePhase 
    {
        protected CombatManager CombatManager { get; private set; }

        protected BattlePhase(CombatManager combatManager)
        {
            CombatManager = combatManager; 
        }

        public abstract void Enter();
//        public abstract void Update();
        public abstract void Exit(); 
        public abstract void PushState();

    }
}