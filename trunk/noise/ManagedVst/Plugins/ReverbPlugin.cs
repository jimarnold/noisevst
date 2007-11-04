using System;
using Noise.Framework;
using Noise.ManagedVst.Processes;
namespace Noise.ManagedVst.Plugins
{
	public class ReverbPlugin : Plugin
	{
		const int WET_PARAM = 0;
		const int DRY_PARAM = 1;
		const int SIZE_PARAM = 2;
		const int DAMP_PARAM = 3;
		const int WIDTH_PARAM = 4;

		Reverb reverb;

		public ReverbPlugin() : base("Net Reverb", 5)
		{
			parameterNames[0] = "Wet";
			parameterNames[1] = "Dry";
			parameterNames[2] = "Room Size";
			parameterNames[3] = "Damp";
			parameterNames[4] = "Width";

			parameterLabels[0] = "dB";
			parameterLabels[1] = "dB";
			parameterLabels[2] = "Acres";
			parameterLabels[3] = "Wet patches";
			parameterLabels[4] = "Microns";
		}

		public override void Resume()
		{
			reverb = new Reverb();
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
			reverb.ProcessReplacing(inputs, outputs, sampleFrames);
		}

		public override void SetParameter(int index, float value)
		{
			switch (index)
			{
				case WET_PARAM:
					reverb.SetWet(value);
					break;
				case DRY_PARAM:
					reverb.SetDry(value);
					break;
				case SIZE_PARAM:
					reverb.SetRoomSize(value);
					break;
				case DAMP_PARAM:
					reverb.SetDamp(value);
					break;
				case WIDTH_PARAM:
					reverb.SetWidth(value);
					break;
			}
		}

		public override float GetParameter(int index)
		{
			switch (index)
			{
				case WET_PARAM:
					return reverb.GetWet();
				case DRY_PARAM:
					return reverb.GetDry();
				case SIZE_PARAM:
					return reverb.GetRoomSize();
				case DAMP_PARAM:
					return reverb.GetDamp();
				case WIDTH_PARAM:
					return reverb.GetWidth();
				default:
					return 0f;
			}
		}

		public override string GetParameterDisplay(int index)
		{
			switch (index)
			{
				case WET_PARAM:
					return reverb.GetWet().ToString();
				case DRY_PARAM:
					return reverb.GetDry().ToString();
				case SIZE_PARAM:
					return reverb.GetRoomSize().ToString();
				case DAMP_PARAM:
					return reverb.GetDamp().ToString();
				case WIDTH_PARAM:
					return reverb.GetWidth().ToString();
				default:
					return String.Empty;
			}
		}
	}
}