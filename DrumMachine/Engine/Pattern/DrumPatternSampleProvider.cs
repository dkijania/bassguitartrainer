using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace DrumMachine.Engine.Pattern
{
    class DrumPatternSampleProvider : ISampleProvider
    {
        private readonly MixingSampleProvider mixer;
        private readonly WaveFormat waveFormat;
        private readonly PatternSequencer sequencer;
        
        public DrumPatternSampleProvider(PatternSequencer patternSequencer)
        {
            var kit = patternSequencer.DrumKit;
            sequencer = patternSequencer;
            waveFormat = kit.WaveFormat;
            mixer = new MixingSampleProvider(waveFormat);
        }

        public int Tempo
        {
            get
            {
                return sequencer.Tempo;
            }
            set
            {
                sequencer.Tempo = value;
            }
        }

        public WaveFormat WaveFormat
        {
            get { return waveFormat; }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            foreach (var mixerInput in sequencer.GetNextMixerInputs(count))
            {
                mixer.AddMixerInput(mixerInput);
            }

            // now we just need to read from the mixer
            var samplesRead = mixer.Read(buffer, offset, count);
            while (samplesRead < count)
            {
                buffer[samplesRead++] = 0;
            }
            return samplesRead;
        }
    }
}
