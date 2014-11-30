namespace BassNotesMasterApi.Utils.Keyboard
{
    public abstract class AbstractKeyboardEventManager : Manager
    {
        public bool OctaveCheckEnabled { get; set; }
        public event OnCombinationPressed OnCombinationPressedEvent;

        public delegate void OnCombinationPressed(Note note);

        protected void FireOnCombinationPressedEvent(Note note)
        {
            var handler = OnCombinationPressedEvent;
            if (handler != null) handler(note);
        }

        public override ManagerMode Mode { get; set; }

        public override void RemoveAllSubscribers()
        {
            OnCombinationPressedEvent = null;
        }

        public override void OnModeChanged(ManagerMode mode)
        {
        }
    }
}