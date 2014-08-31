using WpfExtensions;

namespace Metronome.UI.WPF
{
    public interface IMetronomeView : ICommandExceptionHandler
    {
        void EnableFullScreenMode();
        void DisableFullScreenMode();
    }
}