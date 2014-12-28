using BassTrainer.Core.Const;
using NUnit.Framework;

namespace BassNotesMasterUnitTests
{

    [TestFixture]
    public class NotesToStringFretboardMappingTest
    {
        readonly NotesToStringFretBoardMapping _fretBoardMapping = NotesToStringFretBoardMapping.Instance; 

        [Test]
        public void TestGetNote()
        {
            var note = _fretBoardMapping.GetNote(new StringFretPair(FretBoardOptions.StringName.E, 0));
            Assert.That(note,Is.EqualTo(new Note("E1")));
        }

        [Test]
        public void TestGetAllEquivalentPositions()
        {
            var expected = new[]
                               {
                                   new StringFretPair(FretBoardOptions.StringName.A, 3),
                                   new StringFretPair(FretBoardOptions.StringName.E, 8)
                               };

            var positions = _fretBoardMapping.GetAllEquivalentPositions(expected[0]);
            Assert.That(positions, Is.EquivalentTo(expected));
        }

        [Test]
        public void TestGetAllMatchingNotes()
        {
            var expected = new[]
                               {
                                   new StringFretPair(FretBoardOptions.StringName.A, 3),
                                   new StringFretPair(FretBoardOptions.StringName.E, 8)
                               };

     
            var stringPairs = _fretBoardMapping.GetAllMatchingNotes(new Note("C2"));
            Assert.That(stringPairs, Is.EquivalentTo(expected));
        }

        [Test]
        public void TestGetAllMatchingNotesWithHigherOrEqualOctave()
        {
            var expected = new[]
                               {
                                   new StringFretPair(FretBoardOptions.StringName.A, 3),
                                   new StringFretPair(FretBoardOptions.StringName.E, 8),
                                   new StringFretPair(FretBoardOptions.StringName.D, 10),
                                   new StringFretPair(FretBoardOptions.StringName.G, 5),
                               };


            var stringPairs = _fretBoardMapping.GetAllMatchingNotesWithHigherOrEqualOctave(new Note("C2"));
            Assert.That(stringPairs, Is.EquivalentTo(expected));
        }

        [Test]
        public void TestGetMatchingNoteFromPossibilities()
        {
            var possibilities = new[]
                               {
                                   new StringFretPair(FretBoardOptions.StringName.A, 4),
                                   new StringFretPair(FretBoardOptions.StringName.E, 8),
                              };


            var stringPair = _fretBoardMapping.GetMatchingNote(new Note("C2"), possibilities);
            Assert.That(stringPair, Is.EqualTo(possibilities[1]));
        }

        [Test]
        public void TestGetFullOctaveScalesRootPosition()
        {
            var expected = new[]
                               {
                                   new StringFretPair(FretBoardOptions.StringName.A, 3),
                                   new StringFretPair(FretBoardOptions.StringName.E, 8)
                               };


            var stringPairs = _fretBoardMapping.GetFullOctaveScalesRootPosition(new Note("C"));
            Assert.That(stringPairs, Is.EquivalentTo(expected));
        }
    }
}
