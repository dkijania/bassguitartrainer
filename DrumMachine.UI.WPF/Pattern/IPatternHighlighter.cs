using DrumMachine.Engine.Pattern;

namespace DrumMachine.UI.WPF.Pattern
{
    public interface IPatternHighlighter
    {
        void HighlightColumnOnBeat(DrumPattern drumPattern);
        void CleanUpHighlight();
    }
}