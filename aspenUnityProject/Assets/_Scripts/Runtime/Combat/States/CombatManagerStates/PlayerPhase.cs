using System.Collections.Generic;
using _Scripts.Runtime.Combat.States.ContextData;
using UnityEngine.InputSystem;

namespace _Scripts.Runtime.Combat.States.CombatManagerStates
{
    public class PlayerPhase : BattlePhase
    {
        private readonly Stack<ActionState>  stateStack = new Stack<ActionState>();
        
        private readonly List<ActionState> states  = new List<ActionState>();

        public PlayerPhase(CombatContext combatContext) : base(combatContext)
        {
           states.Add(new SelectTileState(combatContext, this)); 
           states.Add(new SelectUnitState(combatContext, this));
        }

        public override void Enter()
        {
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
            while (stateStack.Count > 0)
                PopState();
        }
        
        public override void PopState()
        {
            if (stateStack.Count == 0)
                return;
            stateStack.Pop().Exit(); 
            if (stateStack.Count == 0)
            {
                CombatContext.cfb.RedoSelection();
            }
            else
                stateStack.Peek().Enter();
        }

        /// <summary>
        /// removes one state from the stack. Called when backing out.
        /// </summary>
        /// <param name="context"></param>
        public void PopState(InputAction.CallbackContext context)
        {
            PopState();
        }

        public override void PushState()
        {
            if (stateStack.Count > states.Count - 1)
            {
                return;
            }
            if (stateStack.Count > 0)
                stateStack.Peek().Exit(); 
            stateStack.Push(states[stateStack.Count]);
            stateStack.Peek().Enter();
        }

       
    }
}