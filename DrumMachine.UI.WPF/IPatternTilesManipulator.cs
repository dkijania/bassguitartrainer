using DrumMachine.Engine.Pattern;

namespace DrumMachine.UI.WPF
{
    public interface IPatternTilesManipulator
    {
        void SetColumnsCount(int count, int span);
        void AddBar();
        void RemoveBar();
        void FillDrumPattern(DrumPattern drumPattern);
        IPatternHighlighter PatternHighlighter { get; }
    }
}