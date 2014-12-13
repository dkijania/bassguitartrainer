using BassTrainer.Core.Const;
using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Components.NotesView
{
    public class NotesView
    {
        private readonly INotesViewGuiBuilder _guiBuilder;

        public NotesView(INotesViewGuiBuilder guiBuilder)
        {
            _guiBuilder = guiBuilder;
        }

        public Note GetSenderAsNote(object sender)
        {
            return _guiBuilder.GetSenderAsNote(sender);
        }

        public void HandleShowChange(FretBoardOptions fretboardOptions)
        {
            _guiBuilder.HandleShowChange(fretboardOptions);
        }

        public void EnableAllButtons()
        {
            _guiBuilder.EnableAllButtons();
        }

        public void DisableAllButtons()
        {
            _guiBuilder.DisableAllButtons();
        }

        public void EnableButtons(Note[] notes)
        {
            _guiBuilder.EnableButtons(notes);
        }

        public void EnableButtonsExclusive(Note[] notes)
        {
            _guiBuilder.EnableButtonsExclusive(notes);
        }

        public void ShowButtonExclusive(Note note)
        {
            _guiBuilder.ShowButtonExclusive(note);
        }

        public void HideAllButtons()
        {
            _guiBuilder.HideAllButtons();
        }

        public void ShowAllButtons()
        {
            _guiBuilder.ShowAllButtons();
        }

        public void SetTextForButton(Note note, Note text, bool withOctaveNumber)
        {
            _guiBuilder.SetTextForButton(note, text, withOctaveNumber);
        }

        public void RevertGui()
        {
            _guiBuilder.RevertGui();
        }
    }
}
