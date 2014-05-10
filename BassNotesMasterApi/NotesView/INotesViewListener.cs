using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.NotesView
{
    public interface INotesViewListener
    {
        void OnMouseClick(Note note);
    }
}