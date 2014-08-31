namespace Metronome.UI.WPF
{
    public class BpmModel
    {
        private int _bpmValue = 80;
        private const int MinValue = 20;
        private const int MaxValue = 320;

        public void Increment()
        {
            BpmValue++;
        }

        public void Decrement()
        {
            BpmValue--;
        }

        public void Set(int bpm)
        {
            BpmValue = bpm;
        }

        public int BpmValue
        {
            get { return _bpmValue; }
            set
            {
                if (value > MaxValue || value < MinValue)
                    return;
                _bpmValue = value;
            }
        }
    }
}