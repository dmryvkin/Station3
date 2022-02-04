using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Station.Device
{


    public class SimulationParameter
    {
        public string Name { get; set; }
        public double Mean { get; set; }
        public double Delta { get; set; }
        public int Delay { get; set; }

        [XmlIgnore]
        public bool IsAnalyzer
        {

            get
            {
                return Delay > 0;
            }

        }


    }

    public class ParameterCollection
    {
        [XmlArray("Parameters"), XmlArrayItem("Parameter")]
        public List<SimulationParameter> Collection { get; set; }
    }
}
