using UnityEngine;
using Consystently;

public class EnemyCatalog : Catalog<EnemyUnitSO>
{
  /// <summary>
  /// Returns an enemy unit with the specified name in the catalog. Returns null if no such enemy can be found.
  /// </summary>
  /// <param name="name">The enemy's's name.</param>
  /// <returns>An enemy from the catalog.</returns>
  public EnemyUnitSO GetEnemyByName(string name)
  {
    EnemyUnitSO enemyUnit = objects.Find(x => x.Name == name);

    if (enemyUnit)
      return enemyUnit;
    else
    {
      Debug.LogError($"Enemy named {name} not found in the Catalog!");
      return null;
    }
  }
}