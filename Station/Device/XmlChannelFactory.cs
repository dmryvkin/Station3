using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Station.Device
{
    public class XmlChannelFactory : IChannelFactory
    {
        string _configFile;

        public XmlChannelFactory(string configFile)
        {

            _configFile = configFile;

        }


        public IEnumerable<IChannel> GetChannels()
        {

            try
            {

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ChannelCollection));

                TextReader reader = new StreamReader(_configFile);

                ChannelCollection channels = (ChannelCollection)xmlSerializer.Deserialize(reader);

                reader.Close();

                return channels.Collection;
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка загрузки конфигурации измерительных каналов",e);
            }
            
        }

        public  void Save(IEnumerable<IChannel> channels)
        {
            try
            {
                ChannelCollection cc = new ChannelCollection() { Collection = new List<Channel>() };

                foreach (IChannel c in channels) {

                    cc.Collection.Add((Channel)c);

                }

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ChannelCollection));

                TextWriter writer = new StreamWriter(_configFile);

                xmlSerializer.Serialize(writer, cc);

                writer.Close();

               
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка загрузки конфигурации измерительных каналов", e);
            }


        }

    }
}
