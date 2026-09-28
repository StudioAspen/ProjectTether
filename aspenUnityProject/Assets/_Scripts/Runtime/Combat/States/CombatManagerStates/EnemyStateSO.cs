using _Scripts.Runtime.Combat.States.ContextData;
using UnityEngine;

namespace _Scripts.Runtime.Combat.States.CombatManagerStates
{
    public abstract class EnemyStateSO : ScriptableObject
    {
        //percentage at which the state exits
        [SerializeField] private float percentHPAtExit;
        public float PercentHPAtExit => percentHPAtExit;
        
        //add weights or something 

        public abstract void Enter(EnemyCombatContext ecb);
      //  public abstract void Update(EnemyPhase ep);
        public abstract void Exit(EnemyCombatContext ecb);
    }
}