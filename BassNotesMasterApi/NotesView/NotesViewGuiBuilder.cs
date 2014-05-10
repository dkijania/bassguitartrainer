using BassNotesMasterApi.Const;
using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.NotesView
{
    public abstract class NotesViewGuiBuilder
    {
        public abstract Note GetSenderAsNote(object sender);
        public abstract void HandleShowChange(FretBoardOptions fretboardOptions);
        public abstract void EnableAllButtons();
        public abstract void DisableAllButtons();
        public abstract void EnableButtons(Note[] notes);
        public abstract void EnableButtonsExclusive(Note[] notes);
        public abstract void ShowButtonExclusive(Note note);
        public abstract void HideAllButtons();
        public abstract void ShowAllButtons();
        public abstract void SetTextForButton(Note note, Note text, bool withOctaveNumber);
        public abstract void RevertGui();
    }
}