using System.Collections.Generic;

namespace Noise.Framework.Events
{
    /*
     * This class stores a sequence of VST events.  The offset is stored with the event,
     * so calling HasEvent() will show you if the current event (at the top of the queue) applies
     * to the current offset.  Call Next() to dequeue the current event and shift the sequence on by one.
     * At the end of a Process block, all events should have been dequeued.
     * 
     * The BIG PROBLEM with this design is that after you have "used" an event, it disappears from the
     * map!  This is a side effect of using a queue, but I wanted an efficient way to look up events for
     * the current sample frame.  A better design would probably use an ordered tree, but .Net doesn't
     * provide one and I'm not about to write one myself...
    */

    public class EventMap
    {
        private Queue<IndexedMidiEvent> events = new Queue<IndexedMidiEvent>();

        public int Count
        {
            get { return events.Count; }
        }

        public void Add(MidiEvent e)
        {
            events.Enqueue(new IndexedMidiEvent(e.deltaFrames, e));
        }

        public bool HasEvent(int offset)
        {
            return (events.Count > 0 && events.Peek().index == offset);
        }

        public MidiEvent Next()
        {
            return events.Dequeue().midiEvent;
        }

        public void Clear()
        {
            events.Clear();
        }

        private struct IndexedMidiEvent
        {
            public int index;
            public MidiEvent midiEvent;

            public IndexedMidiEvent(int index, MidiEvent midiEvent)
            {
                this.index = index;
                this.midiEvent = midiEvent;
            }
        }
    }
}
