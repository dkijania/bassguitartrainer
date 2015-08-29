using BassTrainer.Core.Const.Intervals;

namespace BassTrainer.Core.Components.Interval
{
    public interface IExcerciseIntervalListener
    {
        void IntervalExcerciseEvent(IntervalRow row);
    }
}