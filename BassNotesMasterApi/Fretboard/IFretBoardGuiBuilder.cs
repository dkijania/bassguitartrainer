using System.Collections.Generic;
using System.Windows;
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


    public interface IFretBoardGuiBuilder
    {
        AlwaysRedrawCollection AlwaysRedrawCollection { get; set; }
        Point TryToNormalizeClickedPiont(Point cords);
        void RemoveAllVisibleGraphicNotesRepresentation();
        void Refresh();
        void ClearView();
        void DrawNote(StringFretPair position,double transparency = 1.0,bool drawLabel = true);
        Note GetNote(StringFretPair position);
        void DrawAllGraphicNoteRepresentation();
        StringFretPair GetPosition(Point clikedPoint);
        void DrawNotes(StringFretPair[] collection, double transparency = 1.0,bool drawLabel = true);
        void RedrawNote(StringFretPair stringFretPair, bool isCorrect, bool drawLabel = true);
        StringFretPair[] GetCurrentlyShownPosition();
        void DrawNoteWithQuestionMark(StringFretPair stringFretToFind);
        void RedrawNoteWithQuestionMark(StringFretPair stringFretToFind, bool isCorrect = true);
        bool IsAlreadyDrawn(StringFretPair stringFretPair);
        void DrawNotesIfNotExist(StringFretPair[] collectionOfStringFretPair);
        void ForceClearView();
        void RedrawNotes(StringFretPair[] collection);

        bool ApplyColorForOctaves { get; set; }
        bool ApplyColorForNotes { get; set; }
        bool IgnoreColoring { get; set; }
        bool HideNoteLabel { get; set; }
        void Reset();
    }
}