namespace CryptoPriceFetcherConsoleApp;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        return services;
    }
}
