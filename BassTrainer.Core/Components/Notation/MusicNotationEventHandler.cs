namespace BassTrainer.Core.Components.Notation
{
    public abstract class MusicNotationEventHandler
    {
        public abstract void UnregisterEventsForCanvas();
        public abstract void RegisterEventsForCanvas();
        public abstract void UnregisterEventsForButtons();
        public abstract void RegisterEventsForButtons();

        public abstract void ClearAllEvents();

        public MusicNotation MusicNotation { get; set; }
    }
}