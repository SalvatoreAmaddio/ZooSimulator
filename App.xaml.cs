using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using ZooSimulatorLibrary.Animals;
using ZooSimulatorLibrary.Zoo;
using ZooSimulatorLibrary.Zoo.DependencyRegistration;

namespace ZooSimulator
{
    public partial class App : Application
    {
        public static int NumOfAnimals = 5;
        public static Zoo? Zoo { get; private set; } = ZooServices.Provider.GetService<Zoo>();

        public App()
        {
        }

        protected async override void OnStartup(StartupEventArgs e)
        {
            if (Zoo == null) throw new NullReferenceException();
            Zoo.Animals = await AnimalGenerator.GenerateZooAsync(NumOfAnimals);
            base.OnStartup(e);
        }
    }
}
