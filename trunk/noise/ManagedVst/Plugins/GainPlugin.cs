using System;
using Noise.Framework;
using Noise.ManagedVst.Processes;

namespace Noise.ManagedVst.Plugins
{
    public class GainPlugin : Plugin
    {
        private Gain gain;
        const int GAIN_PARAM = 0;

        public GainPlugin() : base("Gain", 2)
        {
            parameterNames[0] = "Level";
            parameterLabels[0] = "dB";
        }

        public override void ProcessReplacing(IChannelGroup inputs, IChannelGroup outputs, int sampleFrames)
        {
            for(int i = 0; i < sampleFrames; i++)
            {
                outputs.GetChannel(0)[i] = gain.Process(inputs.GetChannel(0)[i]);
            }
        }

        public override void Resume()
        {
            gain = new Gain(1f);
        }

        public override void SetParameter(int index, float value)
        {
            switch (index)
            {
                case GAIN_PARAM:
                    gain.Level = value;
                    break;
            }
        }

        public override float GetParameter(int index)
        {
            switch (index)
            {
                case GAIN_PARAM:
                    return gain.Level;
                default:
                    return 0f;
            }
        }

        public override string GetParameterDisplay(int index)
        {
            switch (index)
            {
                case GAIN_PARAM:
                    return gain.Level.ToString();
                default:
                    return String.Empty;
            }
        }

        public override int NumberOfInputs
        {
            get { return 1; }
        }

        public override int NumberOfOutputs
        {
            get { return 1; }
        }
    }
}
