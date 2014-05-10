using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using BassNotesMasterApi;
using BassNotesMasterApi.Statistics;
using BassNotesMasterApi.Utils.ResultSerializer;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace BassNotesMaster.WpfControls
{
    /// <summary>
    /// Interaction logic for StatisticControl.xaml
    /// </summary>
    public partial class StatisticControl
    {
        public StatisticControl()
        {
            InitializeComponent();
            ShowDataGridPanel();
        }

        public MetroWindow Window { get; set; }

        private void ShowLast_Click(object sender, RoutedEventArgs e)
        {
            if (RaiseExceptionIfNoRowSelected())
                return;

            var data = ReadData();
            if (RaiseExceptionIfDataEmpty(data))
                return;

            var charts = StatsTable.SelectedItems;

            var chartToData = charts.Cast<StatisticRow>().Select(statisticRow =>
                                                                 data.GetData().First(
                                                                     x =>
                                                                     x.GetName().Equals(statisticRow.Excercise,
                                                                                        StringComparison.
                                                                                            InvariantCultureIgnoreCase)));

            CreatePieChartForLastResult(chartToData.ToArray());
            ShowGraphPanel();
            LineChart.Visibility = Visibility.Hidden;
        }


        private SerializerRoot ReadData()
        {
            return ManagersLocator.Instance.ResultSerializer.Read();
        }

        private void ShowHistory_Click(object sender, RoutedEventArgs e)
        {
            if (RaiseExceptionIfNoRowSelected())
            {
                return;
            }
            if (RaiseExceptionIfMoreThanOneRowSelected())
            {
                return;
            }
            var data = ReadData();
            if (RaiseExceptionIfDataEmpty(data))
                return;

            var selectedItem = (StatisticRow) StatsTable.SelectedItem;

            var chartToData = data.GetData().First(x => x.GetName().Equals(selectedItem.Excercise));
            CreateLineChartForHistory(chartToData);
            ShowGraphPanel();
            PieChart.Visibility = Visibility.Hidden;
        }

        private bool RaiseExceptionIfNoRowSelected()
        {
            if (StatsTable.SelectedIndex == -1)
            {
                Window.ShowMessageAsync("No excercise selected", "Statistic error");
                return true;
            }
            return false;
        }

        private bool RaiseExceptionIfMoreThanOneRowSelected()
        {
            if (StatsTable.SelectedItems.Count > 1)
            {
                Window.ShowMessageAsync("For history chart only one row should be selected", "Statistic error");
                return true;
            }
            return false;
        }

        private bool RaiseExceptionIfDataEmpty(SerializerRoot root)
        {
            var rows = StatsTable.SelectedItems;
            if (rows.Cast<StatisticRow>().Any(root.HasDataFor))
            {
                return false;
            }
            Window.ShowMessageAsync("There is no record for this excercise", "Chart data error");
            return true;
        }

        private void ShowGraphPanel()
        {
            StatsTable.Visibility = Visibility.Hidden;
            ShowHistory.Visibility = Visibility.Hidden;
            ShowLast.Visibility = Visibility.Hidden;

            Back.Visibility = Visibility.Visible;
            PieChart.Visibility = Visibility.Visible;
            LineChart.Visibility = Visibility.Visible;
            ChartTitle.Visibility = Visibility.Visible;
        }

        private void ShowDataGridPanel()
        {
            StatsTable.Visibility = Visibility.Visible;
            ShowHistory.Visibility = Visibility.Visible;
            ShowLast.Visibility = Visibility.Visible;

            Back.Visibility = Visibility.Hidden;
            PieChart.Visibility = Visibility.Hidden;
            LineChart.Visibility = Visibility.Hidden;
            ChartTitle.Visibility = Visibility.Hidden;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ShowDataGridPanel();
        }

        private void CreatePieChartForLastResult(params SerializerExcerciseData[] data)
        {
            PieChart.Brushes = new List<Brush>
                                   {
                                       new SolidColorBrush(Colors.LawnGreen),
                                       new SolidColorBrush(Colors.Red),
                                       new SolidColorBrush(Colors.Orange)
                                   };

            PieChart.TitleMemberPath = "Name";
            PieChart.ValueMemberPath = "Count";
            var resultRow = new SerializerStatisticRow();
            foreach (var row in data.
                Select(serializerExcerciseData => serializerExcerciseData.GetData().Last()))
            {
                resultRow.AddAll(row);
            }

            PieChart.DataSource = ConvertToPieData(resultRow);
            if (data.Count() == 1)
                ChartTitle.Content = data[0].GetName();
        }

        private void CreateLineChartForHistory(SerializerExcerciseData data)
        {
            var lines = (from statisticRow in data.GetData()
                         let rate =(
                             statisticRow.Passed/
                             (double) (statisticRow.Passed + statisticRow.Failed + statisticRow.Skipped))
                         select new LineData {TestNo = statisticRow.TestNo, SuccessRate = rate}).ToList();
            Area.Title = data.GetName();
            LineChart.ValueFormatString = "0 %";
            LineChart.DataSource = lines.ToList();
        }

        private IEnumerable<PieData> ConvertToPieData(SerializerStatisticRow row)
        {
            var outputList = new List<PieData>
                                 {
                                     new PieData {Count = row.Passed, Name = "Correct"},
                                     new PieData {Count = row.Failed, Name = "Failed"},
                                     new PieData {Count = row.Skipped, Name = "Skipped"}
                                 };
            return outputList;
        }
    }

    public class PieData
    {
        public string Name { get; set; }
        public double Count { get; set; }
    }

    public class LineData
    {
        public long TestNo { get; set; }
        public double SuccessRate { get; set; }
    }
}