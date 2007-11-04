using System.Collections.Generic;
using Noise.Framework.Unsafe;

namespace Noise.Framework
{
    public class ChannelGroup : IChannelGroup
    {
        private readonly IList<IChannel> channels;

        public ChannelGroup(int channelCount)
        {
            channels = new List<IChannel>();

            for (int i = 0; i < channelCount; i++)
            {
                channels.Add(new Channel());
            }
        }

        public int Count
        {
            get
            {
                return channels.Count;
            }
        }

        public IChannel GetChannel(int index)
        {
            return channels[index];
        }
    }
}