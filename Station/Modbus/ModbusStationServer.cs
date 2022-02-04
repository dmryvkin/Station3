using Station.Config;
using Station.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyModbus;

namespace Station.Modbus
{
    public class ModbusStationServer : IDataWriter
    {
        private EasyModbus.ModbusServer modbusServer;

        public const int PERIOD_REGISTER  = 2;
        public const int ID_REGISTER = 1;
        public const int CHANNELS_COUNT_REGISTER = 3;


        public bool IsBusy
        {
            get
            {
                return false;
            }
        }

        public ModbusServer ModbusServer
        {
            get
            {
                return modbusServer;
            }
                        
        }

        public ModbusStationServer() {


            modbusServer = new EasyModbus.ModbusServer();

            modbusServer.Listen();
        }

        public void WriteConfigData(StationConfiguration config) {


            modbusServer.holdingRegisters[1] = (short)config.ID;
            modbusServer.holdingRegisters[2] = (short)config.Period;

        }

        public void WriteChannelData(IChannel channel)
        {
            int regIndex = (int) (10 + channel.Id * 8);
            int coilIndex = (int)(channel.Id);

            if (regIndex < UInt16.MaxValue - 8)
            {
                modbusServer.coils[coilIndex] = channel.IsActive;

                modbusServer.holdingRegisters[regIndex] = Convert.ToInt16(channel.IsActive);
                modbusServer.holdingRegisters[regIndex + 1] = 0;
                modbusServer.holdingRegisters[regIndex + 2] = HighRegister(channel.CurrentValue);
                modbusServer.holdingRegisters[regIndex + 3] = LowRegister(channel.CurrentValue);
                modbusServer.holdingRegisters[regIndex + 4] = HighRegister(channel.TargetValue);
                modbusServer.holdingRegisters[regIndex + 5] = LowRegister(channel.TargetValue);
                modbusServer.holdingRegisters[regIndex + 6] = HighRegister(channel.Status.Code);
                modbusServer.holdingRegisters[regIndex + 7] = LowRegister(channel.Status.Code);

            }

        }


        private short LowRegister(double? floatValue) {

            short res = 0;

            if (floatValue != null)
            {

                byte[] valueBytes = BitConverter.GetBytes((float)floatValue);

                res = BitConverter.ToInt16(valueBytes, 0);
            }


            return res;

        }

        private short HighRegister(double? floatValue)
        {
            short res = 0;

            if (floatValue != null)
            {

                byte[] valueBytes = BitConverter.GetBytes((float)floatValue);
                res = BitConverter.ToInt16(valueBytes, 2);

            }

            return res;

        }

        private short LowRegister(string stringValue)
        {
           short res;
          
           byte[] valueBytes = Encoding.ASCII.GetBytes(stringValue);

           res =  (short) (valueBytes[0] + valueBytes[1] << 8);

           return res;    

        }

        private short HighRegister(string stringValue)
        {
            short res = 0;

            byte[] valueBytes = Encoding.ASCII.GetBytes(stringValue);

            res = (short)(valueBytes[2]);

            return res;

        }

        public void WriteData(int id, int period, DateTime time, bool emergency, IEnumerable<IChannel> channels)
        {
            modbusServer.holdingRegisters[ID_REGISTER] = (short)id;
            modbusServer.holdingRegisters[PERIOD_REGISTER] = (short)period;
            modbusServer.holdingRegisters[CHANNELS_COUNT_REGISTER] = (short)channels.Count();



            foreach (IChannel channel in channels)
            {

                WriteChannelData(channel);

            }

        }

        public void AddParameter(string v1, object v2)
        {
          
        }

        public void OpenWriter(int stationID)
        {
           
        }

        public void CloseWriter()
        {
           
        }
    }
}
