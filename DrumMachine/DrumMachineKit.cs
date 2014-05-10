using System.Collections.Generic;
using System.Windows.Input;
using DrumMachine.Audio;
using DrumMachine.Engine.Model;
using DrumMachine.Engine.Pattern;
using DrumMachine.Engine.Sample;

namespace DrumMachine
{
    public class DrumMachineKit
    {
        private readonly DrumMachineModel _model;

        public event DrumKit.OnBeatHit OnBeatHitEvent
        {
            add { _model.OnBeatHitEvent += value; }
            remove { _model.OnBeatHitEvent -= value; }
        }

        public DrumMachineKit(TimeSignature.TimeSignatureOptions timeSignatureOptions)
            : this(timeSignatureOptions.ToDrumPattern())
        {
        }

        public DrumMachineKit(TimeSignature.TimeSignatureOptions timeSignatureOptions, IEnumerable<SampleSource> samples)
            : this(timeSignatureOptions.ToDrumPattern(), samples)
        {
        }
        
        public DrumMachineKit(DrumPattern pattern) : this(pattern, DefaultAudioSampler.Instance.Samples)
        {
        }

        public DrumMachineKit(DrumPattern pattern, IEnumerable<SampleSource> samples)
        {
            _model = new DrumMachineModel(pattern, samples);
        }

        public void Play()
        {
            _model.Play();
            IsStopped = false;
        }

        public void Stop()
        {
            _model.Stop();
            IsStopped = true;
        }

        public bool IsStopped { get; private set; }

        public int Tempo
        {
            get { return _model.Tempo; }
            set { _model.Tempo = value; }
        }
    }
}