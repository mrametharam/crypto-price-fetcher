using CryptoPriceFetcher.Infrastructure.Configurations;
using CryptoPriceFetcher.Domain.Interfaces;
using CryptoPriceFetcher.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace CryptoPriceFetcher.Infrastructure.Repository;

public class CryptoPricesRepository(
    ILogger<CryptoPricesRepository> logger,
    IOptions<DbConnectionStringOptions> dbConnectionOptions
    ) : ICryptoPricesRepository
{
    private string SqlConnectionString
        => dbConnectionOptions.Value.DefaultConnection
            ?? throw new InvalidOperationException("No connection string!");

    public async Task SaveCryptoPrices(long startTime, IAsyncEnumerable<CryptoPriceRec> cryptoPrices)
    {
        using SqlConnection conn = new(SqlConnectionString);
        conn.Open();

        logger.LogInformation("Open connection to Db: {timeStamp}", Stopwatch.GetElapsedTime(startTime));

        await foreach (var rec in cryptoPrices)
        {
            logger.LogInformation($"{rec}");

            #region Save to Db
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

            logger.LogInformation("Inserted {Crypto}: {Result}", rec.Crypto, cmd.ExecuteNonQuery());
        }

        conn.Close();
        logger.LogInformation("Save all to Db: {timeStamp}", Stopwatch.GetElapsedTime(startTime));
        #endregion
    }
}
