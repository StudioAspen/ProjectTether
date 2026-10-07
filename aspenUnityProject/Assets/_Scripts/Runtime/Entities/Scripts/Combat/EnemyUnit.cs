using System;
using System.Collections.Generic;
using _Scripts.Runtime.Combat;
using _Scripts.Runtime.Combat.States.CombatManagerStates;

//TODO: xp system
namespace _Scripts.Runtime.Entities.Scripts.Combat
{
   public class EnemyUnit : Unit
   {
   
      //TODO: add skills here because enemies do not have classes
      public List<EnemyStateSO> behaviourStates { get; private set; }
      
   
      public EnemyUnit(EnemyUnitSO unit) : base(unit)
      {
         SetFaction(Faction.Enemy);
         behaviourStates = unit.BehaviourStates;
      }

      public override void ChangeHealthRemaining(int value)
      {
         HealthRemaining -= value;
         if (HealthRemaining <= 0)
            IsDead = true;
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
}
