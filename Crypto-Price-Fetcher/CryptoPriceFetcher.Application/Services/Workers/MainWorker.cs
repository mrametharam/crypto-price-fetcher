using CryptoPriceFetcher.Application.Configurations;
using CryptoPriceFetcher.Domain.Interfaces;
using CryptoPriceFetcher.Domain.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace CryptoPriceFetcher.Application.Services.Workers;

public class MainWorker(
    ILogger<MainWorker> logger,
    IOptions<MainWokerOptions> workerOptions,
    ICryptoSymbolApiClient cryptoSymbolApiClient,
    ICryptoPriceApiClient cryptoPriceApiClient,
    ICryptoPricesRepository cryptoPricesRepository
    ) : BackgroundService
{
    #region Private Properties
    private int WorkerInterval
        => workerOptions.Value.Interval;
    #endregion

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Performing task...");

            var startTime = Stopwatch.GetTimestamp();

            CryptoSymbolApiResponseDto? data = await cryptoSymbolApiClient.FetchCryptoSumbols(startTime);

            if (data is not null)
            {
                var cryptoPrices = cryptoPriceApiClient.FetchCryptoPrices(startTime, data);

                await cryptoPricesRepository.SaveCryptoPrices(startTime, cryptoPrices);

                logger.LogInformation("All done!: {timeStamp}", Stopwatch.GetElapsedTime(startTime));
            }
            else
            {
                logger.LogError("No data");
            }

            await Task.Delay(TimeSpan.FromMinutes(WorkerInterval), stoppingToken);
        }
    }
}
