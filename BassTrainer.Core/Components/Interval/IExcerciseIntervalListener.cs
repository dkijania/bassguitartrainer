using BassTrainer.Core.Components.Interval.Data;

namespace BassTrainer.Core.Components.Interval
{
    public interface IExcerciseIntervalListener
    {
        void IntervalExcerciseEvent(IntervalRow row);
    }
}