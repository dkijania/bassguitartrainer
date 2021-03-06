using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BassNotesMasterApi.Components.Interval;
using BassNotesMasterApi.Components.Interval.Data;
using BassNotesMasterApi.Excercise;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace BassNotesMaster.Intervals
{
    public class WpfIntervalEventHandler : IntervalEventHandler
    {
        private readonly DataGrid _dataGrid;
        private readonly MetroWindow _metroWindow;

        public WpfIntervalEventHandler(DataGrid dataGrid,MetroWindow metroWindow)
        {
            _dataGrid = dataGrid;
            _metroWindow = metroWindow;
        }

        public void PlaySelectedIntervalClick(object sender, RoutedEventArgs e)
        {
            var intervalRow = FindRowOfEvent(sender);
            RaisePlayEvent(intervalRow);
        }

        public void ShowSelectedIntervalClick(object sender, RoutedEventArgs e)
        {
            var intervalRow = FindRowOfEvent(sender);
            try
            {
                RaiseShowEvent(intervalRow);
            }
            catch (ExcerciseException ex)
            {
                _metroWindow.ShowMessageAsync(ex.Message,"Excercise Exception");
            }
        }

        public void ExcerciseButtonClick(object sender, RoutedEventArgs e)
        {
            var clickedButton = (Button) sender;
            var row = IntervalData.ByName(clickedButton.Content.ToString());
            RaiseExcerciseEvent(row);
        }

        public void ShowSelectedIntervalsButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in _dataGrid.SelectedItems)
                {
                    RaiseShowEvent((IntervalRow)item);
                }
            }
            catch (ExcerciseException exception)
            {
                _metroWindow.ShowMessageAsync(exception.Message, "Interval manager error");
        
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