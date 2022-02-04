using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WQMSDevice
{
    public class DeviceControl
    {

        private int _controlRegister;

        EasyModbus.ModbusClient _modbusClient;

        public DeviceControl(int controlRegister, string ipAddress)
        {
            _controlRegister = controlRegister;

            _modbusClient = new EasyModbus.ModbusClient();

            _modbusClient.IPAddress = ipAddress;

        }


        public void StartAnalisys()
        {
            try
            {
                _modbusClient.Connect();

            }
            catch(Exception e) 
            {

                throw new Exception("Ошибка подключения Modbus");

            }

            _modbusClient.WriteSingleRegister(_controlRegister, 1);

            _modbusClient.Disconnect();



        }

    }
}
