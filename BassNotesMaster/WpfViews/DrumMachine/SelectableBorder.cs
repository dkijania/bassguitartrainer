using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BassNotesMaster.WpfViews.DrumMachine
{
    public class SelectableBorder : Border
    {
        private readonly SolidColorBrush _notSelectedBush = new SolidColorBrush(Colors.WhiteSmoke);
        private readonly SolidColorBrush _selectedBrush = new SolidColorBrush(AvailableColors[SelectableColorIndex]);
        private readonly DrumMachineTile _drumMachineTile;

        public bool IsSelected { get; private set; }

        protected static int ColorIndex = -1;

        protected static int SelectableColorIndex
        {
            get
            {
                ColorIndex++;
                if (ColorIndex >= AvailableColors.Count())
                {
                    ColorIndex = 0;
                }
                return ColorIndex;
            }
        }

        public event DrumMachineTile.OnSelect OnSelectEvent
        {
            add { _drumMachineTile.OnSelectEvent += value; }
            remove { _drumMachineTile.OnSelectEvent -= value; }
        }


        protected static Color[] AvailableColors =
        {
            Colors.Yellow,
            Colors.YellowGreen,
            Colors.DeepSkyBlue,
            Colors.Blue,
            Colors.BlueViolet,
            Colors.Purple,
            Colors.Magenta,
            Colors.MediumVioletRed,
            Colors.Red,
            Colors.OrangeRed,
            Colors.Orange
        };

        public SelectableBorder()
        {
            Background = _notSelectedBush;
            Margin = new Thickness(2);
            MouseUp += MouseUpEventHandler;
            IsSelected = false;
        }

        public SelectableBorder(int row, int column)
            : this()
        {
            _drumMachineTile = DrumMachineTileBuilder.Build(row, column);
        }

        public void MouseUpEventHandler(object sender, MouseButtonEventArgs e)
        {
            IsSelected = !IsSelected;
            Background = IsSelected ? _selectedBrush : _notSelectedBush;
            if (IsSelected) _drumMachineTile.InvokeOnSelectEvent();
        }
    }
}