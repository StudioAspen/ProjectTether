using System.Collections.Generic;
using _Scripts.Runtime.Managers;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Scriptable Objects/Unit/Enemy", order = 1)]
public class EnemyUnitSO : UnitDataSO
{
  public override Faction Faction => Faction.Enemy;
  
  [SerializeField] private List<EnemyStateSO> behaviourStates; 
  public List<EnemyStateSO> BehaviourStates => behaviourStates; 
}