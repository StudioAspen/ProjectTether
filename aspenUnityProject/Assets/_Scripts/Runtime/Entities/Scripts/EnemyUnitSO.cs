using System.Collections.Generic;
using _Scripts.Runtime.Combat;
using _Scripts.Runtime.Combat.States.CombatManagerStates;
using UnityEngine;

namespace _Scripts.Runtime.Entities.Scripts
{
  [CreateAssetMenu(fileName = "NewEnemyData", menuName = "Scriptable Objects/Unit/Enemy", order = 1)]
  public class EnemyUnitSO : UnitDataSO
  {
    public override Faction Faction => Faction.Enemy;
  
    [SerializeField] private List<EnemyStateSO> behaviourStates; 
    public List<EnemyStateSO> BehaviourStates => behaviourStates; 
  }
}