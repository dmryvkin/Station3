using System;
using System.IO;
using System.Xml.Serialization;

namespace Station.Config
{
    [Serializable]
    public class StationConfiguration
    {
     

        public int ID{get; set;}

        public string Name { get; set; }

        public int Period { get; set; }

        public bool WriteAlarmEnabled { get; set; }

        public bool ModbusEnabled { get; set; }
                

        public static StationConfiguration Create() {

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(StationConfiguration));

            TextReader reader = new StreamReader("Data/Station.xml");

            StationConfiguration newInstance = (StationConfiguration)xmlSerializer.Deserialize(reader); 

            reader.Close();

            return newInstance;

        }

        public  void Save()
        {

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(StationConfiguration));

            TextWriter writer = new StreamWriter("Data/Station.xml");

            xmlSerializer.Serialize(writer, this);

            writer.Close();

        }



    }
}
