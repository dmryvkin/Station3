using EasyModbus;
using Station.Device;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WQMSDevice
{
    [Serializable()]
    public class WQMSDevice : IDevice
    {
        
        private string _name;

        private int     _pollingInterval;               // Период опроса устройства 
        private string  _ipAddress;                     // IP адрес устройства 
        private int     _analysisBlockRegister;         //Начальный регистр блока данных измерений
        private int     _analysisBlockQuantity;         //Количество параметров в блоке данных измерений
        private int     _telemetryFlagsRegister;        //Начальный регистр блока флагов телеметрии измерительной системы
        private int     _telemetryFlagsQuantity;        //количество параметров в блоке флагов телеметрии измерительной системы
        private int     _telemetryParametersRegister;   //Начальный регистр блока параметров телеметрии измерительной системы
        private int     _telemetryParametersQuantity;   //количество параметров в блоке флагов телеметрии измерительной системы
        private int     _supportFlagsRegister;          //Начальный регистр блока флагов системы жизнеобеспечения
        private int     _supportFlagsQuantity;          //количество параметров в блоке флагов 
        private int     _supportParametersRegister;     //Начальный регистр блока параметров телеметрии системы жизнеобеспечения
        private int     _supportParametersQuantity;     //количество параметров в блоке флагов телеметрии системы жизнеобеспечения
        private int     _controlRegister;               //Первый регистр управления                     
       

        private List<IDataBlock> _dataBlocks; 

        private Hashtable _parameters = new Hashtable();

        private DeviceControl _deviceControl;

       
        private bool _stop = false;

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

        [DeviceParameter]
        public int PollingInterval
        {
            get
            {
                return _pollingInterval;
            }

            set
            {
                _pollingInterval = value;
            }
        }

        [DeviceParameter]
        public string IpAddress
        {
            get
            {
                return _ipAddress;
            }

            set
            {
                _ipAddress = value;
            }
        }

        [DeviceParameter]
        public int AnalysisBlockRegister
        {
            get
            {
                return _analysisBlockRegister;
            }

            set
            {
                _analysisBlockRegister = value;
            }
        }

        [DeviceParameter]
        public int AnalysisBlockQuantity
        {
            get
            {
                return _analysisBlockQuantity;
            }

            set
            {
                _analysisBlockQuantity = value;
            }
        }

        [DeviceParameter]
        public int ControlRegister
        {
            get
            {
                return _controlRegister;
            }

            set
            {
                _controlRegister = value;
            }
        }

        public event DeviceEventHandler OnUpdate;

        public void AddParameter(string parameterName, object value)
        {
            _parameters.Add(parameterName,value);
        }

        public void Init()
        {
            InitParameters();

            InitDataBlocks();

            InitControl();

            RunPolling();

        }

        private void InitControl()
        {
            _deviceControl = new DeviceControl(ControlRegister, IpAddress);
        }

        private void InitDataBlocks()
        {
            _dataBlocks = new List<IDataBlock>();


            var analysisDataBlock = new AnalysisDataBlock(_analysisBlockRegister, _analysisBlockQuantity);
            analysisDataBlock.IpAddress = _ipAddress;

            _dataBlocks.Add(analysisDataBlock);


        }

        private void RunPolling()
        {
            Task.Run(() =>
            {
                while (!_stop)
                {

                    foreach (IDataBlock block in _dataBlocks)
                    {
                        block.Read();
                    }


                    InvokeDataUpdate();


                    Thread.Sleep(_pollingInterval * 1000);

                }

            });
        }

        private void InvokeDataUpdate()
        {

            DeviceEvent ev = new DeviceEvent();

            ev.StartPosition = 0;

            foreach (IDataBlock block in _dataBlocks)
                AddBlock(ev, block);

            OnUpdate?.Invoke(this, ev);

        }

        private void AddBlock(DeviceEvent ev, IDataBlock block) {

            ev.Count += block.Data.Length;

            for (int i = 0; i < block.Data.Length; i++)
                ev.AddData(block.Data[i].Value, block.Data[i].Status);

        }


        private void InitParameters()
        {
            foreach (PropertyInfo property in this.GetType().GetProperties()){

                Attribute attr = property.GetCustomAttribute(typeof(DeviceParameterAttribute));

                if (attr != null)
                {

                    if (!_parameters.Contains(property.Name))
                            throw new Exception($"Параметр {property.Name} не найден!");

                    if (property.PropertyType == typeof(string)) {

                        property.SetMethod.Invoke(this, new object[] { _parameters[property.Name] });
                    }

                    if (property.PropertyType == typeof(int))
                    {
                        property.SetMethod.Invoke(this, new object[] { Convert.ToInt32(_parameters[property.Name]) });
                    }


                }
            }


           
        }

        public void Shutdown()
        {
            
        }

        public void StartMeasurement()
        {
            _deviceControl.StartAnalisys();
        }
    }

    internal class DeviceParameterAttribute : Attribute
    {
    }
}
