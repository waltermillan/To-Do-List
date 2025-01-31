using API.Extensions;
using Core.Interfases;
using Infrastructure.Configuration;
using Infrastructure.Data;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configurar AutoMapper
builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());

// Cargar la configuración en AppConfig
var appConfig = AppConfig.GetInstance();
appConfig.ConnectionString = builder.Configuration.GetConnectionString("Connection");

// Agregar servicios al contenedor
builder.Services.ConfigureCors();
builder.Services.AddAplicacionServices();
builder.Services.AddControllers();

// Registrar DbContext con la cadena de conexión desde AppConfig
builder.Services.AddDbContext<Context>(options =>
{
    options.UseSqlServer(appConfig.ConnectionString); // Usamos la cadena de conexión desde AppConfig
});

// Registrar UnitOfWork en el contenedor de dependencias
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Configurar Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
        var context = services.GetRequiredService<Context>();
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
