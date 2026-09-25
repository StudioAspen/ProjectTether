using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Runtime.Managers
{
    public class PlayerPhase : BattlePhase
    {
        private Stack<ActionState>  stateStack = new Stack<ActionState>();
        
        //TODO: add new state class for individual unit selection
        private List<ActionState> states  = new List<ActionState>();

        public PlayerPhase(CombatManager combatManager) : base(combatManager)
        {
           states.Add(new SelectTileState(combatManager, this)); 
           states.Add(new SelectUnitState(combatManager, this));
        }

        public override void Enter()
        {
           Debug.Log("player phase entered"); 
           stateStack.Clear();
        }

        /*
        //TODO: get rid of update from all states later? 
        public override void Update()
        {
            if(stateStack.Count > 0)
                stateStack.Peek().Update();
        }
        */

        public override void Exit()
        {
            foreach (ActionState state in stateStack)
               state.Exit();
            stateStack.Clear();
        }

        public void PopState(InputAction.CallbackContext context)
        {
           if (stateStack.Count == 0)
                return;
           stateStack.Pop().Exit();
           if (stateStack.Count == 0)
               CombatManager.RedoSelection(); 
           else
               stateStack.Peek().Enter();
        }

        public override void PushState()
        {
            if (stateStack.Count > states.Count - 1)
            {
                Debug.Log("playerState stack bug. how is this possible");
                return;
            }
            if (stateStack.Count > 0)
                stateStack.Peek().Exit(); 
            stateStack.Push(states[stateStack.Count]);
            stateStack.Peek().Enter();
        }
        
    }
}