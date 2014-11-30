using System.Windows;
using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Components.Fretboard
{
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