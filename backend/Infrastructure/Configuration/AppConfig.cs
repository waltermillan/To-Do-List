
namespace Infrastructure.Configuration;

public class AppConfig
{
    private static AppConfig? _instance;
    private static readonly Lock _lock = new Lock();

    // Propiedades de configuración global
    public string? ConnectionString { get; set; }

    // Constructor privado para evitar la instanciación externa
    private AppConfig() { }

    // Método para obtener la instancia única de AppConfig
    public static AppConfig GetInstance()
    {
        lock (_lock)
        {
            if (_instance == null)
            {
                _instance = new AppConfig();
            }
            return _instance;
        }
    }
}
