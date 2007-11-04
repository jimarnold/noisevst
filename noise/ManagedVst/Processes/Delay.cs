namespace Noise.ManagedVst.Processes
{
	public class Delay
	{
		float samplingRate;
		float[] delayLine;
		int delayLineIndex;
		int bufferLength;
		private float delayTime;
		private float feedback;
		private float dryLevel;
		private float wetLevel;

		private const int MAX_DELAY_TIME = 1000;

		public Delay(float delayTime, float feedBack, float dryLevel, float wetLevel, float samplingRate)
		{
			this.delayTime = delayTime;
			this.feedback = feedBack;
			this.dryLevel = dryLevel;
			this.wetLevel = wetLevel;
			this.samplingRate = samplingRate;

			SetBufferLength(MAX_DELAY_TIME);

			delayLine = new float[bufferLength];

			SetBufferLength(delayTime);

			delayLineIndex = 0;
		}

		public float Process(float input)
		{
			float output = (dryLevel * input) + (wetLevel * delayLine[delayLineIndex]);

			delayLine[delayLineIndex] = input + (feedback * delayLine[delayLineIndex]);

			if (++delayLineIndex >= bufferLength)
			{
				delayLineIndex = 0;
			}

			return output;
		}

		public float DelayTime
		{
			get { return delayTime; }
			set 
			{
				this.delayTime = value;
				SetBufferLength(delayTime);	
			}
		}

		public float Feedback
		{
			get { return feedback; }
			set { feedback = value; }
		}

		public float DryLevel
		{
			get { return dryLevel; }
			set { dryLevel = value; }
		}

		public float WetLevel
		{
			get { return wetLevel; }
			set { wetLevel = value; }
		}

		private void SetBufferLength(float delayTime)
		{
			bufferLength = (int)(delayTime * samplingRate / 1000);
		}
	}
}