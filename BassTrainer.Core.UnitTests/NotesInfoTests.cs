using System.Linq;
using BassTrainer.Core.Const;
using BassTrainer.Core.Utils;
using NUnit.Framework;

namespace BassNotesMasterUnitTests
{
    public class NotesInfoTests
    {
        private readonly NotesInfo _notesInfo = new NotesInfo();

        [Test]
        public void TestOrderWithoutAccidentalNotes()
        {
            var orderNotes = _notesInfo.Order;
            Assert.That(orderNotes[0].EqualsWithoutOctaveNumber(new Note("C")), Is.True);
            Assert.That(orderNotes[1].EqualsWithoutOctaveNumber(new Note("D")), Is.True);
            Assert.That(orderNotes[2].EqualsWithoutOctaveNumber(new Note("E")), Is.True);
            Assert.That(orderNotes[3].EqualsWithoutOctaveNumber(new Note("F")), Is.True);
            Assert.That(orderNotes[4].EqualsWithoutOctaveNumber(new Note("G")), Is.True);
            Assert.That(orderNotes[5].EqualsWithoutOctaveNumber(new Note("A")), Is.True);
            Assert.That(orderNotes[6].EqualsWithoutOctaveNumber(new Note("B")), Is.True);
            Assert.That(orderNotes.Count(), Is.EqualTo(7));
        }

        [Test]
        public void TestOrderWithAccidentalNotes()
        {
            var orderNotes = _notesInfo.OrderWithAccidentals;
            Assert.That(orderNotes[0].EqualsWithoutOctaveNumber(new Note("C")), Is.True);
            Assert.That(orderNotes[1].EqualsWithoutOctaveNumber(new Note("C#")), Is.True);
            Assert.That(orderNotes[2].EqualsWithoutOctaveNumber(new Note("D")), Is.True);
            Assert.That(orderNotes[3].EqualsWithoutOctaveNumber(new Note("D#")), Is.True);
            Assert.That(orderNotes[4].EqualsWithoutOctaveNumber(new Note("E")), Is.True);
            Assert.That(orderNotes[5].EqualsWithoutOctaveNumber(new Note("F")), Is.True);
            Assert.That(orderNotes[6].EqualsWithoutOctaveNumber(new Note("F#")), Is.True);
            Assert.That(orderNotes[7].EqualsWithoutOctaveNumber(new Note("G")), Is.True);
            Assert.That(orderNotes[8].EqualsWithoutOctaveNumber(new Note("G#")), Is.True);
            Assert.That(orderNotes[9].EqualsWithoutOctaveNumber(new Note("A")), Is.True);
            Assert.That(orderNotes[10].EqualsWithoutOctaveNumber(new Note("A#")), Is.True);
            Assert.That(orderNotes[11].EqualsWithoutOctaveNumber(new Note("B")), Is.True);
            Assert.That(orderNotes.Count(), Is.EqualTo(12));
        }

        [Test]
        public void TestAllNotesWithoutAccidentalsExists()
        {
            Assert.That(_notesInfo.NoteExists(new Note("E"), NotesInfo.Accidentals.None), Is.True);
            Assert.That(_notesInfo.NoteExists(new Note("F"), NotesInfo.Accidentals.None), Is.True);
            Assert.That(_notesInfo.NoteExists(new Note("G"), NotesInfo.Accidentals.None), Is.True);
            Assert.That(_notesInfo.NoteExists(new Note("A"), NotesInfo.Accidentals.None), Is.True);
            Assert.That(_notesInfo.NoteExists(new Note("B"), NotesInfo.Accidentals.None), Is.True);
            Assert.That(_notesInfo.NoteExists(new Note("C"), NotesInfo.Accidentals.None), Is.True);
            Assert.That(_notesInfo.NoteExists(new Note("D"), NotesInfo.Accidentals.None), Is.True);
            Assert.That(_notesInfo.NoteExists(new Note("D#"), NotesInfo.Accidentals.None), Is.True);
            Assert.That(_notesInfo.NoteExists(new Note("Bb"), NotesInfo.Accidentals.None), Is.True);
            Assert.That(_notesInfo.NoteExists(new Note("D#"), NotesInfo.Accidentals.Sharp), Is.True);
            Assert.That(_notesInfo.NoteExists(new Note("Bb"), NotesInfo.Accidentals.Flat), Is.True);
            Assert.That(_notesInfo.NoteExists(new Note("E"), NotesInfo.Accidentals.Sharp), Is.False);
            Assert.That(_notesInfo.NoteExists(new Note("F"), NotesInfo.Accidentals.Flat), Is.False);
            Assert.That(_notesInfo.NoteExists(new Note("C"), NotesInfo.Accidentals.Flat), Is.False);
            Assert.That(_notesInfo.NoteExists(new Note("B"), NotesInfo.Accidentals.Sharp), Is.False);

        }

        [Test]
        public void TestGetLowestNote()
        {
            var notesWithSameTune = new[] {new Note("E1"),new Note("E2")};
            var differentNotesButSameOctave = new[] {  new Note("E"), new Note("C"),new Note("D")};
            var differentNotesAndDifferentOctaves = new[] {new Note("D3"),new Note("B2"),new Note("F#1")};
            var accidentals = new[] {new Note("D2"), new Note("Db2")};

            Assert.That(_notesInfo.GetLowestNote(notesWithSameTune),Is.EqualTo(notesWithSameTune[0]));
            Assert.That(_notesInfo.GetLowestNote(differentNotesButSameOctave),Is.EqualTo(differentNotesButSameOctave[1]));
            Assert.That(_notesInfo.GetLowestNote(differentNotesAndDifferentOctaves), Is.EqualTo(differentNotesAndDifferentOctaves[2]));
            Assert.That(_notesInfo.GetLowestNote(accidentals), Is.EqualTo(accidentals[1]));
        }

        [Test]
        public void GetNoteWithDistanceForwardFromLowestNoteOnlyNaturals()
        {
           Assert.That(_notesInfo.GetNoteWithDistanceForwardFromLowestNote(1,onlyNatural:true),Is.EqualTo(new Note("D1")));
           Assert.That(_notesInfo.GetNoteWithDistanceForwardFromLowestNote(7,onlyNatural:true),Is.EqualTo(new Note("C2")));
           Assert.That(_notesInfo.GetNoteWithDistanceForwardFromLowestNote(8,onlyNatural:true),Is.EqualTo(new Note("D2")));
           Assert.That(_notesInfo.GetNoteWithDistanceForwardFromLowestNote(14,onlyNatural:true),Is.EqualTo(new Note("C3")));
        }

        [Test]
        public void GetNoteWithDistanceForwardFromLowestNoteWithAccidentals()
        {
            Assert.That(_notesInfo.GetNoteWithDistanceForwardFromLowestNote(1, onlyNatural: false), Is.EqualTo(new Note("C#1")));
            Assert.That(_notesInfo.GetNoteWithDistanceForwardFromLowestNote(11, onlyNatural: false), Is.EqualTo(new Note("B1")));
            Assert.That(_notesInfo.GetNoteWithDistanceForwardFromLowestNote(12, onlyNatural: false), Is.EqualTo(new Note("C2")));
            Assert.That(_notesInfo.GetNoteWithDistanceForwardFromLowestNote(24, onlyNatural: false), Is.EqualTo(new Note("C3")));
        }

        [Test]
        public void GetNoteWithDistanceForwardFromNote()
        {
            Assert.That(_notesInfo.GetNoteWithDistanceForward(new Note("E1"), 11, onlyNatural: false),Is.EqualTo(new Note("D#2")));
            Assert.That(_notesInfo.GetNoteWithDistanceForward(new Note("F1"),4, onlyNatural: false), Is.EqualTo(new Note("A1")));
            Assert.That(_notesInfo.GetNoteWithDistanceForward(new Note("E1"),8, onlyNatural: true), Is.EqualTo(new Note("F2")));
            Assert.That(_notesInfo.GetNoteWithDistanceForward(new Note("G1"),3, onlyNatural: true), Is.EqualTo(new Note("C2")));
        }
    }
}
