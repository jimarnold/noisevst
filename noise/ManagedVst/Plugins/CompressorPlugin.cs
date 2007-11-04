using System;
using Noise.Framework;

namespace Noise.ManagedVst.Plugins
{
	/*
	 * This plugin is NOT finished.  It doesn't really work as a proper compressor yet.
	 * The RMS windowing code seems to work OK, but the sidechain is screwed :-(
	 * Feel free to hack around with it, though :-)
	*/
	public class CompressorPlugin : Plugin
	{
		float threshold = 0.05f;
		float attackTime = 0.001f;
		float releaseTime = 0.1f;
		float slope = 0.2f;
		float gain = 1f;

		int windowSamples;
		float theta;
		float targetGain;
		int state;
		const int STABLE = 0;
		const int ATTACKING = 1;
		const int RELEASING = 2;

		public CompressorPlugin() : base("Compressor", 4)
		{
			parameterLabels[0] = "dB";
			parameterLabels[1] = "ms";
			parameterLabels[2] = "ms";
			parameterLabels[3] = "dB";

			parameterNames[0] = "Threshold"; 
			parameterNames[1] = "Attack"; 
			parameterNames[2] = "Release"; 
			parameterNames[3] = "Slope"; 

			float windowTime = 0.001f;

			windowSamples = (int)(sampleRate * windowTime);

            SetID("COMP");
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
				float level = GetLevel(i, sampleFrames, inputs.GetChannel(0), inputs.GetChannel(1));

				SideChain(level);

				outputs.GetChannel(0)[i] = inputs.GetChannel(0)[i] * gain;
                outputs.GetChannel(1)[i] = inputs.GetChannel(1)[i] * gain;
			}
		}

		private void SideChain(float level)
		{
			if(level > threshold)
			{
				if(state == ATTACKING)
				{
					KeepAttacking();
				}
				else
				{
					float signalAboveThreshold = level - threshold;
					targetGain = (1f - signalAboveThreshold) * slope;

					StartAttacking();
				}
			}
			else
			{
				if(gain != 1f)
				{
					if(state == RELEASING)
					{
						KeepReleasing();
					}
					else
					{
						StartReleasing();
					}					
				}
			}
		}

		private void KeepAttacking()
		{
			if(gain <= targetGain)
			{
				gain = targetGain;
				state = STABLE;
			}
			else
			{
				gain -= theta;
			}
		}

		private void KeepReleasing()
		{
			if(gain >= 1f)
			{
				gain = 1f;
				state = STABLE;
			}
			else
			{
				gain += theta;
			}
		}

		private void StartAttacking()
		{
			float steps = attackTime * sampleRate;
			theta = (gain - targetGain) / steps;

			state = ATTACKING;
		}

		private void StartReleasing()
		{
			float steps = releaseTime * sampleRate;
			theta = (1f - gain) / steps;

			state = RELEASING;
		}

        private float GetLevel(int samplePosition, int sampleFrames, IChannel inLeft, IChannel inRight)
		{
			int samplesLeft = sampleFrames - samplePosition;
			if( samplesLeft < windowSamples)
			{
				windowSamples = samplesLeft;
			}

			float sum = 0f;

			for(int i = 0; i < windowSamples; i++)
			{
				sum += (Math.Abs(inLeft[i + samplePosition] * 0.5f)) + (Math.Abs(inRight[i + samplePosition]) * 0.5f);
			}

			return sum / windowSamples;
		}

		public override void SetParameter(int index, float value)
		{
			switch(index)
			{
				case 0:
					threshold = value;
					break;
				case 1:
					if(value >= 0.001f)
						attackTime = value / 10f;
					break;
				case 2:
					if(value >= 0.001f)
						releaseTime = value;
					break;
				case 3:
					slope = value;
					break;
			}
		}

		public override float GetParameter(int index)
		{
			switch(index)
			{
				case 0:
					return threshold;
				case 1:
					return attackTime * 10f;
				case 2:
					return releaseTime;	
				case 3:
					return slope;
				default:
					return 0f;
			}
		}

		public override string GetParameterDisplay(int index)
		{
			switch(index)
			{
				case 0:
					return Amplitude2Db(threshold).ToString();
				case 1:
					return (attackTime * 1000f).ToString();
				case 2:
					return (releaseTime * 1000f).ToString();	
				case 3:
					return (slope).ToString();
				default:
					return (0f).ToString();
			}
		}
	}
}