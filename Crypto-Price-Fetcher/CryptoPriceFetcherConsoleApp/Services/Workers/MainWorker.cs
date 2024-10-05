using CryptoPriceFetcherConsoleApp.Models;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using Newtonsoft.Json;

namespace CryptoPriceFetcherConsoleApp.Services.Workers;

public class MainWorker(
    IConfiguration configuration
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("Performing task...");

            var startTime = Stopwatch.GetTimestamp();

            var httpClient = ConfigureHttpClient();

            CryptoResponseData? data = await FetchCryptoSumbols(httpClient, startTime);

            if (data == null)
            {
                Console.WriteLine("No data");
                return;
            }

            var cryptoPrices = FetchCryptoPrices2(httpClient, startTime, data);

            await SaveCryptoPrices(startTime, cryptoPrices);

            Console.WriteLine($"All done!: {Stopwatch.GetElapsedTime(startTime)}");

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

    internal HttpClient ConfigureHttpClient()
    {
        var retVal = new HttpClient();
        retVal.DefaultRequestHeaders.Add("Accept", "application/json");
        retVal.DefaultRequestHeaders.Add("X-Api-Key", ApiKey);
        retVal.Timeout = TimeSpan.FromSeconds(10);

        return retVal;
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

    internal async Task<List<CryptoPriceRec>> FetchCryptoPrices(HttpClient httpClient, long startTime, CryptoResponseData? data)
    {
        List<CryptoPriceRec> retVal = [];
        int recs = 0;

        var startTime2 = Stopwatch.GetTimestamp();

        // Get the price of each symbol
        foreach (var symbol in data!.Symbols)
        {
            var startTime3 = Stopwatch.GetTimestamp();

            // Call the API
            var response2 = await httpClient.GetAsync($"{CryptoPriceUrl}?symbol={symbol}");

            Console.WriteLine($"Got the price of {symbol}: {Stopwatch.GetElapsedTime(startTime3)}");

            // Read the response
            var responseData2 = await response2.Content.ReadAsStringAsync();

            // Print the status code if it was not successful
            if (!response2.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response2.StatusCode}");
                return retVal;
            }

            //Console.WriteLine(responseData2);

            // Deserialize the response
            var data2 = JsonConvert.DeserializeObject<CryptoPriceResponseData>(responseData2);

            if (data2 is not null)
            {
                retVal.Add(new CryptoPriceRec
                {
                    Crypto = data2.Symbol,
                    Price = data2.Price,
                    TimeStamp = data2.DateTimeStamp
                });
            }

            Console.WriteLine($"Added {symbol} to the list: {Stopwatch.GetElapsedTime(startTime3)}");

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

        return retVal;
    }

    internal async IAsyncEnumerable<CryptoPriceRec> FetchCryptoPrices2(HttpClient httpClient, long startTime, CryptoResponseData? data)
    {
        List<CryptoPriceRec> retVal = [];
        int recs = 0;

        var startTime2 = Stopwatch.GetTimestamp();

        // Get the price of each symbol
        foreach (var symbol in data!.Symbols)
        {
            var startTime3 = Stopwatch.GetTimestamp();

            // Call the API
            var response2 = await httpClient.GetAsync($"https://api.api-ninjas.com/v1/cryptoprice?symbol={symbol}");

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

    internal async Task SaveCryptoPrices(long startTime, IAsyncEnumerable<CryptoPriceRec> cryptoPrices)
    {
        string connStr = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("No connection string!");

        using SqlConnection conn = new(connStr);
        conn.Open();
        Console.WriteLine($"Open connection to Db: {Stopwatch.GetElapsedTime(startTime)}");

        await foreach (var rec in cryptoPrices)
        {
            Console.WriteLine($"{rec}");

            #region Save to Db
            // Open connection to Db

            string sql = @"
INSERT INTO [CryptoPrices]
    ([Crypto], [Price], [TimeStamp])
--  OUTPUT INSERTED.*
  VALUES
    (@Crypto, @Price, @TimeStamp)";

            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandTimeout = TimeSpan.FromSeconds(5).Seconds;

            cmd.Parameters.Clear();

            cmd.Parameters.AddWithValue("@Crypto", rec.Crypto);
            cmd.Parameters.AddWithValue("@Price", rec.Price);
            cmd.Parameters.AddWithValue("@TimeStamp", rec.TimeStamp);

            Console.WriteLine($"Inserted {rec.Crypto}: {cmd.ExecuteNonQuery()}");
        }

        conn.Close();
        Console.WriteLine($"Save all to Db: {Stopwatch.GetElapsedTime(startTime)}");
        #endregion
    }
}
