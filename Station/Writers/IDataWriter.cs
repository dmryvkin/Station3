using System;
using System.Collections.Generic;

namespace Station.Device
{
    public interface IDataWriter
    {

     
        void WriteData(int id, int period, DateTime time, bool emergency, IEnumerable<IChannel> enumerable);

        void AddParameter(string v1, object v2);

        bool   IsBusy { get; }

        void OpenWriter(int stationID);

        void CloseWriter();


    }
}
