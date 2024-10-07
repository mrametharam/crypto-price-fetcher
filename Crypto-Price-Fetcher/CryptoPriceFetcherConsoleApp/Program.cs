using CryptoPriceFetcherConsoleApp.Configuration.Options;
using CryptoPriceFetcherConsoleApp.Services.Workers;
using CryptoPriceFetcherConsoleApp.Data;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .Configure<CryptoApiOptions>(
        builder.Configuration.GetSection(CryptoApiOptions.Section)
        );

builder.Services.AddHostedService<MainWorker>();
builder.Services.AddSingleton<CryptoDataRepository>();
builder.Services.AddSingleton<CryptoPricesRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.Run();

await Log.CloseAndFlushAsync();