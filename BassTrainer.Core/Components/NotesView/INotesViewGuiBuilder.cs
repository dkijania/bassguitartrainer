using BassTrainer.Core.Const;
using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Components.NotesView
{
    public interface INotesViewGuiBuilder
    {
        Note GetSenderAsNote(object sender);
        void HandleShowChange(FretBoardOptions fretboardOptions);
        void EnableAllButtons();
        void DisableAllButtons();
        void EnableButtons(Note[] notes);
        void EnableButtonsExclusive(Note[] notes);
        void ShowButtonExclusive(Note note);
        void HideAllButtons();
        void ShowAllButtons();
        void SetTextForButton(Note note, Note text, bool withOctaveNumber);
        void RevertGui();
    }
}