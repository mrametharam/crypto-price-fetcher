using CryptoPriceFetcherConsoleApp.Models;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using Newtonsoft.Json;
using CryptoPriceFetcherConsoleApp.Services.Workers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<MainWorker>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();
