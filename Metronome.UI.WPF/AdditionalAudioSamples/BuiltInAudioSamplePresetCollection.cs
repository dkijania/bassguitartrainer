using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DrumMachine.Audio;
using DrumMachine.Engine.Sample;

namespace Metronome.UI.WPF.AdditionalAudioSamples
{
    public class BuiltInAudioSamplePresetCollection : IAudioSamplesPresetCollection
    {
        private readonly AudioPresets _audioPresets = new AudioPresets();

        public BuiltInAudioSamplePresetCollection()
        {
            _audioPresets.Add(AudioPresetEnum.DrumStick, GetDrumStickSamples()); 
            _audioPresets.Add(AudioPresetEnum.Percussion, GetPercussionSamples()); 
            _audioPresets.Add(AudioPresetEnum.RealMetronome1, GetRealMetronome1Samples()); 
            _audioPresets.Add(AudioPresetEnum.RealMetronome2, GetRealMetronome2Samples()); 
            _audioPresets.Add(AudioPresetEnum.Ring, GetRingSamples()); 
            _audioPresets.Add(AudioPresetEnum.Drum,GetDrumSamples());
        }
        
        private Tuple<SampleSource, SampleSource> GetDrumStickSamples()
        {
            const string root = @"AdditionalAudioSamples\DrumStick";
            var hiSample = SampleSource.CreateFromWaveFile(Path.Combine(root,"hi.wav"));
            var lowSample = SampleSource.CreateFromWaveFile(Path.Combine(root, "low.wav"));
            return new Tuple<SampleSource, SampleSource>(hiSample,lowSample);
        }

        private Tuple<SampleSource, SampleSource> GetPercussionSamples()
        {
            const string root = @"AdditionalAudioSamples\Percussion";
            var hiSample = SampleSource.CreateFromWaveFile(Path.Combine(root, "hi.wav"));
            var lowSample = SampleSource.CreateFromWaveFile(Path.Combine(root, "low.wav"));
            return new Tuple<SampleSource, SampleSource>(hiSample, lowSample);
        }
        private Tuple<SampleSource, SampleSource> GetRealMetronome1Samples()
        {
            const string root = @"AdditionalAudioSamples\RealMetronome1";
            var hiSample = SampleSource.CreateFromWaveFile(Path.Combine(root, "hi.wav"));
            var lowSample = SampleSource.CreateFromWaveFile(Path.Combine(root, "low.wav"));
            return new Tuple<SampleSource, SampleSource>(hiSample, lowSample);
        }
        private Tuple<SampleSource, SampleSource> GetRealMetronome2Samples()
        {
            const string root = @"AdditionalAudioSamples\RealMetronome2";
            var hiSample = SampleSource.CreateFromWaveFile(Path.Combine(root, "hi.wav"));
            var lowSample = SampleSource.CreateFromWaveFile(Path.Combine(root, "low.wav"));
            return new Tuple<SampleSource, SampleSource>(hiSample, lowSample);
        }
        private Tuple<SampleSource, SampleSource> GetRingSamples()
        {
            const string root = @"AdditionalAudioSamples\Ring";
            var hiSample = SampleSource.CreateFromWaveFile(Path.Combine(root, "hi.wav"));
            var lowSample = SampleSource.CreateFromWaveFile(Path.Combine(root, "low.wav"));
            return new Tuple<SampleSource, SampleSource>(hiSample, lowSample);
        }

        private Tuple<SampleSource, SampleSource> GetDrumSamples()
        {
            var samples = DefaultAudioSampler.Instance.Samples;
            var sampleSources = samples as SampleSource[] ?? samples.ToArray();
            return new Tuple<SampleSource, SampleSource>(sampleSources[1],sampleSources[2]);
        } 
        
        public SampleSource GetLow(AudioPresetEnum presetEnum)
        {
            return _audioPresets[presetEnum].Item2;
        }

        public SampleSource GetHigh(AudioPresetEnum presetEnum)
        {
            return _audioPresets[presetEnum].Item1;
        }

        public IEnumerable<SampleSource> GetBoth(AudioPresetEnum presetEnum)
        {
            return new[] {GetLow(presetEnum), GetHigh(presetEnum)};
        }

        public bool IsDrumStickEnabled { get; set; }
        public bool IsRealMetronome1Enabled { get; set; }
        public bool IsRealMetronome2Enabled { get; set; }
        public bool IsRingEnabled { get; set; }
        public bool IsPercussionEnabled { get; set; }
    }
}