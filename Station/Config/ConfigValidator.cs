using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Station.Config
{
    public class ConfigValidator
    {

        public static bool Validate(StationConfiguration config) {

            return (config.Period >= 0 && config.Period <= 180);

        } 

    }
}
