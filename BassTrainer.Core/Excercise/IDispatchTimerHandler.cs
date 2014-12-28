using System;

namespace BassTrainer.Core.Excercise
{
    public interface IDispatchTimerHandler
    {
        void InvokeActionWithDelay(EventHandler<object> action, double second);
    }
}