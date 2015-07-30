using DrumMachine.Engine.Pattern;
using DrumMachine.UI.WPF.Pattern.Converters;

namespace DrumMachine.UI.WPF.Pattern
{
    public interface IPatternTilesManipulator
    {
        int GetMaximumSpanValue();
        int GetMinimumSpanValue();
        int BarsCount { get; }
        IPatternHighlighter PatternHighlighter { get; }
        void SetColumnsCount(int count, int note);
        void AddBar();
        void RemoveBar();
        void FillDrumPattern(DrumPattern drumPattern, UiDrumPatternConverter uiDrumPatternConverter);
        event DrumMachineTile.OnSelect OnSelectEvent;
        event DrumMachineTile.IgnoreMouseClick IgnoreMouseEvent;
        void SplitCell(int row, int column);
        void JoinCell(int row, int column);
        void ResetBarsCount();
        void Clear();
        void ResetUi();
        void ImportToUi(DrumPattern drumPattern, UiDrumPatternConverter uiDrumPatternConverter);
        void SetColumnsSpan(int note);
        void AddCell(int row, int column, int columnSpan, bool isSelected = false);

    }
}