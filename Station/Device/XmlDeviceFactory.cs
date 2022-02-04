using Station.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace Station.Device
{
    public class XmlDeviceFactory : IDeviceFactory
    {


        private string _configFilePath;

        private IEnumerable<DeviceInfo> _config;

        public XmlDeviceFactory(string configFilePath) {

            _configFilePath = configFilePath;
            
        }

        public IEnumerable<IDevice> GetDevices()
        {
            try {

                _config = ParseDeviceConfig(_configFilePath);
            }
            catch(Exception e)
            {

                throw new Exception("Ошибка загрузки конфигурации модулей устройств", e);
            }

            //ParameterCollection pc = new ParameterCollection();
           // pc.Collection = new List<SimulationParameter>();


            foreach (DeviceInfo di in _config) {

                IDevice device = CreateDeviceInstance(di);

               /* SimulationParameter par = new SimulationParameter();

                par.Mean = Convert.ToDouble(di.Parameters["Mean"]);
                par.Delta = Convert.ToDouble(di.Parameters["Delta"]);
                par.Delay = Convert.ToInt32(di.Parameters["Delay"]);
                par.Name = di.Name;
                */


                if (device != null)
                {

                 //   pc.Collection.Add(par);
                    yield return device;
                }
                else
                {

                    Logger.Log.Error($"Ошибка загрузки устройства {di.Name} ");
                }


            }

            /*XmlSerializer xmlSerializer = new XmlSerializer(typeof(ParameterCollection));

            TextWriter writer = new StreamWriter("parameters.xml");

            xmlSerializer.Serialize(writer, pc);

            writer.Close();*/

        }

        private IDevice CreateDeviceInstance(DeviceInfo di)
        {
            AppDomain domain = CreateDomain(Directory.GetCurrentDirectory());

            string assemblyFileName =  di.DLL;
                        

            string typeName = GetDeviceType(assemblyFileName, typeof(IDevice), domain);

            if (typeName != null)
            {

                System.Runtime.Remoting.ObjectHandle handle;

                try
                {
                    handle = domain.CreateInstanceFrom(assemblyFileName, typeName);
                }
                catch (MissingMethodException)
                {
                    return null;
                }
                object obj = handle.Unwrap();
                IDevice device = (IDevice)obj;

                device.DeviceName = di.Name;

                foreach (object key in di.Parameters.Keys) {

                    device.AddParameter(key.ToString(),di.Parameters[key]);
                }




                return device;
            }
            else
                return null;


        }

        private static string GetDeviceType(string assemblyFileName, Type interfaceFilter, AppDomain domain)
        {
            try
            {

                Assembly asm = domain.Load(AssemblyName.GetAssemblyName(assemblyFileName));
                Type[] types = asm.GetTypes();
                foreach (Type type in types)
                {
                    if (type.GetInterface(interfaceFilter.Name) != null)
                    {
                        return type.FullName;
                    }
                }

                return null;

            }
            catch (Exception e)
            {
                Logger.Log.Debug(e);         
                return null;

            }

            
        }


        static AppDomain CreateDomain(string path)
        {
            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationBase = path;
            return AppDomain.CreateDomain("Temporary domain", null, setup);
        }


        private IEnumerable<DeviceInfo> ParseDeviceConfig(string configFilePath)
        {
            XmlDocument xDoc = new XmlDocument();

            try
            {

                xDoc.Load(configFilePath);

            }
            catch (Exception e) {

                throw new Exception("Ошибка загрузки конфигурации модулей устройств!", e);
            }
            


            XmlElement xRoot = xDoc.DocumentElement;

            if (xRoot != null)
            {
                foreach (XmlNode xnode in xRoot)
                {

                    if (xnode.NodeType == XmlNodeType.Comment)
                        continue;

                    DeviceInfo deviceInfo = new DeviceInfo() { };

                    XmlNode attrName = xnode.Attributes.GetNamedItem("name");
                    XmlNode attrDll = xnode.Attributes.GetNamedItem("dll");

                    if (attrName != null && attrDll != null)
                    {

                        deviceInfo.Name = attrName.Value;
                        deviceInfo.DLL = attrDll.Value;


                        
                        foreach (XmlNode childnode in xnode.ChildNodes)
                        {

                            

                            XmlNode paramValue = childnode.Attributes.GetNamedItem("value");

                            if (paramValue != null) {

                                deviceInfo.Parameters.Add(childnode.Name, paramValue.Value);
                            }

                            
                        }

                        yield return deviceInfo;

                    }
                }
            }



        } 

    }


    class DeviceInfo {


        public DeviceInfo() {

            Parameters = new Hashtable();
        }


        public string Name { get; set; }
        public string DLL { get; set; }

        public Hashtable Parameters;


    }
}
