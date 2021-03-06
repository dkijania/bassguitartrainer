using BassTrainer.Core.Const;
using BassTrainer.Core.Resources;
using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Components.Notation
{
    public abstract class MusicNotationGraphicObjectsManager
    {
        protected double CanvasHeight;
        protected double CanvasWidth;
        protected double LineOffset;
        protected double HeightCenter;
        protected double WidthCenter;

        protected  double FourthSteveLineYPosition;

        protected readonly ResourcesManager ResourceManager = ResourcesManager.Instance;
        
        public abstract void ClearView();
        public abstract void DrawBassStave();
        public abstract void DrawNote(int distanceFromBottom, NotesInfo.Accidentals accidentals, NotesInfo.NoteStem stemDirection,
                                      Note note, StringFretPair stringFretPair, bool isCorrect);

        public abstract double GetDistanceFromBottom(double height);

        public abstract void RemoveTransparentLedgerLines(); 
        public abstract void DrawTransparentLedgerLines();
        
    }
}