using CryptoPriceFetcher.Infrastructure.Configurations;
using CryptoPriceFetcher.Domain.Interfaces;
using CryptoPriceFetcher.Domain.Entities;
using CryptoPriceFetcher.Domain.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace CryptoPriceFetcher.Infrastructure.APIs.CryptoApi.Clients;

public class CryptoPriceApiClient(
    ILogger<CryptoPriceApiClient> logger,
    IOptions<CryptoApiOptions> cryptoApiOptions,
    IHttpClientFactory httpClientFactory
    ) : ApiClientBase(logger, httpClientFactory, cryptoApiOptions.Value.ApiKey),
    ICryptoPriceApiClient
{
    public async IAsyncEnumerable<CryptoPriceRec> FetchCryptoPrices(long startTime, CryptoSymbolApiResponseDto? data)
    {
        if (data == null)
        {
            yield break;
        }

        var httpClient = ConfigureHttpClient();
        var startTime2 = Stopwatch.GetTimestamp();

        int recCount = 0;

        // Get the price of each symbol
        foreach (var symbol in data!.Symbols)
        {
            var startTime3 = Stopwatch.GetTimestamp();

            // Call the API
            var response = await httpClient.GetAsync($"{cryptoApiOptions.Value.CryptoPriceUrl}?symbol={symbol}");

            logger.LogInformation("Got the price of {symbol}: {timeStamp}", symbol, Stopwatch.GetElapsedTime(startTime3));

            var priceData = await HandleApiResponse<CryptoPriceApiResponseDto>(response);

            if (priceData is not null)
            {
                yield return new CryptoPriceRec
                {
                    Crypto = priceData.Symbol,
                    Price = priceData.Price,
                    TimeStamp = priceData.DateTimeStamp
                };

                logger.LogInformation("Added {symbol} to the list: {timeStamp}", symbol, Stopwatch.GetElapsedTime(startTime3));
            }

            recCount++;

            if (recCount > 10)
            {
                break;
            }
        }

        logger.LogInformation("Fetched the prices: {timeStamp}", Stopwatch.GetElapsedTime(startTime2));
    }
}
