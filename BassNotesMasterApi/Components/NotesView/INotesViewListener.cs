using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Components.NotesView
{
    public interface INotesViewListener
    {
        void OnMouseClick(Note note);
    }
}