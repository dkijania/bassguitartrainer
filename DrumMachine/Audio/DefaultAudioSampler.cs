using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrumMachine.Engine.Sample;

namespace DrumMachine.Audio
{
    public class DefaultAudioSampler
    {
        private static DefaultAudioSampler _instance;

        public static DefaultAudioSampler Instance
        {
            get { return _instance ?? (_instance = new DefaultAudioSampler()); }
        }

        private readonly Dictionary<String, SampleSource> _dictionary = new Dictionary<string, SampleSource>();

        private DefaultAudioSampler()
        {
            _dictionary.Add("Kick", SampleSource.CreateFromWaveFile("Audio\\Samples\\kick-trimmed.wav"));
            _dictionary.Add("Snare", SampleSource.CreateFromWaveFile("Audio\\Samples\\snare-trimmed.wav"));
            _dictionary.Add("Closed Hat", SampleSource.CreateFromWaveFile("Audio\\Samples\\closed-hat-trimmed.wav"));
            _dictionary.Add("Open Hat", SampleSource.CreateFromWaveFile("Audio\\Samples\\open-hat-trimmed.wav"));
            _dictionary.Add("Crash", SampleSource.CreateFromWaveFile("Audio\\Samples\\crash-trimmed.wav"));
            _dictionary.Add("Hi-mid Tom", SampleSource.CreateFromWaveFile("Audio\\Samples\\Hi-Mid Tom.wav"));
            _dictionary.Add("High Floor Tom", SampleSource.CreateFromWaveFile("Audio\\Samples\\High Floor Tom.wav"));
            _dictionary.Add("Low Floor Tom", SampleSource.CreateFromWaveFile("Audio\\Samples\\Low Floor Tom.wav"));
            _dictionary.Add("High Tom", SampleSource.CreateFromWaveFile("Audio\\Samples\\High Floor Tom.wav"));
            _dictionary.Add("Low Tom", SampleSource.CreateFromWaveFile("Audio\\Samples\\Low Tom.wav"));
            _dictionary.Add("Low-mid Tom", SampleSource.CreateFromWaveFile("Audio\\Samples\\Low-Mid Tom.wav"));
        }


        public IEnumerable<String> Names
        {
            get { return _dictionary.Keys; }
        }

        public IEnumerable<SampleSource> Samples
        {
            get { return _dictionary.Values; }
        }

        public SampleSource this[string key]
        {
            get { return _dictionary[key]; }
        }
    }
}