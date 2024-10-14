///The AnimalControl class is a pivotal component in this WPF application's UI, 
///providing a dynamic and interactive representation of animals within the zoo simulator. 
///By leveraging WPF's powerful data binding and dependency property systems, it ensures that the 
///UI accurately reflects the underlying model's state, offering a responsive and engaging user experience. 
///Proper implementation of resource management and event handling further enhances the control's robustness 
///and maintainability within the application.
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using ZooSimulatorLibrary.Animals;
using ZooSimulatorLibrary.Animals.States;
using System.Windows.Input;
using ZooSimulator.View.Customs.Services;

namespace ZooSimulator.View.Customs
{
    /// <summary>
    /// A custom WPF control that represents an animal and its current status within the zoo simulator.
    /// Handles the visual representation of the animal, its health state, and user interactions.
    /// </summary>
    public class AnimalControl : Control, IDisposable
    {
        #region LifeState

        /// <summary>
        /// Identifies the <see cref="LifeState"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LifeStateProperty =
            DependencyProperty.Register(
                nameof(LifeState),
                typeof(IAnimalLifeState),
                typeof(AnimalControl),
                new PropertyMetadata(new AliveState(), OnLifeStateChanged));

        /// <summary>
        /// Gets or sets the current life state of the animal.
        /// Changing this property updates the animal's visual representation and behavior.
        /// </summary>
        public IAnimalLifeState LifeState
        {
            get => (IAnimalLifeState)GetValue(LifeStateProperty);
            set => SetValue(LifeStateProperty, value);
        }

        /// <summary>
        /// Called when the <see cref="LifeState"/> property changes.
        /// Triggers the visual update of the animal's image based on its new state.
        /// </summary>
        /// <param name="d">The dependency object where the property changed.</param>
        /// <param name="e">Event data for the property change.</param>
        private static void OnLifeStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AnimalControl)d).SwitchPicture();
        }

        /// <summary>
        /// Updates the animal's image and behavior based on its current life state.
        /// Handles transitions between alive, dying, and dead states.
        /// </summary>
        private async void SwitchPicture()
        {
            if (LifeState is AliveState)
            {
                AnimalImage = ViewUtils.CreateBitmapImage(Animal.ImgURI);
                return;
            }

            AnimalImage = ViewUtils.CreateBitmapImage(Animal.DyingImgURI);

            if (!Animal.CanWalk)
            {
                _movingService.Stop();
            }

            if (LifeState is DeadState)
            {
                await Task.Delay(2000);
                if (Parent is AnimalCage canvas)
                {
                    canvas.Children.Remove(this);
                    Dispose();
                }
            }
        }

        #endregion

        #region AnimalImage

        /// <summary>
        /// Identifies the <see cref="AnimalImage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AnimalImageProperty =
            DependencyProperty.Register(
                nameof(AnimalImage),
                typeof(BitmapImage),
                typeof(AnimalControl),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the image representing the animal's current state.
        /// </summary>
        public BitmapImage AnimalImage
        {
            get => (BitmapImage)GetValue(AnimalImageProperty);
            set => SetValue(AnimalImageProperty, value);
        }

        #endregion

        #region Health

        /// <summary>
        /// Identifies the <see cref="Health"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HealthProperty =
            DependencyProperty.Register(
                nameof(Health),
                typeof(float),
                typeof(AnimalControl),
                new PropertyMetadata(0.0f));

        /// <summary>
        /// Gets or sets the health value of the animal.
        /// This property is bound to the animal's health in the model.
        /// </summary>
        public float Health
        {
            get => (float)GetValue(HealthProperty);
            set => SetValue(HealthProperty, value);
        }

        #endregion

        #region Animal

        /// <summary>
        /// Identifies the <see cref="Animal"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AnimalProperty =
            DependencyProperty.Register(
                nameof(Animal),
                typeof(IAnimal),
                typeof(AnimalControl),
                new PropertyMetadata(OnAnimalPropertyChanged));

        /// <summary>
        /// Gets or sets the <see cref="IAnimal"/> instance associated with this control.
        /// </summary>
        public IAnimal Animal
        {
            get => (IAnimal)GetValue(AnimalProperty);
            set => SetValue(AnimalProperty, value);
        }

        /// <summary>
        /// Called when the <see cref="Animal"/> property changes.
        /// Sets up data bindings for the new animal instance.
        /// </summary>
        /// <param name="d">The dependency object where the property changed.</param>
        /// <param name="e">Event data for the property change.</param>
        private static void OnAnimalPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AnimalControl)d).SetBindings();
        }

        /// <summary>
        /// Establishes data bindings between the control's properties and the animal's properties.
        /// </summary>
        private void SetBindings()
        {
            // Bind the Health property of the control to the Health property of the animal
            Binding healthBinding = ViewUtils.CreateBinding(nameof(Animal.Health), Animal);
            SetBinding(HealthProperty, healthBinding);

            // Bind the LifeState property of the control to the LifeMonitorService's state
            Binding lifeStateBinding = ViewUtils.CreateBinding(nameof(LifeState), Animal.HealthMonitorService, BindingMode.OneWay);
            SetBinding(LifeStateProperty, lifeStateBinding);

            // Set the initial animal image
            AnimalImage = ViewUtils.CreateBitmapImage(Animal.ImgURI);

            // Set the tooltip to display animal information
            ToolTip = $"{Animal}";
        }

        #endregion

        #region Fields

        private IMovingService _movingService;

        #endregion

        #region Static Constructor

        /// <summary>
        /// Static constructor to override the default style key for the <see cref="AnimalControl"/> class.
        /// Ensures that the control uses the appropriate style defined in XAML.
        /// </summary>
        static AnimalControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimalControl), new FrameworkPropertyMetadata(typeof(AnimalControl)));
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimalControl"/> class.
        /// Subscribes to mouse and loaded events.
        /// </summary>
        public AnimalControl()
        {
            MouseDown += OnMouseDown;
            Loaded += AnimalControlOnLoaded;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the MouseDown event on the control.
        /// Initiates movement of the animal if it can walk.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data for the mouse button event.</param>
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!Animal.CanWalk)
            {
                MessageBox.Show("The animal died or is going to die.", "HELP!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            _movingService.Move(200, 100, Animal.WalkingSpeed);
        }

        /// <summary>
        /// Handles the Loaded event of the control.
        /// Initializes the moving service for the animal.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data for the RoutedEvent.</param>
        private void AnimalControlOnLoaded(object sender, RoutedEventArgs e)
        {
            _movingService = new MovingService(this);
        }

        #endregion

        #region IDisposable Implementation

        /// <summary>
        /// Releases all resources used by the <see cref="AnimalControl"/> class.
        /// Disposes of the associated animal and unsubscribes from events.
        /// </summary>
        public void Dispose()
        {
            Animal.Dispose();
            MouseDown -= OnMouseDown;
            Loaded -= AnimalControlOnLoaded;
            GC.SuppressFinalize(this);
        }

        #endregion
    }

}