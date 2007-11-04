namespace Noise.ManagedVst.Processes
{
	public class Filter
	{
		private FilterType filterType;
		private float frequency;
		private float bandwidth;

		BiQuad quad;
		private float sampleRate;

		public Filter(FilterType filterType, float frequency, float bandwidth, float sampleRate)
		{
			this.filterType = filterType;
			this.frequency = frequency;
			this.bandwidth = bandwidth;
			this.sampleRate = sampleRate;

			CreateFilter();
		}

		public float Process(float input)
		{
			return quad.Step(input);
		}		

		private void CreateFilter()
		{
			quad = new BiQuad(filterType, sampleRate, frequency, bandwidth);
		}

		public float Frequency
		{
			get { return frequency; }
			set { frequency = value; CreateFilter(); }
		}

		public float Bandwidth
		{
			get { return bandwidth; }
			set { bandwidth = value;  CreateFilter(); }
		}

		public FilterType FilterType
		{
			get { return filterType; }
			set { filterType = value; CreateFilter(); }
		}
	}
}