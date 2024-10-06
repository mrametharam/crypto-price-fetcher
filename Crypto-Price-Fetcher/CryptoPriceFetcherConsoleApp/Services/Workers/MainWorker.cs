using CryptoPriceFetcherConsoleApp.Models;
using CryptoPriceFetcherConsoleApp.Data;
using System.Diagnostics;

namespace CryptoPriceFetcherConsoleApp.Services.Workers;

public class MainWorker(
    ILogger<MainWorker> logger,
    IConfiguration configuration,
    CryptoDataRepository cryptoDataRepository,
    CryptoPricesRepository cryptoPricesRepository
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Performing task...");

            var startTime = Stopwatch.GetTimestamp();

            var httpClient = ConfigureHttpClient();

            CryptoResponseData? data = await cryptoDataRepository.FetchCryptoSumbols(httpClient, startTime);

            if (data == null)
            {
                logger.LogError("No data");
                return;
            }

            var cryptoPrices = cryptoDataRepository.FetchCryptoPrices(httpClient, startTime, data);

            await cryptoPricesRepository.SaveCryptoPrices(startTime, cryptoPrices);

            logger.LogInformation("All done!: {timeStamp}", Stopwatch.GetElapsedTime(startTime));

            await Task.Delay(TimeSpan.FromMinutes(WorkerInterval), stoppingToken);
        }
    }

    private int WorkerInterval
    {
        get
        {
            int retVal = configuration
                .GetValue<int>("WorkerInterval");

            return retVal;
        }
    }

    private string ApiKey
    {
        get
        {
            string retVal = configuration
                .GetValue<string>("CryptoApi:ApiKey")
                ?? throw new InvalidOperationException("No API key!");

            return retVal;
        }
    }

    internal HttpClient ConfigureHttpClient()
    {
        var retVal = new HttpClient();
        retVal.DefaultRequestHeaders.Add("Accept", "application/json");
        retVal.DefaultRequestHeaders.Add("X-Api-Key", ApiKey);
        retVal.Timeout = TimeSpan.FromSeconds(10);

        return retVal;
    }
}
