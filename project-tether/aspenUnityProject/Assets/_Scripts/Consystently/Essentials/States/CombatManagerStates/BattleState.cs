using UnityEngine.InputSystem;

namespace Consystently.Essentials
{
    public abstract class BattleState : IState
    {
        protected CombatManager CombatManager { get; private set; }

        protected BattleState(CombatManager combatManager)
        {
            CombatManager = combatManager; 
        }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit(); 
        public abstract void PushState();

    }
}