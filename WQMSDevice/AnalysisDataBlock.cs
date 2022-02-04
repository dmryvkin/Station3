using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Station.Device;
using EasyModbus;

namespace WQMSDevice
{
    class AnalysisDataBlock : IDataBlock
    {
        private Analysis[] _data;

        private string _ipAddress = "127.0.0.1";

        private ModbusClient _modbusClient;

        private int _startRegister;

        private int _blockSize;


        public AnalysisDataBlock(int startRegister, int blockSize)
        {
            _startRegister = startRegister;

            _blockSize = blockSize;

            _data = new Analysis[blockSize * 2];

            for(int i = 0; i< blockSize * 2; i++)
            {
                _data[i] = new Analysis();

            }

            _modbusClient = new ModbusClient();


            
            foreach (Analysis a in _data) {

                a.Value = null;
                a.Status = ChannelStatus.WAITING_FOR_REPLY;
            }

        }



        public Analysis[] Data
        {
            get
            {
                return _data;
            }
        }

        public string IpAddress
        {
            get
            {
                return _ipAddress;
            }

            set
            {
                _ipAddress = value;
                _modbusClient.IPAddress = _ipAddress;
            }
        }

        public void Read()
        {
            try
            {

                _modbusClient.Connect();

                int[] registers = _modbusClient.ReadHoldingRegisters(_startRegister, _blockSize * 4);

                ProcRegisters(registers);

                _modbusClient.Disconnect();
            }

            catch(Exception e){

                SetModbusError();

            }



        }

        private void ProcRegisters(int[] registers)
        {
            for(int i= 0; i< _blockSize; i++)
            {
                int index = i * 4;

                int dataAvailable = registers[index] & 0x100;

                int status = registers[index] & 0xFF;

                int alarm = registers[index + 1];

               


                if (dataAvailable != 0) {

                    _data[i].Value = (double?)ModbusClient.ConvertRegistersToFloat(new int[] { registers[index + 2], registers[index + 3] }, ModbusClient.RegisterOrder.HighLow);


                    _data[i + _blockSize].Value = alarm;


                    if ((status & 1) != 0) {
                        _data[i].Status = ChannelStatus.ANALISYS_IS_DONE;
                        _data[i + _blockSize].Status = ChannelStatus.ANALISYS_IS_DONE;
                    }

                    if ((status & 2) != 0)
                    {
                        _data[i].Status = ChannelStatus.DEVICE_ERROR;
                        _data[i + _blockSize].Status = ChannelStatus.DEVICE_ERROR;
                    };

                    if ((status & 4) != 0)
                    {
                        _data[i].Status = ChannelStatus.ANALISYS_IN_PROGRESS;
                        _data[i + _blockSize].Status = ChannelStatus.ANALISYS_IS_DONE;
                    };

                    if ((status & 8) != 0)
                    {
                        _data[i].Status = ChannelStatus.DEVICE_WARMUP;
                        _data[i + _blockSize].Status = ChannelStatus.ANALISYS_IS_DONE;
                    };

                    if ((status & 16) != 0)
                    {
                        _data[i].Status = ChannelStatus.ANALISYS_IS_DONE;
                        _data[i + _blockSize].Status = ChannelStatus.ANALISYS_IS_DONE;
                    };


                }
                               
                else
                {
                    _data[i].Value = null;

                    _data[i + _blockSize].Value = alarm;

                    _data[i].Value = null;

                    _data[i + _blockSize].Value = alarm;

                    if ((status & 1) != 0)
                    {
                        _data[i].Status = ChannelStatus.WAITING_FOR_REPLY;
                        _data[i + _blockSize].Status = ChannelStatus.WAITING_FOR_REPLY;
                    }

                    if ((status & 2) != 0)
                    {
                        _data[i].Status = ChannelStatus.DEVICE_ERROR;
                        _data[i + _blockSize].Status = ChannelStatus.DEVICE_ERROR;
                    };

                    if ((status & 4) != 0)
                    {
                        _data[i].Status = ChannelStatus.ANALISYS_IN_PROGRESS;
                        _data[i + _blockSize].Status = ChannelStatus.ANALISYS_IS_DONE;
                    };

                    if ((status & 8) != 0)
                    {
                        _data[i].Status = ChannelStatus.CALIBRATHION;
                        _data[i + _blockSize].Status = ChannelStatus.ANALISYS_IS_DONE;
                    };

                    if ((status & 16) != 0)
                    {
                        _data[i].Status = ChannelStatus.ANALISYS_IS_DONE;
                        _data[i + _blockSize].Status = ChannelStatus.ANALISYS_IS_DONE;
                    };


                }




            }


        }

        private void SetModbusError()
        {
            foreach (Analysis a in _data)
            {
                a.Value = null;
                a.Status = ChannelStatus.CONNECTION_ERROR;
            }
        }
    }
}
