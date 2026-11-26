using System;
using System.Collections.Generic;
using _Scripts.Runtime.Entities.Scripts.Combat;
using _Scripts.Runtime.Utilities;
using Unity.VisualScripting;
using UnityEngine;


namespace _Scripts.Runtime.Combat.Mechanics
{
    public class TurnQueue
    {

        
        private const int NUM_TURNS_PRECALCULATED = 15;

        /// <summary>
        /// TurnQueueItem -> Initiative 
        /// </summary>
        private SortedSet<TurnQueueItem> _unitTurns = new SortedSet<TurnQueueItem>();

        private List<UnitController> units;


        /// <summary>
        /// basic index for ID
        /// </summary>
        private int totalTurns;
        private int unitCycle = 0;

       public static event Action<SortedSet<TurnQueueItem>> OnShuffled;

        
        public TurnQueue(List<UnitController> units)
        {
            this.units = units;
            SetNextTurns();
        }

       /// <summary>
       /// Best explained by the image attached to initiative system in the gdd.
       /// </summary>
       private void SetNextTurns()
       {
           //Display currently becomes broken eventually, but the display can be fixed by adding a foreach loop and adding each unit in an unbounded set. This is bad for long fights, I think.
           while(_unitTurns.Count < NUM_TURNS_PRECALCULATED)
           {
               if (!units[unitCycle].GetData().IsDead)
               {
                   TurnQueueItem tqi = new TurnQueueItem(units[unitCycle].GetData().Turns, totalTurns, units[unitCycle]);
                   _unitTurns.Add(tqi);
                   totalTurns++;
                   units[unitCycle].GetData().Turns++;
               }
               unitCycle=(unitCycle+1)%units.Count;
           }

           OnShuffled?.Invoke(_unitTurns);

       }

       private void SetNextTurn()
       {
           bool allUnitsDead = false;
           while(!allUnitsDead)
           {
               for (int i = 0; i < units.Count; i++, unitCycle = (unitCycle + i) % units.Count)
               {
                   if (!units[unitCycle].GetData().IsDead || true)
                   {
                       TurnQueueItem tqi = new TurnQueueItem(units[unitCycle].GetData().Turns, totalTurns, units[unitCycle]);
                       _unitTurns.Add(tqi);
                       totalTurns++;
                       units[unitCycle].GetData().Turns++;
                       unitCycle=(unitCycle+i+1)%units.Count;
                       /*
                       foreach (var unc in _unitTurns)
                       {
                           Debug.Log($" {unc.Initiative} {unc.GetUnitData().Name}");
                       }
                       */
                       OnShuffled?.Invoke(_unitTurns);
                       return;
                   }
                       
               }
               allUnitsDead = true;
           }
       }

       /// <summary>
       /// gets highest prio unit
       /// </summary>
       /// <returns></returns>
       public TurnQueueItem GetCurrentTurn()
       {
           return _unitTurns.Min;
       } 
       
       /// <summary>
       /// dequeues a unit. Should be called when exiting a turn
       /// </summary>
       public void Consume()
       {
           if (_unitTurns.Count > 0)
           {
               _unitTurns.Remove(_unitTurns.Min);
               SetNextTurn();
           }
       }


       public void RemoveUnit(UnitController unitController)
       {
           _unitTurns.RemoveWhere(u => u.CurrentUnit == unitController);
       }
        


    }
}