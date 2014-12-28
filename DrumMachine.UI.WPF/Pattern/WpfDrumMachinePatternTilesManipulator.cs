using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DrumMachine.Engine.Pattern;
using DrumMachine.Pattern;
using DrumMachine.Pattern.Converters;
using DrumMachine.Pattern.DrumMachineTile;
using DrumMachine.Resources.DrumMachine.Audio;
using DrumMachine.UI.WPF.Pattern.Converters;

namespace DrumMachine.UI.WPF.Pattern
{
    public class WpfDrumMachinePatternTilesManipulator : IPatternTilesManipulator
    {
        public const int MaximumSpanValue = 64;
        public const int MinimumSpanValue = 4;
        private const int Span = MaximumSpanValue;
        
        //private readonly uiIuiDrumPatternConverter _uiIuiDrumPatternConverter = new uiIuiDrumPatternConverter();
        private readonly Grid _patternGrid;
        private readonly GridLength _delimeterColumnWidth = new GridLength(35);
        private readonly GridLength _tileColumnWidth = new GridLength(2);
        private int _count;
        private int _note;
        
        public event DrumMachineTile.OnSelect OnSelectEvent;
        public event DrumMachineTile.IgnoreMouseClick IgnoreMouseEvent;

        public int GetMaximumSpanValue()
        {
            return MaximumSpanValue;
        }

        public int GetMinimumSpanValue()
        {
            return MinimumSpanValue;
        }

        public IPatternHighlighter PatternHighlighter
        {
            get { return new PatternHighlighter(_patternGrid); }
        }

        public void Clear()
        {
            var tiles = _patternGrid.Children.OfType<DrumMachineTile>();
            foreach (var drumMachineTile in tiles)
            {
                drumMachineTile.Unselect();
            }
        }

        public void SplitCell(int row, int column)
        {
            var element = GetCellFor(row, column);
            var span = GetElementColumnSpan(element);
            if (span == MinimumSpanValue) return;
            ChangeSpanForElement(element, 0.5);
            AddCellNextTo(element, row, column);
        }

        public void JoinCell(int row, int column)
        {
            var element = GetCellFor(row, column);
            var span = GetElementColumnSpan(element);
            if (IsALastColumn(column) || NextCellHasDifferentSize(element, row, column) ||
                span == MaximumSpanValue)
                return;
            RemoveCellNextTo(element, row, column);
            ChangeSpanForElement(element, 2);
        }

        private bool NextCellHasDifferentSize(DependencyObject uiElement, int row, int column)
        {
            var span = GetElementColumnSpan(uiElement);
            var nextElement = GetCellFor(row, column + span);
            return nextElement == null || GetElementColumnSpan(nextElement) != span;
        }

        private bool IsALastColumn(int column)
        {
            return column == _patternGrid.ColumnDefinitions.Count - 1;
        }

        private UIElement GetCellFor(int row, int column)
        {
            return _patternGrid.Children.Cast<UIElement>()
                .FirstOrDefault(e => Grid.GetColumn(e) == column && Grid.GetRow(e) == row);
        }

        private void RemoveCellNextTo(DependencyObject element, int row, int column)
        {
            var span = (int) element.GetValue(Grid.ColumnSpanProperty);
            var cellToDelete = GetCellFor(row, column + span);
            _patternGrid.Children.Remove(cellToDelete);
        }

        private int GetElementColumnSpan(DependencyObject element)
        {
            return (int) element.GetValue(Grid.ColumnSpanProperty);
        }

        private void AddCellNextTo(DependencyObject element, int row, int column)
        {
            var span = (int) element.GetValue(Grid.ColumnSpanProperty);
            AddElementToGrid(row, span, column + span, _patternGrid);
        }

        private void ChangeSpanForElement(DependencyObject element, double factor)
        {
            var columnSpan = (int) element.GetValue(Grid.ColumnSpanProperty);
            columnSpan = (int) (columnSpan*factor);
            element.SetValue(Grid.ColumnSpanProperty, columnSpan);
        }

        public WpfDrumMachinePatternTilesManipulator(Grid patternGrid)
        {
            _patternGrid = patternGrid;
            FillFirstGridColumnWithSamplesNames();
            ResetBarsCount();
        }

        public void SetColumnsCount(int count, int note)
        {
            _count = count;
            _note = note;
            ResetUi();
            AddBar();
        }

        public void ResetUi()
        {
            var columnDefintions = _patternGrid.ColumnDefinitions;
            columnDefintions.Clear();
            _patternGrid.RowDefinitions.Clear();
            _patternGrid.Children.Clear();
            columnDefintions.Add(new ColumnDefinition {Width = GridLength.Auto});
            FillFirstGridColumnWithSamplesNames();
            ResetBarsCount();
        }

        public void SetColumnsSpan(int note)
        {
            var newSpan = MaximumSpanValue/note;
            for (var i = 0; i < _patternGrid.RowDefinitions.Count; ++i)
            {
                for (var j = 1; j < _patternGrid.ColumnDefinitions.Count;)
                {
                    var cell = GetCellFor(i, j);
                    if (IsBlankColumn(_patternGrid.ColumnDefinitions[j]) || cell == null)
                    {
                        ++j;
                        continue;
                    }
                    cell.SetValue(Grid.ColumnSpanProperty, newSpan);
                    j += newSpan;
                }
            }
        }

        private bool IsBlankColumn(ColumnDefinition columnDefinition)
        {
            return columnDefinition.Width.Equals(_delimeterColumnWidth);
        }

        private void FillFirstGridColumnWithSamplesNames()
        {
            foreach (var name in DefaultAudioSampler.Instance.Names)
            {
                var rowDefinition = new RowDefinition {Height = GridLength.Auto};
                _patternGrid.RowDefinitions.Add(rowDefinition);
                AddLabelToGrid(name, _patternGrid);
            }
        }

        private void AddLabelToGrid(string label, Grid grid)
        {
            var labelElement = new Label {Content = label,};
            labelElement.SetValue(Grid.ColumnProperty, 0);
            labelElement.SetValue(Grid.RowProperty, grid.RowDefinitions.Count - 1);
            grid.Children.Add(labelElement);
        }

        public void AddColumns(int count, int span)
        {
            var scaleFactor = span/_note;
            FillGridWithCheckboxes(count, scaleFactor);
        }

        private void FillGridWithCheckboxes(int count, int scaleFactor)
        {
            foreach (var i in Enumerable.Range(0, count))
            {
                for (int j = 0; j < scaleFactor; ++j)
                {
                    AddTileColumn();
                }
                AddCheckboxesToColumn(scaleFactor);
            }
        }

        private void AddTileColumn()
        {
            var columnDefinition = new ColumnDefinition {Width = _tileColumnWidth};
            _patternGrid.ColumnDefinitions.Add(columnDefinition);
        }

        private void AddCheckboxesToColumn(int scaleFactor)
        {
            var count = DefaultAudioSampler.Instance.Names.Count();

            foreach (var i in Enumerable.Range(0, count))
            {
                AddElementToGrid(i, scaleFactor, _patternGrid);
            }
        }

        private void AddElementToGrid(int row, int span, Grid grid)
        {
            var column = grid.ColumnDefinitions.Count - (span);
            AddElementToGrid(row, span, column, grid);
        }

        private void AddElementToGrid(int row, int span, int column, Panel grid, bool isSelected = false)
        {
            var element = new SelectableBorder(row, column, OnSelectEvent, IgnoreMouseEvent,isSelected);
            element.SetValue(Grid.RowProperty, row);
            element.SetValue(Grid.ColumnProperty, column);
            element.SetValue(Grid.ColumnSpanProperty, span);
            grid.Children.Add(element);
        }

        /**
         * There is no easy way to add separator between two columns. 
         * Therefore a separator is an another column.
         */
        public void AddBar()
        {
            AddColumns(_count, Span);
            AddBlankColumn();
        }

        private void AddBlankColumn()
        {
            _patternGrid.ColumnDefinitions.Add(new ColumnDefinition {Width = _delimeterColumnWidth});
            BarsCount++;
        }

        public void ResetBarsCount()
        {
            BarsCount = 0;
        }

        public int BarsCount { get; private set; }

        public void RemoveBar()
        {
            if (BarsCount == 1) return;
            RemoveBaFromPattern();
            BarsCount--;
        }

        private void RemoveBaFromPattern()
        {
            var lastColumnIndex = _patternGrid.ColumnDefinitions.Count - 1;
            bool ignoreFirst = true;
            for (var i = lastColumnIndex; i >= 0; i--)
            {
                if (IsBlankColumn(_patternGrid.ColumnDefinitions[i]))
                {
                    if (ignoreFirst)
                    {
                        ignoreFirst = false;
                    }
                    else
                    {
                        break;
                    }
                }
                DeleteChildrenAtColumn(i);
                _patternGrid.ColumnDefinitions.RemoveAt(i);
            }
        }

        public void DeleteChildrenAtColumn(int column)
        {
            var collection = _patternGrid.Children.OfType<UIElement>().ToArray();
            for (var i = collection.Length - 1; i >= 0; --i)
            {
                if (Grid.GetColumn(collection[i]) == column)
                {
                    _patternGrid.Children.RemoveAt(i);
                }
            }
        }
        
        public void FillDrumPattern(DrumPattern drumPattern, IUIDrumPatternConverter uiIuiDrumPatternConverter)
        {
            ((UiDrumPatternConverter)uiIuiDrumPatternConverter).ColumnsCount = _patternGrid.ColumnDefinitions.Count;
            ((UiDrumPatternConverter)uiIuiDrumPatternConverter).RowsCount = _patternGrid.RowDefinitions.Count;
            ((UiDrumPatternConverter)uiIuiDrumPatternConverter).ElementCollection = _patternGrid.Children;
            uiIuiDrumPatternConverter.FillDrumPattern(drumPattern);
        }

        public void ImportToUi(DrumPattern drumPattern, IUIDrumPatternConverter uiIuiDrumPatternConverter)
        {
            ResetUi();
            var splittedPartsCollection = uiIuiDrumPatternConverter.FillPatternGrid(drumPattern);
            foreach (var part in splittedPartsCollection)
            {
                if (part.Bar > BarsCount)
                {
                    AddBlankColumn();
                }
                AddCell(part.RowId, 1  + part.Bar + part.StartIndex, part.Length , part.IsSelected);
            }
            BarsCount = drumPattern.Settings.Bars;
        }

        public void AddCell(int row, int column, int columnSpan, bool isSelected = false)
        {
            AddRequiredAmountOfColumns(column, columnSpan);
            AddElementToGrid(row, columnSpan, column, _patternGrid,isSelected);
        }

        private void AddRequiredAmountOfColumns(int column, int columnSpan)
        {
            for (int i = 0; i < columnSpan; i++)
            {
                AddTileColumn();
            }
            
        }
        
    }
}