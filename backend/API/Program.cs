using API.Extensions;
using AutoMapper;
using Core.Interfaces;
using Infrastructure.Configuration;
using Infrastructure.Data;
using Infrastructure.Logging;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Filters;

var builder = WebApplication.CreateBuilder(args);

var appName = builder.Configuration["SystemName:AppName"] ?? "TodoListAPI";

// Clean up other logging providers
builder.Logging.ClearProviders();

// Configurar filtros de logging para ASP.NET Core y Entity Framework Core
builder.Logging.AddFilter("Microsoft", LogLevel.Warning); // Filters for the entire Microsoft part
builder.Logging.AddFilter("System", LogLevel.Warning); // Filters for System logs

// Configuring Serilog to write to file only
Log.Logger = new LoggerConfiguration()
    .WriteTo.File($"logs/{appName}-.log", rollingInterval: RollingInterval.Day) // Log in daily archive
    .Filter.ByExcluding(Matching.FromSource("Microsoft.EntityFrameworkCore")) // Exclude logs from Entity Framework Core
    .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore")) // Exclude logs from ASP.NET Core
    .CreateLogger();

builder.Logging.AddSerilog(); // Add Serilog as a log provider

// Other services and configurations you already have
var configuration = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<Core.Entities.Task, Core.Entities.Task>();
    cfg.CreateMap<Core.Entities.TaskHistory, Core.Entities.TaskHistory>();
    cfg.CreateMap<Core.Entities.State, Core.Entities.State>();
    cfg.CreateMap<Core.Entities.User, Core.Entities.User>();
});

var mapper = configuration.CreateMapper();
builder.Services.AddSingleton(mapper);

// Load configuration in AppConfig
var appConfig = AppConfig.GetInstance();
appConfig.ConnectionString = builder.Configuration.GetConnectionString("Connection");

// Adding services to the container
builder.Services.ConfigureCors(builder.Configuration);
builder.Services.AddAplicacionServices();
builder.Services.AddControllers();

// Register DbContext with connection string from AppConfig
builder.Services.AddDbContext<TodoListContext>(options =>
{
    options.UseSqlServer(appConfig.ConnectionString); // Use connection string from AppConfig
});

// Register UnitOfWork in the dependency container
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register ILoggingService for dependency injection
builder.Services.AddSingleton<ILoggingService, SerilogLoggingService>();

var app = builder.Build();

// Configuring the HTTP request pipeline
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
        logger.LogError(ex, "An error occurred during migration");
    }
}

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
