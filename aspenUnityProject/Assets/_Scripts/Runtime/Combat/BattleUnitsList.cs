using System.Collections.Generic;
using _Scripts.Runtime.Entities.Scripts.Combat;
using UnityEngine;

namespace _Scripts.Runtime.Combat
{
    /// <summary>
    /// class for info relevant to collection/set of units active in combat 
    /// </summary>
    public class BattleUnitsList
    {
        public List<UnitController> AllUnits { get; private set; } = new List<UnitController>();
        public List<AllyUnitController> Allies { get; private set; } = new List<AllyUnitController>();
        public List<EnemyUnitController> Enemies { get; private set; } = new List<EnemyUnitController>();

        HashSet<UnitController> DeadUnits = new HashSet<UnitController>();
        public int DeadAllies { get; private set; }
        public int DeadEnemies { get; private set; }
        

        /// <summary>
        /// adds unit to respective lists
        /// </summary>
        /// <param name="unit"></param>
        public void AddUnit(UnitController unit)
        {
            Unit unitData = unit.GetData();
            AllUnits.Add(unit);
            if (unitData.Faction == Faction.Ally)
            {
                Allies.Add((AllyUnitController)unit);
            }
            else if (unitData.Faction == Faction.Enemy)
            {
                Enemies.Add((EnemyUnitController)unit);               
            }
        }

        /// <summary>
        /// adds unit to corresponding lists and updates relevant info 
        /// </summary>
        /// <param name="unit"></param>
        public void KillUnit(UnitController unit)
        {
            Unit unitData = unit.GetData();
            if (unitData.Faction == Faction.Ally)
            {
                DeadAllies++;
            }
            else
            {
               DeadEnemies++; 
            }
            DeadUnits.Add(unit);
        }
    }
}