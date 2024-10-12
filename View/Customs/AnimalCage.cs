using System.Collections;
using System.Windows;
using System.Windows.Controls;
using ZooSimulatorLibrary.Animals;

namespace ZooSimulator.View.Customs
{
    /// <summary>
    /// This is a custom class which represents the cage where a group of animals are held.
    /// </summary>
    public class AnimalCage : Canvas, IDisposable
    {
        public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(
        nameof(ItemsSource),
        typeof(IEnumerable),
        typeof(AnimalCage),
        new PropertyMetadata(null, OnItemsSourceChanged));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AnimalCage canvas)
            {
                canvas.UpdateChildren();
            }
        }

        private void UpdateChildren()
        {
            Children.Clear();

            if (ItemsSource == null) return;

            foreach (var item in ItemsSource)
            {
                AnimalControl element = new()
                {
                    Animal = (IAnimal)item
                };

                Children.Add(element);

                SetLeft(element, GetRandomPosition(0, ActualWidth - 50));
                SetTop(element, GetRandomPosition(0, ActualHeight - 50));
            }
        }

        private static double GetRandomPosition(double min, double max)
        {
            return new Random().NextDouble() * (max - min) + min;
        }

        public AnimalCage()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Window win = Window.GetWindow(this);
            win.Unloaded += OnWinUnloaded;
        }

        private void OnWinUnloaded(object sender, RoutedEventArgs e)
        {
            Dispose();
        }

        public void Dispose()
        {
            Window win = Window.GetWindow(this);
            win.Unloaded -= OnWinUnloaded;
            Loaded -= OnLoaded;

            foreach (var item in ItemsSource)
            {
                ((IDisposable)item).Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}