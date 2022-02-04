using System.Windows;

namespace Station.UI
{
    /// <summary>
    /// Interaction logic for ConfigurationDialogBox.xaml
    /// </summary>
    public partial class ConfigurationDialogBox : Window
    {
        public ConfigurationDialogBox()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
