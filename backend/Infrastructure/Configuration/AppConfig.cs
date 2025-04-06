
namespace Infrastructure.Configuration;

public class AppConfig
{
    private static AppConfig? _instance;
    private static readonly Lock _lock = new Lock();

    // Global configuration properties
    public string? ConnectionString { get; set; }

    // Private builder to avoid external financing
    private AppConfig() { }

    // Method to get the single instance of AppConfig
    public static AppConfig GetInstance()
    {
        lock (_lock)
        {
            // SINGLETON PATTERN: The Singleton pattern ensures that a class has only one instance throughout
            // the application and provides a global access point to that instance.
            if (_instance is null)
                _instance = new AppConfig();

            return _instance;
        }
    }
}
