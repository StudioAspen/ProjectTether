using _Scripts.Runtime.Combat.States.ContextData;
using _Scripts.Runtime.Managers;

namespace _Scripts.Runtime.Combat.States.CombatManagerStates
{
    public abstract class BattlePhase 
    {
        protected CombatContext CombatContext { get; private set; }

        protected BattlePhase(CombatContext combatContext)
        {
            CombatContext = combatContext;
        }

        public abstract void Enter();
//        public abstract void Update();
        public abstract void Exit(); 
        public abstract void PushState();

        public abstract void PopState(); 


    }
}