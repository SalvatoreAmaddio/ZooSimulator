///The IMovingService interface provides a clear and concise contract for managing the movement 
///of animals within your zoo simulator's UI. By defining essential methods for initiating and 
///stopping movement, it facilitates the creation of flexible and maintainable movement services. 
///Proper implementation of this interface will enhance the interactivity and realism of the 
///animal behaviors in your application.
namespace ZooSimulator.View.Customs.Services
{
    /// <summary>
    /// Defines methods for controlling the movement of an animal within the zoo simulator.
    /// </summary>
    public interface IMovingService
    {
        /// <summary>
        /// Initiates the movement of an animal to a new position over a specified duration.
        /// </summary>
        /// <param name="newLeft">The new left position (X-axis) for the animal.</param>
        /// <param name="newTop">The new top position (Y-axis) for the animal.</param>
        /// <param name="durationInSeconds">The duration of the movement in seconds.</param>
        void Move(double newLeft, double newTop, double durationInSeconds);

        /// <summary>
        /// Stops the ongoing movement of the animal.
        /// </summary>
        void Stop();
    }
}