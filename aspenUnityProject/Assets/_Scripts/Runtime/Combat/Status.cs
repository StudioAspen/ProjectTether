namespace _Scripts.Runtime.Combat
{
    //need this class to count down status durations 
    public class Status
    {
        public StatusSO StatusData { get; set; }
        public int Duration { get; private set; }
        
        public Status(StatusSO statusData)
        {
            StatusData = statusData;
            Duration = statusData.Duration;
        }

        public void CountDown(int val)
        {
            Duration-=val;
        }

        public void CountUp(int val)
        {
            Duration+=val;
        }
    }
}