using Ardalis.SmartEnum;

namespace Station.Device
{

    public abstract class ChannelStatus : SmartEnum<ChannelStatus, int>
    {
        public static readonly ChannelStatus ANALISYS_IS_DONE = new AnalisysIsDoneStatus();
        public static readonly ChannelStatus ANALISYS_IN_PROGRESS = new AnalisysInProgressStatus();
        public static readonly ChannelStatus WAITING_FOR_REPLY = new WaitingForReplyStatus();
        public static readonly ChannelStatus CONNECTION_ERROR = new ConnectionErrorStatus();
        public static readonly ChannelStatus PORT_ERROR = new PortErrorStatus();
        public static readonly ChannelStatus DEVICE_ERROR = new DeviceErrorStatus();
        public static readonly ChannelStatus DEVICE_WARMUP = new DeviceWarmupStatus();
        public static readonly ChannelStatus ALARM = new DeviceAlarmStatus();
        public static readonly ChannelStatus OFF = new OffStatus();
        public static readonly ChannelStatus SHUTDOWN = new ShutdownStatus();
        public static readonly ChannelStatus CALIBRATHION = new CalibrationStatus();




        private ChannelStatus(string name, int value) : base(name, value)
        {
        }

        public abstract string Code { get; }
        public abstract string Decsription { get; }


        private sealed class AnalisysIsDoneStatus : ChannelStatus
        {
            public AnalisysIsDoneStatus() : base("ANALISYS_IS_DONE", 1) { }

            public override string Code
            {
                get { return "M00"; }

            }

            public override string Decsription
            {
                get { return "Измерение выполнено"; }
            }

        }

        private sealed class AnalisysInProgressStatus : ChannelStatus
        {
            public AnalisysInProgressStatus() : base("ANALISYS_IN_PROGRESS", 2) { }

            public override string Code
            {
                get { return "M01"; }

            }

            public override string Decsription
            {
                get { return "Измерение выполняется"; }
            }

        }

        private sealed class WaitingForReplyStatus : ChannelStatus
        {
            public WaitingForReplyStatus() : base("WAITING_FOR_REPLY", 3) { }

            public override string Code
            {
                get { return "MGF"; }

            }

            public override string Decsription
            {
                get { return "Ожидание ответа"; }
            }

        }
        private sealed class ConnectionErrorStatus : ChannelStatus
        {
            public ConnectionErrorStatus() : base("CONNECTION_ERROR", 4) { }

            public override string Code
            {
                get { return "MGA"; }

            }

            public override string Decsription
            {
                get { return "Нет соединения"; }
            }

        }

        private sealed class PortErrorStatus : ChannelStatus
        {
            public PortErrorStatus() : base("PORT_ERROR", 5) { }

            public override string Code
            {
                get { return "MGC"; }

            }

            public override string Decsription
            {
                get { return "Ошибка открытия порта"; }
            }

        }

        private sealed class DeviceErrorStatus : ChannelStatus
        {
            public DeviceErrorStatus() : base("DEVICE_ERROR", 6) { }

            public override string Code
            {
                get { return "E00"; }

            }

            public override string Decsription
            {
                get { return "Ошибка прибора"; }
            }

        }

        private sealed class DeviceWarmupStatus : ChannelStatus
        {
            public DeviceWarmupStatus() : base("DEVICE_WARMUP", 7) { }

            public override string Code
            {
                get { return "P00"; }

            }

            public override string Decsription
            {
                get { return "Прогрев прибора"; }
            }

        }

        private sealed class DeviceAlarmStatus : ChannelStatus
        {
            public DeviceAlarmStatus() : base("ALARM", 8) { }

            public override string Code
            {
                get { return "M00 П"; }

            }

            public override string Decsription
            {
                get { return "Выход за норму"; }
            }

        }

        private sealed class OffStatus : ChannelStatus
        {
            public OffStatus() : base("OFF", 9) { }

            public override string Code
            {
                get { return "X00"; }

            }

            public override string Decsription
            {
                get { return "Канал отключен"; }
            }

        }

        private sealed class CalibrationStatus : ChannelStatus
        {
            public CalibrationStatus() : base("CALIBRATION", 10) { }

            public override string Code
            {
                get { return "S00"; }

            }

            public override string Decsription
            {
                get { return "Калибровка"; }
            }

        }

        private sealed class ShutdownStatus : ChannelStatus
        {
            public ShutdownStatus() : base("SHUTDOWN", 11) { }

            public override string Code
            {
                get { return "O00"; }

            }

            public override string Decsription
            {
                get { return "Cтанция остановлена"; }
            }

        }

    }
    }
