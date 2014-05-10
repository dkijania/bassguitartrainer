using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WpfExtensions
{
        public static class DependencyObjectExtension
        {
            public static IEnumerable<DependencyObject> GetVisuals(this DependencyObject root)
            {
                foreach (var child in LogicalTreeHelper.GetChildren(root).OfType<DependencyObject>())
                {
                    yield return child;
                    foreach (var descendants in GetVisuals(child))
                        yield return descendants;
                }
            }
        }
    }

