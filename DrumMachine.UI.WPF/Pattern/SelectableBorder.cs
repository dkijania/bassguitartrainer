using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DrumMachine.Engine.Pattern;

namespace DrumMachine.UI.WPF.Pattern
{
    public class SelectableBorder : Border
    {
        private class GraphicalProperties
        {
            public readonly Thickness Margin = new Thickness(2);
            public readonly SolidColorBrush BorderBrush = new SolidColorBrush { Color = Colors.Black };
            public readonly CornerRadius CornerRadius = new CornerRadius(10);

            public void Apply(Border border)
            {
                border.CornerRadius = CornerRadius;
                border.BorderBrush = BorderBrush;
                border.Margin = Margin;
            }
        }
        
        private readonly BorderDecorator _borderDecorator;
        private readonly DrumMachineTile _drumMachineTile;

        public SelectableBorder() : this(false)
        {
        }

        public SelectableBorder(bool initialState)
        {
            IsSelected = initialState;
            _borderDecorator = new BorderDecorator();
            ApplyGraphicalProperties();
            MouseUp += MouseUpEventHandler;
        }

        private void ApplyGraphicalProperties()
        {
            var graphicalProperties = new GraphicalProperties();
            graphicalProperties.Apply(this);
            Background = _borderDecorator.GetBrushForState(IsSelected);
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
            DrumMachineTile.IgnoreMouseClick onIgnoreMouseEvent, bool isSelected) : this(row, column, isSelected)
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