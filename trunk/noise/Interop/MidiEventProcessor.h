#pragma once

#include "stdafx.h"
#include "audioeffectx.h"

using namespace Noise::Framework;
using namespace Noise::Framework::Events;
using namespace Noise::Framework::Unsafe;

using namespace System::Collections::Generic;

namespace Noise
{
	namespace Interop
	{
		public ref class MidiEventProcessor : public System::Object
		{
			public:
				MidiEventProcessor(INoisePlugin^ plugin);
				void process (VstEvents* ev);
				
			private:
				INoisePlugin^ plugin;
				DebugWindow^ debug;
				EventMap^ midiEvents;
		};
	}
}