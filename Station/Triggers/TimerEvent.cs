using System;

namespace Station.Triggers
{
    public class TimerEvent : ITriggerEvent
    {
        private DateTime nextTime;

        public TimerEvent(ITrigger source, DateTime nextTime)
        {
            this.Source = source;

            this.nextTime = nextTime;

        }


        public DateTime NextTime
        {
            get
            {
                return nextTime;
            }

            set
            {
                nextTime = value;
            }
        }

        public ITrigger Source
        {
            get;


            set;

        }
    }
}
