using System;
using System.Collections.Generic;
using System.Linq;
using DrumMachine.Engine.Pattern;
using DrumMachine.Engine.Sample;
using NAudio.Wave;

namespace DrumMachine.Engine.Model
{
    public class DrumKit
    {
        private readonly IEnumerable<SampleSource> _sampleSources;
        private readonly DrumPattern _pattern;
        private readonly WaveFormat _waveFormat;
        
        public OnBeatHit OnBeatHitEvent;

        public delegate void OnBeatHit(DrumPattern drumPattern);

        public DrumKit(IEnumerable<SampleSource> sampleSources, DrumPattern pattern)
        {
            if(!sampleSources.Any())
            {
                throw new Exception("Collection is empty");
            }
            this._sampleSources = sampleSources;
            _pattern = pattern;

            var templateSample = sampleSources.Last();
            _waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(templateSample.SampleWaveFormat.SampleRate, templateSample.SampleWaveFormat.Channels);
        }

      
        public virtual WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }

        public MusicSampleProvider GetSampleProvider(int note)
        {
           var  abstractBeatNotifier =  new MusicSampleProvider(_sampleSources.ElementAt(note));
           abstractBeatNotifier.OnBeat += AbstractBeatNotifierOnOnBeat;
           return abstractBeatNotifier;
        }

        private void AbstractBeatNotifierOnOnBeat()
        {
            if(OnBeatHitEvent != null)
            {
                OnBeatHitEvent(_pattern);
            }
        }
    }
}
