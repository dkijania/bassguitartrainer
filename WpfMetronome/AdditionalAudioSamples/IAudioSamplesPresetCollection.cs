using System.Collections.Generic;
using DrumMachine.Engine.Sample;

namespace WpfMetronome.AdditionalAudioSamples
{
    public interface IAudioSamplesPresetCollection
    {
        SampleSource GetLow(AudioPresetEnum presetEnum);
        SampleSource GetHigh(AudioPresetEnum presetEnum);
        IEnumerable<SampleSource> GetBoth(AudioPresetEnum presetEnum);
 }
}