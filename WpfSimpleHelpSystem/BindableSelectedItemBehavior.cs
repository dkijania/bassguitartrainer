using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace WpfSimpleHelpSystem
{
    public class BindableSelectedItemBehavior : Behavior<TreeView>
    {
        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof (object), typeof (BindableSelectedItemBehavior),
                new UIPropertyMetadata(null, OnSelectedItemChanged));

        public static event OnSelectedTreeItemChanged OnSelectedTreeItemChangedEvent;

        private static void InvokeOnSelectedTreeItemChangedEvent(object item)
        {
            OnSelectedTreeItemChanged handler = OnSelectedTreeItemChangedEvent;
            if (handler != null) handler(item);
        }

        public delegate void OnSelectedTreeItemChanged(object item);


        private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            InvokeOnSelectedTreeItemChangedEvent(e.NewValue);
        }
        
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
            {
                AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
            }
        }

        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItem = e.NewValue;
        }
    }
}