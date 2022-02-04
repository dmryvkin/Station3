using Station.Device;
using Station.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmulationDevice
{
    [Serializable()]
    public class Emulation : IDevice
    {
        private const int TIMEOUT = 5;
        private string _name;

        private Hashtable _parameters = new Hashtable();

        private double _meanValue = 1.0;

        private double _delta = 0.1;

        private int _delay = 60;

        private bool _stop = false;

        private bool _started = false;

       

        private Random _random = new Random();

        public string DeviceName
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

     
        public event DeviceEventHandler OnUpdate;

        public void AddParameter(string parameterName, object value)
        {
            _parameters.Add(parameterName, value);
        }

        public void Init()
        {


            try
            {

                InitParameters();


            }
            catch(Exception e){

             
                throw e;
            }



           
             InvokeDataUpdate(0, null, ChannelStatus.WAITING_FOR_REPLY);
            
           
        }

        private void InitParameters()
        {
            try
            {

                if (_parameters.ContainsKey("Mean"))
                    _meanValue = Convert.ToDouble(_parameters["Mean"]);

                if (_parameters.ContainsKey("Delta"))
                    _delta = Convert.ToDouble(_parameters["Delta"]);

                if (_parameters.ContainsKey("Delay"))
                    _delay = Convert.ToInt32(_parameters["Delay"]);

            }

            catch (Exception e)
            {
                throw new Exception($"Ошибка загрузки параметров устройства {this.DeviceName}",e);

            }


        }



        private void  InvokeDataUpdate(int pos, double? value, ChannelStatus status) {

            DeviceEvent ev = new DeviceEvent();

            ev.StartPosition =pos;
            ev.Count = 1;
            ev.AddData(value, status);

             OnUpdate?.Invoke(this,ev);

        }


        public void Shutdown()
        {
            _stop = true;
        }

        public void StartMeasurement()
        {

            Thread.Sleep(1000);
            


            if (_delay == 0) {

                if (!_started)

                    DoEmulationLoop();

                _started = true;
            }
            else
            {

                InvokeDataUpdate(0, null, ChannelStatus.ANALISYS_IN_PROGRESS);

                Thread.Sleep(_delay * 1000);


                InvokeDataUpdate(0, SimulatedValue(), ChannelStatus.ANALISYS_IS_DONE);
            }

        }

        private void DoEmulationLoop()
        {
            Task.Run(() =>
            {
                while (!_stop)
                {

                    InvokeDataUpdate(0, SimulatedValue(), ChannelStatus.ANALISYS_IS_DONE);

                    Thread.Sleep(TIMEOUT * 1000);

                }

            });
        }

        private double SimulatedValue()
        {
            return NextGaussian();

        }


        private double NextGaussian( )
        {
            var u1 = _random.NextDouble();
            var u2 = _random.NextDouble();

            var rand_std_normal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                Math.Sin(2.0 * Math.PI * u2);

            var rand_normal = _meanValue + _delta * rand_std_normal;

            return rand_normal;
        }



    }
}
