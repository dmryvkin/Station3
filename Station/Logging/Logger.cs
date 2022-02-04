using log4net.Appender;
using log4net;


namespace Station.Logging
{
    public class Logger
    {

        private static ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public static ILog Log
        {
            get { return log; }
        }

        public static void AddAppender(IAppender appender)
        {
            ((log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository()).Root.AddAppender(appender);
        }

        public static void RemoveAppender(IAppender appender)
        {
            ((log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository()).Root.RemoveAppender(appender);
        }

    }
}
