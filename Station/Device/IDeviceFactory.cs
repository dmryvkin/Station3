using System.Collections.Generic;

namespace Station.Device
{
    public interface IDeviceFactory
    {

        IEnumerable<IDevice> GetDevices(); 

    }
}
