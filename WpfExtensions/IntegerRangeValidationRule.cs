using System;
using System.Globalization;
using System.Windows.Controls;

namespace WpfExtensions
{
    public class IntegerRangeValidationRule : ValidationRule
    {
        public int Min { get; set; }

        public int Max { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var val = 0;
            var inputValue = (string)value;
            try
            {
                if (!string.IsNullOrEmpty(inputValue))
                    val = Int32.Parse(inputValue);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, "Illegal characters or " + e.Message);
            }

            if ((val < Min) || (val > Max))
            {
                return new ValidationResult(false,
                    "Please provide value in the range: " + Min + " - " + Max + ".");
            }
            return new ValidationResult(true, null);
        }
    }

    public class DoubleRangeValidationRule : ValidationRule
    {
        public double Min { get; set; }

        public double Max { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var val = 0d;
            var inputValue = (string)value;
            try
            {
                if (!string.IsNullOrEmpty(inputValue))
                    val = Double.Parse(inputValue);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, "Illegal characters or " + e.Message);
            }

            if ((val < Min) || (val > Max))
            {
                return new ValidationResult(false,
                    "Please provide value in the range: " + Min + " - " + Max + ".");
            }
            return new ValidationResult(true, null);
        }
    }
}