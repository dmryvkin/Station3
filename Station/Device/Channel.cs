using Station.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Station.Device
{
    public class Channel : IChannel
    {

        private double?    _currentValue;
        private double?    _targetValue;


        private DateTime   _trendFixTime;

        private int        _trendMinIntervalSec = 10;
        private int        _trendMaxPeriodSec = 3600;

        private double    _trendMin;
        private double    _trendMax;


        private LinkedList<TrendElement> _trend;


        public uint Id
        { get; set; }

        public string DeviceName
        { get; set; }

        public bool IsActive
        { get; set; }


        public Parameter Parameter
         { get; set; }

        public uint Position
        { get; set; }

        /// <summary>
        /// Текущее значение, обновляемое при получении сообщения от прибора
        /// </summary>
        [XmlIgnore]
        public double? CurrentValue
        {
            get
            { return _currentValue; }

          set {
                _currentValue = value;
                FixTrend(value);

            } }

      
        /// <summary>
        /// Статус канала
        /// </summary>
        [XmlIgnore]
        public ChannelStatus Status
        { get; set; }
        
        /// <summary>
        /// Признак завершения анализа
        /// </summary>
        [XmlIgnore]
        public bool AnalizysIsCompleted
        { get; set; }

        /// <summary>
        /// Целевое значение канала устанавливается после завершения измерения
        /// либо в резултате усреднения 
        /// </summary>
        [XmlIgnore]
        public double? TargetValue
        { get { return _targetValue; } }

        /// <summary>
        /// Массив элементов тренда
        /// </summary>
        [XmlIgnore]
        public TrendElement[] Trend
        {
            get
            {
                lock (_trend)
                {

                    return _trend.ToArray();
                }
            }
        }

        /// <summary>
        ///  Минимальное значение сохраненное в тренде
        /// </summary>
        [XmlIgnore]
        public double TrendMin
        {
            get
            {
                return _trendMin;
            }

           
        }
        /// <summary>
        /// Максимальное значение сохраненное в тренде
        /// </summary>
        [XmlIgnore]
        public double TrendMax
        {
            get
            {
                return _trendMax;
            }

           
        }
        /// <summary>
        /// Минимальный интервал сохранения значения в тренд в секундах
        /// по умолчанию - 5 секунд
        /// </summary>
        [XmlIgnore]
        public int TrendMinIntervalSec
        {
            get
            {
                return _trendMinIntervalSec;
            }

            set
            {
                _trendMinIntervalSec = value;
            }
        }
        /// <summary>
        /// Период сохранения в памяти тренда значений канала в секундах
        /// </summary>
        [XmlIgnore]
        public int TrendMaxPeriodSec
        {
            get
            {
                return _trendMaxPeriodSec;
            }

            set
            {
                _trendMaxPeriodSec = value;
            }
        }

        public bool EnableAlarm
        {
            get; set;
            
        }

        public Channel(uint id, Parameter parameter, string device, uint position)
        {

            _trendFixTime = DateTime.Now;
            _trend = new LinkedList<TrendElement>();

            _trendMin = Double.MaxValue;
            _trendMax = Double.MinValue;


            Id = id;
            Parameter = parameter;
            Position = position;
            DeviceName = device;

            Status = ChannelStatus.WAITING_FOR_REPLY;

            CurrentValue = null;

        }

        public Channel()
        {
            _trendFixTime = DateTime.Now;
            _trend = new LinkedList<TrendElement>();

            _trendMin = Double.MaxValue;
            _trendMax = Double.MinValue;

            Parameter = new Parameter() { Name = "Новый параметр" }; 

            Status = ChannelStatus.WAITING_FOR_REPLY;

            CurrentValue = null;

        }
        /// <summary>
        /// Устанавливает целевое значение
        /// (тут может быть логика обработки тренда)
        /// </summary>
        public void UpdateTarget()
        {

            _targetValue = _currentValue;
             

        }
        /// <summary>
        ///  Проверяет выход целевого значения канала за норму
        /// </summary>
        /// <returns></returns>
        public bool CheckAlarm()
        {

            if (TargetValue >= Parameter.Max || TargetValue < Parameter.Min)
            {

                this.Status = ChannelStatus.FromName(ChannelStatus.ALARM.Name);

                return true;
            }

            return false;
        }

        /// <summary>
        ///  Сохраняет текущее значение канала в тренд 
        /// </summary>
        private void FixTrend(double? value)
        {

            Task.Run(() => { 

            if (TimeUtil.SecondsInterval(_trendFixTime, DateTime.Now)  > _trendMinIntervalSec && value!=null)
            {
                TrendElement te = new TrendElement { Value = (double)value, Time = DateTime.Now };

                _trend.AddLast(te);


                if(value <_trendMin) _trendMin = (double)value;


                if (value > _trendMax) _trendMax = (double)value;

               
                    DateTime firstElementTime = _trend.First.Value.Time;

                    if (DateTime.Now.Subtract((DateTime)firstElementTime).Duration().TotalSeconds > _trendMaxPeriodSec) {

                            _trend.RemoveFirst();
                    }

                _trendFixTime = DateTime.Now;

            }
            });
           
            
        }


        public override string ToString()
        {
            return $"{Id} {Parameter.Name}";
        }




    }

    public class ChannelCollection
    {
        [XmlArray("Channels"), XmlArrayItem("Channel")]
        public List<Channel> Collection { get; set; }
    }
}



