using System;
using System.Collections.Generic;
using _Scripts.Runtime.Entities.Scripts.Combat;
using Unity.VisualScripting;
using UnityEngine;
using Unit = Unity.VisualScripting.Unit;

namespace _Scripts.Runtime.Combat.Mechanics
{
    public class TurnQueue
    {
        private List<UnitController> _units { get; set; } = new List<UnitController>();

        /// <summary>
        /// total speed of all units. Averaged speed of all units will be the denominator
        /// </summary>
        private int _speedAll;

        private float denominator;

        private int _totalUnits; 

        private List<UnitController> _cappedUnits = new List<UnitController>();

        /// <summary>
        /// tries to prevent one unit from going 20+ times. Resets when everyone has taken a turn
        /// </summary>
        private HashSet<UnitController> _agingSet =  new HashSet<UnitController>();
        private int _allyBurstCounter;
        private int _enemyBurstCounter;
        
        public static event Action<List<UnitController>> OnShuffled;
        
        public TurnQueue() {}

        public TurnQueue(List<UnitController> units)
        {
            foreach (UnitController unit in units)
            {
                Add(unit);
            }
            denominator = _speedAll*1.0f / _totalUnits;
        }

        public void Add(UnitController unit)
        {
            _units.Add(unit);
            _totalUnits++;
            _speedAll+=unit.GetData().Stats[Stat.SPE].EffectiveStat;
        }

        /// <summary>
        /// goes through each unit in the list and adds to their initiative value
        /// </summary>
        public bool UpdateInitiatives()
        {
            if (_cappedUnits.Count > 0)
            {
               return true;
            }

            if(_agingSet.Count == _cappedUnits.Count)
                ResetInitMod(1.0f);
            for (int i = 0; i < _units.Count; i++)
            {
                if (_units[i].GetData().IsDead)
                    continue;
                CalcInitiative(_units[i].GetData());
            }
            //ally should be before enemy due to enum value being higher
            
            _units.Sort((a, b) => b.CompareTo(a));
            for(int i = 0; i < _units.Count; i++)
            {
                if (_units[i].GetData().Initiative >= 100)
                    _cappedUnits.Add(_units[i]);
                else
                    break;
            }
            OnShuffled?.Invoke(_units);
            return false;
        }

        /// <summary>
        /// Gets unit with max init.
        /// combatManager will currently only use the first element for a tie
        /// </summary>
        /// <returns></returns>
        public List<UnitController> GetCappedUnits()
        {
            return _cappedUnits;
        }

        /// <summary>
        /// removes unit from _cappedUnits and sets their initiative to zero or higher if they defended. Used by CombatManager
        /// after it selects a unit from a tie 
        /// </summary>
        /// <returns></returns>
        public void ConsumeUnit(UnitController unit)
        {
            if (unit.GetData().Faction == Faction.Ally)
                _allyBurstCounter++;
            else
                _enemyBurstCounter++;
            if ( _allyBurstCounter > 2 || _enemyBurstCounter > 2)
            {
                if (unit.GetData().Faction == Faction.Ally)
                    _allyBurstCounter = 0;
                if (unit.GetData().Faction == Faction.Enemy)
                    _enemyBurstCounter = 0;
                return;
            }

            if (_cappedUnits.Remove(unit))
            {
                if(!_agingSet.Add(unit))
                    unit.GetData().DecInitMod();
                if (unit.GetData().IsBlocking) 
                    unit.GetData().SetInitMod(unit.GetData().InitModifier+0.01f); //temp value/formula
                else
                    unit.GetData().SetInit(0);
            }
        }


        /// <summary>
        /// improvised formula for initiative. Actual calculations/formula not described in the gdd.
        /// </summary>
        /// <param name="u"></param>
        /// <param name="initialInit"></param>
        /// <returns></returns>
        private void CalcInitiative(Entities.Scripts.Combat.Unit u)
        {
            //current formula is terrible because a player will go 2x before an enemy even with a minor speed advantage
            int result = (int)((1.0f * u.Stats[Stat.SPE].EffectiveStat * u.InitModifier + u.Initiative) / (_speedAll/(_totalUnits*1.0f)) * 100.0f);
            int clamp = System.Math.Clamp(result, 0, 100);
            u.SetInit(clamp);
        }

        private void ResetInitMod(float baseline)
        {
            _agingSet.Clear();
            for(int i = 0; i < _units.Count; i++)
            {
               _units[i].GetData().SetInitMod(baseline); 
            }
        }

        public void Clear()
        {
            _units.Clear();
        }

    }
}