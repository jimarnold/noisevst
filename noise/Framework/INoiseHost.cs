using System;

namespace Noise.Framework
{
	public interface INoiseHost
	{
		void SetParameterAutomated(int index, float value);
		int GetMasterVersion();
		void SetUniqueID(int id);
		int GetCurrentUniqueId();
		void MasterIdle();
		float GetSampleRate();
		int GetBlockSize();
		void SetInitialDelay(int delay);
	}
}