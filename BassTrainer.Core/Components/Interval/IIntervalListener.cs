using BassTrainer.Core.Const.Intervals;

namespace BassTrainer.Core.Components.Interval
{
    public interface IIntervalListener
    {
        void IntervalShowEvent(IntervalRow row);
        void IntervalPlayEvent(IntervalRow row);
    }
}