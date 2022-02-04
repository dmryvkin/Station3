namespace Station.Device
{
    public interface IDevice
    {
        string DeviceName { get; set; }

        void AddParameter(string parameterName, object value);

        void Init();

        void Shutdown();
        
        void StartMeasurement();

        event DeviceEventHandler OnUpdate;
               

    }
}
