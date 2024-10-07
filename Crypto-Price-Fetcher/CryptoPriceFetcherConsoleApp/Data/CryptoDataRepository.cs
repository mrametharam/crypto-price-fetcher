using CryptoPriceFetcherConsoleApp.Configuration.Options;
using CryptoPriceFetcherConsoleApp.Models;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using Newtonsoft.Json;

namespace CryptoPriceFetcherConsoleApp.Data;

public class CryptoDataRepository(
    ILogger<CryptoDataRepository> logger,
    IOptions<CryptoApiOptions> options,
    IHttpClientFactory httpClientFactory
    )
{
    private string CryptoSymbolUrl
    {
        get
        {
            string retVal = options.Value.CryptoSymbolUrl;

            if (string.IsNullOrWhiteSpace(retVal))
            {
                throw new InvalidOperationException("No CryptoSymbolUrl configured!");
            }

            return retVal;
        }
    }

    private string CryptoPriceUrl
    {
        get
        {
            string retVal = options.Value.CryptoPriceUrl;

            if (string.IsNullOrWhiteSpace(retVal))
            {
                throw new InvalidOperationException("No CryptoPriceUrl configured!");
            }

            return retVal;
        }
    }

    private string ApiKey
    {
        get
        {
            string retVal = options.Value.ApiKey;

            if (string.IsNullOrWhiteSpace(retVal))
            {
                throw new InvalidOperationException("No Crypto API key configured!");
            }

            return retVal;
        }
    }

    private async Task<T?> HandleApiResponse<T>(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Error: {ErrorCode}", response.StatusCode);

            return default;
        }

        var responseData = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<T>(responseData);
    }

    internal async Task<CryptoResponseData?> FetchCryptoSumbols(long startTime)
    {
        var httpClient = ConfigureHttpClient();

        // Call the API
        var response = await httpClient.GetAsync(CryptoSymbolUrl);

        logger.LogInformation("Completed API call: {timeStamp}", Stopwatch.GetElapsedTime(startTime));

        return await HandleApiResponse<CryptoResponseData>(response);
    }

    internal async IAsyncEnumerable<CryptoPriceRec> FetchCryptoPrices(long startTime, CryptoResponseData? data)
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
            var response2 = await httpClient.GetAsync($"{CryptoPriceUrl}?symbol={symbol}");

            logger.LogInformation("Got the price of {symbol}: {timeStamp}", symbol, Stopwatch.GetElapsedTime(startTime3));

            var priceData = await HandleApiResponse<CryptoPriceResponseData>(response2);

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

    private HttpClient ConfigureHttpClient()
    {
        var retVal = httpClientFactory.CreateClient();
        retVal.DefaultRequestHeaders.Add("Accept", "application/json");
        retVal.DefaultRequestHeaders.Add("X-Api-Key", ApiKey);
        retVal.Timeout = TimeSpan.FromSeconds(10);

        return retVal;
    }
}
