using CryptoPriceFetcherConsoleApp.Configurations;
using CryptoPriceFetcherConsoleApp.ApiEndpoints;

namespace CryptoPriceFetcherConsoleApp;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        return services;
    }

    public static IServiceCollection ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        var corsOptions = configuration
            .GetSection(CorsOptions.Section)
            .Get<CorsOptions>();

        services.AddCors((options) =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder => builder
                .WithOrigins(corsOptions!.AllowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod());
        });

        services.AddCors((options) =>
        {
            options.AddPolicy("AllowAnyOrigin",
                builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
        });

        return services;
    }

    public static IEndpointRouteBuilder MapPresentationEndpoints(this IEndpointRouteBuilder app)
    {
        app.RegisterCryptoPricesEndpoints();

        return app;
    }
}
