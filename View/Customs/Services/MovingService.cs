using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace ZooSimulator.View.Customs.Services
{
    public class MovingService : IMovingService
    {
        private double originalLeft;
        private double originalTop;
        private CancellationTokenSource _cancellationTokenSource;

        private Control _control;
        public MovingService(Control _control)
        {
            this._control = _control;
            originalLeft = Canvas.GetLeft(_control);
            originalTop = Canvas.GetTop(_control);
        }

        /// <summary>
        /// This methods is invoke when an Animal dies while its moving.
        /// </summary>
        public void Stop()
        {
            _control.BeginAnimation(Canvas.LeftProperty, null);
            _control.BeginAnimation(Canvas.TopProperty, null);
        }

        public void Move(double newLeft, double newTop, double durationInSeconds)
        {
            // Get the current position, even if the control is being animated
            double currentLeft = _control.GetValue(Canvas.LeftProperty) as double? ?? originalLeft;
            double currentTop = _control.GetValue(Canvas.TopProperty) as double? ?? originalTop;

            // Animate movement to the new position
            DoubleAnimation moveLeft = new()
            {
                From = currentLeft,
                To = newLeft,
                Duration = TimeSpan.FromSeconds(durationInSeconds),
                EasingFunction = new SineEase(),  // Easing for smooth movement
                FillBehavior = FillBehavior.Stop  // Prevent holding the final position after the animation
            };

            DoubleAnimation moveTop = new()
            {
                From = currentTop,
                To = newTop,
                Duration = TimeSpan.FromSeconds(durationInSeconds),
                EasingFunction = new SineEase(),
                FillBehavior = FillBehavior.Stop  // Prevent holding the final position after the animation
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