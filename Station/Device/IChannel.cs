namespace Station.Device
{
    public interface IChannel
    {
         uint Id { get; set; }

         Parameter Parameter { get; set; }

        bool IsActive  { get; set; }

        string DeviceName { get; set; }

         uint  Position { get; set; }

         double?  CurrentValue { get; set; }

         double? TargetValue { get; }

         ChannelStatus Status { get; set; }

         bool AnalizysIsCompleted { get; set; }

         void UpdateTarget();

         bool CheckAlarm();

         TrendElement[] Trend  { get; }

         double TrendMin  { get; }
        
         double TrendMax { get; }

         bool EnableAlarm { get; set; }

    }
}
