using System;
using Noise.Framework;
using Noise.ManagedVst.Processes;

namespace Noise.ManagedVst.Plugins
{
	public class DelayPlugin : Plugin
	{
		private Delay leftDelay;
		private Delay rightDelay;

		private const int DELAY_TIME_PARAM = 0;
		private const int FEEDBACK_GAIN_PARAM = 1;
		private const int DRY_LEVEL_PARAM = 2;
		private const int WET_LEVEL_PARAM = 3;
		private const int MAX_DELAY_TIME = 1000;

		public DelayPlugin() : base("Net Delay", 4)
		{
			parameterLabels[0] = "ms";
			parameterLabels[1] = "dB";
			parameterLabels[2] = "dB";
			parameterLabels[3] = "dB";

			parameterNames[0] = "Delay Time"; 
			parameterNames[1] = "Feedback Gain"; 
			parameterNames[2] = "Dry Level"; 
			parameterNames[3] = "Wet Level";
		}

		public override void Resume()
		{
			leftDelay = new Delay(500f, 0.2f, 0.8f, 0.5f, sampleRate);
			rightDelay = new Delay(500f, 0.2f, 0.8f, 0.5f, sampleRate);
		}

		public override bool HasEditor
		{
			get { return false; }
		}

	    public override int NumberOfInputs
	    {
	        get { return 2; }
	    }

	    public override int NumberOfOutputs
	    {
	        get { return 2; }
	    }

	    public override void ProcessReplacing(IChannelGroup inputs, IChannelGroup outputs, int sampleFrames)
		{
			for(int i = 0; i < sampleFrames; i++)
			{
				outputs.GetChannel(0)[i] = leftDelay.Process(inputs.GetChannel(0)[i]);
				outputs.GetChannel(1)[i] = rightDelay.Process(inputs.GetChannel(1)[i]);
			}
		}

		public override float GetParameter(int index)
		{
			switch(index)
			{
				case DELAY_TIME_PARAM :
					return leftDelay.DelayTime / MAX_DELAY_TIME;
				case FEEDBACK_GAIN_PARAM :
					return leftDelay.Feedback;
				case DRY_LEVEL_PARAM :
					return leftDelay.DryLevel;
				case WET_LEVEL_PARAM :
					return leftDelay.WetLevel;
				default :
					return 0f;
			}
		}

		public override void SetParameter(int index, float normalisedValue)
		{
			switch(index)
			{
				case DELAY_TIME_PARAM :
					float realValue = normalisedValue * MAX_DELAY_TIME;
					leftDelay.DelayTime = realValue;
					rightDelay.DelayTime = realValue;
					break;
				case FEEDBACK_GAIN_PARAM :
					leftDelay.Feedback = normalisedValue;
					rightDelay.Feedback = normalisedValue;
					break;
				case DRY_LEVEL_PARAM :
					leftDelay.DryLevel = normalisedValue;
					rightDelay.DryLevel = normalisedValue;
					break;
				case WET_LEVEL_PARAM :
					leftDelay.WetLevel = normalisedValue;
					rightDelay.WetLevel = normalisedValue;
					break;
			}
		}

		public override string GetParameterDisplay(int index)
		{
			switch(index)
			{
				case DELAY_TIME_PARAM :
					return leftDelay.DelayTime.ToString();
				case FEEDBACK_GAIN_PARAM :
					return ToDbString(leftDelay.Feedback);
				case DRY_LEVEL_PARAM :
					return ToDbString(leftDelay.DryLevel);
				case WET_LEVEL_PARAM :
					return ToDbString(leftDelay.WetLevel);
				default:
					return String.Empty;
			}
		}
	}
}