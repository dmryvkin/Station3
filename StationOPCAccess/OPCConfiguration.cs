using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StationOPCAccess
{
    [Serializable]
    public class OPCConfiguration
    {
        public string OPCPrefix { get; set; }
        public int MaxTags { get; set; }
        public int StationID { get; set; }
        public int PollInterval { get; set; }
        public string ConnectionString { get; set; }
        public int MaxDataAge { get; set; }




        public static OPCConfiguration Create()
        {

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(OPCConfiguration));

            TextReader reader = new StreamReader($"{AppDomain.CurrentDomain.BaseDirectory}\\Data\\OPC.xml");

            OPCConfiguration newInstance = (OPCConfiguration)xmlSerializer.Deserialize(reader);

            reader.Close();

            return newInstance;

        }

        
    }
}
