using BassTrainer.Core.Components.Interval.Data;

namespace BassTrainer.Core.Components.Interval
{
    public interface IIntervalListener
    {
        void IntervalShowEvent(IntervalRow row);
        void IntervalPlayEvent(IntervalRow row);
    }
}