using CryptoPriceFetcher.Infrastructure;
using WebAppBasic.ApiEndpoints;
using WebAppBasic;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.AddInfrastructureLogging(builder.Configuration);

builder.Services.ConfigureServices(builder.Configuration);


var app = builder.Build();

app.UseInfrastructureRequestLogging();

app.UseDefaultFiles();
app.UseStaticFiles();

app.RegisterConfigurationEndpoints();

app.Run();
