using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace DrumMachine.UI.WPF.TimeSignature
{
    public class BooleanOnlyFirstConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return FirstIsTrueAndDefined(values) && AllWithSkippedAreFalse(1, values);
        }

        private bool AllWithSkippedAreFalse(int i, IEnumerable<object> values)
        {
            return values.Skip(i).All(IsFalse);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("BooleanAndConverter is a OneWay converter.");
        }

        private bool FirstIsTrueAndDefined(IList<object> values)
        {
            if (values.Count <= 1) return false;
            var value = values[0];
            return IsTrue(value);
        }

        private bool IsFalse(object value)
        {
            return !IsTrue(value);
        }
        
        private bool IsTrue(object value)
        {
            return (value is bool) && (bool)value;
        }

    }
}