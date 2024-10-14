///The MovingService class is a vital component for animating and controlling the movement of animal 
///representations within the WPF zoo simulator. By providing clear methods to start and stop movements 
///with smooth animations, it enhances the interactivity and visual appeal of the application. 
///Proper implementation and management of this service ensure a responsive and engaging user experience.

using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace ZooSimulator.View.Customs.Services
{
    /// <summary>
    /// Provides functionality to animate the movement of a WPF <see cref="Control"/> within a <see cref="Canvas"/>.
    /// Implements the <see cref="IMovingService"/> interface to control the movement and stopping of the control.
    /// </summary>
    public class MovingService : IMovingService
    {
        private double originalLeft;
        private double originalTop;
        private CancellationTokenSource _cancellationTokenSource;

        private Control _control;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovingService"/> class for the specified control.
        /// </summary>
        /// <param name="control">The WPF <see cref="Control"/> to be moved.</param>
        public MovingService(Control control)
        {
            _control = control;
            originalLeft = Canvas.GetLeft(_control);
            originalTop = Canvas.GetTop(_control);
        }

        /// <summary>
        /// Stops any ongoing movement animations of the associated control.
        /// This method is invoked when an animal dies while its control is moving.
        /// </summary>
        public void Stop()
        {
            // Remove any animations on Canvas.Left and Canvas.Top properties
            _control.BeginAnimation(Canvas.LeftProperty, null);
            _control.BeginAnimation(Canvas.TopProperty, null);
        }

        /// <summary>
        /// Initiates the movement of the associated control to a new position over a specified duration.
        /// </summary>
        /// <param name="newLeft">The target left position (X-axis) for the control.</param>
        /// <param name="newTop">The target top position (Y-axis) for the control.</param>
        /// <param name="durationInSeconds">The duration of the movement in seconds.</param>
        public void Move(double newLeft, double newTop, double durationInSeconds)
        {
            // Get the current position, even if the control is being animated
            double currentLeft = _control.GetValue(Canvas.LeftProperty) as double? ?? originalLeft;
            double currentTop = _control.GetValue(Canvas.TopProperty) as double? ?? originalTop;

            // Create animations for the left and top properties
            DoubleAnimation moveLeft = new()
            {
                From = currentLeft,
                To = newLeft,
                Duration = TimeSpan.FromSeconds(durationInSeconds),
                EasingFunction = new SineEase(),  // Provides smooth easing for the animation
                FillBehavior = FillBehavior.Stop    // Prevents the animation from holding the final position
            };

            DoubleAnimation moveTop = new()
            {
                From = currentTop,
                To = newTop,
                Duration = TimeSpan.FromSeconds(durationInSeconds),
                EasingFunction = new SineEase(),
                FillBehavior = FillBehavior.Stop
            };

            // Apply the animations to the Canvas.Left and Canvas.Top attached properties
            _control.BeginAnimation(Canvas.LeftProperty, moveLeft);
            _control.BeginAnimation(Canvas.TopProperty, moveTop);

            // Update the original position after the animation completes
            moveLeft.Completed += (s, e) => Canvas.SetLeft(_control, newLeft);
            moveTop.Completed += (s, e) => Canvas.SetTop(_control, newTop);
        }
    }

}