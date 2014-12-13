using System;

namespace BassTrainer.Core.Const
{
    public class Note : ICloneable
    {
        public static string BemolSuffix = NotesConverter.BemolSuffix;
        public static string SharpSuffix = NotesConverter.SharpSuffix;

        public string SharpOrRegularRepresenation;
        public string BemolRepresenation;
        public int OctaveNumber;

        public static string PrintDelimeter = @"\";

        private void ParseString(string noteAsString)
        {
            if (noteAsString.Contains(PrintDelimeter))
            {
                char delim = Convert.ToChar(PrintDelimeter);
                noteAsString = noteAsString.Split(delim)[0];
            }
            if (int.TryParse(noteAsString.Substring(noteAsString.Length - 1), out OctaveNumber))
            {
                noteAsString = noteAsString.Replace(Convert.ToString(OctaveNumber), String.Empty);
            }
            if (noteAsString.Contains(BemolSuffix))
            {
                BemolRepresenation = noteAsString;
                SharpOrRegularRepresenation = NotesConverter.ConvertBemolToSharp(noteAsString);
            }
            else if (noteAsString.Contains(SharpSuffix))
            {
                SharpOrRegularRepresenation = noteAsString;
                BemolRepresenation = NotesConverter.ConvertSharpToBemol(noteAsString);
            }
            else
            {
                SharpOrRegularRepresenation = BemolRepresenation = noteAsString;
            }
        }

        public Note(string noteAsString)
        {
            ParseString(noteAsString);
        }

        public Note(Note noteAsString, NotesInfo.Accidentals accidentals)
        {
            if(!noteAsString.IsNatural())
            {
                ParseString(noteAsString.SharpOrRegularRepresenation + noteAsString.OctaveNumber);
                return;
            }

            switch (accidentals)
            {
                case NotesInfo.Accidentals.Sharp:
                    ParseString(noteAsString.SharpOrRegularRepresenation + SharpSuffix + noteAsString.OctaveNumber);
                    break;
                case NotesInfo.Accidentals.Flat:
                    ParseString(noteAsString.SharpOrRegularRepresenation + BemolSuffix + noteAsString.OctaveNumber);
                    break;
                default:
                    ParseString(noteAsString.SharpOrRegularRepresenation + noteAsString.OctaveNumber);
                    break;
            }
        }

        public string WithoutAccidental
        {
           get { return SharpOrRegularRepresenation.Replace(SharpSuffix, String.Empty); } 
        }

        public bool EqualsWithoutAccidentals(Note other)
        {
            return WithoutAccidental.Equals(other.WithoutAccidental);
        }

        public bool IsNatural()
        {
            return !SharpOrRegularRepresenation.Contains(SharpSuffix) && !BemolRepresenation.Contains(BemolSuffix);
        }

        public override string ToString()
        {
            return IsNatural()
                       ? SharpOrRegularRepresenation
                       : String.Format(@"{0}{1}{2}", SharpOrRegularRepresenation, PrintDelimeter, BemolRepresenation);
        }

        public bool EqualsOrHigherThan(Note other)
        {
            return OctaveNumber >= other.OctaveNumber && string.Equals(BemolRepresenation, other.BemolRepresenation) &&
                   string.Equals(SharpOrRegularRepresenation, other.SharpOrRegularRepresenation);
        }

        public bool EqualsWithoutOctaveNumber(Note other)
        {
            return string.Equals(BemolRepresenation, other.BemolRepresenation) &&
                   string.Equals(SharpOrRegularRepresenation, other.SharpOrRegularRepresenation);
        }

        protected bool Equals(Note other)
        {
            return OctaveNumber == other.OctaveNumber && string.Equals(BemolRepresenation, other.BemolRepresenation) &&
                   string.Equals(SharpOrRegularRepresenation, other.SharpOrRegularRepresenation);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Note) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = OctaveNumber;
                hashCode = (hashCode*397) ^ (BemolRepresenation != null ? BemolRepresenation.GetHashCode() : 0);
                hashCode = (hashCode*397) ^
                           (SharpOrRegularRepresenation != null ? SharpOrRegularRepresenation.GetHashCode() : 0);
                return hashCode;
            }
        }

        

        public object Clone()
        {
            return new Note(SharpOrRegularRepresenation+OctaveNumber);
        }
    }
}