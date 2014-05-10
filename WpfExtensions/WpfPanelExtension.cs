using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfExtensions
{
    public static class WpfPanelExtension
    {
        public static void RemoveVisual<T>(this Panel myVisual, Predicate<T> predicate = null) where T : UIElement
        {
            var intTotalChildren = myVisual.Children.Count - 1;
            for (var intCounter = intTotalChildren; intCounter > 0; intCounter--)
            {
                if (myVisual.Children[intCounter].GetType() != typeof (T)) continue;
                var ucCurrentChild = (T) myVisual.Children[intCounter];
                if (predicate == null)
                    myVisual.Children.Remove(ucCurrentChild);
                else if (predicate.Invoke(ucCurrentChild))
                    myVisual.Children.Remove(ucCurrentChild);
            }
        }
    }
}