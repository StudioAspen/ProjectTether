using _Scripts.Runtime.Combat.States.ContextData;
using _Scripts.Runtime.Managers;

namespace _Scripts.Runtime.Combat.States.CombatManagerStates
{
    public abstract class ActionState
    {
        protected CombatContext CombatContext { get; private set; }
        protected BattlePhase BattlePhase { get; private set; }

        protected ActionState(CombatContext combatContext, BattlePhase battlePhase)
        {
           CombatContext = combatContext; 
           BattlePhase = battlePhase; 
        }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit(); 
    }
}