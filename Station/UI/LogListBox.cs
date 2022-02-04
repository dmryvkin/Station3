using log4net.Core;
using Station.Logging;
using System;
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
    ///     <MyNamespace:LogListBox/>
    ///
    /// </summary>
    public class LogListBox : ListBox
    {
        private const int MAX_ITEMS = 1000;

        static LogListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LogListBox), new FrameworkPropertyMetadata(typeof(LogListBox)));
        }


        public  void AddLogEvent(LoggingEvent loggingEvent)
        {
            DateTime time = loggingEvent.TimeStamp.ToLocalTime();


            string message = loggingEvent.MessageObject.ToString();

            int index =0;

            if (loggingEvent.Level == Level.Error)

                index =this.Items.Add(new LogItem() { Message = message, Time = time.ToString(), Foreground = "Red" });
            else

                 if (loggingEvent.Level == Level.Debug)
                    index =this.Items.Add(new LogItem() { Message = message, Time = time.ToString(), Foreground = "LightGray" });
            else
                index = this.Items.Add(new LogItem() { Message = message, Time = time.ToString() });


            if (this.Items.Count > MAX_ITEMS)
                this.Items.RemoveAt(0);

            this.ScrollIntoView(this.Items[index]);

        }

    }
}
