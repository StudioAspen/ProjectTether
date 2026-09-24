using System;
using System.Collections.Generic;
using Consystently.Essentials;

//TODO: xp system
public class EnemyUnit : Unit
{
   
   public List<EnemyStateSO> behaviourStates { get; private set; }
   public event Action<EnemyUnit> OnDeath;
   public event Action<EnemyUnit> OnDefend;
   
   public EnemyUnit(EnemyUnitSO unit) : base(unit)
   {
      SetFaction(Faction.Enemy);
      behaviourStates = unit.BehaviourStates;
   }

   public override void ChangeHealthRemaining(int value)
   {
      HealthRemaining -= value; 
      if(HealthRemaining <= 0)
         OnDeath?.Invoke(this);
   }

   public override void Defend()
   {
      IsBlocking = true;
      OnDefend?.Invoke(this);
   }

   public override void EndDefend()
   {
      IsBlocking = false;
   }

   //can modify depending on difficulty desired 
   public override void ChangeEnergyRemaining(int value)
   {
      EnergyRemaining -= value;
   }
    
   
}
