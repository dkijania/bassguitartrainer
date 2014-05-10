using System.Windows;

namespace WpfExtensions
{
    public static class RectangleExtension
    {
        public static Point GetCentrePoint(this Rect rect)
        {
            return new Point((int)(rect.X + rect.Width / 2), (int)(rect.Y + rect.Height / 2));
        }
    }
}
