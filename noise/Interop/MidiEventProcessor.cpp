#include "MidiEventProcessor.h"

using namespace Noise::Interop;

MidiEventProcessor::MidiEventProcessor(INoisePlugin^ plugin)
{
	this->plugin = plugin;
	this->midiEvents = gcnew EventMap();
}

void MidiEventProcessor::process (VstEvents* events)
{
	midiEvents->Clear();

	for(int i = 0; i < events->numEvents; i++)
	{
		VstEvent* vstEvent = events->events[i++];

		if(vstEvent->type == kVstMidiType) //can't handle other event types yet!
		{
			VstMidiEvent* midiEvent = (VstMidiEvent*)vstEvent;
			midiEvents->Add(
				MidiEvent(
					midiEvent->midiData[0],
					midiEvent->midiData[1],
					midiEvent->midiData[2],
					midiEvent->deltaFrames,
					midiEvent->detune,
					midiEvent->flags == kVstMidiEventIsRealtime,
					midiEvent->noteLength,
					midiEvent->noteOffset,
					midiEvent->noteOffVelocity
				)
			);
		}
	}

	plugin->ProcessMidiEvents(midiEvents);
}