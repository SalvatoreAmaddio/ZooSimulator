using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ZooSimulator.View.Customs
{
    /// <summary>
    /// Provides utility methods for creating bindings and bitmap images in WPF applications.
    /// </summary>
    public static class ViewUtils
    {
        /// <summary>
        /// Creates a new <see cref="Binding"/> with the specified path, source, and mode.
        /// </summary>
        /// <param name="path">The path to the binding source property.</param>
        /// <param name="source">The source object for the binding.</param>
        /// <param name="mode">The binding mode. Default is <see cref="BindingMode.TwoWay"/>.</param>
        /// <returns>A configured <see cref="Binding"/> object ready for use in data binding.</returns>
        public static Binding CreateBinding(string path, object source, BindingMode mode = BindingMode.TwoWay)
        {
            return new(path)
            {
                Source = source,
                Mode = mode,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
        }

        /// <summary>
        /// Creates a new <see cref="BitmapImage"/> from the specified relative path within the application.
        /// </summary>
        /// <param name="path">The relative path to the image resource within the application.</param>
        /// <returns>A <see cref="BitmapImage"/> object representing the image at the specified path.</returns>
        public static BitmapImage CreateBitmapImage(string path)
        {
            Uri imageUri = new Uri($"pack://application:,,,/ZooSimulator;component/{path}", UriKind.Absolute);
            BitmapImage bitmapImage = new();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = imageUri;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            return bitmapImage;
        }
    }

}
