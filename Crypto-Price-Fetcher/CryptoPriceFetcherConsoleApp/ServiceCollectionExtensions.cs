using CryptoPriceFetcherConsoleApp.ApiEndpoints;

namespace CryptoPriceFetcherConsoleApp;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        return services;
    }

    public static IEndpointRouteBuilder MapPresentationEndpoints(this IEndpointRouteBuilder app)
    {
        app.RegisterCryptoPricesEndpoints();

        return app;
    }
}
