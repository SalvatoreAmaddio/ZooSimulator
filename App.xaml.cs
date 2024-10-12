using System.Windows;
using ZooSimulatorLibrary.Animals;
using ZooSimulatorLibrary.Zoo;

namespace ZooSimulator
{
    public partial class App : Application
    {
        public static int NumOfAnimals = 5;
        public static Zoo Zoo { get; private set; } = new();

        public App() 
        {
        }

        protected async override void OnStartup(StartupEventArgs e)
        {
            Zoo.Animals = await AnimalGenerator.GenerateZooAsync(NumOfAnimals);
            base.OnStartup(e);
        }
    }
}
