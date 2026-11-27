using System;
using _Scripts.Runtime.Entities.Scripts.Combat;
using UnityEngine;

namespace _Scripts.Runtime.Combat.Mechanics
{
    public class TurnQueueItem : IComparable<TurnQueueItem>
    {
        public UnitController CurrentUnit { get; private set; }
        public float Initiative { get; private set; }
        
        /// <summary>
        /// unique identifier for the set 
        /// </summary>
        public int ID {get; private set;}
        public int Turn {  get; private set; }

        public TurnQueueItem(int turn, int ID, UnitController unitController)
        {
            CurrentUnit = unitController;
            Turn = turn;
            this.ID = ID;
            //BASED off ryan's new formula turn/speed, where lower is higher prio 
            CalcInitiative();
        }

        public int CompareTo(TurnQueueItem other)
        {
            if (other is null) return 1;
            return (Initiative, CurrentUnit, ID).CompareTo((other.Initiative, other.CurrentUnit, other.ID));
        }

        private void CalcInitiative()
        {
           Initiative = Turn*1.0f/CurrentUnit.GetData().Stats[Stat.SPE].EffectiveStat;
        }

        public Unit GetUnitData()
        {
            return CurrentUnit.GetData();
        }
        
    }
}