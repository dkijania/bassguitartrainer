using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace DrumMachine.UI.WPF.Pattern
{
    public class BorderDecorator
    {
        private readonly SolidColorBrush _notSelectedBush = new SolidColorBrush(Colors.WhiteSmoke);
        private readonly SolidColorBrush _selectedBrush = new SolidColorBrush(AvailableColors[SelectableColorIndex]);

        protected static int ColorIndex = -1;

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
        
        protected static int SelectableColorIndex
        {
            get
            {
                ColorIndex = ++ColorIndex % AvailableColors.Count();
                return ColorIndex;
            }
        }

        public SolidColorBrush SelectedBrush { get { return _selectedBrush; } }
        public SolidColorBrush NotSelectedBrush { get { return _notSelectedBush; } }

        public SolidColorBrush GetBrushForState(bool state)
        {
            return state ? SelectedBrush : NotSelectedBrush;
        }

    }
}