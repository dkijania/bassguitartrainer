using System.Windows.Input;
using WpfExtensions;

namespace Metronome
{
    public class Bpm : BindingDataContextBase
    {
        private int _bpmValue = 60;
        private const int MinValue = 20;
        private const int MaxValue = 320;
        public ICommand Increment { get; private set; }
        public ICommand Decrement { get; private set; }
        public ICommand Set { get; private set; }

        public Bpm()
        {
            Increment = new DelegateCommand(() => BpmValue++);
            Decrement = new DelegateCommand(() => BpmValue--);
            Set = new RelayCommand(x => BpmValue = int.Parse(x.ToString()));
        }

        public int BpmValue
        {
            get { return _bpmValue; }
            set
            {
                if (value > MaxValue || value < MinValue)
                    return;
                _bpmValue = value;
                OnPropertyChanged();
            }
        }
    }
}