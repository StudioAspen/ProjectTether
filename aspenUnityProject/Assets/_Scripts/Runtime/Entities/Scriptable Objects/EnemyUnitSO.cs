using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Scriptable Objects/Unit/Enemy", order = 1)]
public class EnemyUnitSO : UnitDataSO
{
  public override Faction Faction => Faction.Enemy;
}