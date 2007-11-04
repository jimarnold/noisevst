using System;
using Noise.Framework.Unsafe;
using NUnit.Framework;

namespace Noise.Test.Framework
{
    [TestFixture]
    public class ChannelTests
    {
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        unsafe public void ShouldThrowIfAccessAttemptedBeyondEndOfBuffer()
        {
            ISwitchingChannel channel = new Channel();
            float* pBuffer = stackalloc float[2];

            pBuffer[0] = 1.0f;
            pBuffer[1] = 1.0f;

            channel.SetBuffer(pBuffer, 2);

            Assert.AreEqual(1.0f, channel[0]);
            Assert.AreEqual(1.0f, channel[1]);
            
            float sample = channel[2];
        }
    }
}