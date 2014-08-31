namespace DrumMachine.TimeSignature
{
    public class TimeSignature
    {
        public bool IsCustomTimeSignatureEnabled;
        public bool IsTimeSignatureEnabled;

        public TimeSignatureOptions StandardSignature = new TimeSignatureOptions();
        public TimeSignatureOptions CustomSignature = new TimeSignatureOptions();

        public TimeSignatureOptions SelectedTimeSignature
        {
            get
            {
                if (!IsTimeSignatureEnabled)
                {
                    return TimeSignatureOptions.Unison;
                }
                return IsCustomTimeSignatureEnabled ? CustomSignature : StandardSignature;
            }
        }
    }
}