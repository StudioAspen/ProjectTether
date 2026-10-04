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
    
        public event Action<AllyUnit> OnDeath;
        public event Action<AllyUnit> OnDefend;

        private List<SuitData> _suits;
    
        //private CombatClass combatClass; 
        public AllyUnit(UnitDataSO unit) : base(unit)
        {
            SetFaction(Faction.Ally);
            foreach (SuitBaseDataSO sb in ((PlayableCharacterUnitSO)unit).Suits)
            {
               _suits.Add(new SuitData(sb)); 
            }
        }
    
        //probably do damage formula later either here or in a diff class 
        public override void ChangeHealthRemaining(int value)
        {
            HealthRemaining -= value; 
            if(HealthRemaining <= 0)
                OnDeath?.Invoke(this);
        }

        public override void ChangeEnergyRemaining(int value)
        {
            EnergyRemaining -= value;
        }

        //TODO:  
        //make it so xp adds to next level when it overflows  
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

        public override void Defend()
        {
            IsBlocking = true;
            OnDefend?.Invoke(this);
        }

        public override void EndDefend()
        {
            IsBlocking = false;
            //new event action here for end defend animation
        }

        //TODO: 
        //redesign current xp system into pure c# class before finishing this method
        //leveling up will modify every stat, and stat gain will be dependent on tier
        private void LevelUp()
        {
            Level++;
            //add stat changing algorithm from a new pure c# static ExperienceSystem class  
        }

        //TODO:
        //Implement class system before doing this 
        public void ChangeClass(CombatClassType classType)
        {
        
        }
    

    }
}