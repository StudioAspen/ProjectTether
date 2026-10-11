using System;
using System.Collections.Generic;
using _Scripts.Runtime.Characters.Classes.Scripts;
using _Scripts.Runtime.Characters.Scripts;
using _Scripts.Runtime.Combat;
using Unity.VisualScripting;

namespace _Scripts.Runtime.Entities.Scripts.Combat
{
    public class AllyUnit : Unit
    {
        public event Action<AllyUnit> HasLeveled;


        public List<SuitData> Suits { get; private set; } = new List<SuitData>();
        public OverclockTiers OverclockLevel { get; private set; }

        public SuitData CurrentSuit { get; private set; }
        public Dictionary<CombatClassType, SuitData> SuitDictionary { get; private set; }

        public bool HasOverclocked { get; private set; } 
        public int OverclockStack { get; private set; }
        

        //private CombatClass combatClass; 
        public AllyUnit(UnitDataSO unit) : base(unit)
        {
            SetFaction(Faction.Ally);
            OverclockLevel = OverclockTiers.Off;
            foreach (SuitBaseDataSO sb in ((PlayableCharacterUnitSO)unit).Suits)
            {
               SuitData suit = new SuitData(sb); 
               Suits.Add(suit); 
               
               //should throw exception/crash because there should never be two suits of the same combat class
               SuitDictionary.Add(suit.GetClassType(),suit);
            }
            // waiting for pod combat dynamics 
            // CurrentSuit = SuitDictionary[(CombatClassType)((PlayableCharacterUnitSO)unit).StartingSuit.ClassType];
            // OverclockLevel = SetOverclock(CurrentSuit.OverclockLevel);
            
        }
    
        //probably do damage formula later either here or in a diff class 
        public override void ChangeHealthRemaining(int value)
        {
            HealthRemaining -= value;
            if (HealthRemaining <= 0)
                IsDead = true;
        }

        public override void ChangeEnergyRemaining(int value)
        {
            EnergyRemaining -= value;
        }

        /*
        public void ChangeXp(int value)
        { 
            XpToNextLevel -= value;
            if (XpToNextLevel <= 0)
            {
                TotalXp += value;
                LevelUp(); 
                HasLeveled?.Invoke(this);
            }

        }
        */


        public override void EndDefend()
        {
            IsBlocking = false;
            //new event action here for end defend animation
        }

        public override void AddStatus(StatusSO status)
        {
            if(Statuses.TryGetValue(status.StatusId, out Status value))
            {
                value.CountUp(status.Duration);
            }else
            {
                Statuses.Add(status.StatusId, new Status(status));
                foreach (StatMultiplier sm in status.GetStatMultiplier(CurrentSuit.GetClassType()))
                    MultiplyEStat(sm.stat, sm.multiplier);            
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
                    foreach (StatMultiplier sm in status.StatusData.GetStatMultiplier(CurrentSuit.GetClassType()))
                       RevertEStat(sm.stat,sm.multiplier); 
                }                                                                                
            }                                                                                    
            foreach (var delete in toDelete)                                                     
                Statuses.Remove(delete);                                                         
        }

        private void LevelUp()
        {
            Level++;
            //add stat changing algorithm from a new pure c# static ExperienceSystem class  
        }

        public void ChangeClass(CombatClassType classType)
        {
            CurrentSuit = SuitDictionary[classType];
        }

        private void SetOverclock(OverclockTiers overclockLevel)
        {
            OverclockLevel = overclockLevel;
            OverclockStack = (int)overclockLevel;
            if(OverclockLevel != OverclockTiers.Off)
                HasOverclocked = true;
        }

        public void Overclock()
        {
            if(OverclockLevel != OverclockTiers.Extreme)
                OverclockLevel = (OverclockTiers)((int)OverclockLevel + 1);
            HasOverclocked = true;
        }

        public void Underclock()
        {
            if (OverclockLevel != OverclockTiers.Off)
                OverclockLevel = (OverclockTiers)((int)OverclockLevel - 1);
        }

        public bool CheckOverclock()
        {
            if (OverclockLevel != OverclockTiers.Off)
                HasOverclocked = true;
            return HasOverclocked;
        }

    

    }
}