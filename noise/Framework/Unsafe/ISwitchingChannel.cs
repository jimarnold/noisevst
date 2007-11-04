namespace Noise.Framework.Unsafe
{
    unsafe public interface ISwitchingChannel : IChannel
    {
        void SetBuffer(float* pNewBuffer, int newLength);
    }
}