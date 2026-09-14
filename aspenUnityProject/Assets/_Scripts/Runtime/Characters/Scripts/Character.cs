using UnityEngine;

[RequireComponent(typeof(ExperienceSystem), typeof(CombatClassSystem))]
public class Character : Entity
{
  protected ExperienceSystem _experienceSystem;
  protected CombatClassSystem _combatClassSystem;

  protected override void Awake()
  {
    base.Awake();
    
    _experienceSystem ??= GetComponent<ExperienceSystem>();
    _combatClassSystem ??= GetComponent<CombatClassSystem>();

    _experienceSystem.Intialize(UnitData);
    _combatClassSystem.Intialize(UnitData);
  }

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
      
  }

  // Update is called once per frame
  void Update()
  {
      
  }
}
