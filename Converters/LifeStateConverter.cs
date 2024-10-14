using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using ZooSimulatorLibrary.Animals.States;

namespace ZooSimulator.Converters
{
    /// <summary>
    /// Converts an <see cref="IAnimalLifeState"/> instance to a corresponding <see cref="Brush"/> color.
    /// </summary>
    public class LifeStateColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IAnimalLifeState lifeState)
            {
                if (lifeState is AliveState)
                {
                    return Brushes.Green;
                }
                else if (lifeState is DyingState)
                {
                    return Brushes.Red;
                }
                else if (lifeState is DeadState)
                {
                    return Brushes.Black;
                }
            }

            return Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts an <see cref="IAnimalLifeState"/> instance to its string representation.
    /// </summary>
    public class LifeStateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IAnimalLifeState lifeState)
            {
                return $"{lifeState}";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
