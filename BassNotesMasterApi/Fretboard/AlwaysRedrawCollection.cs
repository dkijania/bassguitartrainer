using System.Collections.Generic;
using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Fretboard
{
    public class AlwaysRedrawCollection
    {
        private readonly List<StringFretPair> _alwaysRedraw;

        public bool DrawLabels { get; set; }
        
        
        public bool IsTransparencyEnabled { set; get; }
        public double TransparencyRate = 0.5;

        public double Transparency
        {
            get { return IsTransparencyEnabled ? TransparencyRate : 0.0; }
        }

        public AlwaysRedrawCollection()
            : this(new List<StringFretPair>())
        {

        }

        public AlwaysRedrawCollection(IEnumerable<StringFretPair> alwaysRedraw)
            : this(new List<StringFretPair>(alwaysRedraw))
        {
        }

        public AlwaysRedrawCollection(List<StringFretPair> alwaysRedraw)
        {
            _alwaysRedraw = alwaysRedraw;
            IsTransparencyEnabled = true;
            DrawLabels = true;
        }

        public StringFretPair[] ToArray()
        {
            return _alwaysRedraw.ToArray();
        }

        public void Clear()
        {
            _alwaysRedraw.Clear();
        }

        public void AddRange(IEnumerable<StringFretPair> collection)
        {
            _alwaysRedraw.AddRange(collection);
        }
    }
}