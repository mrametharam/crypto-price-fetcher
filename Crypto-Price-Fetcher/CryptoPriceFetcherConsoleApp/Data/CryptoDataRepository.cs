using CryptoPriceFetcherConsoleApp.Models;
using System.Diagnostics;
using Newtonsoft.Json;

namespace CryptoPriceFetcherConsoleApp.Data;

public class CryptoDataRepository(
    IConfiguration configuration
    )
{
    private string CryptoSymbolUrl
    {
        get
        {
            string retVal = configuration
                .GetValue<string>("CryptoApi:CryptoSymbolUrl")
                ?? throw new InvalidOperationException("No API key!");

            return retVal;
        }
    }

    private string CryptoPriceUrl
    {
        get
        {
            string retVal = configuration
                .GetValue<string>("CryptoApi:CryptoPriceUrl")
                ?? throw new InvalidOperationException("No API key!");

            return retVal;
        }
    }

    internal async Task<CryptoResponseData?> FetchCryptoSumbols(HttpClient httpClient, long startTime)
    {
        CryptoResponseData? retVal = null;

        // Call the API
        var response = await httpClient.GetAsync(CryptoSymbolUrl);

        Console.WriteLine($"Completed API call: {Stopwatch.GetElapsedTime(startTime)}");

        // Read the response
        var responseData = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Read the response: {Stopwatch.GetElapsedTime(startTime)}");

        // Print the status code if it was not successful
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}");
            return retVal;
        }

        // Deserialize the response
        retVal = JsonConvert.DeserializeObject<CryptoResponseData>(responseData);

        Console.WriteLine($"Deserialized the data: {Stopwatch.GetElapsedTime(startTime)}");

        Console.WriteLine($"Status Code: {response.StatusCode}");
        //Console.WriteLine(responseData);

        // Print the symbols
        Console.WriteLine($"Symbols: {retVal?.Symbols.Length}");

        return retVal;
    }

    internal async IAsyncEnumerable<CryptoPriceRec> FetchCryptoPrices(HttpClient httpClient, long startTime, CryptoResponseData? data)
    {
        List<CryptoPriceRec> retVal = [];
        int recs = 0;

        var startTime2 = Stopwatch.GetTimestamp();

        // Get the price of each symbol
        foreach (var symbol in data!.Symbols)
        {
            var startTime3 = Stopwatch.GetTimestamp();

            // Call the API
            string url = $"{CryptoPriceUrl}?symbol={symbol}";
            var response2 = await httpClient.GetAsync(url);

            Console.WriteLine($"Got the price of {symbol}: {Stopwatch.GetElapsedTime(startTime3)}");

            // Read the response
            var responseData2 = await response2.Content.ReadAsStringAsync();

            // Print the status code if it was not successful
            if (!response2.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error ({symbol}): {response2.StatusCode}... {Stopwatch.GetElapsedTime(startTime3)}");
                continue;
            }

            //Console.WriteLine(responseData2);

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

                Console.WriteLine($"Added {symbol} to the list: {Stopwatch.GetElapsedTime(startTime3)}");
            }

            //Console.WriteLine(data2?.ToString());
            recs++;

            if (recs > 10)
            {
                break;
            }
        }

        Console.WriteLine($"Fetched the prices: {Stopwatch.GetElapsedTime(startTime2)}");

        // Print the symbols
        Console.WriteLine($"Symbols: {retVal.Count}");

        //return retVal;
    }

}
