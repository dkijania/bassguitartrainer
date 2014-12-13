using BassTrainer.Core.Components;
using BassTrainer.Core.Const;

namespace BassTrainer.Core.Utils.Keyboard
{
    public abstract class AbstractKeyboardEventComponent : Component
    {
        public bool OctaveCheckEnabled { get; set; }
        public event OnCombinationPressed OnCombinationPressedEvent;

        public delegate void OnCombinationPressed(Note note);

        protected void FireOnCombinationPressedEvent(Note note)
        {
            var handler = OnCombinationPressedEvent;
            if (handler != null) handler(note);
        }
        
        public override void RemoveAllSubscribers()
        {
            OnCombinationPressedEvent = null;
        }

        public override void OnModeChanged(ComponentMode mode)
        {
        }
    }
}