using log4net.Appender;
using System;
using System.Collections.Generic;
using System.Windows;
using log4net.Core;
using Station.Logging;
using System.Windows.Threading;
using log4net.Config;
using Station.Device;
using Station.Writers;
using Station.Config;
using Station.UI;
using Station.Util;
using Station.Modbus;
using EasyModbus;

namespace Station
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IAppender, IDataWriter
    {


        StationConfiguration config;

        private  DateTime lastGridUpdateTime = DateTime.Now;
        private DateTime  lastTrendUpdateTime = DateTime.Now;

        private int gridUpdateInterval = 2;
        private int trendUpdateInterval = 5;


        ModbusStationServer modbusServer; 


        public MainWindow()
        {
            InitializeComponent();


            config = StationConfiguration.Create();

            InitLog();

            SetupApplicationInfo();

            InitApplucation();

        }

        private void InitApplucation()
        {
            Logger.Log.Info("Начало работы");

            Logger.Log.Info("Загрузка модулей устройств");

         
            DeviceManager.Instance.StationID = config.ID;
            DeviceManager.Instance.Period = config.Period;
            

            try
            {

                foreach (IDevice device in DeviceManager.Instance.Devices)
                {
                    Logger.Log.Info($"Модуль устройства {device.DeviceName} {device.GetType().Assembly.GetName()} загружен");

                }

            }
            catch (Exception e)
            {

                Logger.Log.Error(e.Message);


                Logger.Log.Debug(e);

                return;
            }

            Logger.Log.Info($"Измерительных каналов: { DeviceManager.Instance.Channels.Count}");

         

            InitWriters();

            if(config.ModbusEnabled)
                InitModbus();

            StartItem.IsEnabled = (DeviceManager.Instance.Period == 0);

            DeviceManager.Instance.InitDevices();

            DeviceManager.Instance.OnCompleted += OnMeasurementCompleted;

            WriteData(config.ID, config.Period, DateTime.Now, false, DeviceManager.Instance.Channels);

            trendsControl.Channels = DeviceManager.Instance.Channels;

        }

        private void InitModbus()
        {
            modbusServer = new ModbusStationServer();
            DeviceManager.Instance.AddCurrentWriter(modbusServer);

            modbusServer.ModbusServer.CoilsChanged += new ModbusServer.CoilsChangedHandler(CoilsChanged);
            modbusServer.ModbusServer.HoldingRegistersChanged += new ModbusServer.HoldingRegistersChangedHandler(HoldingRegistersChanged);


        }
        /// <summary>
        ///  Управление конфигурацией через установку регистров сервера 
        /// </summary>
        /// <param name="register"></param>
        /// <param name="numberOfRegisters"></param>
        private void HoldingRegistersChanged(int register, int numberOfRegisters)
        {
            if (register == ModbusStationServer.PERIOD_REGISTER) {

                StationConfiguration tempConfig = StationConfiguration.Create();

                tempConfig.Period = modbusServer.ModbusServer.holdingRegisters[2];

                if (ConfigValidator.Validate(tempConfig))
                {

                    config = tempConfig;


                    config.Save();

                    UpdateConfiguration();

                }
                else {
                    Logger.Log.Info("Неверный параметр конфигурации!");
                }



            }

        }
        /// <summary>
        /// Включение/ отключение каналов через установку  коилов
        /// </summary>
        /// <param name="coil"></param>
        /// <param name="numberOfCoils"></param>
        private void CoilsChanged(int coil, int numberOfCoils)
        {
            int channelId = coil ;   

            IChannelFactory channelFactory = new XmlChannelFactory("Data/channels.xml");

            var channels =  channelFactory.GetChannels();

            foreach (IChannel c in channels) {

                if (c.Id == channelId) {

                    if(c.IsActive != modbusServer.ModbusServer.coils[coil]){

                        c.IsActive = modbusServer.ModbusServer.coils[coil];

                        Logger.Log.Info($"Изменение состояния канала:  {c.Parameter.Name} ");

                        DeviceManager.Instance.Channels = new List<IChannel> (channels);
                        WriteData(config.ID, config.Period, DateTime.Now, false, DeviceManager.Instance.Channels);
                        channelFactory.Save(channels);

                    }

                }
            }


          


        }

        private  void InitWriters()
        {
            DeviceManager.Instance.AddCurrentWriter(this);

            XmlWriterFactory writersFactory = new XmlWriterFactory("Data/writers.xml");

            foreach (IDataWriter writer in writersFactory.GetCurrentWriters())
            {

                DeviceManager.Instance.AddCurrentWriter(writer);
            }

            foreach (IDataWriter writer in writersFactory.GetTargetWriters())
            {

                DeviceManager.Instance.AddTargetWriter(writer);
            }
        }

        private void OnMeasurementCompleted()
        {
            Logger.Log.Info("Измерение завершено.");

            Dispatcher.Invoke(() => {
                            StartItem.IsEnabled = (DeviceManager.Instance.Period == 0);
                            }, DispatcherPriority.Normal);
            
        }

        private void InitLog()
        {
            XmlConfigurator.Configure(new System.IO.FileInfo("Data/log.xml"));

            Logger.AddAppender(this);

        }


        private void SetupApplicationInfo() {

            Logger.Log.Info($"{ApplicationInfo.Title}  { ApplicationInfo.Version} [{config.Name}]");
            Logger.Log.Info(ApplicationInfo.Copyright);

            this.Title = $"{ApplicationInfo.Title} {ApplicationInfo.Version} [{config.Name} {config.ID}]";
        }


        private void CloseLog()
        {

            Logger.RemoveAppender(this);
        }

        public void DoAppend(LoggingEvent loggingEvent)
        {
            Dispatcher.Invoke(() => { logBox.AddLogEvent(loggingEvent); }, DispatcherPriority.Normal);


        }

       

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show(this, "Завершить работу приложения?", this.Title, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {

                ShutdownApplication();
               
            }
            else
            {
                e.Cancel = true;

            }
        }

        private  async void  ShutdownApplication()
        {
            Logger.Log.Info("Завершение работы");

            CloseLog();

            DeviceManager.Instance.RemoveCurrentWriter(this);

            await DeviceManager.Instance.Shutdown();

          
        }

        public void WriteData(int ststiomId, int Period, DateTime time, bool emergency, IEnumerable<IChannel> channels)
        {
           
            if (TimeUtil.SecondsInterval(lastGridUpdateTime, DateTime.Now) > gridUpdateInterval)
            {
                lastGridUpdateTime = DateTime.Now;

                Dispatcher.Invoke(() =>
                {

                    UpdateDataGrid(channels);

                }, DispatcherPriority.Normal);

            }

            if (TimeUtil.SecondsInterval(lastTrendUpdateTime, DateTime.Now) > trendUpdateInterval)
            {
                lastTrendUpdateTime = DateTime.Now;

                Dispatcher.Invoke(() => {


                    trendsControl.UpdateChart();

                }, DispatcherPriority.Normal); 

             }
        }

        private void UpdateDataGrid(IEnumerable<IChannel> channels)
        {
            

            dataGrid.UpdateGrid(channels);


        }

        public void AddParameter(string v1, object v2)
        {
           
        }

      

        public void OpenWriter(int id)
        {
        }

        public void CloseWriter()
        {
        }

        public bool IsBusy
        {
            get
            {
                return false;
            }

            set
            {


            }
        }

        private void StartItem_Click(object sender, RoutedEventArgs e)
        {
            Logger.Log.Info("Запуск измерения");

            DeviceManager.Instance.StartMeasurement();
            StartItem.IsEnabled = false;
        }

        private void ConfigurationItem_Click(object sender, RoutedEventArgs e)
        {

            ConfigurationDialogBox dlg = new ConfigurationDialogBox();

            dlg.Owner = this;


            StationConfiguration tempConfig = StationConfiguration.Create();

            dlg.DataContext = tempConfig;

            if (dlg.ShowDialog()== true)
            {

                config = tempConfig;

                config.Save();

                UpdateConfiguration();

            }

        }

        private void UpdateConfiguration()
        {
            Logger.Log.Info("Изменение конфигурации станции");
                      

            DeviceManager.Instance.StationID = config.ID;
            DeviceManager.Instance.Period = config.Period;
            DeviceManager.Instance.Restart();

            Dispatcher.Invoke(() => {


                  StartItem.IsEnabled = (DeviceManager.Instance.Period == 0);

            }, DispatcherPriority.Normal);

          
        }

        private void ChannelsItem_Click(object sender, RoutedEventArgs e)
        {
            ChannelsDialogBox dlg = new ChannelsDialogBox();

            dlg.Owner = this;

            IChannelFactory channelFactory = new XmlChannelFactory("Data/channels.xml");

            dlg.Channels =  channelFactory.GetChannels();
          

            if (dlg.ShowDialog() == true)
            {
                List<IChannel> newList = new List<IChannel>(dlg.Channels);

                DeviceManager.Instance.Channels =  newList;
                WriteData(config.ID, config.Period, DateTime.Now, false, DeviceManager.Instance.Channels);
                channelFactory.Save(dlg.Channels);

            }

        }

        private void AboutItem_Click(object sender, RoutedEventArgs e)
        {
            AboutBox aboutBox = new AboutBox();

            aboutBox.ShowDialog();
        }
    }

   


}
