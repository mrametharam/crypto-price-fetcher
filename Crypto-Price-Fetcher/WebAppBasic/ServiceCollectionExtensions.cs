using WebAppBasic.Configurations;

namespace WebAppBasic;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configurations
        services.Configure<CryptoApiOptions>(configuration.GetSection(CryptoApiOptions.Section));

        return services;
    }
}
