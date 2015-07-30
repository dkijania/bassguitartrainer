using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DrumMachine.UI.WPF.Pattern
{
    public class SelectableBorder : Border
    {
        private readonly BorderDecorator _borderDecorator;
        private readonly DrumMachineTile _drumMachineTile;

        public SelectableBorder() : this(false)
        {
        }

        public SelectableBorder(bool initialState)
        {
            IsSelected = initialState;
            _borderDecorator = new BorderDecorator();
            Background = _borderDecorator.GetBrushForState(IsSelected);
            CornerRadius = new CornerRadius(10);
            BorderBrush = new SolidColorBrush {Color = Colors.Black};
            Margin = new Thickness(2);
            MouseUp += MouseUpEventHandler;
        }

        public SelectableBorder(int row, int column)
            : this()
        {
            _drumMachineTile = new DrumMachineTile(row, column);
        }

        public SelectableBorder(int row, int column, bool isSelected)
            : this(isSelected)
        {
            _drumMachineTile = new DrumMachineTile(row, column);
        }


        public SelectableBorder(int row, int column, DrumMachineTile.OnSelect onOnSelectEvent,
            DrumMachineTile.IgnoreMouseClick onIgnoreMouseEvent, bool isSelected) : this(row, column,isSelected)
        {
            _drumMachineTile.OnSelectEvent += onOnSelectEvent;
            _drumMachineTile.OnIgnoreMouseClick += onIgnoreMouseEvent;
        }

        public bool IsSelected { get; set; }

        public event DrumMachineTile.OnSelect OnSelectEvent
        {
            add { _drumMachineTile.OnSelectEvent += value; }
            remove { _drumMachineTile.OnSelectEvent -= value; }
        }

        public void MouseUpEventHandler(object sender, MouseButtonEventArgs e)
        {
            if (!_drumMachineTile.InvokeOnIgnoreMouseClick())
            {
                ChangeSelectionState(!IsSelected);
            }
            _drumMachineTile.InvokeOnSelectEvent(_drumMachineTile.Row, _drumMachineTile.Column, IsSelected);
        }

        public void Unselect()
        {
            ChangeSelectionState(false);
        }

        public void Select()
        {
            ChangeSelectionState(true);
        }

        private void ChangeSelectionState(bool newState)
        {
            IsSelected = newState;
            AdjustBackgroundForSelectionState();
        }

        private void AdjustBackgroundForSelectionState()
        {
            Background = _borderDecorator.GetBrushForState(IsSelected);
        }
    }
}