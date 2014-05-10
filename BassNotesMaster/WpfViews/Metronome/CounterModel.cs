using System.Windows;
using System.Windows.Input;
using WpfExtensions;

namespace BassNotesMaster.WpfViews.Metronome
{
    public class CounterModel : BindingDataContextBase
    {
        public Window View { get; protected set; }
        public ICommand Stop { get; private set; }
        public ICommand PauseContinue { get; private set; }
        public WpfMetronome.Metronome Metronome { get; set; }

        private string _beatNumber;

        public string BeatNumber
        {
            get { return _beatNumber; }
            set
            {
                _beatNumber = value;
                OnPropertyChanged();
            }
        }

        public CounterModel(Window view)
        {
            View = view;
            Stop = new DelegateCommand(StopMetronomeAndCloseDialog);
            PauseContinue = new DelegateCommand(PauseContineMetronome);
        }

        public void StopMetronomeAndCloseDialog()
        {
            Metronome.StopMetronome();
            View.Hide();
        }

        public void PauseContineMetronome()
        {
            Metronome.PlayStopMetronome();
            if(!Metronome.IsStopped)
            {
                BeatNumber = 0.ToString(string.Empty);
            }
        }
    }
}