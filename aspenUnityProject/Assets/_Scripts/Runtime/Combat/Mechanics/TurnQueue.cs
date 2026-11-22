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
        /// +=1 every time all units have their turns calculated 
        /// </summary>
        public int TurnCycle { get; private set; }

        /// <summary>
        /// basic index for ID
        /// </summary>
        private int totalTurns; 

       public static event Action<SortedSet<TurnQueueItem>> OnShuffled;

        
        public TurnQueue(List<UnitController> units)
        {
            this.units = units;
            TurnCycle = 1;
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
                   TurnQueueItem tqi = new  TurnQueueItem(TurnCycle, totalTurns, unitController);
                   _unitTurns.Add(tqi); 
                   totalTurns++;
                   if (_unitTurns.Count >= NUM_TURNS_PRECALCULATED) 
                       break;
               }
               TurnCycle++;
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
           _unitTurns.Remove(_unitTurns.Min);
           SetNextTurns();
       }
       
       
        


    }
}