namespace Noise.Framework.Events
{
    /*
     * This is a pretty raw implementation - in the future there will be support for figuring out
     * what type of event it is, what note the event relates to, etc.
     */
    public struct MidiEvent
    {
        public int deltaFrames;	///< sample frames related to the current block start sample position
        public bool isRealtime;
        public int noteLength;	///< (in sample frames) of entire note, if available, else 0
        public int noteOffset;	///< offset (in sample frames) into note from note start if available, else 0
        public byte data1;
        public byte data2;
        public byte data3;		///< 1 to 3 MIDI bytes; midiData[3] is reserved (zero)
        public byte detune;			///< -64 to +63 cents; for scales other than 'well-tempered' ('microtuning')
        public byte noteOffVelocity;	///< Note Off Velocity [0, 127]

        public MidiEvent(byte data1, byte data2, byte data3, int deltaFrames, byte detune, bool isRealtime, int noteLength, int noteOffset, byte noteOffVelocity)
        {
            this.data1 = data1;
            this.data2 = data2;
            this.data3 = data3;
            this.deltaFrames = deltaFrames;
            this.detune = detune;
            this.isRealtime = isRealtime;
            this.noteLength = noteLength;
            this.noteOffset = noteOffset;
            this.noteOffVelocity = noteOffVelocity;
        }

        public MidiStatus Status
        {
            get
            {
                return (MidiStatus)(data1 & 0xF0);
            }
        }
    }
}
