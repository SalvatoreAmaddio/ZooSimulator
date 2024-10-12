namespace ZooSimulator.View.Customs.Services
{
    public interface IMovingService
    {
        void Move(double newLeft, double newTop, double durationInSeconds);
        public void Stop();
    }
}