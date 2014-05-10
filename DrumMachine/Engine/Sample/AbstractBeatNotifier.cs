namespace DrumMachine.Engine.Sample
{
    public class AbstractBeatNotifier
    {
        public event OnBeatEvent OnBeat;
        public delegate void OnBeatEvent();

        protected virtual void RaiseOnBeatEvent()
        {
            var handler = OnBeat;
            if (handler != null) handler();
        }
    }
}