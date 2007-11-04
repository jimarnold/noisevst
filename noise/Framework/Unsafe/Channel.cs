using System;

namespace Noise.Framework.Unsafe
{
    unsafe public class Channel : ISwitchingChannel
    {
        protected float* pBuffer;
        protected int length;

        //If this is set while a plugin is accessing the old buffer, it could
        //screw things up.  So, er, don't
        public void SetBuffer(float* pNewBuffer, int newLength)
        {
            pBuffer = pNewBuffer;
            length = newLength;
        }

        public float this[int index]
        {
            get
            {
                if (index >= length)
                {
                    throw new InvalidOperationException("Index exceeded length of channel");
                }
                return pBuffer[index];
            }
            set
            {
                if (index >= length)
                {
                    throw new InvalidOperationException("Index exceeded length of channel");
                }
                pBuffer[index] = value;
            }
        }
    }
}