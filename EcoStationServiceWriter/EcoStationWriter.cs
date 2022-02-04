using EcoStationServiceWriter.ServiceReference;
using Station.Device;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationServiceWriter
{

    [Serializable]
    public class EcoStationWriter : IDataWriter
    {

        EcoStationServiceClient _client;


        private bool _busy = false;

        private int _stationId = 0;


        private Hashtable _parameters;


        public EcoStationWriter()
        {
            _parameters = new Hashtable();

        }
        

        public bool IsBusy
        {
            get
            {
                return false;
            }
        }

        public void AddParameter(string v1, object v2)
        {
            _parameters.Add(v1, v2);
        }

        public void CloseWriter()
        {
            _client.addEvent(_stationId, DateTime.Now, 0, 0, "Завершение  работы");
        }

        public void OpenWriter(int stationId)
        {
            _stationId = stationId;

            try
            {
                _client = getClient((string)_parameters["Url"], null, null);


                _client.addEvent(stationId, DateTime.Now, 0, 0, "Начало работы");
            }

            catch (Exception e)
            {
                throw new Exception($"EcoStationWriter {e.Message}", e);


            }

        }

        public void WriteData(int id, int period, DateTime time,  bool emergency, IEnumerable<IChannel> channels)
        {
            int probeId = 0;

            _busy = true;

            try
            {

                 probeId = _client.addProbe(time, id, period, emergency);

            }
            catch (Exception e)
            {
                throw new Exception($"Ошибка создания  записи  {e.Message}", e);


            }
            finally {

                _busy = false;
            }



            foreach (Channel channel in channels)
            {
                try
                {

                    if (channel.TargetValue != null)
                        _client.addResult(probeId, channel.Parameter.Code, (double)channel.TargetValue, false, channel.Status.Code);
                    else
                        _client.addEmptyResult(probeId, channel.Parameter.Code, false, channel.Status.Code);

                }
                catch(Exception e)
                {
                    throw new Exception($"Ошибка записи значения параметра {channel.Parameter.Name} {e.Message}", e);

                }
                finally
                {
                    _busy = false;
                }



            }
            _busy = false;


        }

        private EcoStationServiceClient getClient(string url, string user, string password)
        {
            try
            {

                var binding = new BasicHttpBinding(BasicHttpSecurityMode.None);

                binding.Name = "binding";

                binding.MaxReceivedMessageSize = 10000;


                EndpointAddress adress = new EndpointAddress(url);

                EcoStationServiceClient client = new EcoStationServiceClient(binding, adress);

                return client;


            }
            catch (Exception ex)
            {
                throw ex;

            }



        }
    }
}
