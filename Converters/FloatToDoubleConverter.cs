using System.Windows.Data;

namespace ZooSimulator.Converters
{
    /// <summary>
    /// Converts a <see cref="float"/> value to a scaled <see cref="double"/> value by multiplying by 100,
    /// and converts back by dividing a <see cref="double"/> value by 100 to retrieve the original <see cref="float"/>.
    /// </summary>
    public class FloatToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return System.Convert.ToDouble(value) * 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return System.Convert.ToSingle(value) / 100;
        }
    }
}