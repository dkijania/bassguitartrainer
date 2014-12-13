using BassTrainer.Core.Components.Fretboard;
using BassTrainer.Core.Const;
using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Components.NotesView
{
    public abstract class NotesViewEventHandler
    {
        public INotesViewGuiBuilder Builder { get; set; }
        public FretboardComponent FretboardComponent { get; set; }
        public event OnButtonClick OnButtonClickEvent;
        public ComponentMode Mode { get; set; } 

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
            switch (Mode)
            {
                case ComponentMode.Info:
                    FretboardComponent.FretBoard.DrawAllMatchingNoteWithoutOctaveCheck(note);
                    break;
                case ComponentMode.Excercise:
                    break;
            }
        }

        public void ResetEvents()
        {
            OnButtonClickEvent = null;
        }
    }
}