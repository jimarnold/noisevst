using System;
using Noise.Framework;
using Noise.Framework.Events;
using Noise.ManagedVst.Plugins;

namespace Noise.ManagedVst.Instruments
{
    public class Cynth : Plugin
    {
        private float volume;
        private Random random;
        private bool noteOn;
        private EventMap events;

        public Cynth() : base("Cynth", 1)
        {
            parameterNames[0] = "Level";
            parameterLabels[0] = "dB";
            random = new Random();
        }

        public override bool IsSynth
        {
            get
            {
                return true;
            }
        }

        public override void ProcessReplacing(IChannelGroup inputs, IChannelGroup outputs, int sampleFrames)
        {
            for(int frame = 0; frame < sampleFrames; frame++)
            {
                while (events.HasEvent(frame))
                {
                    MidiEvent midiEvent = events.Next();

                    if (midiEvent.Status == MidiStatus.NoteOn)
                    {
                        noteOn = true;
                    }
                    else if (midiEvent.Status == MidiStatus.NoteOff)
                    {
                        noteOn = false;
                    }
                }

                if (noteOn)
                {
                    outputs.GetChannel(0)[frame] = (float) random.NextDouble()*volume;
                }
            }
        }

        public override void ProcessMidiEvents(EventMap events)
        {
            this.events = events;
        }

        public override void SetParameter(int index, float value)
        {
            volume = value;
        }

        public override float GetParameter(int index)
        {
            return volume;
        }

        public override string GetParameterDisplay(int index)
        {
            return ToDbString(volume);
        }

        public override bool CanReceiveVstEvents
        {
            get
            {
                return true;
            }
        }

        public override bool CanReceiveVstMidiEvents
        {
            get
            {
                return true;
            }
        }

        public override bool SupportsMidiProgramNames
        {
            get
            {
                return true;
            }
        }

        public override int NumberOfInputs
        {
            get { return 1; }
        }

        public override int NumberOfOutputs
        {
            get { return 1; }
        }
    }
}