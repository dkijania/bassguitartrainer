using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DrumMachine.Audio;
using DrumMachine.Engine.Pattern;

namespace DrumMachine.UI.WPF
{
    public class WpfDrumMachinePatternTilesManipulator : IPatternTilesManipulator
    {
        public IPatternHighlighter PatternHighlighter { get { return new PatternHighlighter(_patternGrid); } }

        private readonly Grid _patternGrid;
        private readonly GridLength _delimeterColumnWidth = new GridLength(35);
        private int _count;
        private int _span;

        public WpfDrumMachinePatternTilesManipulator(Grid patternGrid)
        {
            _patternGrid = patternGrid;
            FillFirstGridColumnWithSamplesNames(); 
        }

        public void SetColumnsCount(int count, int span)
        {
            _count = count;
            _span = span;
            var columnDefintions = _patternGrid.ColumnDefinitions;
            columnDefintions.Clear();
            _patternGrid.RowDefinitions.Clear();
            _patternGrid.Children.Clear();
            columnDefintions.Add(new ColumnDefinition {Width = GridLength.Auto});
            FillFirstGridColumnWithSamplesNames();
            AddColumns(count, span);
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
            FillGridWithCheckboxes(count, span);
            var columnDefintions = _patternGrid.ColumnDefinitions;
            columnDefintions.Add(new ColumnDefinition() {Width = _delimeterColumnWidth});
        }

        private void FillGridWithCheckboxes(int count, int scaleFactor)
        {
            foreach (var i in Enumerable.Range(0, count))
            {
                var columnDefinition = new ColumnDefinition {Width = new GridLength(10*scaleFactor)};
                _patternGrid.ColumnDefinitions.Add(columnDefinition);
                AddCheckboxesToColumn();
            }
        }

        private void AddCheckboxesToColumn()
        {
            var count = DefaultAudioSampler.Instance.Names.Count();

            foreach (var i in Enumerable.Range(0, count))
            {
                AddElementToGrid(i, _patternGrid);
            }
        }

        private void AddElementToGrid(int row, Grid grid)
        {
            var column = grid.ColumnDefinitions.Count - 1;
            var element = new SelectableBorder(row, column);
            element.SetValue(Grid.RowProperty, row);
            element.SetValue(Grid.ColumnProperty, column);
            grid.Children.Add(element);
        }

        public void AddBar()
        {
            AddColumns(_count, _span);
        }

        public void RemoveBar()
        {
            var lastColumnIndex = _patternGrid.ColumnDefinitions.Count - 1;
            bool ignoreFirst = true;
            for (var i = lastColumnIndex; i >= 0; i--)
            {
                if (_patternGrid.ColumnDefinitions[i].Width.Equals(_delimeterColumnWidth))
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

        public void FillDrumPattern(DrumPattern drumPattern)
        {
            foreach (var row in Enumerable.Range(0, _patternGrid.RowDefinitions.Count))
            {
                var columnCount = _patternGrid.ColumnDefinitions.Count();
                int offset = 1;
                for (var column = offset; column < columnCount; column++)
                {
                    var element =
                        _patternGrid.Children.Cast<UIElement>()
                            .FirstOrDefault(e => Grid.GetColumn(e) == column && Grid.GetRow(e) == row);
                    var border = element as SelectableBorder;
                    if (border == null)
                    {
                        offset++;
                        continue;
                    }

                    drumPattern[row, (column - offset) * (drumPattern.Interval)] = border.IsSelected
                        ? DrumPattern.Hit
                        : DrumPattern.NoHit;
                }
            }
        }

        
    }
}