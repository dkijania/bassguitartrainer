using BassNotesMasterApi.Utils;
using NUnit.Framework;

namespace BassNotesMasterUnitTests
{
    [TestFixture]
    public class NoteTest
    {
        [Test]
        public void TestSharpNote()
        {
            var note = new Note("F#");
            Assert.That(note.IsNatural(), Is.False);
            Assert.That(note.SharpOrRegularRepresenation, Is.EqualTo("F#"));
            Assert.That(note.BemolRepresenation, Is.EqualTo("Gb"));
            Assert.That(note.WithoutAccidental,Is.EqualTo("F"));
        }

        [Test]
        public void TestBemolNote()
        {
            var note = new Note("Ab");
            Assert.That(note.IsNatural(), Is.False);
            Assert.That(note.SharpOrRegularRepresenation, Is.EqualTo("G#"));
            Assert.That(note.BemolRepresenation, Is.EqualTo("Ab"));
        }

        [Test]
        public void TestNaturalNote()
        {
            var note = new Note("F");
            Assert.That(note.IsNatural(), Is.True);
            Assert.That(note.SharpOrRegularRepresenation, Is.EqualTo("F"));
            Assert.That(note.BemolRepresenation, Is.EqualTo("F"));
        }

        [Test]
        public void EqualsWithoutOctaveDoesntCheckOctaveNumber()
        {
            var firstNote = new Note("F");
            var secondNote = new Note("F2");
            Assert.That(firstNote.EqualsWithoutOctaveNumber(secondNote), Is.True);
        }

        [Test]
        public void EqualsDefaultBehaviour()
        {
            var firstNote = new Note("D");
            var secondNote = new Note("D2");
            var thridNote = new Note("D#");
            var fourthNote = new Note("Db");
            var fifthNote = new Note("D");
            Assert.That(firstNote.Equals(secondNote), Is.False);
            Assert.That(firstNote.Equals(thridNote), Is.False);
            Assert.That(firstNote.Equals(fourthNote), Is.False);
            Assert.That(firstNote.Equals(fifthNote), Is.True);
        }

        [Test]
        public void EqualsOrHigher()
        {
            var firstNote = new Note("F");
            var secondNote = new Note("F2");
            Assert.That(secondNote.EqualsOrHigherThan(firstNote), Is.True);
            Assert.That(firstNote.EqualsOrHigherThan(secondNote), Is.False);
        }

        [Test]
        public void EqualsWithoutAccidentals()
        {
            var firstNote = new Note("D");
            var secondNote = new Note("D#");
            var thirdNote = new Note("Db");
            var sameNote = new Note("D");
            var differentNote = new Note("E");
            Assert.That(firstNote.EqualsWithoutAccidentals(secondNote), Is.True);
            Assert.That(firstNote.EqualsWithoutAccidentals(thirdNote), Is.False);
            Assert.That(firstNote.EqualsWithoutAccidentals(sameNote), Is.True);
            Assert.That(firstNote.EqualsWithoutAccidentals(differentNote), Is.False);
        }

        [Test]
        public void NoteWithoutAccidental()
        {
            var note = new Note("F#");
            var secondNote = new Note("F");
            Assert.That(note.WithoutAccidental, Is.EqualTo("F"));
            Assert.That(secondNote.WithoutAccidental, Is.EqualTo("F"));
        }
    }

}