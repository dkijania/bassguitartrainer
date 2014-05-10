using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.NotePlayer
{
    public interface IBassNotesPlayer
    {
        void PlayNote(params StringFretPair[] stringFretPairs);
        void PlayNote(params Note[] notes);
    }
}
