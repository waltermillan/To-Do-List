﻿using Core.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWork;

namespace API.Extensions;
public static class ApplicationServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration) =>
        services.AddCors(options =>
        {
            string[] verbs = configuration.GetSection("CorsSettings:Methods").Get<string[]>();

            var origins = configuration.GetSection("CorsSettings:Origins").Get<string[]>();

            options.AddPolicy("CorsPolicy", builder =>
                builder.WithOrigins(origins)  // Allows only of these developing origins
                    .WithMethods(verbs)
                    .AllowAnyHeader());
        });

    public static void AddAplicacionServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
