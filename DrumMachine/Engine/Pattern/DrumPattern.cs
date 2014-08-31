using System.Collections.Generic;
using System.Linq;
using DrumMachine.TimeSignature;

namespace DrumMachine.Engine.Pattern
{
    public class DrumPattern
    {
        private readonly byte[,] _hits;
        public static byte Hit = 127;
        public static byte NoHit = 0;
      
        public DrumPattern(int samples, int steps)
        {
            Samples = samples;
            Steps = steps;
            
            _hits = new byte[samples,steps];
        }

        public DrumPattern(int samples, TimeSignatureOptions timeSignatureOptions):
            this(samples,GetNrOfSamples(timeSignatureOptions))
        {
             Interval = 64/(int) timeSignatureOptions.NoteType;
        }

        private static int GetNrOfSamples(TimeSignatureOptions timeSignatureOptions)
        {
            var nrOfSamples = timeSignatureOptions.NotesPerMeasure * 64 / (int)timeSignatureOptions.NoteType;
            if (nrOfSamples <= 4)
            {
                nrOfSamples = 16;
            }
            return nrOfSamples;
        }
        
        public int Samples { get; set; }
        public int Steps { get; private set; }
        public int Interval { get; private set; }
        public int NumberOfHits{get { return GetHitsIndices().Length; }}

        public int[] HitsColumns
        {
            get
            {
                return GetHitsIndices().Select(x => x/Interval).ToArray();
            }
        }

        public int[] GetHitsIndices()
        {
            var list = new List<int>();
            for (var i = 0; i < Steps; i++)
            {
                for (var j = 0; j < Samples; j++)
                {
                    if (_hits[j, i] != Hit) continue;
                    list.Add(i);
                    break; 
                }
            }
            return list.ToArray();
        }
        
        public byte this[int note, int step]
        {
            get { return _hits[note, step]; }
            set { _hits[note, step] = value; }
        }

        public byte[,] Array { get { return _hits; } }
    }
}