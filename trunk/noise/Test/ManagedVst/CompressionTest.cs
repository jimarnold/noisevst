using System.Collections.Generic;
using Noise.Framework;
using Noise.ManagedVst.Plugins;
using NUnit.Framework;

namespace Noise.Test.ManagedVst.Plugins
{
	[TestFixture]
	public class CompressionTest
	{
		[Test]
		public void ShouldBypassWhenUnderThreshold()
		{
			CompressorPlugin plugin = new CompressorPlugin();
			plugin.SetParameter(0, 0.5f);
			plugin.SetParameter(1, 0.5f);
			plugin.SetParameter(2, 0.5f);

			int sampleFrames = 100;

			float[] inLeft = new float[sampleFrames];
			float[] inRight = new float[sampleFrames];

			for(int i = 0; i < sampleFrames; i++)
			{
				inLeft[i] = 0.4f;
				inRight[i] = 0.4f;
			}

            SafeChannelGroup inputs = new SafeChannelGroup();
            SafeChannelGroup outputs = new SafeChannelGroup();

            inputs.AddChannel(new SafeChannel(inLeft));
            inputs.AddChannel(new SafeChannel(inRight));
            outputs.AddChannel(new SafeChannel(inLeft));
            outputs.AddChannel(new SafeChannel(inRight));

		    plugin.ProcessReplacing(inputs, outputs, sampleFrames);

			for(int i = 0; i < sampleFrames; i++)
			{
				Assert.AreEqual(0.4f, inLeft[i]);
				Assert.AreEqual(0.4f, inRight[i]);
			}
		}

	    [Test]
	    public void ShouldCompressWhenOverThreshold()
	    {
	        CompressorPlugin plugin = new CompressorPlugin();
	        plugin.SetParameter(0, 0.001f);
	        plugin.SetParameter(1, 0.0001f);
	        plugin.SetParameter(2, 0.0001f);
	        plugin.SetParameter(3, 0.5f);
	        plugin.SetParameter(4, 0.5f);

	        int sampleFrames = 441;

	        float[] inLeft = new float[sampleFrames];
	        float[] inRight = new float[sampleFrames];

	        for(int i = 0; i < sampleFrames; i++)
	        {
	            inLeft[i] = 0.99f;
	            inRight[i] = 0.99f;
	        }

	        SafeChannelGroup inputs = new SafeChannelGroup();
	        SafeChannelGroup outputs = new SafeChannelGroup();

            inputs.AddChannel(new SafeChannel(inLeft));
            inputs.AddChannel(new SafeChannel(inRight));
            outputs.AddChannel(new SafeChannel(inLeft));
            outputs.AddChannel(new SafeChannel(inRight));

            plugin.ProcessReplacing(inputs, outputs, sampleFrames);

	        for(int i = 0; i < sampleFrames; i++)
	        {
	            Assert.IsTrue(inLeft[i] <= 0.99f, inLeft[i].ToString());
	            Assert.IsTrue(inRight[i] <= 0.99f, inLeft[i].ToString());
	        }
	    }

        public class SafeChannelGroup : IChannelGroup
        {
            private IList<IChannel> channels;

            public SafeChannelGroup()
            {
                channels = new List<IChannel>();
            }

            public int Count
            {
                get { return channels.Count; }
            }

            public IChannel GetChannel(int index)
            {
                return channels[index];
            }

            public void AddChannel(IChannel channel)
            {
                channels.Add(channel);
            }
        }

	    public class SafeChannel : IChannel
	    {
	        private float[] buffer;

	        public SafeChannel(float[] buffer)
	        {
	            this.buffer = buffer;
	        }

	        public float this[int index]
	        {
	            get { return buffer[index]; }
	            set { buffer[index] = value; }
	        }
	    }
	}
}