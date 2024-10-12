using System.Globalization;
using System.Windows.Data;

namespace ZooSimulator.Converters
{
    public class FloatToPercentageStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float floatValue)
            {
                return (floatValue * 100).ToString("F2") + " %";
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                if (stringValue.EndsWith(" %") &&
                    float.TryParse(stringValue.Replace(" %", ""), out float result))
                {
                    return result / 100;
                }
            }

            return Binding.DoNothing;
        }
    }
}