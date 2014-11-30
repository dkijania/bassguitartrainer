using BassNotesMasterApi.Components.Interval.Data;

namespace BassNotesMasterApi.Components.Interval
{
    public interface IExcerciseIntervalListener
    {
        void IntervalExcerciseEvent(IntervalRow row);
    }
}