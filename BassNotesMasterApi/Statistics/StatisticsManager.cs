using System;
using System.Collections.ObjectModel;
using System.Linq;
using BassNotesMasterApi.Excercise;
using BassNotesMasterApi.Fretboard;
using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Statistics
{
    public class StatisticsManager : Manager
    {
        private readonly ObservableCollection<StatisticRow> _rows = new ObservableCollection<StatisticRow>();
        private readonly IStatisticsGuiManager _guiManager;

        public StatisticsManager(IStatisticsGuiManager guiManager, ExcercisesDictionary excercisesDictionary)
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

        public override ManagerMode Mode { get; set; }

        public override void RemoveAllSubscribers()
        {
        }

        public override void OnModeChanged(ManagerMode mode)
        {
            if (mode == ManagerMode.Excercise)
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