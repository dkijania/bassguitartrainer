using System;
using System.Windows.Input;
using DrumMachine.Engine.Pattern;
using DrumMachine.TimeSignature;
using DrumMachine.UI.WPF.TimeSignature;
using WpfExtensions;

namespace Metronome.UI.WPF
{
    public class MetronomeViewModel : BindingDataContextBase
    {
        private readonly MetronomeModel _metronomeModel;
        public IMetronomeView MetronomeView { get; private set; }

        public ICommand StopStart { get; private set; }
        public ICommand SetAudioPresetCommand { get; set; }

        private String _progressMessage;
        private bool _isFullScreen;
        private bool _showCounter;
        private int _number;

        public string ProgressMessage
        {
            get { return _progressMessage; }
            set
            {
                _progressMessage = value;
                OnPropertyChanged();
            }
        }

        public bool IsFullScreen
        {
            get { return _isFullScreen; }
            set
            {
                _isFullScreen = value;
                OnPropertyChanged();
            }
        }

        public bool ShowCounter
        {
            get { return _showCounter; }
            set
            {
                _showCounter = value;
                OnPropertyChanged();
            }
        }

        private bool ShowCounterForBeat
        {
            get { return ShowCounter && _metronomeModel.TimeSignature.IsTimeSignatureEnabled; }
        }

        public void OnBeat(DrumPattern drumPattern)
        {
            if (!ShowCounterForBeat) return;
            _number %= drumPattern.NumberOfHits;
            UpdatePresenter(++_number);
        }

        public void UpdatePresenter(int number)
        {
            ProgressMessage = number.ToString(String.Empty);
        }

        public void OnStartClick()
        {
            if (IsFullScreen)
            {
                MetronomeView.EnableFullScreenMode();
                ProgressMessage = "START";
            }
            else
            {
                ProgressMessage = "STOP";
            }
            if (ShowCounterForBeat && !_metronomeModel.IsStopped)
            {
                UpdatePresenter(1);
            }
        }

        public void OnStopClick()
        {
            if (IsFullScreen)
            {
                MetronomeView.DisableFullScreenMode();
            }
            ProgressMessage = "START";
            _number = 0;
        }

        public MetronomeViewModel(IMetronomeView metronomeView, BpmModelView bpmModelView, TimeSignature timeSignature)
        {
            MetronomeView = metronomeView;
            _metronomeModel = new MetronomeModel(timeSignature, bpmModelView.Model);
            StopStart = new DelegateCommand(PlayStopMetronome);
            SetAudioPresetCommand = new RelayCommand(_metronomeModel.SetAudioPreset);
            ProgressMessage = "START";
            bpmModelView.PropertyChanged += _metronomeModel.SetTempo;
        }

        public MetronomeViewModel(IMetronomeView metronomeView, BpmModelView bpmViewModel,
            TimeSignatureViewModel timeSignatureViewModel)
            : this(metronomeView, bpmViewModel, timeSignatureViewModel.TimeSignature)
        {
        }

        public void PlayStopMetronome()
        {
            if (_metronomeModel.IsStopped)
            {
                _metronomeModel.PlayMetronome(OnBeat);
                OnStartClick();
            }
            else
            {
                StopMetronome();
            }
        }

        public void PauseContinueMetronome()
        {
            if (_metronomeModel.IsStopped)
            {
                _metronomeModel.PlayMetronome(OnBeat);
            }
            else
            {
                _metronomeModel.StopMetronome();
            }
        }

        public void StopMetronome()
        {
            OnStopClick();
            _metronomeModel.StopMetronome();
        }

        public bool IsStopped
        {
            get { return _metronomeModel.IsStopped; }
        }
    }
}