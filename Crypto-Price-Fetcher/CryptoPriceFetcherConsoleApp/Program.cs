using CryptoPriceFetcherConsoleApp.Services.Workers;
using CryptoPriceFetcherConsoleApp.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<MainWorker>();
builder.Services.AddSingleton<CryptoDataRepository>();
builder.Services.AddSingleton<CryptoPricesRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();
