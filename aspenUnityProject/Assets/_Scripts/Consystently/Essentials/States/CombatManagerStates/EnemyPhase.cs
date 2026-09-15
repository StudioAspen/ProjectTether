using System;
using System.Collections.Generic;
using Tether.CharacterSystems;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Consystently.Essentials
{
    //current implementation is for the mvp 
    public class EnemyPhase : BattlePhase
    {

        //since we're using a stack already, we could represent enemy 'phases' with them quite easily
        //e.g., when the enemy is low on hp, pop the aggressive state and drop them into a defensive one
        private Stack<ActionState> stateStack = new Stack<ActionState>();
        
        //IN THE FUTURE, we could read the current unit (first have them have a list of states in the SO)
        //from combatManager and then populate the list with the unit's decided states. We then pick a 
        //state from there to add to the stack. 
        private List<ActionState> states = new List<ActionState>();
        private EnemyUnitController euc; 

        public EnemyPhase(CombatManager combatManager) : base(combatManager)
        {
           states.Add(new EnemyAggressiveState(combatManager, this)); 
        }

        public override void Enter()
        {
            Debug.Log("enemy phase entered");
            stateStack.Clear();
            euc = (EnemyUnitController)CombatManager.TurnOrder[CombatManager.CurrentUnitTurn];
            //also clear and add new states later on 
        }

        public override void Update()
        {
            if(stateStack.Count > 0) 
                stateStack.Peek().Update();
        }

        public override void Exit()
        {
            stateStack.Clear(); 
        }

        public override void PushState()
        {
            if (stateStack.Count > states.Count - 1)
                return;
            if (stateStack.Count > 0)
                stateStack.Peek().Exit();        
            stateStack.Push(states[stateStack.Count]);
            stateStack.Peek().Enter();
        }

        //to be used when the enemy transitions 'phases' in the future.
        //e.g., an enemy is attacked and loses enough hp to transition into 
        //a cautious state
        public void PopState()
        {
            if (stateStack.Count == 0)
                return;
            stateStack.Pop().Exit();
            if(stateStack.Count == 0)
                Debug.Log("Enemy should be dead before/at this stage in the future implementation"); 
            else
                stateStack.Peek().Enter();
        }
        
            
    }
}