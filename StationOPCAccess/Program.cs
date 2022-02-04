using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Graybox.OPC.ServerToolkit.CLRWrapper;
using Station.Device;
using System.Collections;
using log4net.Config;
using log4net.Appender;
using System.IO;

namespace StationOPCAccess
{
    static class Program
    {

        const string GUID = "EA1370BF-AC53-41f2-940D-3A834208BBFB";
        const string VENDOR = "DIEM";
        const string PROG_ID = "DIEM.ECO.DataAccess";
        const string PROG_NAME = "StationOPCAccess";
        const string VERSION = "2.0";

        static int stop = 0;
     
        static Guid srv_guid;

        static Log log;

        static OPCConfiguration configuration;

       [MTAThread]
        static void Main(string[] args)
        {
            srv_guid = new Guid(GUID);

            Dictionary<string,int> tagIds = new Dictionary<string, int>();


            ProcCommandLine(args);

            log = new Log($"{AppDomain.CurrentDomain.BaseDirectory}\\Logs\\opc.log");

            log.Info(" Connect...");

            configuration = OPCConfiguration.Create();

            IDataSource dataSource = new ODBCDataSource(configuration.StationID, configuration.ConnectionString);

            IEnumerable<IChannel> channels = GetChannels();

            OPCDAServer srv = new OPCDAServer();

            srv.Events.WriteItems += new WriteItemsEventHandler(Events_WriteItems);

            srv.Events.ServerReleased += new ServerReleasedEventHandler(Events_ServerReleased);

            srv.Initialize(srv_guid, 50, 50, ServerOptions.NoAccessPaths, '.', configuration.MaxTags);


            // Создание тегов
            
            foreach (IChannel channel in channels)
            {

                try
                {
                    int tagId = srv.CreateTag(1, $"{configuration.OPCPrefix}.{channel.Parameter.Sign}", AccessRights.readWritable,0);

                    tagIds.Add(channel.Parameter.Sign, tagId);
                    
                }
                catch (Exception e)
                {
                    log.Error(e);
                }


            }

            srv.RegisterClassObject();

            try
            {
                dataSource.Open();
            }
            catch (Exception e) {

                log.Error(e);
            }




            while (System.Threading.Interlocked.CompareExchange(ref stop, 1, 1) == 0)
            {
                System.Threading.Thread.Sleep(configuration.PollInterval * 1000);


                try
                {

                    dataSource.BeginAccess();

                    srv.BeginUpdate();

                    DateTime dataSourceRecordTime = dataSource.GetDateTime();

                
                    
                        foreach (Channel channel in channels)
                        {
                            var value = dataSource.GetValue(channel.Parameter.Sign);

                            string status = dataSource.GetCode(channel.Parameter.Sign);

                            if (status != null)
                            {
                                Quality quality = GetQuality(dataSourceRecordTime, status);

                                srv.SetTag(tagIds[channel.Parameter.Sign], value, quality, FileTime.UtcNow);


                            }
                            else
                            {
                                Quality quality = Quality.Bad;
                                quality.QualityStatus = QualityBits.badOutOfService;

                                srv.SetTag(tagIds[channel.Parameter.Sign], null, quality, FileTime.UtcNow);

                            }

                        }
                    


                }
                catch (Exception e)
                {

                    log.Error(e);
                }
                finally {


                    srv.EndUpdate(false);

                    dataSource.EndAccess();

                }



            }

            log.Info("Disconnect. ");


            log.Close();

            dataSource.Close();

            srv.RevokeClassObject();
        }

        

        private static Quality GetQuality(DateTime time, string status)
        {
            Quality result = Quality.Unspecified;

            char mode = status[0];
            char err1 = status[1];
            char err2 = status[2];


            if (mode == 'O' || !IsDataActual(time)) {

                result = Quality.Bad;
                result.QualityStatus = QualityBits.badOutOfService;
                return result;

            }


            switch (mode)
            {
                case 'P':// P XX - device warmup
                    result = Quality.Bad ; 
                    result.QualityStatus = QualityBits.badDeviceFailure;
                    break;
                case 'D':  // D XX - device error on modbus protocol
                    result = Quality.Bad; 
                    result.QualityStatus = QualityBits.badDeviceFailure;
                    break;

                case 'E':  // E XX - device specific error 
                    result = Quality.Bad;
                    result.QualityStatus = QualityBits.badDeviceFailure;
                    break;

                case 'S':// S XX - span calibration
                    result = Quality.Unspecified;
                    result.QualityStatus = QualityBits.uncertainSensorNotAccurate;
                    break;
                case 'R':  // R XX - reference calibration
                    result = Quality.Unspecified;
                    result.QualityStatus = QualityBits.uncertainSensorNotAccurate;
                    break;

                case 'Z':     // Z XX - zero calibration
                    result = Quality.Unspecified;
                    result.QualityStatus = QualityBits.uncertainSensorNotAccurate;
                    break;

                case 'T': ; //T XX - timeout after calibration or protocol initial config
                    result = Quality.Bad;
                    result.QualityStatus = QualityBits.badWaitingForInitialData;
                    break;

                case 'X':  // X XX - sensor is manually set to inactive by acquisition data system
                    result = Quality.Good;
                    result.QualityStatus = QualityBits.goodLocalOverride;
                    break;


                case 'M':
                    {
                        if (err1=='G')
                        {
                            switch (err2)
                            {
                                case 'A':  // M GA - no answer from device
                                    result = Quality.Bad;
                                    result.QualityStatus = QualityBits.badNotConnected;
                                    break;

                                case 'B':  // M GB - protocol CRC error, possibly wrong config
                                    result = Quality.Bad;
                                    result.QualityStatus = QualityBits.badConfigurationError;
                                    break;

                                case 'C':  // M GC - communication interface failure
                                    result = Quality.Bad;
                                    result.QualityStatus = QualityBits.badCommFailure;
                                    break;

                                case 'D':  // M GD - no data on particular device sensor
                                    result = Quality.Bad;
                                    result.QualityStatus = QualityBits.badSensorFailure;
                                    break;

                                case 'E':   // M GE - protocol error cannot be identified
                                    result = Quality.Bad;
                                    result.QualityStatus = QualityBits.badConfigurationError;
                                    break;


                                case 'F':     // M GF - configuration error (or, at start, timeout is still not expired)
                                    result = Quality.Bad;
                                    result.QualityStatus = QualityBits.badWaitingForInitialData;
                                    break;

                            }
                        }
                        else if (err1 == '0' && err2 == '0')
                        {
                            result = Quality.Good;   // NaN M 00 - code indicated all's Ok and no data present
                            result.QualityStatus = QualityBits.good;

                        }
                        else
                        {
                            result = Quality.Bad;// M XX (XX != 00) - specific error on channel (at measurement mode)
                            result.QualityStatus = QualityBits.badLastKnownValue;
                        }
                    }
                    break;
                default:
                    result =Quality.Bad;
                    break;
            }

            return result;
        }

        private static bool IsDataActual(DateTime dataSourceRecordTime)
        {
            TimeSpan span = DateTime.Now - dataSourceRecordTime;

            return (span.TotalSeconds < configuration.MaxDataAge);

        }


        private static IEnumerable<IChannel> GetChannels()
        {
            XmlChannelFactory channelsFactory = new XmlChannelFactory($"{AppDomain.CurrentDomain.BaseDirectory}\\Data\\channels.xml");

            IEnumerable<IChannel> channels = channelsFactory.GetChannels();
            return channels;
        }




        static void ProcCommandLine(string[] args) {

         

            if (args.Length > 0)
            {
                try
                {
                    // Регистрация сервера OPC
                    if (args[0].IndexOf("-r") != -1)
                    {
                        OPCDAServer.RegisterServer(
                            srv_guid,
                            VENDOR,
                            PROG_NAME,
                            PROG_ID,
                            VERSION);
                        return;
                    }
                    // Отмена регистрации.
                    if (args[0].IndexOf("-u") != -1)
                    {
                        OPCDAServer.UnregisterServer(srv_guid);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    return;
                }
            }
        }


      
        static void Events_WriteItems(object sender, WriteItemsArgs e)
        {
            for (int i = 0; i < e.Count; i++)
            {
                if (e.ItemIds[i].TagId == 0) continue;
                try
                {
                    int v = Convert.ToInt32(e.Values[i], System.Globalization.CultureInfo.InvariantCulture);
                    if (v < 0 || v > 100) throw new ArgumentOutOfRangeException();
                }
                catch (Exception ex)
                {
                    e.Errors[i] = (ErrorCodes)System.Runtime.InteropServices.Marshal.GetHRForException(ex);
                    e.ItemIds[i].TagId = 0;
                    e.MasterError = ErrorCodes.False;
                }
            }
        }

        static void Events_ServerReleased(object sender, ServerReleasedArgs e)
        {
            e.Suspend = true;
            System.Threading.Interlocked.Exchange(ref stop, 1);
        }
    }

}
