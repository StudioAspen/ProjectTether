using _Scripts.Runtime.Entities.Scripts.Combat;

namespace _Scripts.Runtime.Combat.Statuses
{
    //need this class to count down status durations 
    public class Status
    {
        public StatusContext StatusData { get; set; }
        
        public Status(StatusSO statusData)
        {
            StatusData = new StatusContext()
            {
                StatusSOData = statusData,
                Duration = statusData.Duration
            };
        }

        public Status(StatusContext statusData)
        {
            StatusData = statusData;
        }

        public void CountDown(int val)
        {
            StatusData.Duration-=val;
            StatusData.TurnsPassed+=val;
        }

        public void CountUp(int val)
        {
            StatusData.Duration+=val;
        }

        /// <summary>
        /// whatever occurs when a status is just added to a unit
        /// </summary>
        public void OnAdd()
        {
           StatusData.StatusSOData.OnAdd(StatusData); 
        }

        public void OnStack()
        {
            CountUp(StatusData.Duration);
            if (StatusData.StatusSOData.HasStackableEffects)
            {
                StatusData.StatusSOData.OnAdd(StatusData);
            }
        }

        /// <summary>
        /// status effects that are repeated per turn 
        /// </summary>
        public void Tick()
        {
            StatusData.StatusSOData.Tick(StatusData);
           /*
           StatusData.StatusSOData.Tick(StatusData);
           foreach (var t in StatusData.StatusSOData.StatusTimeline)
           {
               if (StatusData.TurnsPassed == t.turnToAdd)
                   uc.GetData().AddStatus(t.statusToAdd);
           }
           */
        }

        /// <summary>
        /// whatever occurs when a status is healed/removed 
        /// </summary>
        public void OnRemove()
        {
            StatusData.StatusSOData.OnRemove(StatusData);
        }

    }
}