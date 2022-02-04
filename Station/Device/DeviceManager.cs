using Station.Logging;
using Station.Triggers;
using Station.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Station.Device
{
    public class DeviceManager
    {

        private static DeviceManager instance;


        private List<IDevice> _devices;

        private List<IChannel> _channels;

        private List<IDataWriter> _currentWriters;

        private List<IDataWriter> _targetWriters;


        TimerTrigger _trigger ;

        DateTime    _currentProbeTime;

        private int _measurementPeriod = 2;

        private int _minimalAlarmIntervalSec = 300;

        private DateTime? _alarmTime;

        private volatile bool   _measurement = false;


        private static object syncRoot = new Object();

        public event MeasurementCompletedEventHandler OnCompleted ;

    


        protected DeviceManager()
        {
            _devices = new List<IDevice>();

            _channels = new List<IChannel>();

            _currentWriters = new List<IDataWriter>();

            _targetWriters = new List<IDataWriter>();


        }

        /// <summary>
        /// Идентификатор автоматической станции
        /// </summary>
        public int StationID { get; set; }

        /// <summary>
        /// Период автоматического запуска измерения
        /// при 0 - измерения не запускаются автоматически
        /// </summary>
        public int Period
        {
            get
            {

                return _measurementPeriod;

            }


            set
            {

                _measurementPeriod = value;

            }

        }

        /// <summary>
        /// Признак того, что система находится в состояении выполнения измерения 
        /// </summary>
        public bool IsMeasurement{

            get
            {
                return _measurement; 

            }

        } 
                

        /// <summary>
        /// Экземпляр класса для синглтона 
        /// </summary>
        public static DeviceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {


                            try
                            {

                                instance = new DeviceManager();

                                instance.Initialize();

                            }
                            catch (Exception e)
                            {

                                throw new Exception(e.Message, e.InnerException);
                            }

                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Коллекция драйверов измерительных устройств
        /// </summary>
        public List<IDevice> Devices { get { return _devices; } }

        /// <summary>
        /// Коллекция измерительных каналов
        /// </summary>
        public List<IChannel> Channels
        {
            get
            {
                return _channels;
            }

            set
            {
                SetChannels(value);

            }


        }
                

        /// <summary>
        /// Создает коллеции канало и приборов из файлов конфигурации
        /// </summary>
        private  void Initialize() {

            IDeviceFactory deviceFactory = new XmlDeviceFactory("Data/devices.xml");

            IChannelFactory channelFactory = new XmlChannelFactory("Data/channels.xml");

            _devices.AddRange(deviceFactory.GetDevices());

            _channels.AddRange(channelFactory.GetChannels());
        }

        /// <summary>
        /// Запуск драйверов приборов
        /// </summary>
        public async void  InitDevices()
        {

            await Task.Run(() =>
            {

                Logger.Log.Info($"Инициализация  устройств... ");

                foreach (IDevice device in _devices)
                {

                    device.OnUpdate += Device_OnUpdate;

                    InitDevice(device);
                }



            });

          
            RunMeasurementLoop();

        }
        /// <summary>
        /// Перезапуск, вызывается в случае изменеия конфигурации и измерительных каналов
        /// </summary>
        public void Restart()
        {
            RunMeasurementLoop();
        }

        /// <summary>
        /// Запуск цикла автоматических измерений
        /// </summary>
        private void RunMeasurementLoop()
        {
          


            if (_measurementPeriod != 0)
            {
                Logger.Log.Info($"Инициализация  цикла измерений. Период {_measurementPeriod} мин");

                if (_trigger == null)
                {
                    _trigger = new TimerTrigger(_measurementPeriod);
                    _trigger.OnTrigger += _trigger_OnTrigger;
                }

                _trigger.Interval = _measurementPeriod;

                _trigger.Start();

                Logger.Log.Info($"Следующее измерение: {_trigger.getNextTime()}");
            }
            else {

                Logger.Log.Info($"Автоматический  цикла измерений отключен ");

                if (_trigger != null)
                    _trigger.Stop();
            }
            


           
        }
        /// <summary>
        /// Инициализация драйвера прибора 
        /// </summary>
        /// <param name="device"></param>
        private  void InitDevice(IDevice device)
        {
            try
            {

                device.Init();

             
                 
            }
            catch (Exception e)
            {
                Logger.Log.Error(e.Message);
                Logger.Log.Debug(e);

            }
        }

        private void _trigger_OnTrigger(object sender, ITriggerEvent ev)
        {
            Logger.Log.Info($"Запуск измерения. Следующее измерение: {((TimerEvent)ev).NextTime}");

            StartMeasurement();

        }
        /// <summary>
        /// Начало выполнения измерения
        /// посылка команды на запуск приборам 
        /// </summary>
        public void StartMeasurement()
        {
            _currentProbeTime = DateTime.Now;

            _measurement = true;

            foreach (IChannel channel in _channels)
            {
                channel.AnalizysIsCompleted = false;

            }


            foreach (IDevice device in _devices)
            {

                Task.Run(() => { device.StartMeasurement(); });

            }
        }
        /// <summary>
        /// Проверяет состояние измерительных каналов
        /// определяет завершено ли измерени (все каналы ушли из состояние "в процессе")
        /// </summary>
        /// <returns>true - измерение завершено, false - не завершено</returns>
        private bool CheckMeasurementCompleted()
        {
            if (_measurement )
            {
                TimeSpan span = DateTime.Now - _currentProbeTime;

                if (span.TotalSeconds < 10)
                    return false;


                foreach(Channel channel in _channels)
                {
                    if (channel.Status.Equals(ChannelStatus.ANALISYS_IN_PROGRESS))
                        return false;
                }

                return true;
            }

            return false;

        }


        /// <summary>
        ///  Обработчие сообщение от драйвера прибора 
        /// </summary>
        /// <param name="sender">драйвер прибора </param>
        /// <param name="ev">событие </param>
        private void Device_OnUpdate(IDevice sender, DeviceEvent ev)
        {
            UpdateChannels(sender, ev);

            InvokeCurrentWriters(false);

            if (CheckMeasurementCompleted())
            {

                lock (syncRoot)
                {

                    if (IsMeasurement)
                    {
                        InvokeTargetWriters(false);
                        CompleteMeasurement();
                    }

                }
            }

        }
        /// <summary>
        /// Обновление значения канала при оповещении ор драйвера прибора 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        private void UpdateChannels(IDevice sender, DeviceEvent ev)
        {
            for (int i = 0; i < ev.Data.Count; i++)
            {

                IChannel channel = GetChannel(sender.DeviceName, ev.StartPosition + i);

                if (channel != null)
                {
                    if (channel.IsActive)
                    {
                        channel.Status = ChannelStatus.FromName(ev.Data[i].Status.Name);
                        channel.CurrentValue = ev.Data[i].Value;

                        if (channel.Status.Equals(ChannelStatus.ANALISYS_IS_DONE) && channel.AnalizysIsCompleted == false)
                        {
                            channel.AnalizysIsCompleted = true;
                            channel.UpdateTarget();


                        }

                        if (channel.CheckAlarm())
                        {
                            

                            if(channel.EnableAlarm)
                                FireAlarm(channel);
                        }
                    }
                    else
                    {
                        channel.Status = ChannelStatus.FromName(ChannelStatus.OFF.Name);
                        channel.CurrentValue = null;
                        channel.UpdateTarget();

                    }


                }
                else
                {
                    Logger.Log.Debug($"Не найден канал {sender.DeviceName}  {ev.StartPosition + i}");

                }
            }
        }
        /// <summary>
        /// Действия при выходе значения канала за норму 
        /// тут может вызываться сохранение внепланового события в базу или иной тип сохранения оповещения. 
        /// </summary>
        private void FireAlarm(IChannel  channel)
        {
            if (_alarmTime != null)
            {
                if (TimeUtil.SecondsInterval((DateTime)_alarmTime,DateTime.Now) < _minimalAlarmIntervalSec)
                    return;
                else
                    _alarmTime = null;

            }


            Logger.Log.Error($"Вход за норму по параметру {channel.Parameter.Name}");
            Logger.Log.Info("Внеплановое измерение");

            InvokeTargetWriters(true);


            

        }
        /// <summary>
        /// Действия по завершении текущего измерения
        /// </summary>
        private void CompleteMeasurement()
        {
            _measurement = false;


            OnCompleted?.Invoke();

         
        }
        /// <summary>
        /// Возвращает экземпляр объекта канала 
        /// </summary>
        /// <param name="device"> имя прибора </param>
        /// <param name="position"> позиция параметра в привзке к измерительному прибору </param>
        /// <returns></returns>
        private IChannel GetChannel(string device, int position) {

            return _channels.FindLast(x => x.DeviceName.Equals(device) && x.Position == position);

        }

        /// <summary>
        /// Добавляет обработчик записи текущего состояния
        /// </summary>
        /// <param name="writer"></param>
        public void  AddCurrentWriter(IDataWriter writer)
        {
            
            try
            {
                writer.OpenWriter(StationID);
                _currentWriters.Add(writer);

            }
            catch (Exception e)
            {

                Logger.Log.Error($"Ошибка инициализации обработчика {e.Message}");
            }


        }

        /// <summary>
        ///  Добавялет обработчик  записи результата измерения 
        /// </summary>
        /// <param name="writer"></param>
        public void AddTargetWriter(IDataWriter writer)
        {

            try
            {
                writer.OpenWriter(StationID);
                _targetWriters.Add(writer);
            }
            catch (Exception e) {

                Logger.Log.Error($"Ошибка инициализации обработчика {e.Message}");
            }

        }


        public void RemoveCurrentWriter(IDataWriter writer)
        {

            _currentWriters.Remove(writer);

        }

        /// <summary>
        /// Вызывает все обработчики записи текущего состояния 
        /// </summary>
        /// <param name="emergency"></param>
        private void InvokeCurrentWriters(bool emergency) {

            foreach (IDataWriter writer in _currentWriters) {

                  if(!writer.IsBusy)
                    ProcWriter(DateTime.Now, emergency, writer);

              
            }


        }
        /// <summary>
        /// Вызывает все обработчики записи результата измерения
        /// </summary>
        /// <param name="emergency">признак внепланового измерения</param>
        private void InvokeTargetWriters(bool emergency )
        {

            foreach (IDataWriter writer in _targetWriters)
            {

                if (!writer.IsBusy)
                {
                    if(emergency)
                        ProcWriter(DateTime.Now, emergency, writer); //Внеплановая запись
                    else
                        ProcWriter(_currentProbeTime, emergency, writer); // Плановая запись

                }

            }


        }

        /// <summary>
        /// Асинхронный вызов обработсчика записи
        /// </summary>
        /// <param name="time"></param>
        /// <param name="emergency"></param>
        /// <param name="writer"></param>
        private async void ProcWriter(DateTime time,  bool emergency, IDataWriter writer)
        {
            try
            {
                

                await Task.Run(() => {  writer.WriteData(StationID, Period,  time, emergency, _channels.AsEnumerable()); });

            }
            catch (Exception e)
            {

                Logger.Log.Error($"Ошибка записи данных {e.Message}", e);
                Logger.Log.Debug(e);


            }

        }

        private async Task CloseWriters() {

            foreach (IDataWriter writer in _currentWriters)
            {

                await Task.Run(() => { writer.CloseWriter(); });
                

            }

            foreach (IDataWriter writer in _targetWriters)
            {

                await Task.Run(() => { writer.CloseWriter(); });


            }
        }


        /// <summary>
        /// Завершение работы приборов
        /// </summary>
        /// <returns></returns>
        private async   Task     ShutdownDevices() {

            if(_trigger!=null)
                _trigger.Stop();

            foreach(Channel ch in _channels)
            {

                ch.CurrentValue = null;
                ch.Status = ChannelStatus.FromName(ChannelStatus.SHUTDOWN.Name);

            }

            InvokeCurrentWriters(false);

            foreach (IDevice device in _devices)
            {

                device.OnUpdate += Device_OnUpdate;


                await Task.Run(() => { device.Shutdown(); });

                

            }

        }
        /// <summary>
        /// Завершение работы 
        /// </summary>
        /// <returns></returns>
        public async Task Shutdown()
        {
           await  ShutdownDevices();
           await  CloseWriters();

        }

        /// <summary>
        /// Установка нового списка измерительных каналов
        /// </summary>
        /// <param name="source">коллекция источник</param>
        private void  SetChannels (IEnumerable<IChannel> source)
        {

            Hashtable sourceChannels = new Hashtable();
            Hashtable targetChannels = new Hashtable();

            foreach (IChannel c in source) { sourceChannels.Add(c.Id, c); }
            foreach (IChannel c in _channels) { targetChannels.Add(c.Id, c); }

            foreach (object key in targetChannels.Keys)
            {
                if (!sourceChannels.ContainsKey(key))
                    _channels.Remove((IChannel)targetChannels[key]);
            }

            foreach(object key in targetChannels.Keys)
            {
                ((IChannel)targetChannels[key]).IsActive = ((IChannel)sourceChannels[key]).IsActive;

                if (!((IChannel)targetChannels[key]).IsActive) {
                    ((IChannel)targetChannels[key]).Status = ChannelStatus.FromName(ChannelStatus.OFF.Name);
                    ((IChannel)targetChannels[key]).CurrentValue = null;
                    ((IChannel)targetChannels[key]).UpdateTarget();

                }

                ((IChannel)targetChannels[key]).DeviceName = ((IChannel)sourceChannels[key]).DeviceName;
                ((IChannel)targetChannels[key]).Position = ((IChannel)sourceChannels[key]).Position;
                ((IChannel)targetChannels[key]).Parameter = ((IChannel)sourceChannels[key]).Parameter;

            }

            foreach (object key in sourceChannels.Keys) {

                if (!targetChannels.ContainsKey(key))
                    _channels.Add((IChannel)sourceChannels[key]);

            }






        }




    }
}
