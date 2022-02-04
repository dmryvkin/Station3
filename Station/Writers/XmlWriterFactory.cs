using System;
using System.Collections.Generic;
using Station.Device;
using System.Collections;
using System.Xml;
using System.Reflection;
using Station.Logging;
using System.IO;

namespace Station.Writers
{
    public class XmlWriterFactory : IWriterFactory
    {
        public IEnumerable<IDataWriter> GetCurrentWriters()
        {
            return GetWriters(false);
        }

        public IEnumerable<IDataWriter> GetTargetWriters()
        {
            return GetWriters(true);
        }

        private string _configFilePath;

        private IEnumerable<WriterInfo> _config;

        public XmlWriterFactory(string configFilePath)
        {

            _configFilePath = configFilePath;

        }

        public IEnumerable<IDataWriter> GetWriters(bool target )
        {
            try
            {

                _config = ParseConfig(_configFilePath);
            }
            catch (Exception e)
            {

                throw new Exception("Ошибка загрузки конфигурации обработчиков", e);
            }



            foreach (WriterInfo wi in _config)
            {
                if (wi.Target == target)
                {

                    IDataWriter writer = CreateDeviceInstance(wi);

                    if (writer != null)
                        yield return writer;
                    else
                    {

                        Logger.Log.Error($"Ошибка загрузки обработчика {wi.DLL} ");
                    }
                }

            }


        }

        private IDataWriter CreateDeviceInstance(WriterInfo di)
        {
            AppDomain domain = CreateDomain(Directory.GetCurrentDirectory());

            string assemblyFileName = di.DLL;


            string typeName = GetType(assemblyFileName,  domain);

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

                IDataWriter writer = (IDataWriter)obj;

              

                foreach (object key in di.Parameters.Keys)
                {

                    writer.AddParameter(key.ToString(), di.Parameters[key]);
                }




                return writer;
            }
            else
                return null;


        }

        private static string GetType(string assemblyFileName,  AppDomain domain)
        {
            try
            {

                Assembly asm = domain.Load(AssemblyName.GetAssemblyName(assemblyFileName));
                Type[] types = asm.GetTypes();
                foreach (Type type in types)
                {
                    if (type.GetInterface(typeof(IDataWriter).Name) != null)
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


        private IEnumerable<WriterInfo> ParseConfig(string configFilePath)
        {
            XmlDocument xDoc = new XmlDocument();

           
            try
            {

                xDoc.Load(configFilePath);

            }
            catch (Exception e)
            {

                throw new Exception("Ошибка загрузки конфигурации обработчиков звписи", e);
            }



            XmlElement xRoot = xDoc.DocumentElement;

            if (xRoot != null)
            {
                foreach (XmlNode xnode in xRoot)
                {

                    if (xnode.NodeType == XmlNodeType.Comment)
                            continue;

                        WriterInfo writerInfo = new WriterInfo() { };

                        XmlNode attrTarget = xnode.Attributes.GetNamedItem("target");
                        XmlNode attrDll = xnode.Attributes.GetNamedItem("dll");

                        if (attrTarget != null && attrDll != null)
                        {

                            writerInfo.Target = Convert.ToBoolean(attrTarget.Value);
                            writerInfo.DLL = attrDll.Value;



                            foreach (XmlNode childnode in xnode.ChildNodes)
                            {



                                XmlNode paramValue = childnode.Attributes.GetNamedItem("value");

                                if (paramValue != null)
                                {

                                    writerInfo.Parameters.Add(childnode.Name, paramValue.Value);
                                }


                            }

                            yield return writerInfo;

                        }

                    
                   
                }
            }



        }

    }


    class WriterInfo
    {


        public WriterInfo()
        {

            Parameters = new Hashtable();
        }


      
        public string DLL { get; set; }
        public bool Target { get; set; }


        public Hashtable Parameters;


    }


}
