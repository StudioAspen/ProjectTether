using _Scripts.Runtime.Entities.Scripts.Combat;

namespace _Scripts.Runtime.Combat.Statuses
{
    //need this class to count down status durations 
    public class Status
    {
        public AbstractStatusSO StatusData { get; set; }
        public int Duration { get; private set; }
        public int TurnsPassed {get; private set;}
        
        public Status(AbstractStatusSO statusData)
        {
            StatusData = statusData;
            Duration = statusData.Duration;
        }

        public void CountDown(int val)
        {
            Duration-=val;
            TurnsPassed+=val;
        }

        public void CountUp(int val)
        {
            Duration+=val;
        }

        /// <summary>
        /// whatever occurs when a status is just added to a unit
        /// </summary>
        public void OnAdd(Unit uc)
        {
           StatusData.OnAdd(uc); 
        }

        public void OnStack(Unit uc)
        {
            CountUp(StatusData.Duration);
            if (StatusData.HasStackableEffects)
            {
                StatusData.Tick(uc);
            }
        }

        /// <summary>
        /// status effects that are repeated per turn 
        /// </summary>
        public void Tick(Unit uc)
        {
           if(StatusData.DurationLoweredOnDamage) 
               CountDown(1); 
           StatusData.Tick(uc);
           foreach (var t in StatusData.StatusTimeline)
           {
               if (TurnsPassed == t.turnToAdd)
                   uc.AddStatus(t.statusToAdd);
           }
        }

        /// <summary>
        /// whatever occurs when a status is healed/removed 
        /// </summary>
        public void OnRemove(Unit uc)
        {
            StatusData.OnRemove(uc);
        }

        public void Trigger(Unit uc)
        {
            StatusData.Trigger(uc);
        }

    }
}