using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using DrumMachine.Engine.Pattern;

namespace DrumMachine.UI.WPF.Pattern
{
    public class PatternHighlighter : IPatternHighlighter
    {
        private readonly Grid _patternGrid;
        private int _currentlyHighlighedColumn;
        private int _lastHighlighedColumn;
        private int _indexToTakeFromPattern;

        public PatternHighlighter(Grid patternGrid)
        {
            _patternGrid = patternGrid;
        }

        public void HighlightColumnOnBeat(DrumPattern drumPattern)
        {
            CalculateColumnToSelect(drumPattern);
            ChangeColumnSelection();
            RegisterLastSelectedColumn();
            IncrementIndex(drumPattern);
        }

        private void CalculateColumnToSelect(DrumPattern drumPattern)
        {
            _currentlyHighlighedColumn = drumPattern.GetHitsIndices(0)[_indexToTakeFromPattern] + 1;
        }

        private void IncrementIndex(DrumPattern drumPattern)
        {
            _indexToTakeFromPattern++;
            _indexToTakeFromPattern %= drumPattern.GetHitsIndices(0).Count();
        }

        private void ChangeColumnSelection()
        {
            foreach (var row in Enumerable.Range(0, _patternGrid.RowDefinitions.Count))
            {
                SelectPlayedColumn(row, _currentlyHighlighedColumn);
                UnselectPlayedColumn(row, _lastHighlighedColumn);
            }
        }

        private void SelectPlayedColumn(int row, int currentHighlightColumn)
        {
            var elementToSelect =
                _patternGrid.Children.Cast<UIElement>()
                    .FirstOrDefault(e => Grid.GetColumn(e) == currentHighlightColumn && Grid.GetRow(e) == row);

            var border = elementToSelect as SelectableBorder;
            if (border != null)
            {
                border.Effect = new DropShadowEffect
                {
                    Opacity = 0.8,
                };
            }
        }

        private void UnselectPlayedColumn(int row, int lastHighlighedColumn)
        {
            var elementToUnselect =
                _patternGrid.Children.Cast<UIElement>()
                    .FirstOrDefault(e => Grid.GetColumn(e) == lastHighlighedColumn && Grid.GetRow(e) == row);

            var border = elementToUnselect as SelectableBorder;
            if (border != null)
            {
                border.Effect = null;
            }
        }

        private void RegisterLastSelectedColumn()
        {
            _lastHighlighedColumn = _currentlyHighlighedColumn;
        }

        public void CleanUpHighlight()
        {
            foreach (var row in Enumerable.Range(0, _patternGrid.RowDefinitions.Count))
            {
                foreach (var column in Enumerable.Range(0, _patternGrid.ColumnDefinitions.Count))
                {
                    UnselectPlayedColumn(row, column);
                }
            }
        }
    }
}