using System;
using System.Windows;
using DrumMachine.UI.WPF.TimeSignature;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Metronome.UI.WPF
{
    /// <summary>
    /// Interaction logic for MetronomeView.xaml
    /// </summary>
    public partial class MetronomeView : IMetronomeView
    {
        public MetroWindow MetroWindow { get; set; }
        public MetronomeViewModel MetronomeViewModel { get; private set; }
        public BpmModelView BpmModelView;

        private readonly CounterDialog _window;
        
        public MetronomeView()
        {
            InitializeComponent();
            var timeSignatureViewModel = new TimeSignatureViewModel();
            TimeSignaturePanel.DataContext = timeSignatureViewModel;
            BpmModelView = new BpmModelView();
            Bpm.DataContext = BpmModelView;
            MetronomeViewModel = new MetronomeViewModel(this, BpmModelView, timeSignatureViewModel.TimeSignature);
            Main.DataContext = MetronomeViewModel;
            _window = new CounterDialog(MetronomeViewModel)
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

        public void OnErrorRaised(Exception exception)
        {
            MetroWindow.ShowMessageAsync("Metronome error", exception.Message);
        }
    }
}