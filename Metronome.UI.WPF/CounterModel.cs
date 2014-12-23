using System.Windows;
using System.Windows.Input;
using WpfExtensions;

namespace Metronome.UI.WPF
{
    public class CounterModel : BindingDataContextBase
    {
        public Window View { get; protected set; }
        public ICommand Stop { get; private set; }
        public ICommand PauseContinue { get; private set; }
        public MetronomeViewModel MetronomeViewModel { get; private set; }

        public string ProgressMessage
        {
            get { return MetronomeViewModel.ProgressMessage; }
            set
            {
                MetronomeViewModel.ProgressMessage = value;
                OnPropertyChanged();
            }
        }

        public CounterModel(Window view,MetronomeViewModel viewModel)
        {
            View = view;
            MetronomeViewModel = viewModel;
            Stop = new DelegateCommand(StopMetronomeAndCloseDialog);
            PauseContinue = new DelegateCommand(PauseContineMetronome);
            MetronomeViewModel.PropertyChanged += MetronomeViewModel_PropertyChanged;
        }

        void MetronomeViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ProgressMessage"))
            {
                OnPropertyChanged(e.PropertyName);
            }
        }

        public void StopMetronomeAndCloseDialog()
        {
            MetronomeViewModel.StopMetronome();
            View.Hide();
        }

        public void PauseContineMetronome()
        {
            MetronomeViewModel.PlayStopMetronome();
       }
    }
}