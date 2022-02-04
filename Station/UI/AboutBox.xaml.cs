using Station.Util;
using System.Windows;

namespace Station.UI
{
    /// <summary>
    /// Interaction logic for AboutBox.xaml
    /// </summary>
    public partial class AboutBox : Window
    {
        public AboutBox()
        {
            InitializeComponent();

        

            labelTitle.Content = ApplicationInfo.Title;
            labelVersion.Content = $"Версия: {ApplicationInfo.Version}";
            labelCopyright.Content = ApplicationInfo.Copyright;


        }
    }
}
