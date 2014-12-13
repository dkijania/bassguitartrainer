using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using BassTrainer.Core.Components;
using BassTrainer.Core.Components.Statistics;
using BassTrainer.Core.Utils.ResultSerializer;

namespace BassTrainer.UI.WPF.WpfControls
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
        
        private void ShowLast_Click(object sender, RoutedEventArgs e)
        {
            RaiseExceptionIfNoRowSelected();
            var data = ReadData();
            RaiseExceptionIfDataEmpty(data);
       

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
            return ComponentsLocator.Instance.ResultSerializer.Read();
        }

        private void ShowHistory_Click(object sender, RoutedEventArgs e)
        {
            RaiseExceptionIfNoRowSelected();
            RaiseExceptionIfMoreThanOneRowSelected();

            var data = ReadData();
            RaiseExceptionIfDataEmpty(data);
        
            var selectedItem = (StatisticRow) StatsTable.SelectedItem;

            var chartToData = data.GetData().First(x => x.GetName().Equals(selectedItem.Excercise));
            CreateLineChartForHistory(chartToData);
            ShowGraphPanel();
            PieChart.Visibility = Visibility.Hidden;
        }

        private void RaiseExceptionIfNoRowSelected()
        {
            if (StatsTable.SelectedIndex == -1)
            {
                throw new Exception("No excercise selected");
            }
        }

        private void RaiseExceptionIfMoreThanOneRowSelected()
        {
            if (StatsTable.SelectedItems.Count > 1)
            throw  new Exception("For history chart only one row should be selected");
            
        }

        private void RaiseExceptionIfDataEmpty(SerializerRoot root)
        {
            var rows = StatsTable.SelectedItems;
            if (rows.Cast<StatisticRow>().Any(root.HasDataFor))
            {
                return;
            }
            throw new Exception("There is no record for this excercise");
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