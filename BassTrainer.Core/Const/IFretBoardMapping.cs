using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Const
{
    public interface IFretBoardMapping
    {
        Note GetNote(StringFretPair key);
        StringFretPair[] GetFullOctaveScalesRootPosition(Note root);
        StringFretPair[] GetAllMatchingNotes(Note prototype);
        StringFretPair[] GetAllMatchingNotesWithHigherOrEqualOctave(Note prototype);
    }
}