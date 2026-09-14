using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Consystently.Essentials
{
    public class EnemyState : BattleState
    {
        public EnemyState(CombatManager combatManager) : base(combatManager) {}

        public override void Enter()
        {
            Debug.Log("enemy phase entered");
        }

        public override void Update()
        {
           Debug.Log("enemy update"); 
        }

        public override void Exit()
        {
            
        }

        public override void PushState()
        {
            Debug.Log("hello3"); 
        }

        public void PopState()
        {
            
        }
    }
}