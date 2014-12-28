using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BassTrainer.Core.Components.Interval;
using BassTrainer.Core.Components.Interval.Data;

namespace BassTrainer.UI.WPF.Intervals
{
    public class WpfIntervalEventHandler : IntervalEventHandler
    {
        private readonly DataGrid _dataGrid;

        public WpfIntervalEventHandler(DataGrid dataGrid)
        {
            _dataGrid = dataGrid;
        }

        public void PlaySelectedIntervalClick(object sender, RoutedEventArgs e)
        {
            var intervalRow = FindRowOfEvent(sender);
            RaisePlayEvent(intervalRow);
        }

        public void ShowSelectedIntervalClick(object sender, RoutedEventArgs e)
        {
            var intervalRow = FindRowOfEvent(sender);
            RaiseShowEvent(intervalRow);
        }

        public void ExcerciseButtonClick(object sender, RoutedEventArgs e)
        {
            var clickedButton = (Button) sender;
            var row = IntervalData.ByName(clickedButton.Content.ToString());
            RaiseExcerciseEvent(row);
        }

        public void ShowSelectedIntervalsButtonClick(object sender, RoutedEventArgs e)
        {
            foreach (var item in _dataGrid.SelectedItems)
            {
                RaiseShowEvent((IntervalRow) item);
            }
        }

        public IntervalRow FindRowOfEvent(object sender)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
            {
                var gridRow = vis as DataGridRow;
                if (gridRow == null) continue;
                var row = gridRow;
                var intervalRow = row.Item as IntervalRow;
                return intervalRow;
            }
            return null;
        }
    }
}