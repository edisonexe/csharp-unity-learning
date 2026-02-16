namespace _Scripts.Interfaces
{
    public interface ILoggerService
    {
        public void Log(string message);
        public void Warn(string message);
        public void Error(string message);
    }
}