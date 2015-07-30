using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DrumMachine.Engine.Pattern;

namespace DrumMachine.UI.WPF.Pattern.Converters
{
    public class UiDrumPatternConverter
    {
        public int RowsCount { get; set; }
        public int ColumnsCount { get; set; }
        public UIElementCollection ElementCollection { get; set; }
        public ColumnDefinitionCollection ColumnDefinition { get; set; }
        private int _barLimit;

        public List<DrumPatternPart> FillPatternGrid(DrumPattern drumPattern)
        {
            var splittedParts = new List<DrumPatternPart>();
            var array = drumPattern.Array;
            _barLimit = CalculateBarLimitIndex(array, drumPattern);
            for (var i = 0; i < array.GetLength(0); i++)
            {
                var row = GetRow(array, i);
                fillSplittedParts(i, 0, row, splittedParts);
            }
            return splittedParts;
        }

        private int CalculateBarLimitIndex(byte[,] array,DrumPattern drumPattern)
        {
            return array.GetLength(1) / drumPattern.Settings.Bars;
        }

        private List<byte> GetRow(byte[,] array,int i)
        {
            var row = new List<byte>();
            for (var j = 0; j < array.GetLength(1); j++)
                row.Add(array[i, j]);
            return row;
        } 

        private void fillSplittedParts(int rowId, int startIndex, List<byte> row, List<DrumPatternPart> splittedParts)
        {
            var bar = CalculateBarNo(startIndex);
            if ((!ContainsHit(row) || HitOnlyOnFirst(row)) && !IsLongerThanBarLimit(row))
            {
                splittedParts.Add(new DrumPatternPart(rowId, startIndex, row.Count, ContainsHit(row), bar));
                return;
            }
            var center = (int) Math.Floor((double) (row.Count/2));
            fillSplittedParts(rowId, startIndex, row.Take(center).ToList(), splittedParts);
            fillSplittedParts(rowId, center + startIndex, row.Skip(center).ToList(), splittedParts);
        }

        private bool IsLongerThanBarLimit(List<byte> row)
        {
            return row.Count > _barLimit;
        }

        private bool HitOnlyOnFirst(List<byte> row)
        {
            return row.First() == DrumPattern.Hit && !ContainsHit(row.Skip(1).ToList());
        }

        private bool ContainsHit(List<byte> row)
        {
            return row.Contains(DrumPattern.Hit);
        }

        private int CalculateBarNo(int startIndex)
        {
            return (int) Math.Floor((double) (startIndex/_barLimit));
        }

        /**
          * Function iterates through all children of grid and if its a DrumMachineTile, it gets its 'IsSelected' property.
          * Column used for data separation is being omitted.
          */

        public void FillDrumPattern(DrumPattern drumPattern)
        {
            var tiles = ElementCollection.OfType<SelectableBorder>();
            foreach (var row in Enumerable.Range(0, RowsCount))
            {
                var drumPatternIndex = 0;
                var columnIndices = GetColumnsIndices(ElementCollection, ColumnsCount, row);
                foreach (var columnIndex in columnIndices)
                {
                    var patternValue = GetPatternValue(tiles, columnIndex, row);
                    drumPattern[row, drumPatternIndex] = patternValue;
                    drumPatternIndex += GetSpanValue(tiles, columnIndex, row);
                }
            }
        }

        private IEnumerable<int> GetColumnsIndices(IEnumerable elementCollection, int columnCount, int row)
        {
            var indices = new List<int>();
            for (var column = 0; column < columnCount; column++)
            {
                var element = GetElementForRow(elementCollection, column, row);
                if (IsProperDrumPatternCell(element))
                {
                    indices.Add(column);
                }
            }
            return indices.ToArray();
        }

        private UIElement GetElementForRow(IEnumerable elements, int column, int row)
        {
            return elements.OfType<UIElement>()
                .FirstOrDefault(e => Grid.GetColumn(e) == column && Grid.GetRow(e) == row);
        }

        private bool IsProperDrumPatternCell(UIElement element)
        {
            return element is SelectableBorder;
        }

        private byte GetPatternValue(IEnumerable<SelectableBorder> tiles, int column, int row)
        {
            var tile = GetTileForCoordinates(tiles, column, row);
            if (tile == null) return DrumPattern.NoHit;
            return tile.IsSelected ? DrumPattern.Hit : DrumPattern.NoHit;
        }

        private SelectableBorder GetTileForCoordinates(IEnumerable<SelectableBorder> tiles, int column, int row)
        {
            return tiles.FirstOrDefault(e => Grid.GetColumn(e) == column && Grid.GetRow(e) == row);
        }

        private int GetSpanValue(IEnumerable<SelectableBorder> tiles, int column, int row)
        {
            var tile = GetTileForCoordinates(tiles, column, row);
            return (int) tile.GetValue(Grid.ColumnSpanProperty);
        }
    }
}