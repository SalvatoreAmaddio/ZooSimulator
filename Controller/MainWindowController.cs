using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Windows;
using System.Windows.Input;
using ZooSimulator.View;
using ZooSimulatorLibrary.Animals;
using ZooSimulatorLibrary.Death;
using ZooSimulatorLibrary.Notifiers;
using ZooSimulatorLibrary.Zoo;

namespace ZooSimulator.Controller
{
    public class MainWindowController : AbstractNotifier
    {
        private Window _window;
        private DateTime _currentTime;
        public ICommand OpenGuideCMD { get; }
        public ICommand JumpCMD { get; }
        public ICommand FeedCMD { get; }
        public string CurrentTime
        {
            get
            {
                return DateTime.Now.ToString("HH:mm:ss");
            }
        }

        public ObservableRangeCollection<Elephant>? Elephants => App.Zoo.Elephants;
        public ObservableRangeCollection<Giraffe>? Giraffes => App.Zoo.Giraffes;
        public ObservableRangeCollection<Monkey>? Monkeys => App.Zoo.Monkeys;
        public DeathManager Manager { get; set; } = new(App.Zoo);

        internal MainWindowController()
        {
            Manager.GameEnded += OnGameEnded;
            FeedCMD = new Command(Feed);
            JumpCMD = new Command(Jump);
            OpenGuideCMD = new Command(OpenGuide);
        }

        public MainWindowController(Window window) : this()
        {
            _window = window;
            _window.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Manager.Run();

            Task.Run(() =>
            {
                while (true)
                {
                    Task.Delay(1000);
                    RaisePropertyChanged(nameof(CurrentTime));
                }
            });
        }

        private async void OnGameEnded(object sender, EventArgs e)
        {

            App.Current.Dispatcher.Invoke(() =>
            {
                MessageBoxResult result = MessageBox.Show(_window, "The Game Has Ended!\nWould you like to play again?", "Choose an option", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    MessageBox.Show("Have a great day!", "Thanks for playing!");

                    _window.Close();
                }
            });

            App.Zoo.Animals = await Task.Run(() => AnimalGenerator.GenerateZooAsync(App.NumOfAnimals));

            RaisePropertyChanged(nameof(Elephants));
            RaisePropertyChanged(nameof(Monkeys));
            RaisePropertyChanged(nameof(Giraffes));
            Manager.Run();
        }

        private void OpenGuide()
        {
            new GuideWindow().Show();
        }

        private void Jump()
        {
            Manager.Stop();
            Manager.AffectLifeExpectancy();
            if (App.Zoo.IsEmpty)
            {
                Manager.InvokeGameEnded();
            }
            else
            {
                Manager.Run();
            }
        }

        private void Feed()
        {
            App.Zoo.FeedingService.Feed();
        }
    }
}