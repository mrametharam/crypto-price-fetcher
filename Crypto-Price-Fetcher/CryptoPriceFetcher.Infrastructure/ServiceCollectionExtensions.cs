﻿using CryptoPriceFetcher.Infrastructure.APIs.CryptoApi.Clients;
using CryptoPriceFetcher.Infrastructure.Configurations;
using CryptoPriceFetcher.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;
using CryptoPriceFetcher.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace CryptoPriceFetcher.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DbConnectionStringOptions>(configuration.GetSection(DbConnectionStringOptions.Section));
        services.Configure<CryptoApiOptions>(configuration.GetSection(CryptoApiOptions.Section));

        services.AddSingleton<ICryptoPricesRepository, CryptoPricesRepository>();
        services.AddSingleton<ICryptoSymbolApiClient, CryptoSymbolApiClient>();
        services.AddSingleton<ICryptoPriceApiClient, CryptoPriceApiClient>();

        services.AddHttpClient();

        return services;
    }

    public static IHostBuilder UseInfrastructureLogging(this IHostBuilder hostBuilder, IConfiguration configuration)
    {
        hostBuilder.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });

        return hostBuilder;
    }

    public static IApplicationBuilder UseInfrastructureRequestLogging(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging();

        return app;
    }
}