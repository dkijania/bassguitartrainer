using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Const
{
    public interface IFretBoardMapping
    {
        Note GetNote(StringFretPair key);
        StringFretPair[] GetFullOctaveScalesRootPosition(Note root);
        StringFretPair[] GetAllMatchingNotes(Note prototype);
        StringFretPair[] GetAllMatchingNotesWithHigherOrEqualOctave(Note prototype);
    }
}