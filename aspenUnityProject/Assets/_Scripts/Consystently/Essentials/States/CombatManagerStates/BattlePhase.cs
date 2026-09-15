using UnityEngine.InputSystem;

namespace Consystently.Essentials
{
    public abstract class BattlePhase : IState
    {
        protected CombatManager CombatManager { get; private set; }

        protected BattlePhase(CombatManager combatManager)
        {
            CombatManager = combatManager; 
        }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit(); 
        public abstract void PushState();

    }
}