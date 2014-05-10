using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BassNotesMasterApi.Excercise;
using WpfExtensions;

namespace BassNotesMaster.Excercises
{
    public class ExcerciseOptionGuiManager : IExcerciseOptionGuiManager
    {
        private readonly GroupBox _parent;
        private readonly Grid _grid;
        
        public ExcerciseOptionGuiManager(GroupBox parent)
        {
            _parent = parent;
            _grid = new Grid();    
            _grid.ColumnDefinitions.Add(new ColumnDefinition {Width = GridLength.Auto});
            _grid.ColumnDefinitions.Add(new ColumnDefinition {Width = GridLength.Auto});
       }
       
        public void Clean()
        {
            _grid.RemoveVisual<FrameworkElement>();
        }

        public void Build()
        {
            _parent.Content = _grid;
        }

        public void Clear()
        {
            _grid.Children.Clear();
        }


        private void AddNewRow()
        {
            _grid.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
        }

        public bool GetBooleanOption(string label)
        {
            var labelElement =  _grid.Children.OfType<Label>().FirstOrDefault(x=>x.Content.Equals(label));
            if (labelElement == null) return false;

            var rowProperty = labelElement.GetValue(Grid.RowProperty);
            var element = _grid.Children
                          .Cast<UIElement>()
                          .Where(i => Grid.GetRow(i) == (int)rowProperty).Skip(1).FirstOrDefault();
            return ((CheckBox) element).IsChecked.Value;
        }

        public string GetStringOption(string label)
        {
            var labelElement = _grid.Children.OfType<Label>().FirstOrDefault();
            var rowProperty = labelElement.GetValue(Grid.RowProperty);
            var element = _grid.Children
                          .Cast<UIElement>()
                          .Where(i => Grid.GetRow(i) == (int)rowProperty).Skip(1).FirstOrDefault();
            return ((ComboBox)element).SelectedItem.ToString();
        }

        public void AddOption(string label, bool trueFalseValue)
        {
            AddNewRow();
            AddLabelToGrid(label);
            AddCheckBoxToGrid(trueFalseValue);
        }

        public void AddOption(string label, IEnumerable<string> enumerable)
        {
            AddNewRow();
            AddLabelToGrid(label);
            AddComboBoxToGrid(enumerable);
        }

        private void AddLabelToGrid(string label)
        {
            var labelElement = new Label {Content = label,
               
            
            };
            labelElement.SetValue(Grid.ColumnProperty, 0);
            labelElement.SetValue(Grid.RowProperty, _grid.RowDefinitions.Count - 1);
            _grid.Children.Add(labelElement);
            
        }

        private void AddCheckBoxToGrid(bool value)
        {
            var checkBox = new CheckBox() { IsChecked = value };
            checkBox.SetValue(Grid.ColumnProperty, 1);
            checkBox.SetValue(Grid.RowProperty, _grid.RowDefinitions.Count - 1);
            _grid.Children.Add(checkBox);
        }

        private void AddComboBoxToGrid(IEnumerable<string> enumerable)
        {
            var checkBox = new ComboBox() { ItemsSource= enumerable };
            checkBox.SetValue(Grid.ColumnProperty, 1);
            checkBox.SetValue(Grid.RowProperty, _grid.ColumnDefinitions.Count - 1);
        }
    }
}