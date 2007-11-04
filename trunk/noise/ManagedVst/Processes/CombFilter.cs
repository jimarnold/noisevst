namespace Noise.ManagedVst.Processes
{
	public class CombFilter
	{
		float feedback;

		float damp1;
		float damp2;
		float[] buffer;
		int bufferIndex;

		float lastInput;

		public CombFilter(int length)
		{
			buffer = new float[length];
		}

		public float Process(float input)
		{
			lastInput = (buffer[bufferIndex] * damp2) + (lastInput * damp1);

			buffer[bufferIndex] = input + (lastInput * feedback);

			if (++bufferIndex >= buffer.Length)
			{
				bufferIndex = 0;
			}

			return buffer[bufferIndex];
		}

		public void SetDamp(float value)
		{
			damp1 = value;
			damp2 = (1 - value);
		}

		public void SetFeedback(float value)
		{
			feedback = value;
		}
	}
}