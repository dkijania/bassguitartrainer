using System.Collections.ObjectModel;

namespace BassTrainer.Core.Components.Statistics
{
    public interface IStatisticsGuiManager
    {
        void FillWithData(ObservableCollection<StatisticRow> rows);
        void EnableGraphButtons();
        void DisableGraphButtons();
    }
}