using System.Windows;
using System.Windows.Controls;

namespace WpfExtensions
{
    public static class FrameworkElementExtension
    {
        public static Rect GetRectangleFromDimensions(this FrameworkElement border)
        {
            var bPoint = new Point(Canvas.GetLeft(border), Canvas.GetTop(border));
            var bSize = new Size(border.ActualWidth, border.ActualHeight);
            return new Rect(bPoint, bSize);
        }
    }
}
