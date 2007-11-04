namespace Noise.ManagedVst.Plugins
{
	unsafe public abstract class StereoPlugin : Plugin
	{
        public StereoPlugin(string pluginName, int numberOfParameters) : base(pluginName, numberOfParameters)
		{
		}

        public override void ProcessReplacing(float** inputs, float** outputs, int sampleFrames)
        {
            float* inLeft = inputs[0];
            float* inRight = inputs[1];
            float* outLeft = outputs[0];
            float* outRight = outputs[1];
            ProcessReplacing(inLeft, inRight, outLeft, outRight, sampleFrames);
        }

        public abstract void ProcessReplacing(float* leftInput, float* rightInput, float* leftOutput, float* rightOutput, int sampleFrames);

        public override int NumberOfInputs
        {
            get { return 2; }
        }

        public override int NumberOfOutputs
        {
            get { return 2; }
        }
	}
}