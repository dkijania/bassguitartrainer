using BassTrainer.Core.Const;
using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Components.NotesView
{
    public interface INotesViewListener
    {
        void OnMouseClick(Note note);
    }
}