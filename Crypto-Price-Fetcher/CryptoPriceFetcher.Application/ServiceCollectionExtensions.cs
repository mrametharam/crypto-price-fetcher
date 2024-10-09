using CryptoPriceFetcher.Application.Services.Workers;
using CryptoPriceFetcher.Application.Configurations;
using Microsoft.Extensions.DependencyInjection;
using CryptoPriceFetcher.Application.Services;
using CryptoPriceFetcher.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CryptoPriceFetcher.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configurations
        services.Configure<MainWokerOptions>(configuration.GetSection(MainWokerOptions.Section));

        // Services
        services.AddSingleton<ICryptoPriceService, CryptoPriceService>();

        // Hosted Services
        //services.AddHostedService<MainWorker>();

        return services;
    }
}
