using DrumMachine.Engine.Pattern;

namespace BassNotesMaster.WpfViews.DrumMachine
{
    public interface IPatternTilesManipulator
    {
        void SetColumnsCount(int count, int span);
        void AddBar();
        void RemoveBar();
        void FillDrumPattern(DrumPattern drumPattern);
    }
}