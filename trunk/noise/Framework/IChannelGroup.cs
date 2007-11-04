namespace Noise.Framework
{
    public interface IChannelGroup
    {
        int Count
        {
            get;
        }

        IChannel GetChannel(int index);
    }
}
