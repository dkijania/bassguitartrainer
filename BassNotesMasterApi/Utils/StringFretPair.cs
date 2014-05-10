using System;
using System.Windows;
using BassNotesMasterApi.Const;

namespace BassNotesMasterApi.Utils
{
    public class StringFretPair : IComparable<StringFretPair>
    {
        public readonly FretBoardOptions.StringName StringName;
        public readonly int FretNo;

        public StringFretPair(FretBoardOptions.StringName stringName, int fretNo)
        {
            StringName = stringName;
            FretNo = fretNo;
        }

        public StringFretPair(String stringName, int fretNo)
            : this((FretBoardOptions.StringName) Enum.Parse(typeof(FretBoardOptions.StringName), stringName), fretNo)
        {
      
        }

        public StringFretPair(int stringNo, int fretNo)
            : this((FretBoardOptions.StringName) stringNo, fretNo)
        {
        }

        public StringFretPair(Point point)
            : this((FretBoardOptions.StringName) point.Y, (int)point.X)
        {
        }

        protected bool Equals(StringFretPair other)
        {
            return StringName.Equals(other.StringName) && FretNo == other.FretNo;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((StringFretPair) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (StringName.GetHashCode()*397) ^ FretNo;
            }
        }

        public int CompareTo(StringFretPair other)
        {
            return FretNo - other.FretNo;
        }

        public override string ToString()
        {
            return String.Format((string) "StringName: {0}, FretNo: {1}", (object) StringName, FretNo);
        }

        public Point AsPoint()
        {
            return new Point(FretNo,(int) StringName);
        }
    }
}