using System;
using Noise.Framework;
using Noise.Framework.Events;

namespace Noise.ManagedVst.Plugins
{
    public abstract class Plugin : INoisePlugin
    {
        protected string programName;
        protected string effectName;
        protected string vendorName;
        protected string productName;
        protected int vendorVersion;

        protected readonly string[] parameterLabels;
        protected readonly string[] parameterNames;

        protected int numberOfPrograms;
        protected int numberOfParameters;
        protected float sampleRate;
        protected INoiseHost host;
        protected IntPtr editorWindow;

        private int uniqueId;

        public Plugin(string pluginName, int numberOfParameters)
		{
			productName = pluginName;
			this.numberOfParameters = numberOfParameters;

			programName = "Default Program";
			effectName = "Default Effect";
			vendorName = "Unknown Vendor";
            uniqueId = GetVstID("MVST");

			parameterLabels = new string[numberOfParameters];
			parameterNames = new string[numberOfParameters];

			sampleRate = 44100f; //Just a default - this should get set by the host
		}

        public abstract void ProcessReplacing(IChannelGroup inputs, IChannelGroup outputs, int sampleFrames);

        public virtual void ProcessMidiEvents(EventMap midiEventList)
        {
            
        }

        public abstract void SetParameter(int index, float value);
        public abstract float GetParameter(int index);
        public abstract string GetParameterDisplay(int index);
        public abstract int NumberOfInputs { get; }
        public abstract int NumberOfOutputs { get; }

        protected void SetID(string id)
        {
            uniqueId = GetVstID(id);
        }

        public int ID
        {
            get
            {
                return uniqueId;
            }
        }

        public virtual bool HasEditor
        {
            get
            {
                return false;
            }
        }

        public virtual bool IsSynth
        {
            get { return false; }
        }

        public virtual bool CanReceiveVstEvents
        {
            get { return false; }
        }

        public virtual bool CanReceiveVstMidiEvents
        {
            get { return false; }
        }

        public virtual bool SupportsMidiProgramNames
        {
            get { return false; }
        }

        public void SetHost(INoiseHost host)
        {
            this.host = host;
        }

        public virtual int Program
        {
            get { return 0; }
            set { }
        }

        public virtual string ProgramName
        {
            get { return programName; }
            set { programName = value; }
        }

        public virtual int NumberOfPrograms
        {
            get { return numberOfPrograms; }
        }

        public virtual void SetParameterAutomated(int index, float value)
        {
            host.SetParameterAutomated(index, value);
        }

        public virtual string GetParameterLabel(int index)
        {
            string label = parameterLabels[index];

            return label != null ? label : "No label";
        }

        public virtual string GetParameterName(int index)
        {
            string name = parameterNames[index];

            return name != null ? name : "No name";
        }

        public virtual int NumberOfParameters
        {
            get { return numberOfParameters; }
        }

        public virtual string EffectName
        {
            get { return effectName; }
        }

        public virtual string VendorName
        {
            get { return vendorName; }
        }

        public virtual string ProductName
        {
            get { return productName; }
        }

        public virtual int VendorVersion
        {
            get { return vendorVersion; }
        }

        public virtual bool CanMono
        {
            get { return false; }
        }

        public virtual bool HasVu
        {
            get { return false; }
        }

        public virtual bool HasClip
        {
            get { return false; }
        }

        public virtual void Open()
        {

        }

        public virtual void Close()
        {

        }

        public virtual void Suspend()
        {
        }

        public virtual void Resume()
        {
        }

        public virtual void SetBlockSize(int size)
        {
        }

        public virtual void SetSampleRate(float rate)
        {
            sampleRate = rate;
        }

        public virtual float GetVu()
        {
            return 0f;
        }

        public virtual INoiseEditor OpenEditorWindow(IntPtr window)
        {
            editorWindow = window;
            return null;
        }

        protected static string ToDbString(float value)
        {
            return (20f * (float)Math.Log10(value)).ToString();
        }

        protected static float Db2Amplitude(float db)
        {
            return (float)Math.Exp(db * Math.Log(10) / 20);
        }

        protected static float Amplitude2Db(float amplitude)
        {
            return (float)(20 * Math.Log10(amplitude));
        }

        private static int GetVstID(string id)
        {
            if (id.Length < 4)
                id = "BAD "; //Should we throw?  Or log errors somehow?

            return ((((int)id[0]) << 24) | (((int)id[1]) << 16) | (((int)id[2]) << 8) | (((int)id[3]) << 0));
        }
    }
}
