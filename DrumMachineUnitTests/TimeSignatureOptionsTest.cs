using DrumMachine.Engine.Pattern;
using DrumMachine.TimeSignature;
using NUnit.Framework;

namespace DrumMachineUnitTests
{
    [TestFixture]
    public class TimeSignatureOptionsTest
    {
        [Test]
        public void TimeSignatureGivesCorrectResultFor4Per4()
        {
            var timeSignature = new TimeSignatureOptions(4, NoteTypeEnum.Quarter);
            var drumPattern = timeSignature.ToDrumPattern();
            var expectedArray = new byte[2, 16];
            expectedArray[0, 0] = 0;
            expectedArray[0, 1] = 0;
            expectedArray[0, 2] = 0;
            expectedArray[0, 3] = 0;
            expectedArray[0, 4] = DrumPattern.Hit;
            expectedArray[0, 5] = 0;
            expectedArray[0, 6] = 0;
            expectedArray[0, 7] = 0;
            expectedArray[0, 8] = DrumPattern.Hit;
            expectedArray[0, 9] = 0;
            expectedArray[0, 10] = 0;
            expectedArray[0, 11] = 0;
            expectedArray[0, 12] = DrumPattern.Hit;
            expectedArray[0, 13] = 0;
            expectedArray[0, 14] = 0;
            expectedArray[0, 15] = 0;

            expectedArray[0, 0] = DrumPattern.Hit;
            expectedArray[1, 1] = 0;
            expectedArray[1, 2] = 0;
            expectedArray[1, 3] = 0;
            expectedArray[1, 4] = 0;
            expectedArray[1, 5] = 0;
            expectedArray[1, 6] = 0;
            expectedArray[1, 7] = 0;
            expectedArray[1, 8] = 0;
            expectedArray[1, 9] = 0;
            expectedArray[1, 10] = 0;
            expectedArray[1, 11] = 0;
            expectedArray[1, 12] = 0;
            expectedArray[1, 13] = 0;
            expectedArray[1, 14] = 0;
            expectedArray[1, 15] = 0;
            Assert.That(drumPattern.Array, Is.EquivalentTo(expectedArray));
        }
        
        [Test]
        public void TimeSignatureGivesCorrectResultFor3Per4()
        {
            var timeSignature = new TimeSignatureOptions(3, NoteTypeEnum.Quarter);
            var drumPattern = timeSignature.ToDrumPattern();
            var expectedArray = new byte[2, 12];
            expectedArray[0, 0] = 0;
            expectedArray[0, 1] = 0;
            expectedArray[0, 2] = 0;
            expectedArray[0, 3] = 0;
            expectedArray[0, 4] = DrumPattern.Hit;
            expectedArray[0, 5] = 0;
            expectedArray[0, 6] = 0;
            expectedArray[0, 7] = 0;
            expectedArray[0, 8] = DrumPattern.Hit;
            expectedArray[0, 9] = 0;
            expectedArray[0, 10] = 0;
            expectedArray[0, 11] = 0;
            expectedArray[1, 0] = DrumPattern.Hit;
            expectedArray[1, 1] = 0;
            expectedArray[1, 2] = 0;
            expectedArray[1, 3] = 0;
            expectedArray[1, 4] = 0;
            expectedArray[1, 5] = 0;
            expectedArray[1, 6] = 0;
            expectedArray[1, 7] = 0;
            expectedArray[1, 8] = 0;
            expectedArray[1, 9] = 0;
            expectedArray[1, 10] = 0;
            expectedArray[1, 11] = 0;
            Assert.That(drumPattern.Array, Is.EquivalentTo(expectedArray));
        }
    }
}
