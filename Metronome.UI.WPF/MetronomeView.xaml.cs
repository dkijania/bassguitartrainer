using System.Windows;
using DrumMachine.UI.WPF.TimeSignature;

namespace Metronome.UI.WPF
{
    /// <summary>
    /// Interaction logic for MetronomeView.xaml
    /// </summary>
    public partial class MetronomeView : IMetronomeView
    {
        private CounterDialog _window;
        
        public MetronomeView()
        {
            InitializeComponent();
            
            var timeSignatureViewModel = new TimeSignatureViewModel();
            TimeSignaturePanel.DataContext = timeSignatureViewModel;
        
            var bpmViewModel = new BpmModelView();
            Bpm.DataContext = bpmViewModel;
            
            var viewModel = new MetronomeViewModel(this, bpmViewModel, timeSignatureViewModel);
            Main.DataContext = viewModel;

            InitializeCounterDialog(viewModel);
         }

        private void InitializeCounterDialog(MetronomeViewModel viewModel)
        {
            _window = new CounterDialog(viewModel)
            {
                Topmost = true,
            };
        }
        
        public void EnableFullScreenMode()
        {
            _window.ShowDialog(Application.Current.MainWindow);
        }

        public void DisableFullScreenMode()
        {
        }
    }
}