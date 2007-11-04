namespace Noise.Framework
{
    public interface IChannel
    {
        float this[int index]
        {
            get;
            set;
        }
    }
}