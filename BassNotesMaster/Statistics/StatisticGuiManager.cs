using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using BassNotesMaster.WpfControls;
using BassNotesMasterApi.Statistics;

namespace BassNotesMaster.Statistics
{
    class StatisticGuiManager : IStatisticsGuiManager
    {
        private readonly DataGrid _statisticTable;
        private readonly ButtonBase _lastTestButton;
        private readonly ButtonBase _historyButton;

        public StatisticGuiManager(StatisticControl statisticControl)
        {
            _statisticTable = statisticControl.StatsTable;
            _lastTestButton = statisticControl.ShowLast;
            _historyButton = statisticControl.ShowHistory;
        }

        public void FillWithData(ObservableCollection<StatisticRow> rows)
        {
            _statisticTable.ItemsSource = rows;
        }

        public void EnableGraphButtons()
        {
           SetEnabledToAllbuttons(isEnabled:true);
        }

        public void DisableGraphButtons()
        {
            SetEnabledToAllbuttons(isEnabled: false);
        }

        private void SetEnabledToAllbuttons(bool isEnabled)
        {
            _lastTestButton.IsEnabled = isEnabled;
            _historyButton.IsEnabled = isEnabled;
        }
    }
}
