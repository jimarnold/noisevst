namespace Noise.Framework.Events
{
    public enum MidiStatus
    {
        NoteOff = 0x80,
        NoteOn = 0x90,
        AfterTouch = 0xA0,
        ControlChange = 0xB0,
        ProgramChange = 0xC0,
        ChannelPressure = 0xD0,
        PitchWheel = 0xE0
    }
}
