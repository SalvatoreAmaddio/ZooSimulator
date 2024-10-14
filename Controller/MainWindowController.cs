///The MainWindowController class is a crucial component in your WPF application's MVVM architecture. 
///It manages user interactions, commands, and updates the UI to reflect the current state of the zoo simulation. 
///By properly implementing property change notifications and commands, it enables a responsive and interactive user experience, 
///ensuring that the application logic and UI remain in sync.
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Windows;
using System.Windows.Input;
using ZooSimulator.View;
using ZooSimulatorLibrary.Animals;
using ZooSimulatorLibrary.Death;
using ZooSimulatorLibrary.Notifiers;

namespace ZooSimulator.Controller
{
    /// <summary>
    /// Acts as the ViewModel for the MainWindow in the WPF application.
    /// Manages commands, data binding, and interactions between the UI and the zoo simulation logic.
    /// </summary>
    public class MainWindowController : AbstractNotifier
    {
        private Window _window;
        private DateTime _currentTime;

        /// <summary>
        /// Gets the command to open the guide window.
        /// </summary>
        public ICommand OpenGuideCMD { get; }

        /// <summary>
        /// Gets the command to simulate a time jump, affecting animals' life expectancy.
        /// </summary>
        public ICommand JumpCMD { get; }

        /// <summary>
        /// Gets the command to feed all animals in the zoo.
        /// </summary>
        public ICommand FeedCMD { get; }

        /// <summary>
        /// Gets the current time formatted as "HH:mm:ss".
        /// Used for displaying the current time in the UI.
        /// </summary>
        public string CurrentTime
        {
            get
            {
                return DateTime.Now.ToString("HH:mm:ss");
            }
        }

        /// <summary>
        /// Gets the collection of elephants in the zoo.
        /// </summary>
        public ObservableRangeCollection<Elephant>? Elephants => App.Zoo.Elephants;

        /// <summary>
        /// Gets the collection of giraffes in the zoo.
        /// </summary>
        public ObservableRangeCollection<Giraffe>? Giraffes => App.Zoo.Giraffes;

        /// <summary>
        /// Gets the collection of monkeys in the zoo.
        /// </summary>
        public ObservableRangeCollection<Monkey>? Monkeys => App.Zoo.Monkeys;

        /// <summary>
        /// Gets or sets the death manager that controls animals' health over time.
        /// </summary>
        public DeathManager Manager { get; set; }

        internal MainWindowController() 
        {
            FeedCMD = new Command(Feed);
            JumpCMD = new Command(Jump);
            OpenGuideCMD = new Command(OpenGuide);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowController"/> class.
        /// Sets up commands and event handlers for the main window.
        /// </summary>
        internal MainWindowController(DeathManager? deathManager = null) : this()
        {
            Manager = deathManager ?? new(App.Zoo);
            Manager.GameEnded += OnGameEnded;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowController"/> class with the specified window.
        /// </summary>
        /// <param name="window">The main window associated with this controller.</param>
        public MainWindowController(Window window, DeathManager? deathManager = null) : this(deathManager)
        {
            _window = window;
            _window.Loaded += OnLoaded;
        }

        /// <summary>
        /// Handles the window's Loaded event.
        /// Starts the death manager and updates the current time periodically.
        /// </summary>
        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            Manager.Run();

            await Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    RaisePropertyChanged(nameof(CurrentTime));
                }
            });
        }

        /// <summary>
        /// Handles the GameEnded event when all animals have died.
        /// Prompts the user to restart or exit the application.
        /// </summary>
        private async void OnGameEnded(object sender, EventArgs e)
        {
            try
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    MessageBoxResult result = MessageBox.Show(
                        _window,
                        "The Game Has Ended!\nWould you like to play again?",
                        "Choose an option",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Opens the guide window.
        /// </summary>
        private void OpenGuide()
        {
            new GuideWindow().Show();
        }

        /// <summary>
        /// Simulates a time jump by affecting animals' life expectancy immediately.
        /// </summary>
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

        /// <summary>
        /// Feeds all animals in the zoo, increasing their health.
        /// </summary>
        private void Feed()
        {
            Manager.Stop();
            App.Zoo.FeedingService.Feed();
            Manager.Run();
        }
    }

}