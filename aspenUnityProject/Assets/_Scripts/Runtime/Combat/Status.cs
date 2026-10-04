namespace _Scripts.Runtime.Combat
{
    //need this class to count down status durations 
    public class Status
    {
        public StatusSO StatusData;
        public int duration;
        
        public Status(StatusSO statusData)
        {
            StatusData = statusData;
            duration = statusData.Duration;
        }

        public void CountDown()
        {
            duration--;
        }
    }
}