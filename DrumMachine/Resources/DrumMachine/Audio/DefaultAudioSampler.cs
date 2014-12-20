using System;
using System.Collections.Generic;
using DrumMachine.Engine.Sample;

namespace DrumMachine.Resources.DrumMachine.Audio
{
    public class DefaultAudioSampler
    {
        private static DefaultAudioSampler _instance;
        private const String AudioSamplesRootFolder = @"Resources\DrumMachine\Audio\Samples\";

        public static DefaultAudioSampler Instance
        {
            get { return _instance ?? (_instance = new DefaultAudioSampler()); }
        }

        private readonly Dictionary<String, SampleSource> _dictionary = new Dictionary<string, SampleSource>();

        private DefaultAudioSampler()
        {
            _dictionary.Add("Kick", SampleSource.CreateFromWaveFile(AudioSamplesRootFolder + "kick-trimmed.wav"));
            _dictionary.Add("Snare", SampleSource.CreateFromWaveFile(AudioSamplesRootFolder + "snare-trimmed.wav"));
            _dictionary.Add("Closed Hat", SampleSource.CreateFromWaveFile(AudioSamplesRootFolder + "closed-hat-trimmed.wav"));
            _dictionary.Add("Open Hat", SampleSource.CreateFromWaveFile(AudioSamplesRootFolder + "open-hat-trimmed.wav"));
            _dictionary.Add("Crash", SampleSource.CreateFromWaveFile(AudioSamplesRootFolder + "crash-trimmed.wav"));
            _dictionary.Add("Hi-mid Tom", SampleSource.CreateFromWaveFile(AudioSamplesRootFolder + "Hi-Mid Tom.wav"));
            _dictionary.Add("High Floor Tom", SampleSource.CreateFromWaveFile(AudioSamplesRootFolder + "High Floor Tom.wav"));
            _dictionary.Add("Low Floor Tom", SampleSource.CreateFromWaveFile(AudioSamplesRootFolder + "Low Floor Tom.wav"));
            _dictionary.Add("High Tom", SampleSource.CreateFromWaveFile(AudioSamplesRootFolder + "High Floor Tom.wav"));
            _dictionary.Add("Low Tom", SampleSource.CreateFromWaveFile(AudioSamplesRootFolder + "Low Tom.wav"));
            _dictionary.Add("Low-mid Tom", SampleSource.CreateFromWaveFile(AudioSamplesRootFolder + "Low-Mid Tom.wav"));
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