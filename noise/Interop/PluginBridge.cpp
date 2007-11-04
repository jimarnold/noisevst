#include "PluginBridge.h"

PluginBridge::PluginBridge (audioMasterCallback audioMaster, INoisePlugin^ plugin) : AudioEffectX (audioMaster, plugin->NumberOfPrograms, plugin->NumberOfParameters)
{
	this->plugin = plugin;

	setNumInputs (plugin->NumberOfInputs);
	setNumOutputs (plugin->NumberOfOutputs);
	isSynth(plugin->IsSynth);
	setUniqueID (plugin->ID);  //This code should be registered with Steinberg to ensure uniqueness

	if(plugin->HasEditor)
	{
		editor = new EditorStub(this, plugin);
	}

	this->midiEventProcessor = gcnew MidiEventProcessor(plugin);
}

VstInt32 PluginBridge::canDo (char* text)
{
	VstInt32 result = 1;

	if (!strcmp (text, "receiveVstEvents"))
		result = plugin->CanReceiveVstEvents;
	if (!strcmp (text, "receiveVstMidiEvent"))
		result = plugin->CanReceiveVstMidiEvents;
	if (!strcmp (text, "midiProgramNames"))
		result = plugin->SupportsMidiProgramNames;
	return result;	// explicitly can't do; 0 => don't know
}

void PluginBridge::setProgramName (char *name)
{
	plugin->ProgramName = gcnew String(name);
}

void PluginBridge::getProgramName (char *name)
{
	stringToChar(plugin->ProgramName, name);
}

void PluginBridge::getParameterName (VstInt32 index, char* name)
{
	stringToChar(plugin->GetParameterName(index), name);
}

void PluginBridge::getParameterDisplay (VstInt32 index, char* text)
{
	stringToChar(plugin->GetParameterDisplay(index), text);
}

void PluginBridge::getParameterLabel (VstInt32 index, char* label)
{
	stringToChar(plugin->GetParameterLabel(index), label);
}

void PluginBridge::setParameter (VstInt32 index, float value)
{
	plugin->SetParameter(index, value);
}

float PluginBridge::getParameter (VstInt32 index)
{
	return plugin->GetParameter(index);
}

VstInt32 PluginBridge::getProgram()
{
	return plugin->Program;
}

void PluginBridge::setProgram(VstInt32 program)
{
	plugin->Program = program;
}

bool PluginBridge::getEffectName(char* name)
{
	stringToChar(plugin->EffectName, name);
	return true;
}

bool PluginBridge::getProductString(char* text)
{
	stringToChar(plugin->ProductName, text);
	return true;
}

bool PluginBridge::getVendorString(char* text)
{
	stringToChar(plugin->VendorName, text);
	return true;
}

VstInt32 PluginBridge::getVendorVersion()
{
	return plugin->VendorVersion;
}

void PluginBridge::setSampleRate(float sampleRate)
{
	plugin->SetSampleRate(sampleRate);
}

void PluginBridge::processReplacing(float** in, float** out, VstInt32 sampleFrames)
{
	for(int i = 0; i < this->plugin->NumberOfInputs; i++)
	{
		ISwitchingChannel^ channel = ((ISwitchingChannel^)inputs->GetChannel(i));
		channel->SetBuffer(in[i], sampleFrames);
	}

	for(int i = 0; i < this->plugin->NumberOfOutputs; i++)
	{
		ISwitchingChannel^ channel = ((ISwitchingChannel^)outputs->GetChannel(i));
		channel->SetBuffer(out[i], sampleFrames);
	}

	plugin->ProcessReplacing(inputs, outputs, sampleFrames);
}

VstInt32 PluginBridge::processEvents (VstEvents* ev)
{
	if(ev->numEvents > 0 && ev->events[0]->type == kVstMidiType)
	{
		midiEventProcessor->process(ev);
		return 1;
	}

	return 0;
}

void PluginBridge::open()
{
	plugin->Open();
}

void PluginBridge::close()
{
	plugin->Close();
}

void PluginBridge::suspend()
{
	plugin->Suspend();
}

void PluginBridge::resume()
{
	AudioEffectX::resume();
	plugin->Resume();
}

void PluginBridge::setBlockSize(VstInt32 blockSize)
{
	inputs = gcnew ChannelGroup(this->plugin->NumberOfInputs);
	outputs = gcnew ChannelGroup(this->plugin->NumberOfOutputs);
	
	plugin->SetBlockSize(blockSize);
}