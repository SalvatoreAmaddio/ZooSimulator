using System.Windows;

namespace ZooSimulator.View
{
    public partial class WelcomeWindow : Window
    {
        public WelcomeWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(4000);
            Hide();
            MainWindow mainWindow = new MainWindow();
            Close();
            mainWindow.Show();
        }
    }
}