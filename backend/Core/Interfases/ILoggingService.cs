namespace Core.Interfases
{
    public interface ILoggingService
    {
        void LogInformation(string message);
        void LogError(string message, Exception exception);
    }
}
