using CryptoPriceFetcher.Infrastructure;
using CryptoPriceFetcher.Application;
using CryptoPriceFetcherConsoleApp;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.AddInfrastructureLogging(builder.Configuration);

// Configure CORS
builder.Services.ConfigureCors(builder.Configuration);

// Register services from different layers
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPresentationServices();


var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

// Configure the request pipeline
app.UseInfrastructureRequestLogging();
app.UseHttpsRedirection();

app.MapPresentationEndpoints();

app.Run();


// Ensure all log entries are flushed and the logging system is cleanly shutdown.
await Log.CloseAndFlushAsync();