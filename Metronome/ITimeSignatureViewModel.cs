using DrumMachine.TimeSignature;

namespace Metronome
{
    public interface ITimeSignatureViewModel
    {
        bool EnableTimeSignature { get; set; }
        TimeSignatureOptions SelectedTimeSignature { get; }
    }
}