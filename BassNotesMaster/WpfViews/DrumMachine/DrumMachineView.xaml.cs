using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BassNotesMaster.NotePlayer;
using BassNotesMaster.WpfViews.Metronome;
using BassNotesMasterApi.NotePlayer;
using DrumMachine.Audio;
using DrumMachine.Engine.Pattern;
using MahApps.Metro.Controls;

namespace BassNotesMaster.WpfViews.DrumMachine
{
    /// <summary>
    /// Interaction logic for DrumMachineView.xaml
    /// </summary>
    public partial class DrumMachineView : UserControl, IViewControl, IPatternTilesManipulator
    {
        private readonly TimeSignatureViewModel _timeSignatureViewModel;
        private readonly DrumMachineViewModel _drumMachineViewModel;

        public MetroWindow MetroWindow { get; set; }

        public DrumMachineView()
        {
            InitializeComponent();
            FillFirstGridColumnWithSamplesNames();
            _timeSignatureViewModel = new TimeSignatureViewModel(TimeSignaturePanel);
            _drumMachineViewModel = new DrumMachineViewModel(this as IPatternTilesManipulator);
            _timeSignatureViewModel.PropertyChanged += _drumMachineViewModel.PropertyChangedHandler;
            _timeSignatureViewModel.StandardTimeChangedEvent += _drumMachineViewModel.StandardTimeChangedEventHandler;
            TimeSignaturePanel.DataContext = _timeSignatureViewModel;
            MainGrid.DataContext = _drumMachineViewModel;
        }

        private void FillFirstGridColumnWithSamplesNames()
        {
            foreach (var name in DefaultAudioSampler.Instance.Names)
            {
                var rowDefinition = new RowDefinition { Height = GridLength.Auto };
                PatternGrid.RowDefinitions.Add(rowDefinition);
                AddLabelToGrid(name, PatternGrid);
            }
        }

        private void FillGridWithCheckboxes(int count, int scaleFactor)
        {
            foreach (var i in Enumerable.Range(0, count))
            {
                var columnDefinition = new ColumnDefinition { Width = new GridLength(10 * scaleFactor) };
                PatternGrid.ColumnDefinitions.Add(columnDefinition);
                AddCheckboxesToColumn();
            }
        }

        private void AddCheckboxesToColumn()
        {
            var count = DefaultAudioSampler.Instance.Names.Count();

            foreach (var i in Enumerable.Range(0, count))
            {
                AddElementToGrid(i, PatternGrid);
            }
        }

        private void AddLabelToGrid(string label, Grid grid)
        {
            var labelElement = new Label { Content = label, };
            labelElement.SetValue(Grid.ColumnProperty, 0);
            labelElement.SetValue(Grid.RowProperty, grid.RowDefinitions.Count - 1);
            grid.Children.Add(labelElement);
        }

        private void AddElementToGrid(int row, Grid grid)
        {
            var column = grid.ColumnDefinitions.Count - 1;
            var element = new SelectableBorder(row, column);
            element.SetValue(Grid.RowProperty, row);
            element.SetValue(Grid.ColumnProperty, column);
            grid.Children.Add(element);
        }

        private int _count;
        private int _span;

        public void SetColumnsCount(int count, int span)
        {
            _count = count;
            _span = span;
            var columnDefintions = PatternGrid.ColumnDefinitions;
            columnDefintions.Clear();
            PatternGrid.RowDefinitions.Clear();
            PatternGrid.Children.Clear();
            columnDefintions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            FillFirstGridColumnWithSamplesNames();
            AddColumns(count, span);
        }

        public void AddColumns(int count, int span)
        {
            FillGridWithCheckboxes(count, span);
            var columnDefintions = PatternGrid.ColumnDefinitions;
            columnDefintions.Add(new ColumnDefinition() { Width = _delimeterColumnWidth });
        }


        private readonly GridLength _delimeterColumnWidth = new GridLength(35);


        public void AddBar()
        {
            AddColumns(_count, _span);
        }

        public void RemoveBar()
        {
           var lastColumnIndex = PatternGrid.ColumnDefinitions.Count - 1;
            bool ignoreFirst = true;
            for (var i = lastColumnIndex; i >= 0; i--)
            {
                if (PatternGrid.ColumnDefinitions[i].Width.Equals(_delimeterColumnWidth))
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
                PatternGrid.ColumnDefinitions.RemoveAt(i);


            }
        }

        public void DeleteChildrenAtColumn(int column)
        {
            var collection = PatternGrid.Children.OfType<UIElement>().ToArray();
            for (var i = collection.Length - 1; i >= 0; --i)
            {
                if (Grid.GetColumn(collection[i]) == column)
                {
                    PatternGrid.Children.RemoveAt(i);
                }
            }
        }

        public void FillDrumPattern(DrumPattern drumPattern)
        {
            foreach (var row in Enumerable.Range(0, PatternGrid.RowDefinitions.Count))
            {
                var columnCount = PatternGrid.ColumnDefinitions.Count();
                int offset = 1;
                for (var column = offset; column < columnCount; column++)
                {
                    var element =
                        PatternGrid.Children.Cast<UIElement>()
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

    public class DrumMachineTile
    {
        public int Row { get; protected set; }
        public int Column { get; protected set; }
        public event OnSelect OnSelectEvent;

        public virtual void InvokeOnSelectEvent()
        {
            var handler = OnSelectEvent;
            if (handler != null) handler(Row, Column);
        }

        public delegate void OnSelect(int row, int column);

        public DrumMachineTile(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }

    public class DrumMachineTileBuilder
    {
        private static readonly WavPlayer WavPlayer = new WavPlayer();

        public static DrumMachineTile Build(int row, int column)
        {
            var drumMachineTile = new DrumMachineTile(row, column);
            drumMachineTile.OnSelectEvent += drumMachineTile_OnSelectEvent;
            return drumMachineTile;
        }

        private static void drumMachineTile_OnSelectEvent(int row, int column)
        {
            var sampleName = DefaultAudioSampler.Instance.Names.ElementAt(row);
            var sample = DefaultAudioSampler.Instance[sampleName];
            WavPlayer.Volume = 0.5f;
            WavPlayer.Play(sample.RawStream);
        }
    }
}