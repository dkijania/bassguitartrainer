using DrumMachine.Engine.Pattern;

namespace DrumMachine.UI.WPF
{
    public interface IPatternHighlighter
    {
        void HighlightColumnOnBeat(DrumPattern drumPattern);
        void CleanUpHighlight();
    }
}