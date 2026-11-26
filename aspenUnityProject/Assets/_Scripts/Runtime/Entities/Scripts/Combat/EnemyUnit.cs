using System;
using System.Collections.Generic;
using _Scripts.Runtime.Combat;
using _Scripts.Runtime.Combat.States.CombatManagerStates;
using _Scripts.Runtime.Combat.Statuses;

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


      public override void EndDefend()
      {
         IsBlocking = false;
      }

      public override void AddStatus(StatusContext statusContext)
      {
         StatusSO status = statusContext.StatusSOData;
         if(Statuses.TryGetValue(status.StatusId, out Status value))
         {
            value.OnStack();
         }else {
            Status newStatus = new Status(status);
            Statuses.Add(status.StatusId, newStatus);
            newStatus.OnAdd();
         } 
      }

      //probably no longer needs to override. Can maybe move code to abstract class
      public override void TriggerStatuses()
      {
         List<string> toDelete = new List<string>();                                          
         foreach(var status in Statuses.Values)
         {
            if (status.StatusData.Duration == 0)
            {
               toDelete.Add(status.StatusData.StatusSOData.StatusId);                                    
               status.OnRemove();
               continue;
            }                                                                                
            status.Tick();
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
