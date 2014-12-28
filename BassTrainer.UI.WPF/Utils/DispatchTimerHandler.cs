using System;
using System.Windows.Threading;
using BassTrainer.Core.Excercise;

namespace BassTrainer.UI.WPF.Utils
{
    public class DispatchTimerHandler : IDispatchTimerHandler
    {
        public void InvokeActionWithDelay(EventHandler<object> action, double second)
        {
           var timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(second)};
            timer.Start();
            timer.Tick += (sender, args) => timer.Stop();
            timer.Tick += (sender, args) => action.Invoke(null,null);
        }
    }
}
