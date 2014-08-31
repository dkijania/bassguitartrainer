using System;
using System.Windows.Input;
using DrumMachine;
using DrumMachine.TimeSignature;
using WpfExtensions;
using WpfMetronome.AdditionalAudioSamples;

namespace WpfMetronome
{
    public class Metronome : BindingDataContextBase
    {
        public Bpm Bpm { get; private set; }
      
        private DrumMachineKit _drumMachine;
        public AudioPresetEnum AudioPreset { get; set; }
        
        public IMetronomeView MetronomeView { get; private set; }
        public ITimeSignatureViewModel TimeSignature { get; set; }
        private readonly IAudioSamplesPresetCollection _samplesPresetCollection;

        public ICommand StopStart { get; private set; }
        public ICommand SetAudioPresetCommand { get; set; }
        
        public Metronome(IMetronomeView metronomeView,ITimeSignatureViewModel timeSignature )
        {
            MetronomeView = metronomeView;
            TimeSignature = timeSignature;
            AudioPreset = AudioPresetEnum.Drum;
            _samplesPresetCollection = new BuiltInAudioSamplePresetCollection();
           StopStart = new DelegateCommand(PlayStopMetronome,MetronomeView);
            SetAudioPresetCommand = new RelayCommand(SetAudioPreset);
            Bpm = new Bpm();
            Bpm.PropertyChanged += SetTempo;
        }

        public void SetAudioPreset(object audioPreset)
        {
            AudioPreset = (AudioPresetEnum) Enum.Parse(typeof (AudioPresetEnum), audioPreset.ToString());
        }

        private void SetTempo(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_drumMachine == null) return;
            _drumMachine.Tempo = Bpm.BpmValue;
        }

        public void PlayStopMetronome()
        {
            if (_drumMachine == null || _drumMachine.IsStopped)
            {
                PlayMetronome();
                MetronomeView.OnStartClick();
            }
            else
            {
                StopMetronome();
                MetronomeView.OnStopClick();
            }
        }

        public void PlayMetronome()
        {
           CheckIfTimeSignatureIsSet();
           _drumMachine = new DrumMachineKit(TimeSignature.SelectedTimeSignature, _samplesPresetCollection.GetBoth(AudioPreset))
            {
                Tempo = Bpm.BpmValue
            };
           _drumMachine.OnBeatHitEvent += _drumMachine_OnBeatHitEvent;
           _drumMachine.Play();
        }


        void _drumMachine_OnBeatHitEvent(DrumMachine.Engine.Pattern.DrumPattern drumPattern)
        {
            MetronomeView.OnBeat(drumPattern);
        }

        private void CheckIfTimeSignatureIsSet()
        {
            if (TimeSignature.SelectedTimeSignature.Equals(TimeSignatureOptions.NotSet))
            {
                throw new MetronomeException("Time singature not defined");
            }           
        }

        public void StopMetronome()
        {
            if (_drumMachine != null)
            {
                _drumMachine.Stop();
            }
        }

        public bool IsStopped { get { return _drumMachine.IsStopped; } }

    }

    public class MetronomeException : Exception
    {
        public MetronomeException()
        {
        }

        public MetronomeException(string message) : base(message)
        {
        }
    }
}