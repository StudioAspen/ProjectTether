using System;
using Tether.CharacterSystems;
using UnityEngine;
using Random = UnityEngine.Random;

public static class CombatFormulas
{
  //TODO: create a separate function for healing?
  public static void Damage(UnitController attacker, Element[] attackElements, UnitController defender)
  {
    Unit attackerStats = attacker.GetData();
    Unit defenderStats = defender.GetData();
    bool crit = Crit(attacker, defender);
    bool miss = Miss(attacker, defender);
    float damage = 0f;

    if (attackerStats == null || defenderStats == null)
    {
      Debug.LogError("Missing UnitStats");
      return;
    }
    //Actual Damage
    //currently picks the highest stat between strength/technical (classes should generally have a focus on either str/tech)
    if(attackerStats.Strength > attackerStats.Tech) 
      damage = MathF.Round(attackerStats.Strength*(166f / (166f + defenderStats.Defense)) * Random.Range(0.9f, 1.1f)); //Round up
    else
      damage = MathF.Round(attackerStats.Tech*(166f / (166f + defenderStats.Defense)) * Random.Range(0.9f, 1.1f)); //Round up
    
    //Dodge Chance
    if(miss)
    {
      Debug.Log($"{attacker.name} missed!");
      return;
    }
    //Modifiers
    //Crit
    if(crit)
    {
      damage*=2f;
      Debug.Log($"{attacker.name} CRIT!");
    }
    //Blocking
    if(defenderStats.IsBlocking)
    {
      damage = Mathf.Floor(damage * 0.5f);
      Debug.Log($"{defender.name} was blocking!");
    }

    foreach (Element element in attackElements)
      damage *= defenderStats.Affinities[element].Multiplier();

    defenderStats.ChangeHealthRemaining(Mathf.CeilToInt(damage));
    Debug.Log($"{attackerStats.Name} dealt {Mathf.Ceil(damage)} damage to {defenderStats.Name}. {defenderStats.Name} has {defenderStats.HealthRemaining} HP remaining."); 
  }

  private static bool Crit(UnitController attacker, UnitController defender)
  {
    Unit attackerStats = attacker.GetData();
    Unit defenderStats = defender.GetData();
    bool crit = false;

    if (attackerStats == null || defenderStats == null)
    {
      Debug.LogError("Missing UnitStats");
        return false;
    }
    
    int critSuccess = Random.Range(1, 21); //random number between 1-20 as 21 falls out of the range of Random.Range
    if(critSuccess == 20)
      crit = true; //5% chance of success

    return crit;
  }

  //true = miss. False = hit 
  private static bool Miss(UnitController attacker, UnitController defender)
  {
    Unit attackerStats = attacker.GetData();
    Unit defenderStats = defender.GetData();

    if (attackerStats == null || defenderStats == null)
    {
      Debug.LogError("Missing UnitStats");
        return false;
    }

    float hitChance = (attackerStats.Precision / (defenderStats.Evasion * 1.0f));
    //Debug.Log(hitchance);

    if (hitChance >= 1f) //If Precision > Finesse i.e. hit > dodge chance
      return false;
    if(hitChance is < 1f and > 0f) //If Precision < Finesse but not 0
    {
      float randomhit = Random.Range(0f, 1f);
      if (randomhit <= hitChance)
        return false;
      return true;
    }
    else //If Precision is under 0 (other values are covered by the above statements)
      return true;

  }
}
