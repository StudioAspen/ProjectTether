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

      public override void AddStatus(StatusSO status)
      {
         if(Statuses.TryGetValue(status.StatusId, out Status value))
         {
            value.CountUp(status.Duration);
            if (status.HasStackableEffects)
            {
               foreach(StatMultiplier sm in status.GenericStatsModified)     
                  MultiplyEStat(sm.stat,sm.multiplier);              
            }
         }else
         {
            Statuses.Add(status.StatusId, new Status(status));   
            foreach(StatMultiplier sm in status.GenericStatsModified)     
               MultiplyEStat(sm.stat,sm.multiplier);
         } 
      }

      public override void TriggerStatuses()
      {
         List<string> toDelete = new List<string>();                                          
         foreach(var status in Statuses.Values)                                               
         {                                                                                    
            HealthRemaining -= status.StatusData.DamageTakenPerTurn;                         
            if (status.StatusData.IsStun)                                                    
               Stunned = true;                                                              
            status.CountDown(1);                                                             
            if (status.Duration == 0)
            {
               toDelete.Add(status.StatusData.StatusId);                                    
               foreach (StatMultiplier sm in status.StatusData.GenericStatsModified)
                  RevertEStat(sm.stat,sm.multiplier);
            }                                                                                
                                                                                          
         }                                                                                    
         foreach (var delete in toDelete)                                                     
            Statuses.Remove(delete);                                                          
      }

      //can modify depending on difficulty desired 
      public override void ChangeEnergyRemaining(int value)
      {
         EnergyRemaining -= value;
      }
    
   
   }
}
