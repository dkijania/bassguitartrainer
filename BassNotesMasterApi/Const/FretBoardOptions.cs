using System;
using System.Collections.Generic;
using System.Linq;

namespace BassNotesMasterApi.Const
{
    public class FretBoardOptions : ICloneable
    {
        public const int NoOfFret = 12;

        public static int NoOfStrings
        {
            get { return Enum.GetNames(typeof (StringName)).Count(); }
        }

        public FretBoardShow Show = FretBoardShow.Sharps;

        public enum StringName
        {
            G = 0,
            D = 1,
            A = 2,
            E = 3
        }

        public IEnumerable<String> Strings
        {
            get { return GetAllString(); }
        }

        private IEnumerable<string> GetAllString()
        {
            return Enum.GetNames(typeof (StringName)).ToList();
        }

        protected bool Equals(FretBoardOptions other)
        {
            return Show.Equals(other.Show);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((FretBoardOptions) obj);
        }

        public override int GetHashCode()
        {
            return Show.GetHashCode();
        }

        public object Clone()
        {
            return new FretBoardOptions
                       {
                           Show = Show,
                       };
        }
    }
}