using System;
namespace Noise.ManagedVst.Processes
{
	public enum FilterType : byte
	{
		LPF,
		HPF,
	}

	//Ported and adapted from Tom St Denis' C version (http://www.musicdsp.org/showone.php?id=64)
	public class BiQuad
	{
		//Coefficients
		double A0, A1, A2, A3, A4;

		//X1 is the input from the previous step, X2 is the input before that
		//Y1 is the output from the previous step, Y2 is the output before that
		double X1, X2, Y1, Y2;

		public BiQuad(FilterType filterType, double sampleRate, double frequency, double bandwidth)
		{
			//A frequency of 0 can lead to very small (denormal) numbers and NaNs.  This is bad because
			//most processors are very slow at calculating very small floating point numbers, so we should
			//clamp such values to something larger.  This might be slightly superstitious, but including
			//the following check banished 100% CPU usage spikes.  Alternatives welcome.
			if(frequency == 0d)
			{
				frequency = 0.00000000000001d;
			}

			const double LogE2 = 0.69314718055994530942;

			double omega = 2 * Math.PI * frequency / sampleRate;
			double sinOmega = Math.Sin(omega);
			double cosOmega = Math.Cos(omega);
			double alpha = sinOmega * Math.Sinh(LogE2 / 2 * bandwidth * omega / sinOmega);
								
			double a0 = 1 + alpha;
			double a1 = -2 * cosOmega;
			double a2 = 1 - alpha;

			double b0 = 0;
			double b1 = 0;
			double b2 = 0;

			if (filterType == FilterType.LPF)
			{
				b0 = (1 - cosOmega) / 2;
				b1 = 1 - cosOmega;
				b2 = (1 - cosOmega) / 2;
			}
			else //filterType == FilterType.HPF
			{
				b0 = (1 + cosOmega) / 2;
				b1 = -(1 + cosOmega);
				b2 = (1 + cosOmega) / 2;
			}

			A0 = b0 / a0;
			A1 = b1 / a0;
			A2 = b2 / a0;
			A3 = a1 / a0;
			A4 = a2 / a0;
		}

		public float Step(float input)
		{
			double output = A0 * input + A1 * X1 + A2 * X2 - A3 * Y1 - A4 * Y2;

			X2 = X1;
			X1 = input;

			Y2 = Y1;
			Y1 = output;

			return (float)output;
		}
	}
}