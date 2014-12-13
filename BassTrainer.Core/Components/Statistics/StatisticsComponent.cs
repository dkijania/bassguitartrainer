using System;
using System.Collections.ObjectModel;
using System.Linq;
using BassTrainer.Core.Excercise;
using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Components.Statistics
{
    public class StatisticsComponent : Component
    {
        private readonly ObservableCollection<StatisticRow> _rows = new ObservableCollection<StatisticRow>();
        private readonly IStatisticsGuiManager _guiManager;

        public StatisticsComponent(IStatisticsGuiManager guiManager, ExcercisesDictionary excercisesDictionary)
        {
            _guiManager = guiManager;
            InitData(excercisesDictionary);
            _guiManager.FillWithData(_rows);
        }

        private void InitData(ExcercisesDictionary excercisesDictionary)
        {
            foreach (var excerciseName in excercisesDictionary.Keys)
            {
                _rows.Add(new StatisticRow(excerciseName.ToString()));
            }
        }

        public StatisticRow GetStatisticForExcercise(string name)
        {
            return _rows.First(x => string.Equals(name, x.Excercise, StringComparison.InvariantCultureIgnoreCase));
        }

        public override void RemoveAllSubscribers()
        {
        }

        public override void OnModeChanged(ComponentMode mode)
        {
            if (mode == ComponentMode.Excercise)
            {
                _guiManager.DisableGraphButtons();
            }
            else
            {
                _guiManager.EnableGraphButtons();
            }
        }
    }
}