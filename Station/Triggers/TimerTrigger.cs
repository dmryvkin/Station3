using System;
using System.Timers;

namespace Station.Triggers
{
    public class TimerTrigger : ITrigger
    {

        private Timer timer;



        private int minutesInterval = 1;

        private int secondsDelay = 0;

        private DateTime nextTriggerTime;


        public event TriggerEventHandler OnTrigger;

        public int Interval { get { return minutesInterval; }  set { minutesInterval = value;  } }

        public TimerTrigger(int minutes)
        {



            if (minutes < 1 || minutes > 360)
                throw new ArgumentOutOfRangeException("minutes", "Значение минут должно быть в пределах 1-360");


            minutesInterval = minutes;




            timer = new Timer(1000);

            timer.Elapsed += OnTimedEvent;
        }




        public void Start()
        {

            nextTriggerTime = getNextTime();

            timer.Enabled = true;


        }

        public void Stop()
        {
            timer.Enabled = false;
        }


        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {


            if (DateTime.Now > nextTriggerTime)
            {
                DateTime nextTime = getNextTime();
                OnTrigger?.Invoke(this, new TimerEvent(this, nextTime));
                nextTriggerTime = nextTime;

            }


        }

        public DateTime getNextTime()
        {

            DateTime currentTime = DateTime.Now;


            DateTime t = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 0, 0, 0);

            while (t <= currentTime)
            {
                t = t.AddMinutes(minutesInterval);

            }

            return t.AddSeconds(secondsDelay);


        }
    }
}