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
    /// This is a custom control which represents an Animal and its current status.
    /// </summary>
    public class AnimalControl : Control, IDisposable
    {
        #region LifeState
        public static readonly DependencyProperty LifeStateProperty =
        DependencyProperty.Register(nameof(LifeState), typeof(IAnimalLifeState),
        typeof(AnimalControl), new PropertyMetadata(new AliveState(), OnLifeStateChanged));

        private static void OnLifeStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AnimalControl)d).SwitchPicture();
        }

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

        public IAnimalLifeState LifeState
        {
            get => (IAnimalLifeState)GetValue(LifeStateProperty);
            set => SetValue(LifeStateProperty, value);
        }
        #endregion

        #region AnimalImage
        public static readonly DependencyProperty AnimalImageProperty =
        DependencyProperty.Register(nameof(AnimalImage), typeof(BitmapImage),
        typeof(AnimalControl), new PropertyMetadata(null));

        public BitmapImage AnimalImage
        {
            get => (BitmapImage)GetValue(AnimalImageProperty);
            set => SetValue(AnimalImageProperty, value);
        }
        #endregion

        #region Health
        public static readonly DependencyProperty HealthProperty =
        DependencyProperty.Register(nameof(Health), typeof(float),
        typeof(AnimalControl), new PropertyMetadata(0.0f));

        public float Health
        {
            get => (float)GetValue(HealthProperty);
            set => SetValue(HealthProperty, value);
        }
        #endregion

        #region Animal
        public static readonly DependencyProperty AnimalProperty =
        DependencyProperty.Register(nameof(Animal), typeof(IAnimal),
        typeof(AnimalControl), new PropertyMetadata(OnAnimalPropertyChanged));

        private static void OnAnimalPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AnimalControl)d).SetBindings();
        }

        public IAnimal Animal
        {
            get => (IAnimal)GetValue(AnimalProperty);
            set => SetValue(AnimalProperty, value);
        }

        private void SetBindings()
        {
            Binding healthBinding = ViewUtils.CreateBinding(nameof(Animal.Health), Animal);
            SetBinding(HealthProperty, healthBinding);

            Binding lifeStateBinding = ViewUtils.CreateBinding(nameof(LifeState), Animal.HealthMonitorService, BindingMode.OneWay);

            SetBinding(LifeStateProperty, lifeStateBinding);

            AnimalImage = ViewUtils.CreateBitmapImage(Animal.ImgURI);
            ToolTip = $"{Animal}";

        }
        #endregion

        private IMovingService _movingService;

        static AnimalControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimalControl), new FrameworkPropertyMetadata(typeof(AnimalControl)));
        }

        public AnimalControl()
        {
            MouseDown += OnMouseDown;
            Loaded += AnimalControlOnLoaded;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!Animal.CanWalk)
            {
                MessageBox.Show("The animal died or is going to", "HELP!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            _movingService.Move(200, 100, Animal.WalkingSpeed);
        }

        private void AnimalControlOnLoaded(object sender, RoutedEventArgs e)
        {
            _movingService = new MovingService(this);
        }

        public void Dispose()
        {
            Animal.Dispose();
            MouseDown -= OnMouseDown;
            Loaded -= AnimalControlOnLoaded;
            GC.SuppressFinalize(this);
        }
    }
}