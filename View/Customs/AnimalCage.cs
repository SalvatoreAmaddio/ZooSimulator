///The AnimalCage class is a pivotal component in the WPF zoo simulator application, providing a 
///structured and dynamic way to display groups of animals within designated cages. 
///By leveraging WPF's data binding and dependency property systems, 
///it ensures that the UI remains responsive and accurately reflects the underlying data model. 
///Proper implementation of resource management through the IDisposable interface further enhances 
///the stability and maintainability of the application.
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using ZooSimulatorLibrary.Animals;

namespace ZooSimulator.View.Customs
{
    /// <summary>
    /// Represents a cage within the zoo simulator where a group of animals are held.
    /// Manages the display and positioning of animal controls within the cage.
    /// </summary>
    public class AnimalCage : Canvas, IDisposable
    {
        #region ItemsSource

        /// <summary>
        /// Identifies the <see cref="ItemsSource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(AnimalCage),
                new PropertyMetadata(null, OnItemsSourceChanged));

        /// <summary>
        /// Gets or sets the collection of animals to be displayed within the cage.
        /// </summary>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Called when the <see cref="ItemsSource"/> property changes.
        /// Updates the child controls to reflect the new collection of animals.
        /// </summary>
        /// <param name="d">The dependency object where the property changed.</param>
        /// <param name="e">Event data for the property change.</param>
        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AnimalCage canvas)
            {
                canvas.UpdateChildren();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimalCage"/> class.
        /// Subscribes to the Loaded event to handle initialization tasks.
        /// </summary>
        public AnimalCage()
        {
            Loaded += OnLoaded;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the Loaded event of the <see cref="AnimalCage"/>.
        /// Subscribes to the window's Unloaded event to manage resource cleanup.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data for the RoutedEvent.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Window win = Window.GetWindow(this);
            if (win != null)
            {
                win.Unloaded += OnWinUnloaded;
            }
        }

        /// <summary>
        /// Handles the Unloaded event of the window containing the <see cref="AnimalCage"/>.
        /// Initiates the disposal of resources held by the cage.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data for the RoutedEvent.</param>
        private void OnWinUnloaded(object sender, RoutedEventArgs e)
        {
            Dispose();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the child controls within the cage based on the current <see cref="ItemsSource"/>.
        /// Clears existing children and creates new <see cref="AnimalControl"/> instances for each animal.
        /// </summary>
        private void UpdateChildren()
        {
            Children.Clear();

            if (ItemsSource == null) return;

            foreach (var item in ItemsSource)
            {
                if (item is IAnimal animal)
                {
                    AnimalControl element = new()
                    {
                        Animal = animal
                    };

                    Children.Add(element);

                    SetLeft(element, GetRandomPosition(0, ActualWidth - 50));
                    SetTop(element, GetRandomPosition(0, ActualHeight - 50));
                }
            }
        }

        /// <summary>
        /// Generates a random position within the specified range.
        /// </summary>
        /// <param name="min">The minimum value for the position.</param>
        /// <param name="max">The maximum value for the position.</param>
        /// <returns>A random double value between <paramref name="min"/> and <paramref name="max"/>.</returns>
        private static double GetRandomPosition(double min, double max)
        {
            // Note: Using a static Random instance or ThreadLocal<Random> is recommended to avoid seed duplication issues.
            return new Random().NextDouble() * (max - min) + min;
        }

        #endregion

        #region IDisposable Implementation

        /// <summary>
        /// Releases all resources used by the <see cref="AnimalCage"/> class.
        /// Disposes of all animal controls and unsubscribes from event handlers.
        /// </summary>
        public void Dispose()
        {
            Window win = Window.GetWindow(this);
            if (win != null)
            {
                win.Unloaded -= OnWinUnloaded;
            }
            Loaded -= OnLoaded;

            if (ItemsSource != null)
            {
                foreach (var item in ItemsSource)
                {
                    if (item is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                }
            }

            GC.SuppressFinalize(this);
        }

        #endregion
    }

}