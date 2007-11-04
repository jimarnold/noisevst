using System;
using Noise.Framework.Events;

namespace Noise.Framework
{
	public interface INoisePlugin
	{
		void ProcessReplacing(IChannelGroup inputs, IChannelGroup outputs, int sampleFrames);
	    void ProcessMidiEvents(EventMap midiEventList);
        int ID {get;}
		int Program { get; set; }
		string ProgramName { get; set; }
		int NumberOfPrograms { get; }

		void SetParameter(int index, float value);
		float GetParameter(int index);
		string GetParameterLabel(int index);
		string GetParameterDisplay(int index);
		string GetParameterName(int index);
		int NumberOfParameters { get; }

		string EffectName { get; }
		string VendorName { get; }
		string ProductName { get; }
		int VendorVersion { get; }

		int NumberOfInputs { get; }
		int NumberOfOutputs { get; }
		bool CanMono { get; }
		bool HasVu { get; }
		bool HasClip { get; }
		bool HasEditor { get; }
        bool IsSynth { get; }
        bool CanReceiveVstEvents { get; }
        bool CanReceiveVstMidiEvents { get; }
        bool SupportsMidiProgramNames { get; }

		void Open();
		void Close();
		void Suspend ();
		void Resume ();
		void SetBlockSize(int blockSize);
		void SetSampleRate(float sampleRate);

		float GetVu();

		INoiseEditor OpenEditorWindow(IntPtr window);
		void SetHost(INoiseHost host);
	}
}
