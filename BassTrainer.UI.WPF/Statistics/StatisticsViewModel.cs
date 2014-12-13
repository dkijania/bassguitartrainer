using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BassTrainer.Core.Components.Statistics;

namespace BassTrainer.UI.WPF.Statistics
{
    public class StatisticsViewModel
    {
        private StatisticsComponent _statisticsComponent;

        public StatisticsViewModel(StatisticsComponent statisticsComponent)
        {
            _statisticsComponent = statisticsComponent;
        }
    }
}
