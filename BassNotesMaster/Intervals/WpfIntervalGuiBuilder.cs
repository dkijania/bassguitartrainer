using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using BassNotesMasterApi.Interval;
using BassNotesMasterApi.Interval.Data;

namespace BassNotesMaster.Intervals
{
    public class WpfIntervalGuiBuilder : IntervalGuiBuilder
    {
        private readonly Panel _info;
        private readonly Panel _excercise;
        private readonly DataGrid _dataGrid;
        private Brush _standardButtonBrush;

        public WpfIntervalGuiBuilder(Panel info, Panel excercise, IntervalEventHandler intervalEventHandler)
            : base(intervalEventHandler)
        {
            _info = info;
            _excercise = excercise;
            _dataGrid = info.Children.OfType<DataGrid>().FirstOrDefault();
            RegisterEventForShowSelectedIntervalsButton();
        }

        private WpfIntervalEventHandler WpfEventHandler
        {
            get { return IntervalEventHandler as WpfIntervalEventHandler; }
        }

        private void RegisterEventForShowSelectedIntervalsButton()
        {
            var showSelectedIntervalsButton = _info.Children.OfType<Button>().FirstOrDefault();
            if (showSelectedIntervalsButton != null)
                showSelectedIntervalsButton.Click += WpfEventHandler.ShowSelectedIntervalsButtonClick;
        }

        public override void PrepareInfoGuiElements(IntervalData data)
        {
            IntervalEventHandler.IntervalData = data;

            FillDataGrid(_dataGrid, data);
            AddShowColumn(_dataGrid);
            AddPlayColumn(_dataGrid);
        }

        public override void PrepareExcerciseGuiElements(IntervalData data)
        {
            FillWithData(_excercise, data);
        }

        private void FillWithData(Panel wrapPanel, IntervalData data)
        {
            foreach (var row in data)
            {
                wrapPanel.Children.Add(CreateButtonWithContent(row.IntervalName));
            }
        }

        public override void EnableAllIntervalsButtons()
        {
            SetForAllButtons(value: true);
        }

        public override void EnableIntervalsButtons(string[] names)
        {
            foreach (var childAsButton in _excercise.Children.Cast<Button>().Where(childAsButton => names.Any(
                x =>
                string.Equals(childAsButton.Content.ToString(), x, StringComparison.InvariantCultureIgnoreCase))))
            {
                childAsButton.IsEnabled = true;
            }
        }

        public override void DisableAllIntervalsButtons()
        {
            SetForAllButtons(value: false);
        }

        private void SetForAllButtons(bool value)
        {
            foreach (var childAsButton in _excercise.Children.Cast<Button>())
            {
                childAsButton.IsEnabled = value;
            }
        }

        public override void EnableIntervalsButtonsExclusive(string[] names)
        {
            DisableAllIntervalsButtons();
            EnableIntervalsButtons(names);
        }

        private Button CreateButtonWithContent(string content)
        {
            var button = new Button
                             {
                                 Content = content,
                                 Height = 40,
                                 FontSize = 15
                             };
            button.Click += WpfEventHandler.ExcerciseButtonClick;
            _standardButtonBrush = button.Foreground;
            return button;
        }

        private void AddShowColumn(DataGrid dataGrid)
        {
            var templateColumn = CreateColumnTemplate("Show", WpfEventHandler.ShowSelectedIntervalClick);
            templateColumn.Width = DataGridLength.Auto;
            dataGrid.Columns.Add(templateColumn);
        }

        private void AddPlayColumn(DataGrid dataGrid)
        {
            var templateColumn = CreateColumnTemplate("Play", WpfEventHandler.PlaySelectedIntervalClick);
            templateColumn.Width = DataGridLength.Auto;
            dataGrid.Columns.Add(templateColumn);
        }

        private DataGridTemplateColumn CreateColumnTemplate(string name, RoutedEventHandler showSelectedIntervalClick)
        {
            var columnTemplate = new DataGridTemplateColumn();
            var dataTemplate = new DataTemplate();
            var buttonTemplate = new FrameworkElementFactory(typeof (Button));
            buttonTemplate.SetValue(ContentControl.ContentProperty, name);
            buttonTemplate.AddHandler(
                ButtonBase.ClickEvent,
                new RoutedEventHandler(showSelectedIntervalClick)
                );
            dataTemplate.VisualTree = buttonTemplate;
            columnTemplate.CellTemplate = dataTemplate;
            return columnTemplate;
        }

        private void FillDataGrid(DataGrid dataGrid, IntervalData data)
        {
            dataGrid.IsReadOnly = true;

            foreach (var row in data)
            {
                dataGrid.Items.Add(row);
            }
        }

        protected override void HideInfoPanel()
        {
            _info.Visibility = Visibility.Collapsed;
        }

        protected override void ShowInfoPanel()
        {
            _info.Visibility = Visibility.Visible;
        }

        protected override void HideExcercisePanel()
        {
            _excercise.Visibility = Visibility.Collapsed;
        }

        protected override void ShowExcercisePanel()
        {
            _excercise.Visibility = Visibility.Visible;
        }

        public override void SetColorForButtonName(IntervalRow row, bool result)
        {
            var button = GetButtonFor(row);
            button.Foreground = result ? new SolidColorBrush(Colors.ForestGreen) : new SolidColorBrush(Colors.Firebrick);
        }

        private Button GetButtonFor(IntervalRow row)
        {
            return _excercise.Children.OfType<Button>().FirstOrDefault(x => x.Content.Equals(row.IntervalName));
        }

        public override void ResetColorForButtonName(IntervalRow row)
        {
            var button = GetButtonFor(row);
            button.Foreground= _standardButtonBrush;
        }
    }
}