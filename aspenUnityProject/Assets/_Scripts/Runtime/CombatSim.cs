using UnityEngine;

//old code. Commented because I do not know if it may be useful later.
/*
public class CombatSim : MonoBehaviour
{
  public GameObject Player;
  public GameObject Enemy;
  public GameObject Attacker;
  public GameObject Defender;

  void Start()
  {
    Debug.Log("1 for player physical attacking, 2 for enemy physical attacking, 3 for player magic attacking, 4 for enemy magic attacking");
  }
  
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Alpha1))
    {
      Attacker = Player;
      Defender = Enemy;
      UnitStats attackerStats = Attacker.GetComponent<UnitStats>();
      UnitStats defenderStats = Defender.GetComponent<UnitStats>();
      attackerStats.damageCategoriesDealt[0] = UnitStats.damageCategory.Physical; //Change damage types between physical and elemental
      attackerStats.damageTypesDealt[0] = UnitStats.damageType.Slashing; //How to change the type of attack
      Fight();
    }
    if (Input.GetKeyDown(KeyCode.Alpha2))
    {
      Attacker = Enemy;
      Defender = Player;
      UnitStats attackerStats = Attacker.GetComponent<UnitStats>();
      UnitStats defenderStats = Defender.GetComponent<UnitStats>();
      attackerStats.damageCategoriesDealt[0] = UnitStats.damageCategory.Physical;
      attackerStats.damageTypesDealt[0] = UnitStats.damageType.Water;
      Fight();
    }
    if (Input.GetKeyDown(KeyCode.Alpha3))
    {
      Attacker = Player;
      Defender = Enemy;
      UnitStats attackerStats = Attacker.GetComponent<UnitStats>();
      UnitStats defenderStats = Defender.GetComponent<UnitStats>();
      attackerStats.damageCategoriesDealt[0] = UnitStats.damageCategory.Elemental;
      attackerStats.damageTypesDealt[0] = UnitStats.damageType.Fire;
      Fight();
    }
    if (Input.GetKeyDown(KeyCode.Alpha4))
    {
      Attacker = Enemy;
      Defender = Player;
      UnitStats attackerStats = Attacker.GetComponent<UnitStats>();
      UnitStats defenderStats = Defender.GetComponent<UnitStats>();
      attackerStats.damageCategoriesDealt[0] = UnitStats.damageCategory.Elemental;
      attackerStats.damageTypesDealt[0] = UnitStats.damageType.Piercing;
      Fight();
    }
  }

  public void Fight()
  {
    CombatFormulas.Damage(Attacker, Defender);
  }
}
*/
