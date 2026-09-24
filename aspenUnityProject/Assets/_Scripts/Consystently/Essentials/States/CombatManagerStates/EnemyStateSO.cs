using UnityEngine;

namespace Consystently.Essentials
{
    public abstract class EnemyStateSO : ScriptableObject
    {
        //percentage at which the state exits
        [SerializeField] private float percentHPAtExit;
        public float PercentHPAtExit => percentHPAtExit;
        
        //add weights or something 

        public abstract void Enter(EnemyPhase ep);
      //  public abstract void Update(EnemyPhase ep);
        public abstract void Exit(EnemyPhase ep);
    }
}