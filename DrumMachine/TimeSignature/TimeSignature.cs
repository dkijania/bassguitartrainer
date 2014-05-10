using System;
using System.Collections.Generic;
using System.Linq;
using DrumMachine.Engine.Pattern;

namespace DrumMachine.TimeSignature
{
    public class TimeSignatureOptions
    {
        public static IEnumerable<int> PosibleNotesTypes = Enum.GetValues(typeof (NoteTypeEnum)).Cast<int>();
        public static IEnumerable<int> PossibleNotesCountPerMeasure = Enumerable.Range(1, 32);

        private int _notesPerMeasure;
        private NoteTypeEnum _noteType;


        public TimeSignatureOptions()
        {
        }

        public TimeSignatureOptions(int notesPerMeasure, NoteTypeEnum noteType)
        {
            _notesPerMeasure = notesPerMeasure;
            _noteType = noteType;
        }

        public int NotesPerMeasure
        {
            get { return _notesPerMeasure; }
            set
            {
                if (!PossibleNotesCountPerMeasure.Contains(value))
                {
                    throw new TimeSignatureException(
                        String.Format("value ({0}) is not in PossibleNotesCountPerMeasure collection", value));
                }
                _notesPerMeasure = value;
            }
        }

        public NoteTypeEnum NoteType
        {
            get { return _noteType; }
            set
            {
                if (!PosibleNotesTypes.Contains((int) value))
                {
                    throw new TimeSignatureException(String.Format(
                        "value ({0}) is not in PosibleNotesTypes collection", value));
                }

                _noteType = value;
            }
        }

        public static TimeSignatureOptions Unison
        {
            get { return new TimeSignatureOptions(1, NoteTypeEnum.Quarter); }
        }
        public static TimeSignatureOptions NotSet
        {
            get { return new TimeSignatureOptions(); }
        }

        protected bool Equals(TimeSignatureOptions other)
        {
            return _notesPerMeasure == other._notesPerMeasure && _noteType == other._noteType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TimeSignatureOptions) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_notesPerMeasure*397) ^ (int) _noteType;
            }
        }

        public enum NoteTypeEnum
        {
            Sixteen = 16,
            Eight = 8,
            Quarter = 4,
            Half = 2,
            Whole = 1
        }

        public DrumPattern ToDrumPattern()
        {
            const int firstSampleId = 1;
            const int secondSampleId = 0;
            var drumPattern = new DrumPattern(2, this);

            var startPos = 0;
            var accentNote = NotesPerMeasure > 1;
            if (accentNote)
            {
                drumPattern[firstSampleId, startPos] = DrumPattern.Hit;
                startPos += drumPattern.Interval;
            }
            for (var i = startPos; i < drumPattern.Steps; i += drumPattern.Interval)
            {
                drumPattern[secondSampleId, i] = DrumPattern.Hit;
            }
            return drumPattern;
        }

       

        public class TimeSignatureException : Exception
        {
            public TimeSignatureException()
            {
            }

            public TimeSignatureException(string message) : base(message)
            {
            }
        }

        public static TimeSignatureOptions FromString(string toString)
        {
            var elements = toString.Split('/');
            int upper, lower;
            var upperResult = int.TryParse(elements[0], out upper);
            var lowerResult = int.TryParse(elements[1], out lower);

            if (elements.Count() == 2 && !upperResult && !lowerResult)
            {
                ThrowExceptionForWrongFormat(toString);
            }
            return new TimeSignatureOptions(upper, (NoteTypeEnum) lower);
        }

        private static void ThrowExceptionForWrongFormat(string toString)
        {
            throw new TimeSignatureException(String.Format("Cannot parse string {0}. Only {1} format is acceptable.",
                toString, "X\\X"));
        }
    }
}