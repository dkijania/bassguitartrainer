using DrumMachine.TimeSignature;

namespace WpfMetronome
{
    public interface ITimeSignatureViewModel
    {
        bool EnableTimeSignature { get; set; }
        TimeSignatureOptions SelectedTimeSignature { get; }
    }
}