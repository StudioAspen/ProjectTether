using System;
using System.Collections.Generic;
using _Scripts.Runtime.Characters.Classes.Scripts;
using _Scripts.Runtime.Combat;
using UnityEngine;

namespace _Scripts.Runtime.Entities.Scripts.Combat
{

    public abstract class Unit
    {
     //TODO: add buff feature (MAKE scriptable objects for buffs/debuffs) then store array in unit
     //TODO: remove energy (it is part of suit)
     
     //TODO: probably need something that lets you set the data with another unit class to pass data from ow for ally


        public Sprite Portrait { get; private set; }
        public string Name { get; set; }
        public Faction Faction { get; protected set; }
        public int Level { get; protected set; }
        public int XpToNextLevel { get; protected set; }
        public int TotalXp { get; protected set; }

        public Dictionary<Stat, Tier> StatGrowths { get; protected set; }
        public Dictionary<Element, Affinity> Affinities { get; protected set; }

        //statMs
        public int Health { get; protected set; }
        
        /*
        //TODO: remove energy from unit (moved to suit)
        public int Energy { get; protected set; }
        public int Strength { get; protected set; }
        public int Defense { get; protected set; }
        public int Tech { get; protected set; }
        public int Resistance { get; protected set; }
        public int Speed { get; protected set; }
        public int Luck { get; protected set; }
        public int Precision { get; protected set; }
        public int Evasion { get; protected set; }
        */

        public Dictionary<Stat, UnitStat> stats;

        public int HealthRemaining { get; protected set; }
        public int EnergyRemaining { get; protected set; }

        public bool IsBlocking { get; protected set; }
        public bool IsDead { get; protected set; }
        public List<AbilitySO> Moves { get; protected set; }

        //TODO: remove and change shit later 
        public CombatClassSO CombatClass { get; protected set; }
        
        public Dictionary<string, Status> Statuses { get; protected set; }
        
        public List<int> tempStats = new List<int>(); 
        public bool Stunned { get; protected set; }


//   public event Action HasDied;

        //level will draw from save
        protected Unit(UnitDataSO unit)
        {
            Portrait = unit.Portrait;
            Name = unit.Name;
            //Fill in faction in child
            Level = unit.Level;
            XpToNextLevel = unit.ExPtsToNextLevel;
            TotalXp = unit.TotalExPtsGained;

            StatGrowths = unit.Aptitudes;
            Affinities = unit.Affinities;

            //actual stat values 
            Health = Formulae.CalculateStat(Stat.HP, StatGrowths[Stat.HP], Level);
            
            //part of suit now
            //Energy = Formulae.CalculateStat(Stat.EN, StatGrowths[Stat.EN], Level);
            /*
            Strength = Formulae.CalculateStat(Stat.STR, StatGrowths[Stat.STR], Level);
            Defense = Formulae.CalculateStat(Stat.DEF, StatGrowths[Stat.DEF], Level);
            Tech = Formulae.CalculateStat(Stat.TEC, StatGrowths[Stat.TEC], Level);
            Resistance = Formulae.CalculateStat(Stat.RES, StatGrowths[Stat.RES], Level);
            Speed = Formulae.CalculateStat(Stat.SPE, StatGrowths[Stat.SPE], Level);
            Luck = Formulae.CalculateStat(Stat.LCK, StatGrowths[Stat.LCK], Level);
            Precision = Formulae.CalculateStat(Stat.PRC, StatGrowths[Stat.PRC], Level);
            */

            stats =  new Dictionary<Stat, UnitStat>();
            foreach (Stat stat in Enum.GetValues(typeof(Stat)))
            {
                //should crash if there are two same stats
                stats.Add(stat, new UnitStat(Formulae.CalculateStat(stat, StatGrowths[stat], Level)));
            }

            HealthRemaining = Health;
            //EnergyRemaining = Energy;

            Moves = unit.Moves;
            CombatClass = unit.CombatClass;
        }
        
        //placeholder

        protected void SetFaction(Faction value)
        {
            Faction = value;
        }

        /// <summary>
        /// stat change will only occur when the status is added 
        /// </summary>
        /// <param name="status"></param>
        public abstract void AddStatus(StatusSO status);

        /// <summary>
        /// triggers per turn. 
        /// </summary>
        public abstract void TriggerStatuses();

        public void RemoveStatus(StatusSO status)
        {
            Statuses.Remove(status.StatusId);
        }

        public abstract void ChangeHealthRemaining(int value);
        public abstract void ChangeEnergyRemaining(int value);

        public void Defend()
        {
            IsBlocking = true;
        }
        
        //abstract because defend will have additional player function later 
        public abstract void EndDefend();

        public Element[] DefaultAttackTypes()
        {
            return CombatClass.DefaultAttackElements;
        }

        public int DefaultAttackRange()
        {
            return CombatClass.DefaultAttackRange;
        }

        public int GetBaseStat(Stat stat)
        {
            return stats[stat].BaseStat;
        }
        
        public int GetEStat(Stat stat)
        {
            return stats[stat].EffectiveStat;
        }
        
        public void SetDead(bool value) { IsDead = value;}

        public void RevertEStat(Stat stat, float multiplier)
        {
            if (multiplier == 0)
            {
                stats[stat].SetEffectiveStat(0);
            }
            else
            {
                stats[stat].SetEffectiveStat((int)(stats[stat].EffectiveStat * 1.0f/multiplier));
            }
        }

        public void MultiplyEStat(Stat stat, float multiplier)
        {
            stats[stat].SetEffectiveStat((int)(stats[stat].EffectiveStat * multiplier));    
        }

        /// <summary>
        /// resets values that should refresh per turn. Should be called at the very start of the turn
        /// </summary>
        public void ResetValues()
        {
            Stunned = false;
            EndDefend();
        }

    }

}
