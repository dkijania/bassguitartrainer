using System;
using System.Collections.Generic;
using DrumMachine.Engine.Model;
using DrumMachine.Engine.Sample;

namespace DrumMachine.Engine.Pattern
{
    public class PatternSequencer
    {
        public DrumPattern Pattern { get; private set; }
        public DrumKit DrumKit { get; set; }
        private int tempo;
        private int samplesPerStep;
        
        public PatternSequencer(DrumPattern pattern, DrumKit kit)
        {
            DrumKit = kit;
            this.Pattern = pattern;
            Tempo = 120;
        }

        public int Tempo
        {
            get
            {
                return this.tempo;
            }
            set
            {
                if (this.tempo != value)
                { 
                    this.tempo = value;
                    this.newTempo = true;
                }
            }
        }

       

        private bool newTempo;
        public int currentStep = 0;
        private double patternPosition = 0;

        public IList<MusicSampleProvider> GetNextMixerInputs(int sampleCount)
        {
            var mixerInputs = new List<MusicSampleProvider>();
            var samplePos = 0;
            if (newTempo)
            {
                int samplesPerBeat = (this.DrumKit.WaveFormat.Channels * this.DrumKit.WaveFormat.SampleRate * 60) / tempo;
                this.samplesPerStep = samplesPerBeat / 16;
                //patternPosition = 0;
                newTempo = false;
            }

            while (samplePos < sampleCount)
            {
                var offsetFromCurrent = (currentStep - patternPosition);
                if (offsetFromCurrent < 0) offsetFromCurrent += Pattern.Steps;
                var delayForThisStep = (int)(this.samplesPerStep * offsetFromCurrent);
                if (delayForThisStep >= sampleCount)
                {
                    // don't queue up any samples beyond the requested time range
                    break;
                }

                for (int note = 0; note < Pattern.Samples; note++)
                {
                    if (Pattern[note, currentStep] != 0)
                    {
                        var sampleProvider = DrumKit.GetSampleProvider(note);
                        sampleProvider.DelayBy = delayForThisStep;
                     //   Console.WriteLine("beat at step {0}, patternPostion={1}, delayBy {2}", currentStep, patternPosition, delayForThisStep);
                        mixerInputs.Add(sampleProvider);
                    }
                }

                samplePos += samplesPerStep;
                currentStep++;
                currentStep = currentStep % Pattern.Steps;
            }
            patternPosition += ((double)sampleCount / samplesPerStep);
            if (patternPosition > Pattern.Steps)
            {
                patternPosition -= Pattern.Steps;
            }
            return mixerInputs;
        }
    }
}
