namespace BassNotesMasterApi.Excercise
{
    public class Options
    {
        public const string ShowTips = "Show tips";
        public bool ShouldShowTips;

        public const string PlayNote = "Play note";
        public bool ShouldplayNote;

        public bool AlwaysStartFromLowestNote;
        public const string AlwaysStartFromLowestNoteParamName = "Always start from lowest";

        public const string RequireCorrectOctave = "Require Correct Octave";
        public bool ShouldRequireCorrectOctave;

        public const string HideNoteLabel = "Hide note label for pressed position";
        public bool ShouldHideNoteLabel;


        public const string Scale = "Choose Scale";
        public string ScaleType;

    }
}