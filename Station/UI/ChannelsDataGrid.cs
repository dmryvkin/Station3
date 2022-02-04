using Station.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Station.UI
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Station.UI"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Station.UI;assembly=Station.UI"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:ChannelsDataGrid/>
    ///
    /// </summary>
    public class ChannelsDataGrid : DataGrid
    {
        static ChannelsDataGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChannelsDataGrid), new FrameworkPropertyMetadata(typeof(ChannelsDataGrid)));
        }


        public void UpdateGrid(IEnumerable<IChannel> channels)
        {
            List<DataItem> items = new List<DataItem>();


            foreach (IChannel channel in channels.OrderBy(x => x.Id))
            {
                DataItem item = new DataItem
                {

                    Name = String.Format("{0}  {1}", channel.Id, channel.Parameter.Name),
                    Status = $" {channel.Status.Decsription} ({channel.Status.Code})",
                    Value = channel.Parameter.FormatValue(channel.CurrentValue),
                    TargetValue =  channel.Parameter.FormatValue(channel.TargetValue),
                    Unit = channel.Parameter.UnitName,
                    Min = channel.Parameter.FormatValue(channel.Parameter.Min),
                    Max = channel.Parameter.FormatValue(channel.Parameter.Max),
                    Background = GetBackgroundColor(channel),
                    Foreground = GetForegroundColor(channel)
                };

                items.Add(item);

            }

            this.ItemsSource = items;


        }

        private string GetBackgroundColor(IChannel channel)
        {
            if (!channel.IsActive)
                return "LightGray";


            switch (channel.Status.Name) {

                case nameof(ChannelStatus.WAITING_FOR_REPLY):
                    return "FloralWhite";

                case nameof(ChannelStatus.ANALISYS_IN_PROGRESS):
                    return "MintCream";

                case nameof(ChannelStatus.ANALISYS_IS_DONE):
                    return "LightGreen";

                case nameof(ChannelStatus.DEVICE_WARMUP):
                    return "MistyRose";

                case nameof(ChannelStatus.CONNECTION_ERROR):
                    return "Gray";

                case nameof(ChannelStatus.PORT_ERROR):
                    return "Gray";

                case nameof(ChannelStatus.DEVICE_ERROR):
                    return "LemonChiffon";

                case nameof(ChannelStatus.ALARM):
                    return "Salmon";


            }

            return "White";
        }
        private string GetForegroundColor(IChannel channel)
        {
           if(channel.IsActive)
            return "Black";
           else
               return "Gray";


        }


    }

    class DataItem
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string Value { get; set; }
        public string TargetValue { get; set; }
        public string Unit { get; set; }
        public string Background { get; set; }
        public string Foreground { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }



    }
}
