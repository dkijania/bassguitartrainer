using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DrumMachine.Pattern.DrumMachineTile;

namespace DrumMachine.UI.WPF.Pattern
{
    public class SelectableBorder : Border, IDrumMachineTilePresenter
    {
        public DrumMachineTile Model { get; private set; }

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

        private readonly SolidColorBrush _notSelectedBush = new SolidColorBrush(Colors.WhiteSmoke);
        private readonly SolidColorBrush _selectedBrush = new SolidColorBrush(AvailableColors[SelectableColorIndex]);

        public SelectableBorder(int row, int column, DrumMachineTile.OnSelect onSelectEventHandler,
            DrumMachineTile.IgnoreMouseClick ignoreMouseClick, bool isSelected)
        {
            Model = new DrumMachineTile(row, column, onSelectEventHandler, ignoreMouseClick, isSelected, this);
            SetSelected(isSelected);
            MouseUp += MouseUpEventHandler;
            Background = _notSelectedBush;
            BorderBrush = new SolidColorBrush {Color = Colors.Black};
            Margin = new Thickness(2);
        }

        public void MouseUpEventHandler(object sender, MouseButtonEventArgs e)
        {
            if (!Model.InvokeIgnoreMouseClick())
            {
                Model.IsSelected = !Model.IsSelected;
                SetSelected(Model.IsSelected);
            }
            Model.InvokeOnSelectEvent(Model.IsSelected);
        }

        protected static int SelectableColorIndex
        {
            get { return ++ColorIndex%AvailableColors.Count(); }
        }

        public bool IsSelected
        {
            get { return Model.IsSelected; }
            set { Model.IsSelected = value; }
        }

        public void SetSelected(bool isSelected)
        {
            if (isSelected)
                Select();
            else
                Unselect();
        }

        public void Select()
        {
            Background = _selectedBrush;
        }

        public void Unselect()
        {
            Background = _notSelectedBush;
        }

        public int GetValueFor(DependencyProperty columnSpanProperty)
        {
            return (int) GetValue(columnSpanProperty);
        }
    }
}