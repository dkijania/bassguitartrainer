using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        
        public static void FillWithComponents(this Panel panel,IEnumerable items, ICommand command,
            Func<ICommand, object, Control> createComponentWithBinding)
        {
            panel.Children.Clear();
            foreach (var item in items)
            {
                panel.Children.Add(createComponentWithBinding.Invoke(command, item));
            }
        }

        public static Control FindButton(this Panel panel, Func<Button, bool> expression)
        {
            return panel.Children.OfType<Button>().First(expression);
        }
        
    }
}