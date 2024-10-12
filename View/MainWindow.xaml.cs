using System.Windows;
using ZooSimulator.Controller;

namespace ZooSimulator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowController(this);
        }
    }
}