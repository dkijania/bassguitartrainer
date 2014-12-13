using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BassTrainer.Core.Components.Interval;

namespace BassTrainer.UI.WPF.Intervals
{
    public class IntervalViewModel
    {
        private readonly IntervalComponent _intervalComponent;

        public IntervalViewModel(IntervalComponent intervalComponent)
        {
            _intervalComponent = intervalComponent;
        }
    }
}
