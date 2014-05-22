using BassNotesMasterApi.Interval.Data;

namespace BassNotesMasterApi.Interval
{
    public interface IExcerciseIntervalListener
    {
        void IntervalExcerciseEvent(IntervalRow row);
    }
}