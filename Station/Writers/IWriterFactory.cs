using Station.Device;
using System.Collections.Generic;

namespace Station.Writers
{
    interface IWriterFactory
    {

        IEnumerable<IDataWriter> GetTargetWriters();
        IEnumerable<IDataWriter> GetCurrentWriters();

    }
}
