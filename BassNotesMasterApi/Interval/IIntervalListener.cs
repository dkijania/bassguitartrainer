namespace BassNotesMasterApi.Interval
{
    public interface IIntervalListener
    {
        void IntervalShowEvent(IntervalRow row);
        void IntervalPlayEvent(IntervalRow row);
    }
}