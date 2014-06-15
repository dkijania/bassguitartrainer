using BassNotesMasterApi.Const;
using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.NotesView
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