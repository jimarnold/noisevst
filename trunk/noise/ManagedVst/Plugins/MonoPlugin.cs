using System;
using System.Runtime.InteropServices;

namespace Noise.ManagedVst.Plugins
{
    unsafe abstract public class MonoPlugin : Plugin
    {
        private float[] safeInput;
        private float[] safeOutput;

        protected MonoPlugin(string pluginName, int numberOfParameters) : base(pluginName, numberOfParameters)
        {
        }

        protected abstract void ProcessReplacing(float[] input, float[] output);

        public override void ProcessReplacing(float** input, float** output, int sampleFrames)
        {
            Marshal.Copy(new IntPtr(*input), safeInput, 0, sampleFrames);

            ProcessReplacing(safeInput, safeOutput);

            Marshal.Copy(safeOutput, 0, new IntPtr(*output), sampleFrames);
        }

        public override int NumberOfInputs
        {
            get { return 1; }
        }

        public override int NumberOfOutputs
        {
            get { return 1; }
        }

        public override bool CanMono
        {
            get { return true; }
        }

        public override void SetBlockSize(int blockSize)
        {
            safeInput = new float[blockSize];
            safeOutput = new float[blockSize];
        }
    }
}