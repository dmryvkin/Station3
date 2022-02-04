using Station.Device;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System;

namespace Station.UI
{
    /// <summary>
    /// Interaction logic for ChannelsDialogBox.xaml
    /// </summary>
    public partial class ChannelsDialogBox : Window
    {

        private ObservableCollection<IChannel> _channels;

        private IChannel _selected;

        public IEnumerable<IChannel> Channels
        {
          get {
                return _channels;
            }

          set {
                _channels = new ObservableCollection<IChannel>(value);
                channelsListBox.ItemsSource = _channels;

            }
        }

        public ChannelsDialogBox()
        {
            InitializeComponent();


            comboBoxDevices.ItemsSource = GetDevicesNames();

        }

        private void channelsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
            if (e.AddedItems.Count > 0) {

                Channel c = (Channel)e.AddedItems[0];

                _selected =c;

                this.DataContext = c;




                checkBoxActive.IsEnabled = true;
                checkBoxFlag.IsEnabled = true;
                comboBoxDevices.IsEnabled = true;
                textBoxName.IsEnabled = true;
                textBoxCode.IsEnabled = true;
                textBoxPosition.IsEnabled = true;
                textBoxPrec.IsEnabled = true;
                textBoxSign.IsEnabled = true;
                textBoxUnit.IsEnabled = true;
                textBoxMax.IsEnabled = true;
                textBoxMin.IsEnabled = true;

              }
            else
            {
                checkBoxActive.IsEnabled = false;
                checkBoxFlag.IsEnabled = false;
                comboBoxDevices.IsEnabled = false;
                textBoxName.IsEnabled = false;
                textBoxCode.IsEnabled = false;
                textBoxPosition.IsEnabled = false;
                textBoxPrec.IsEnabled = false;
                textBoxSign.IsEnabled = false;
                textBoxUnit.IsEnabled = false;
                textBoxMax.IsEnabled = false;
                textBoxMin.IsEnabled = false;

            }

        }


        private List<string> GetDevicesNames() {
            List<string> devices = new List<string>();

            foreach (IDevice d in DeviceManager.Instance.Devices) {
                devices.Add(d.DeviceName);
            }

            return devices;
        }

        private void buttonApply_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selected != null)
            {

                if (MessageBox.Show($"Вы хотите удалить измерительный канал {_selected.Parameter.Name}?", "Удаление канала", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    DeleteChannel();
                }

            }

        }

        private void DeleteChannel()
        {
            _channels.Remove(_selected);

            this.DataContext = null;
            _selected = null;
        }

        private void buttonNew_Click(object sender, RoutedEventArgs e)
        {
            var newChannel =  AddChannel();

            channelsListBox.SelectedItem = newChannel;
            channelsListBox.ScrollIntoView(newChannel);
        }

        private IChannel  AddChannel()
        {
            uint newId = _channels[_channels.Count - 1].Id + 1;
            Channel newChannel = new Channel() { Id = newId };

            _channels.Add(newChannel);

            return newChannel;

         

        }
    }
}
