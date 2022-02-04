namespace Station.Triggers
{
    public delegate void TriggerEventHandler(object sender, ITriggerEvent ev);

    public interface ITrigger
    {
        void Start();
        void Stop();

        event TriggerEventHandler OnTrigger;

    }
}

