using UnityEngine;

namespace Consystently.Essentials
{
    public abstract class EnemyStateSO : ScriptableObject
    {
        public abstract void Enter(EnemyPhase ep);
      //  public abstract void Update(EnemyPhase ep);
        public abstract void Exit(EnemyPhase ep);
    }
}