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
    private int WorkerInterval
    {
        get
        {
            int retVal = configuration
                .GetValue<int>("WorkerInterval");

            return retVal;
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Performing task...");

            var startTime = Stopwatch.GetTimestamp();

            CryptoResponseData? data = await cryptoDataRepository.FetchCryptoSumbols(startTime);

            if (data == null)
            {
                logger.LogError("No data");
                return;
            }

            var cryptoPrices = cryptoDataRepository.FetchCryptoPrices(startTime, data);

            await cryptoPricesRepository.SaveCryptoPrices(startTime, cryptoPrices);

            logger.LogInformation("All done!: {timeStamp}", Stopwatch.GetElapsedTime(startTime));

            await Task.Delay(TimeSpan.FromMinutes(WorkerInterval), stoppingToken);
        }
    }
}
