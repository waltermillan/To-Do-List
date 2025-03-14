using API.Extensions;
using AutoMapper;
using Core.Interfases;
using Infrastructure.Configuration;
using Infrastructure.Data;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using Infrastructure.Logging;
using Serilog.Filters;

var builder = WebApplication.CreateBuilder(args);

// Limpiar otros proveedores de logging
builder.Logging.ClearProviders();

// Configurar filtros de logging para ASP.NET Core y Entity Framework Core
builder.Logging.AddFilter("Microsoft", LogLevel.Warning); // Para toda la parte de Microsoft
builder.Logging.AddFilter("System", LogLevel.Warning); // Para logs de System

// Configuración de Serilog para escribir solo en archivo
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/todolist-.log", rollingInterval: RollingInterval.Day) // Log en archivo diario
    .Filter.ByExcluding(Matching.FromSource("Microsoft.EntityFrameworkCore")) // Excluir logs de Entity Framework Core
    .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore")) // Excluir logs de ASP.NET Core
    .CreateLogger();

builder.Logging.AddSerilog(); // Agregar Serilog como proveedor de logs

// Otros servicios y configuraciones que ya tienes
var configuration = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<Core.Entities.Task, Core.Entities.Task>();
    cfg.CreateMap<Core.Entities.TaskHistory, Core.Entities.TaskHistory>();
    cfg.CreateMap<Core.Entities.State, Core.Entities.State>();
});

var mapper = configuration.CreateMapper();
builder.Services.AddSingleton(mapper);

// Cargar la configuración en AppConfig
var appConfig = AppConfig.GetInstance();
appConfig.ConnectionString = builder.Configuration.GetConnectionString("Connection");

// Agregar servicios al contenedor
builder.Services.ConfigureCors();
builder.Services.AddAplicacionServices();
builder.Services.AddControllers();

// Registrar DbContext con la cadena de conexión desde AppConfig
builder.Services.AddDbContext<TodoListContext>(options =>
{
    options.UseSqlServer(appConfig.ConnectionString); // Usamos la cadena de conexión desde AppConfig
});

// Registrar UnitOfWork en el contenedor de dependencias
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Configurar Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar ILoggingService para la inyección de dependencias
builder.Services.AddSingleton<ILoggingService, SerilogLoggingService>();

var app = builder.Build();

// Configurar el pipeline de solicitud HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = services.GetRequiredService<TodoListContext>();
        await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "Ocurrió un error durante la migración");
    }
}

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
