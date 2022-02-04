using Station.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WQMSDevice
{
    public interface IDataBlock
    {
        Analysis[] Data { get; }

        void Read(); 
    }
}
