/*
 * Created by: 
 * Created: 02 July 2007
 */

using Noise.Framework.Events;
using NUnit.Framework;

namespace Noise.Test.Framework
{
    [TestFixture]
    public class EventMapTests
    {
        [Test]
        public void ShouldMapEventsToSampleFrames()
        {
            EventMap map = new EventMap();
            MidiEvent midiEvent = new MidiEvent();
            midiEvent.deltaFrames = 1234;
            map.Add(midiEvent);

            Assert.IsTrue(map.HasEvent(1234));
        }

        [Test]
        public void ShouldRemoveEventFromMapAfterUse()
        {
            //Actually, I'm not sure the event *should* be removed from the map.  It's an artificial
            //requirement because the EventMap is based on a Queue.  Ideally it would be based on some type
            //of tree.

            EventMap map = new EventMap();
            MidiEvent midiEvent = new MidiEvent();
            midiEvent.deltaFrames = 1;
            map.Add(midiEvent);

            Assert.AreEqual(1, map.Count);
            map.Next();
            Assert.AreEqual(0, map.Count);
        }

        [Test]
        public void ShouldAllowAllEventsToBeCleared()
        {
            EventMap map = new EventMap();
            MidiEvent midiEvent = new MidiEvent();
            midiEvent.deltaFrames = 1;
            map.Add(midiEvent);
            midiEvent.deltaFrames = 2;
            map.Add(midiEvent);
            midiEvent.deltaFrames = 3;
            map.Add(midiEvent);

            map.Clear();

            Assert.AreEqual(0, map.Count);
        }
    }
}