using DrumMachine.Engine.Pattern;
using WpfExtensions;

namespace Metronome
{
    public interface IMetronomePresenter : ICommandExceptionHandler
    {
        void OnStartClick();
        void OnStopClick();
        bool IsFullScreen { get; set; }
        bool ShowCounter { get; set; }
        void OnBeat(DrumPattern drumPattern);
    }
}