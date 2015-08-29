namespace BassTrainer.Core.Const.Intervals
{
    public class IntervalRow
    {
        public int Semitone { get; set; }
        public string IntervalName { get; set; }
        public string DegreeName { get; set; }

        public IntervalRow(int semitone, string degreeName,string intervalName)
        {
            Semitone = semitone;
            IntervalName = intervalName;
            DegreeName = degreeName;
        }
    }
}