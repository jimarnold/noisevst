namespace Noise.ManagedVst.Processes
{
	public class Gain
	{
		float level = 1.0f;

		public Gain(float level)
		{
			this.level = level;
		}

		public float Process(float input)
		{
			return input * level;
		}

		public float Level
		{
			get { return level; }
			set { level = value; }
		}
	}
}