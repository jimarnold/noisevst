#include "stdafx.h"
#include "audioeffectx.h"
#include "EditorStub.h"
#include "MidiEventProcessor.h"
#include <vcclr.h>

using namespace Noise::Framework;
using namespace Noise::Framework::Unsafe;
using namespace Noise::Interop;

using namespace System;
using namespace System::Runtime::InteropServices;

class PluginBridge : public AudioEffectX
{
public:
	PluginBridge (audioMasterCallback audioMaster, INoisePlugin^ plugin);

	virtual void processReplacing (float** inputs, float** outputs, VstInt32 sampleFrames);
	virtual VstInt32 processEvents (VstEvents* ev);

	void GetPluginPath(char buffer[MAX_PATH]);
	virtual void setProgramName (char *name);
	virtual void getProgramName (char* name);

	virtual void setParameter (VstInt32 index, float value);
	virtual float getParameter (VstInt32 index);
	virtual void getParameterLabel (VstInt32 index, char* label);
	virtual void getParameterDisplay (VstInt32 index, char* text);
	virtual void getParameterName (VstInt32 index, char* text);

	virtual VstInt32 getProgram();
	virtual void setProgram(VstInt32 program);

	virtual bool getEffectName (char* name);
	virtual bool getVendorString (char* text);
	virtual bool getProductString (char* text);
	virtual VstInt32 getVendorVersion ();
	virtual void setSampleRate(float sampleRate);
	virtual VstInt32 canDo (char* text);

	virtual void open();
	virtual void close();

	virtual void suspend();
	virtual void resume();

	virtual void setBlockSize(VstInt32 blockSize);

protected:
	gcroot<INoisePlugin^> plugin;
	gcroot<ChannelGroup^> inputs;
	gcroot<ChannelGroup^> outputs;
	gcroot<MidiEventProcessor^> midiEventProcessor;

private:

	void stringToChar(String^ source, char* dest)
	{
		if(source)
		{
			const char* str = (const char*)(Marshal::StringToHGlobalAnsi(source)).ToPointer();
			strcpy(dest, str);
			Marshal::FreeHGlobal(IntPtr((void*)str));
		}
	}
};