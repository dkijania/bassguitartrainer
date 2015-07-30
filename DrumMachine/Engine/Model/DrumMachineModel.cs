using System;
using System.Collections.Generic;
using DrumMachine.Engine.Pattern;
using DrumMachine.Engine.Sample;
using NAudio.Wave;

namespace DrumMachine.Engine.Model
{
    public class DrumMachineModel :  IDisposable
    {
        private IWavePlayer _waveOut;
        private DrumPatternSampleProvider _patternSequencer;
        private int _tempo;
        private readonly PatternSequencer _patternSequence;
        private readonly DrumKit _drumKit;

        public event DrumKit.OnBeatHit OnBeatHitEvent
        {
            add { _drumKit.OnBeatHitEvent += value; }
            remove { _drumKit.OnBeatHitEvent -= value; }
        }


        public DrumMachineModel(DrumPattern pattern, IEnumerable<SampleSource> samples)
        {
            _drumKit = new DrumKit(samples, pattern);
            _patternSequence = new PatternSequencer(pattern, _drumKit);
            _tempo = 120;
        }

        public void Play()
        {
            if (_waveOut != null)
            {
                Stop();
            }
            _waveOut = new WaveOut();
            _patternSequencer = new DrumPatternSampleProvider(_patternSequence) {Tempo = _tempo};
            _waveOut.Init(_patternSequencer);
            _waveOut.Play();
        }

        public void Stop()
        {
            if (_waveOut != null)
            {
                _patternSequencer = null;
                _waveOut.Dispose();
                _waveOut = null;
            }
        }

        public void Dispose()
        {
            Stop();
        }

        public int Tempo
        {
            get
            {
                return _tempo;
            }
            set
            {
                if (_tempo != value)
                {
                    _tempo = value;
                    if (_patternSequencer != null)
                    {
                        _patternSequencer.Tempo = value;
                    }
                }
            }
        }

    }
}
