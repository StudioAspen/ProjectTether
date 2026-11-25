using System;
using _Scripts.Runtime.Combat;
using _Scripts.Runtime.Combat.Abilities;
using _Scripts.Runtime.Combat.Statuses;
using _Scripts.Runtime.Entities.Scripts;
using _Scripts.Runtime.Entities.Scripts.Combat;
using _Scripts.Runtime.Tile_System.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Runtime
{

  public static class CombatFormulas
  {
    #region unit vs unit
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
      if (attackerStats.GetEStat(Stat.STR) > attackerStats.GetEStat(Stat.TEC))
        damage = MathF.Round(attackerStats.GetEStat(Stat.STR) * (166f / (166f + defenderStats.GetEStat(Stat.DEF))) *
                             Random.Range(0.9f, 1.1f));
      else
        damage = MathF.Round(attackerStats.GetEStat(Stat.TEC) * (166f / (166f + defenderStats.GetEStat(Stat.RES))) *
                             Random.Range(0.9f, 1.1f));
      if(miss)
      {
        Debug.Log($"{attacker.name} missed!");
        return;
      }
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
      damage = GetElementalDamage(damage, attackElements, defenderStats);
      defenderStats.DecreaseHealthRemaining(Mathf.CeilToInt(damage));
      if(defenderStats.IsDead)
        defenderStats.SetDeathCause(new DeathContext{DeathCause = DeathCause.Combat, Murderer=attacker});
      Debug.Log(damage);
      Debug.Log($"{attackerStats.Name} dealt {Mathf.Ceil(damage)} damage to {defenderStats.Name}. {defenderStats.Name} has {defenderStats.HealthRemaining} HP remaining."); 
    }

  
    public static void AbilityDamage(UnitController attacker, UnitController defender, AbilitySO ability)
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

      //TODO: ability damage scaling needs to be changed (waiting on design team)
      switch (ability.AbilityType)
      {
        case AbilityType.Physical:
          damage = MathF.Round(attackerStats.GetEStat(Stat.STR)* (166f / (166f + defenderStats.GetEStat(Stat.DEF))) *
            Random.Range(0.9f, 1.1f) + ability.Damage); //Round up
          break;
        case AbilityType.Technical: 
          damage = MathF.Round(attackerStats.GetEStat(Stat.TEC)* (166f / (166f + defenderStats.GetEStat(Stat.RES))) *
            Random.Range(0.9f, 1.1f) + ability.Damage); //Round up
          break;
        case AbilityType.Status:
          Debug.Log("Do something with status in the future");
          break;
        default:
          Debug.Log("Unknown ability type");
          break;
      }

      if(miss && ability.CanMiss)
      {
        Debug.Log($"{attacker.name} missed!");
        return;
      }
    
      if(crit)
      {
        damage*=2f;
        Debug.Log($"{attacker.name} CRIT!");
      }
   
      if(defenderStats.IsBlocking)
      {
        damage = Mathf.Floor(damage * 0.5f);
        Debug.Log($"{defender.name} was blocking!");
      }
      damage = GetElementalDamage(damage, ability.Element, defenderStats);
      defenderStats.DecreaseHealthRemaining(Mathf.CeilToInt(damage));
      if(defenderStats.IsDead)
        defenderStats.SetDeathCause(new DeathContext{DeathCause = DeathCause.Combat, Murderer=attacker});
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

      float hitChance = attackerStats.GetEStat(Stat.PRC) / (defenderStats.GetEStat(Stat.EVA) * 1.0f);
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
    #endregion

    private static float GetElementalDamage(float damage, Element[] elements, Unit receiver)
    {
      float mean = damage;
      if (elements.Length > 0)
        mean /= elements.Length;
      else
        return mean;
      float sum = 0f;
      foreach (Element element in elements)
        sum += mean * receiver.Affinities[element].Multiplier();
      return sum;
    }
    

    /// <summary>
    /// for damage taken from statuses
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="damageTypes"></param>
    /// <param name="receiver"></param>
    public static void StatusDamage(float damage, Element[] damageTypes, Unit receiver)
    {
      damage = GetElementalDamage(damage, damageTypes, receiver);
      receiver.DecreaseHealthRemaining(Mathf.CeilToInt(damage));
      if(receiver.IsDead)
        receiver.SetDeathCause(new DeathContext{DeathCause = DeathCause.Status, Murderer=null}); //todo: setting someone on fire does not currently count as murder 
    }

    /// <summary>
    /// for damage taken from terrain
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="damageTypes"></param>
    /// <param name="receiver"></param>
    public static void TerrainDamage(float damage, Element[] damageTypes, UnitController receiver)
    {
      damage = GetElementalDamage(damage, damageTypes, receiver.GetData());
      receiver.GetData().DecreaseHealthRemaining(Mathf.CeilToInt(damage));
      if(receiver.GetData().IsDead) 
        receiver.GetData().SetDeathCause(new DeathContext{DeathCause = DeathCause.Environment, Murderer=null}); //todo: pushing someone onto lava does not currently count as murder
    }
  }
  

}
