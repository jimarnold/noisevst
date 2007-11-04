#pragma once

#include "stdafx.h"
#include "audioeffectx.h"

using namespace System;
using namespace System::Collections;

using namespace Noise::Framework;

namespace Noise
{
	namespace Interop
	{
		public ref class HostBridge : public System::Object, public INoiseHost
		{
			public:
				HostBridge();

				void SetHost(AudioEffectX* host);
				virtual void SetParameterAutomated(VstInt32 index, float value);
				virtual VstInt32 GetMasterVersion();
				virtual void SetUniqueID(VstInt32 id);
				virtual VstInt32 GetCurrentUniqueId();
				virtual void MasterIdle();
				virtual float GetSampleRate();
				virtual VstInt32 GetBlockSize();
				virtual void SetInitialDelay(VstInt32 delay);

			private:
				INoiseEditor^ pluginEditor;
				AudioEffectX* host;
		};
	}
}
