using System.Collections.Generic;

namespace Station.Device
{
    public interface IChannelFactory
    {

        IEnumerable<IChannel> GetChannels();
        void Save(IEnumerable<IChannel> channels);
    }
}
