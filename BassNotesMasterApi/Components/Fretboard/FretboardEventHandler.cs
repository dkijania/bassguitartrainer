using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Components.Fretboard
{
    public abstract class FretboardEventHandler
    {
        public event OnMouseClick OnMouseClickEvent;
        public event OnModeChanged OnModeChangedEvent;
      
        public delegate void OnModeChanged(ManagerMode mode);
        public delegate void OnMouseClick(StringFretPair stringFretPair, FretBoard fretBoard);

        protected readonly FretBoard FretBoard;

        protected FretboardEventHandler(FretBoard fretboard)
        {
            FretBoard = fretboard;
        }

        protected void RaiseOnMouseClickEvent(StringFretPair stringFretPair, FretBoard fretBoard)
        {
            var evt = OnMouseClickEvent;
            if (evt != null)
            {
                evt(stringFretPair, fretBoard);
            }
          }

        protected internal void RaiseOnModeChangedEvent(ManagerMode mode)
        {
            var evt = OnModeChangedEvent;
            if (evt != null)
            {
                evt(mode);
            }
        }

        public void Subscribe(IFretboardListener listener)
        {
            OnMouseClickEvent += listener.OnMouseClick;
        }

        public void Unsubscribe(IFretboardListener listener)
        {
            OnMouseClickEvent -= listener.OnMouseClick;
        }
        
        public void RemoveAllClickSubscribers()
        {
            OnMouseClickEvent = null;
        }
        
       
        
        public abstract void SubscribeExcerciseHandlerForFretboard();
        public abstract void UnsubscribeExcerciseHandlerForFretboard();
        public abstract void SubscribeInfoHandlerForFretboard();
        public abstract void UnsubscribeInfoHandlerForFretboard();
        public abstract void SubscribeSelectionHandlerForFretboard();
        public abstract void UnsubscribeSelectionHandlerForFretboard();

        
    }
}