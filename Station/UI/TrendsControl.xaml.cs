using Station.Device;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Station.UI
{
    /// <summary>
    /// Interaction logic for TrendsControl.xaml
    /// </summary>
    public partial class TrendsControl : UserControl
    {
        private List<ChannelSelectorItem> _channelSelectorItems;

        public IEnumerable<IChannel> _channels;
        
        public IEnumerable<IChannel> Channels {

            get { return _channels; }

            set {

                _channels = value;
                SetChannels(value);
            }

        }



        public TrendsControl()
        {
            InitializeComponent();

            _channelSelectorItems = new List<ChannelSelectorItem>();

            

            TrendsPlot.Plot.XAxis.DateTimeFormat(true);
            TrendsPlot.Plot.XAxis.TickLabelFormat("HH:mm", dateTimeFormat: true);
        }



        /// <summary>
        /// Заполнение списка каналов
        /// </summary>
        /// <param name="channels"></param>
        private void SetChannels(IEnumerable<IChannel> channels) {

            foreach(IChannel c in channels)
            {
                ChannelSelectorItem csi = new ChannelSelectorItem { Id= c.Id, Name=c.Parameter.Name,IsChecked=false,Channel= c };

                csi.PropertyChanged += ChannelsSelectionChanged;

                _channelSelectorItems.Add(csi);

            }


            channelSelector.ItemsSource = _channelSelectorItems;

        }
        /// <summary>
        /// Обработчик выбора каналов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChannelsSelectionChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateChart();
        }

        /// <summary>
        /// Обновление графика
        /// </summary>
        public void UpdateChart()
        {

            TrendsPlot.Plot.Clear();

            TrendsPlot.Plot.SetAxisLimitsX(DateTime.Now.AddHours(-1).ToOADate(),DateTime.Now.ToOADate());

            double minY, maxY;


            RecalcAxisYSize(out minY, out maxY);


            foreach (ChannelSelectorItem item in _channelSelectorItems) {

                if (item.IsChecked)
                {

                    TrendElement[] data = item.Channel.Trend;

                    if (data.Length > 0)
                    {

                        double[] dataX = new double[data.Length];

                        double[] dataY = new double[data.Length];


                        for (int i = 0; i < data.Length; i++)
                        {
                            dataY[i] = data[i].Value;
                            dataX[i] = data[i].Time.ToOADate();

                        }

                        var plot = TrendsPlot.Plot.AddScatter(dataX, dataY);
                        plot.MarkerSize = 0;
                        plot.Label = $"{item.Name} [{item.Channel.Parameter.UnitName}]";



                        TrendsPlot.Plot.Legend(true, ScottPlot.Alignment.LowerLeft);

                        TrendsPlot.Plot.SetAxisLimitsY(minY, maxY);

                        TrendsPlot.Refresh();
                    }
                }
            }



        }
        /// <summary>
        /// Пересчет диапазона оси Y
        /// </summary>
        /// <param name="minY"></param>
        /// <param name="maxY"></param>
        private void RecalcAxisYSize(out double minY, out double maxY)
        {
            double min = 0;

            double max = Double.MinValue;

            foreach (ChannelSelectorItem item in _channelSelectorItems)
            {

                if (item.IsChecked) {

                    if (min > item.Channel.TrendMin) min = item.Channel.TrendMin;

                    if (max < item.Channel.TrendMax) max = item.Channel.TrendMax;

                }
            }


            minY = min;

            maxY = max;

        }



    }


    class ChannelSelectorItem : INotifyPropertyChanged
    {

        private bool isChecked;

        public uint Id { get; set; }
        public string Name { get; set; }
        public IChannel Channel { get; set; }


        public bool IsChecked { get {

                return isChecked;
            }

            set {

                isChecked = value;

                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }

}
