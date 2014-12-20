using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DrumMachine.Engine.Sample;
using DrumMachine.Resources.DrumMachine.Audio;

namespace Metronome.Resources.Metronome.AudioSamples
{
    public class BuiltInAudioSamplePresetCollection : IAudioSamplesPresetCollection
    {
        private readonly AudioPresets _audioPresets = new AudioPresets();
        const string Root = @"Resources\Metronome\AudioSamples";
        const string DrumStickRoot = Root + @"\DrumStick";
        const string PercussionRoot = Root + @"\Percussion";
        const string Metronome1Root = Root + @"\RealMetronome1";
        const string Metronome2Root = Root + @"\RealMetronome2";
        const string RingRoot = Root + @"\Ring";
        

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
            var hiSample = SampleSource.CreateFromWaveFile(Path.Combine(DrumStickRoot,"hi.wav"));
            var lowSample = SampleSource.CreateFromWaveFile(Path.Combine(DrumStickRoot, "low.wav"));
            return new Tuple<SampleSource, SampleSource>(hiSample,lowSample);
        }

        private Tuple<SampleSource, SampleSource> GetPercussionSamples()
        {
            var hiSample = SampleSource.CreateFromWaveFile(Path.Combine(PercussionRoot, "hi.wav"));
            var lowSample = SampleSource.CreateFromWaveFile(Path.Combine(PercussionRoot, "low.wav"));
            return new Tuple<SampleSource, SampleSource>(hiSample, lowSample);
        }
        private Tuple<SampleSource, SampleSource> GetRealMetronome1Samples()
        {
            var hiSample = SampleSource.CreateFromWaveFile(Path.Combine(Metronome1Root, "hi.wav"));
            var lowSample = SampleSource.CreateFromWaveFile(Path.Combine(Metronome1Root, "low.wav"));
            return new Tuple<SampleSource, SampleSource>(hiSample, lowSample);
        }
        private Tuple<SampleSource, SampleSource> GetRealMetronome2Samples()
        {
            var hiSample = SampleSource.CreateFromWaveFile(Path.Combine(Metronome2Root, "hi.wav"));
            var lowSample = SampleSource.CreateFromWaveFile(Path.Combine(Metronome2Root, "low.wav"));
            return new Tuple<SampleSource, SampleSource>(hiSample, lowSample);
        }
        private Tuple<SampleSource, SampleSource> GetRingSamples()
        {
            var hiSample = SampleSource.CreateFromWaveFile(Path.Combine(RingRoot, "hi.wav"));
            var lowSample = SampleSource.CreateFromWaveFile(Path.Combine(RingRoot, "low.wav"));
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