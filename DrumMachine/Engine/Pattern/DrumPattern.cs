using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrumMachine.TimeSignature;

namespace DrumMachine.Engine.Pattern
{
    public class DrumPattern
    {
        public DrumPatternSettings Settings { get; set; }
        private byte[,] _hits;
        public static byte Hit = 127;
        public static byte NoHit = 0;

        public DrumPattern(int samples, int steps)
        {
            Samples = samples;
            Steps = steps;
            Settings = new DrumPatternSettings();
            _hits = new byte[samples, steps];
        }

        public DrumPattern(int samples, TimeSignatureOptions timeSignatureOptions) :
            this(samples, GetNrOfSamples(timeSignatureOptions))
        {
            Interval = 64/(int) timeSignatureOptions.NoteType;
        }

        public DrumPattern(DrumPatternSettings drumPatternSetttings, Byte[,] array)
        {
            Settings = drumPatternSetttings;
            Array = array;
        }

        private static int GetNrOfSamples(TimeSignatureOptions timeSignatureOptions)
        {
            var nrOfSamples = timeSignatureOptions.NotesPerMeasure*64/(int) timeSignatureOptions.NoteType;
            if (nrOfSamples <= 4)
            {
                nrOfSamples = 16;
            }
            return nrOfSamples;
        }

        public int Samples { get; set; }
        public int Steps { get; private set; }
        public int Interval { get; private set; }

        public int NumberOfHits
        {
            get { return GetHitsIndices().Length; }
        }

        public int[] GetHitsColumnsForRow(int i)
        {
            return GetHitsIndices(i).Select(x => x/Interval).ToArray();
        }


        public int[] GetHitsIndices(int j)
        {
            var list = new List<int>();
            for (var i = 0; i < Steps; i++)
            {
                if (_hits[j, i] == Hit) list.Add(i);
            }
            return list.ToArray();
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

        public byte[,] Array
        {
            get { return _hits; }
            private set { _hits = value; }
        }

        public override String ToString()
        {
            var builder = new StringBuilder();
            var rowLength = Array.GetLength(0);
            var colLength = Array.GetLength(1);

            for (var i = 0; i < rowLength; i++)
            {
                for (var j = 0; j < colLength; j++)
                {
                    builder.Append(string.Format("{0} ", Array[i, j]));
                }
                builder.Append(Environment.NewLine + Environment.NewLine);
            }
            return builder.ToString();
        }
    }
}