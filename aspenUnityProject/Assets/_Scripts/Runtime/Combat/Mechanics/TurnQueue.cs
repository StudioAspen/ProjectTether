using System;
using System.Collections.Generic;
using _Scripts.Runtime.Entities.Scripts.Combat;
using _Scripts.Runtime.Utilities;
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

       public static event Action<SortedSet<TurnQueueItem>> OnShuffled;

        
        public TurnQueue(List<UnitController> units)
        {
            this.units = units;
            SetNextTurns();
        }

       /// <summary>
       /// Best explained by the image attached to initiative system in the gdd
       /// </summary>
       private void SetNextTurns()
       {
           while (_unitTurns.Count < NUM_TURNS_PRECALCULATED)
           {
               foreach (UnitController unitController in units)
               {
                   TurnQueueItem tqi = new  TurnQueueItem(unitController.GetData().Turns, totalTurns, unitController);
                   Debug.Log(units[2].GetData().Name);
                   if (!unitController.GetData().IsDead)
                   {
                       _unitTurns.Add(tqi);
                       totalTurns++;
                       unitController.GetData().Turns++;
                   }
               }
           }
           OnShuffled?.Invoke(_unitTurns);

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
               SetNextTurns();
           }

       }


       public void RemoveUnit(UnitController unitController)
       {
           _unitTurns.RemoveWhere(u => u.CurrentUnit == unitController);
       }
        


    }
}