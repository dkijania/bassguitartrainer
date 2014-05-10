using System.Collections.ObjectModel;

namespace BassNotesMasterApi.Statistics
{
    public interface IStatisticsGuiManager
    {
        void FillWithData(ObservableCollection<StatisticRow> rows);
        void EnableGraphButtons();
        void DisableGraphButtons();
    }
}