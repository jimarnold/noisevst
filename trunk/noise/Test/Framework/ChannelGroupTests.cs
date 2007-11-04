using Noise.Framework;
using NUnit.Framework;

namespace Noise.Test.Framework
{
    [TestFixture]
    public class ChannelGroupTests
    {
        [Test]
        public void ShouldAllocateChannelsAtConstructionTime()
        {
            ChannelGroup channels = new ChannelGroup(4);

            Assert.AreEqual(4, channels.Count);
            Assert.IsNotNull(channels.GetChannel(0));
            Assert.IsNotNull(channels.GetChannel(1));
            Assert.IsNotNull(channels.GetChannel(2));
            Assert.IsNotNull(channels.GetChannel(3));
        }
    }
}