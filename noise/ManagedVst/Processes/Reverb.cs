using Noise.Framework;

namespace Noise.ManagedVst.Processes
{
	public class Reverb
	{
		const int combCount = 8;
		const int allPassCount = 4;
		const int stereoSpread = 23;
		
		const float fixedGain = 0.035f;
		const float scaleWet = 2.0f;
		const float scaleDry = 3.0f;
		const float scaleDamp = 0.1f;
		const float scaleRoom = 0.28f;
		const float roomOffSet = 0.7f;

		float roomSize;
		float damp;
		float wet, wet1, wet2;
		float dry;
		float width;

		CombFilter[] leftCombs;
		CombFilter[] rightCombs;
		AllPassFilter[] leftAllPasses;
		AllPassFilter[] rightAllPasses;

		public Reverb()
		{
			InitializeCombs();
			InitializeAllPasses();

			SetDry(0.6f);
			SetWet(0.4f);
			SetRoomSize(0.6f);
			SetDamp(0.4f);
			SetWidth(1.0f);
		}

		private void InitializeCombs()
		{
			leftCombs = new CombFilter[combCount];
			rightCombs = new CombFilter[combCount];

			int[] tunings = new int[combCount]
			{
				1116, 1188, 1277, 1356, 1422, 1491, 1557, 1617
			};
	
			for(int i = 0; i < combCount; i++)
			{
				leftCombs[i] = new CombFilter(tunings[i]);
				rightCombs[i] = new CombFilter(tunings[i] + stereoSpread);
			};
		}

		private void InitializeAllPasses()
		{
			leftAllPasses = new AllPassFilter[allPassCount];
			rightAllPasses = new AllPassFilter[allPassCount];

			int[] tunings = new int[allPassCount]
			{
				556, 441, 341, 225
			};

			for(int i = 0; i < allPassCount; i++)
			{
				leftAllPasses[i] = new AllPassFilter(tunings[i], 0.5f);
				rightAllPasses[i] = new AllPassFilter(tunings[i] + stereoSpread, 0.5f);
			}
		}

		public void ProcessReplacing(IChannelGroup inputs, IChannelGroup outputs, int sampleFrames)
		{
			SMALL_NUMBER = -SMALL_NUMBER;

			for (int sampleIndex = 0; sampleIndex < sampleFrames; sampleIndex++)
			{
				float leftOut = 0f;
				float rightOut = 0f;
				float leftIn = inputs.GetChannel(0)[sampleIndex];
				float rightIn = inputs.GetChannel(1)[sampleIndex];
				
				float summedInput = Undenormalise((leftIn + rightIn) * fixedGain);

				// Accumulate comb filters in parallel
				for (int i = 0; i < combCount; i++)
				{
					leftOut += leftCombs[i].Process(summedInput);
					rightOut += rightCombs[i].Process(summedInput);
				}

				// Feed through allpasses in series
				for (int i = 0; i < allPassCount; i++)
				{
					leftOut = leftAllPasses[i].Process(leftOut);
					rightOut = rightAllPasses[i].Process(rightOut);
				}

				outputs.GetChannel(0)[sampleIndex] = leftOut * wet1 + rightOut * wet2 + leftIn * dry;
				outputs.GetChannel(1)[sampleIndex] = rightOut * wet1 + leftOut * wet2 + rightIn * dry;
			}
		}

		static float SMALL_NUMBER = 9.8607615E-32f;

		static float Undenormalise(float sample)
		{
			return sample + SMALL_NUMBER;
		}

		private void UpdateWetValues()
		{
			wet1 = wet * (width / 2f + 0.5f);
			wet2 = wet * ((1 - width) / 2f);
		}

		#region properties
		public void SetRoomSize(float value)
		{
			roomSize = (value * scaleRoom) + roomOffSet;

			for (int i = 0; i < combCount; i++)
			{
				leftCombs[i].SetFeedback(roomSize);
				rightCombs[i].SetFeedback(roomSize);
			}
		}

		public float GetRoomSize()
		{
			return (roomSize - roomOffSet) / scaleRoom;
		}

		public void SetDamp(float value)
		{
			damp = value * scaleDamp;
			for (int i = 0; i < combCount; i++)
			{
				leftCombs[i].SetDamp(damp);
				rightCombs[i].SetDamp(damp);
			}
		}

		public float GetDamp()
		{
			return damp / scaleDamp;
		}

		public void SetWet(float value)
		{
			wet = value * scaleWet;
			UpdateWetValues();
		}

		public float GetWet()
		{
			return wet / scaleWet;
		}

		public void SetDry(float value)
		{
			dry = value * scaleDry;
		}

		public float GetDry()
		{
			return dry / scaleDry;
		}

		public void SetWidth(float value)
		{
			width = value;
			UpdateWetValues();
		}

		public float GetWidth()
		{
			return width;
		}
		#endregion
	}

}