using BassNotesMasterApi.Components.Interval.Data;

namespace BassNotesMasterApi.Components.Interval
{
    public interface IIntervalListener
    {
        void IntervalShowEvent(IntervalRow row);
        void IntervalPlayEvent(IntervalRow row);
    }
}