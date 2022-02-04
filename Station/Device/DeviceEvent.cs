using System.Collections.Generic;

namespace Station.Device
{

    public delegate void DeviceEventHandler(IDevice sender, DeviceEvent ev);

    public class DeviceEvent
    {
        private int _position;

        private List<Analysis> _data;
             



        public DeviceEvent() {
            _data = new List<Analysis>();

        }

        public void AddData(double? v, ChannelStatus status) {

            _data.Add(new Analysis() { Value = v, Status = status });
        }



        public int StartPosition { get { return _position;  } set { _position=value; } }

        public List<Analysis> Data { get { return _data; }  }

        
        public int Count { get; set; }

    }
}
