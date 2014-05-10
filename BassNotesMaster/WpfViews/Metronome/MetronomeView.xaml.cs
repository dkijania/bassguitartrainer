using System;
using System.Windows;
using DrumMachine.Engine.Pattern;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using WpfMetronome;

namespace BassNotesMaster.WpfViews.Metronome
{
    /// <summary>
    /// Interaction logic for MetronomeView.xaml
    /// </summary>
    public partial class MetronomeView : IMetronomeView, IViewControl
    {
        public MetroWindow MetroWindow { get; set; }
        public WpfMetronome.Metronome Metronome { get; private set; }

        private readonly CounterDialog _window;
        private readonly TimeSignatureViewModel _timeSignatureViewModel;
       
        private int _number;

        public MetronomeView(MetroWindow mainWindow)
        {
            MetroWindow = mainWindow;
            InitializeComponent();

            _timeSignatureViewModel = new TimeSignatureViewModel(TimeSignaturePanel);
            TimeSignaturePanel.DataContext = _timeSignatureViewModel;
            Metronome = new WpfMetronome.Metronome(this, _timeSignatureViewModel);
            Main.DataContext = Metronome;
            _window = new CounterDialog
                          {
                              Topmost = true,
                              Model = {Metronome = Metronome}
                          };
        }

        public bool IsFullScreen { get; set; }
        public bool ShowCounter { get; set; }


        public void OnBeat(DrumPattern drumPattern)
        {
            if (!ShowCounter) return;

            if (_number >= drumPattern.NumberOfHits)
            {
                _number = 0;
            }
           
               UpdatePresenter(_number++);
            
        }

        public void UpdatePresenter(int number)
        {
            if(IsFullScreen)
            {
                _window.Model.BeatNumber = number.ToString(String.Empty);
            }
            else
            {
                StartButton.Content = _number;
            }
        }

      /*
        public void OnUseCustomTimeSignatureChange(bool value)
        {
            TimeSignaturePanel.OnUseCustomTimeSignatureChange(value);
        }

        public void OnUseTimeSignature(bool value)
        {
            TimeSignaturePanel.OnUseTimeSignature(value);
        }*/

        public void OnStartClick()
        {
            _number = 1;
            if (ShowCounter)
            {
                StartButton.Content = 0;
            }
          
            if (IsFullScreen)
            {
                _window.ShowDialog(Application.Current.MainWindow);
               StartButton.Content = "START";
            }
            else
            {
                StartButton.Content = "STOP";
           
            }
        }

        public void OnStopClick()
        {
            StartButton.Content = "START";
        }

        public void OnErrorRaised(Exception exception)
        {
            MetroWindow.ShowMessageAsync("Metronome error", exception.Message);
        }
    }
}