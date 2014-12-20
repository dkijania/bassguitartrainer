using System.Collections.Generic;
using DrumMachine.Engine.Sample;

namespace Metronome.Resources.Metronome.AudioSamples
{
    public interface IAudioSamplesPresetCollection
    {
        SampleSource GetLow(AudioPresetEnum presetEnum);
        SampleSource GetHigh(AudioPresetEnum presetEnum);
        IEnumerable<SampleSource> GetBoth(AudioPresetEnum presetEnum);
 }
}