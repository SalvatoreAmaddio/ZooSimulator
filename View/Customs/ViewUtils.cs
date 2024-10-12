using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ZooSimulator.View.Customs
{
    /// <summary>
    /// Simple utils class for reusable methods.
    /// </summary>
    public class ViewUtils
    {
        public static Binding CreateBinding(string path, object source, BindingMode mode = BindingMode.TwoWay)
        {
            return new(path)
            {
                Source = source,
                Mode = mode,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
        }

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
