namespace BassNotesMaster.WpfViews.Metronome
{
    public interface ITimeSignatureView
    {
        void OnUseCustomTimeSignatureChange(bool value);
        void OnUseTimeSignature(bool useTimeSignature);
    }
}