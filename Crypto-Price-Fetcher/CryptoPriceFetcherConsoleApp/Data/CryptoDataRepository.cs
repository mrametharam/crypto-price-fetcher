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
            string retVal = options.Value.CryptoSymbolUrl 
                ?? throw new InvalidOperationException("No CryptoSymbolUrl configured!");

            return retVal;
        }
    }

    private string CryptoPriceUrl
    {
        get
        {
            string retVal = options.Value.CryptoPriceUrl 
                ?? throw new InvalidOperationException("No CryptoPriceUrl configured!");

            return retVal;
        }
    }

    private string ApiKey
    {
        get
        {
            string retVal = options.Value.ApiKey
                ?? throw new InvalidOperationException("No Crypto API key configured!");

            return retVal;
        }
    }

    internal async Task<CryptoResponseData?> FetchCryptoSumbols(long startTime)
    {
        CryptoResponseData? retVal = null;

        var httpClient = ConfigureHttpClient();

        // Call the API
        var response = await httpClient.GetAsync(CryptoSymbolUrl);

        logger.LogInformation("Completed API call: {timeStamp}", Stopwatch.GetElapsedTime(startTime));

        // Read the response
        var responseData = await response.Content.ReadAsStringAsync();

        logger.LogInformation("Read the response: {timeStamp}", Stopwatch.GetElapsedTime(startTime));

        // Print the status code if it was not successful
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Error: {ErrorCode}", response.StatusCode);
            return retVal;
        }

        // Deserialize the response
        retVal = JsonConvert.DeserializeObject<CryptoResponseData>(responseData);

        logger.LogInformation("Deserialized the data: {timeStamp}", Stopwatch.GetElapsedTime(startTime));

        logger.LogInformation("Status Code: {StatusCode}", response.StatusCode);

        // Print the symbols
        logger.LogInformation("Symbols: {SymbolsCount}", retVal?.Symbols.Length);

        return retVal;
    }

    internal async IAsyncEnumerable<CryptoPriceRec> FetchCryptoPrices(long startTime, CryptoResponseData? data)
    {
        List<CryptoPriceRec> retVal = [];
        int recs = 0;

        var httpClient = ConfigureHttpClient();

        var startTime2 = Stopwatch.GetTimestamp();

        // Get the price of each symbol
        foreach (var symbol in data!.Symbols)
        {
            var startTime3 = Stopwatch.GetTimestamp();

            // Call the API
            string url = $"{CryptoPriceUrl}?symbol={symbol}";
            var response2 = await httpClient.GetAsync(url);

            logger.LogInformation("Got the price of {symbol}: {timeStamp}", symbol, Stopwatch.GetElapsedTime(startTime3));

            // Read the response
            var responseData2 = await response2.Content.ReadAsStringAsync();

            // Print the status code if it was not successful
            if (!response2.IsSuccessStatusCode)
            {
                logger.LogWarning("Error ({symbol}): {StatusCode}... {timeStamp}", symbol, response2.StatusCode, Stopwatch.GetElapsedTime(startTime3));
                continue;
            }

            // Deserialize the response
            var data2 = JsonConvert.DeserializeObject<CryptoPriceResponseData>(responseData2);

            if (data2 is not null)
            {
                yield return new CryptoPriceRec
                {
                    Crypto = data2.Symbol,
                    Price = data2.Price,
                    TimeStamp = data2.DateTimeStamp
                };

                logger.LogInformation("Added {symbol} to the list: {timeStamp}", symbol, Stopwatch.GetElapsedTime(startTime3));
            }

            recs++;

            if (recs > 10)
            {
                break;
            }
        }

        logger.LogInformation("Fetched the prices: {timeStamp}", Stopwatch.GetElapsedTime(startTime2));

        // Print the symbols
        logger.LogInformation("Symbols: {SymbolsCount}", retVal.Count);
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
