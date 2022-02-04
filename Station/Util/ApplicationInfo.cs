using System.Reflection;

namespace Station.Util
{
    public class ApplicationInfo
    {
        public static string Title
        {

            get
            {

                object[] title = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                return ((AssemblyTitleAttribute)title[0]).Title;

            }
        }

        public static string Version
        {
            get
            {

                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            }


        }

        public static string Copyright
        {
            get
            {
                object[] copyright = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

                return ((AssemblyCopyrightAttribute)copyright[0]).Copyright;

            }


        }
    }

}
