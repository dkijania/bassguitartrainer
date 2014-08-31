using System.Collections.Generic;
using DrumMachine.Engine.Sample;

namespace Metronome.UI.WPF.AdditionalAudioSamples
{
    public interface IAudioSamplesPresetCollection
    {
        SampleSource GetLow(AudioPresetEnum presetEnum);
        SampleSource GetHigh(AudioPresetEnum presetEnum);
        IEnumerable<SampleSource> GetBoth(AudioPresetEnum presetEnum);
 }
}