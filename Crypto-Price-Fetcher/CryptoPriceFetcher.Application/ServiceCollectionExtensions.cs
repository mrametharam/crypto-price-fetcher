using CryptoPriceFetcher.Application.Services.Workers;
using CryptoPriceFetcher.Application.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace CryptoPriceFetcher.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MainWokerOptions>(configuration.GetSection(MainWokerOptions.Section));

        services.AddHostedService<MainWorker>();

        return services;
    }
}
