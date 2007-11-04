namespace Noise.ManagedVst.Processes
{
	public class AllPassFilter
	{
		float feedback;
		float[] buffer;
		int bufferIndex;

		public AllPassFilter(int length, float feedback)
		{
			SetFeedback(feedback);
			buffer = new float[length];
			bufferIndex = 0;
		}

		public float Process(float input)
		{
			float output = -input + buffer[bufferIndex];

			buffer[bufferIndex] = input + (buffer[bufferIndex] * feedback);

			if (++bufferIndex >= buffer.Length)
			{
				bufferIndex = 0;
			}

			return output;
		}

		private void SetFeedback(float feedback)
		{
			this.feedback = feedback;
		}
	}
}