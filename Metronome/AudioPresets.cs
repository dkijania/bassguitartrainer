using System;
using System.Collections.Generic;
using DrumMachine.Engine.Sample;

namespace Metronome
{
    public class AudioPresets
    {
        private readonly IDictionary<AudioPresetEnum, Tuple<SampleSource,SampleSource>> _sampleSources = new Dictionary<AudioPresetEnum, Tuple<SampleSource,SampleSource>>();
       
        public void Add(AudioPresetEnum key, string hiToneFileName,string lowToneFileName)
        {
            var hi = SampleSource.CreateFromWaveFile(hiToneFileName);
            var low = SampleSource.CreateFromWaveFile(lowToneFileName);
            _sampleSources.Add(key, new Tuple<SampleSource, SampleSource>(hi,low));
        }

        public void Add(AudioPresetEnum key, SampleSource hi, SampleSource low)
        {
            _sampleSources.Add(key,new Tuple<SampleSource, SampleSource>(hi,low));
        }

        public void Add(AudioPresetEnum key, Tuple<SampleSource, SampleSource> tuple)
        {
            _sampleSources.Add(key,tuple);
        }

        public Tuple<SampleSource, SampleSource> this[AudioPresetEnum key]
        {
            get { return _sampleSources[key]; }
        }
    }
}