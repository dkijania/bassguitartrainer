using BassNotesMasterApi.Components.Fretboard;
using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Components.NotesView
{
    public abstract class NotesViewEventHandler
    {
        public INotesViewGuiBuilder Builder { get; set; }
        public FretboardManager FretboardManager { get; set; }
        public event OnButtonClick OnButtonClickEvent;

        public delegate void OnButtonClick(Note note);

        public void Subscribe(INotesViewListener listener)
        {
            OnButtonClickEvent += listener.OnMouseClick;
        }

        public void Unsubscribe(INotesViewListener listener)
        {
            OnButtonClickEvent -= listener.OnMouseClick;
        }

        protected void OnClickEventHandler(object sender)
        {
            var note = Builder.GetSenderAsNote(sender);
            OnButtonClickEvent(note);
            switch (ManagersLocator.Instance.Mode)
            {
                case ManagerMode.Info:
                    FretboardManager.FretBoard.DrawAllMatchingNoteWithoutOctaveCheck(note);
                    break;
                case ManagerMode.Excercise:
                    break;
            }
        }

        public void ResetEvents()
        {
            OnButtonClickEvent = null;
        }
    }
}