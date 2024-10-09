using CryptoPriceFetcher.Infrastructure.Configurations;
using CryptoPriceFetcher.Domain.Interfaces;
using CryptoPriceFetcher.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Data.SqlClient;

namespace CryptoPriceFetcher.Infrastructure.Repository;

public class CryptoPricesRepository(
    ILogger<CryptoPricesRepository> logger,
    IOptions<DbConnectionStringOptions> dbConnectionOptions
    ) : ICryptoPricesRepository
{
    private string SqlConnectionString
        => dbConnectionOptions.Value.DefaultConnection
            ?? throw new InvalidOperationException("No connection string!");

    public async IAsyncEnumerable<CryptoSymbol> GetCryptoSymbols()
    {
        using var conn = await GetSqlConnection();

        string sql = CryptoPricesSql.GetCryptoSumbols;

        using SqlCommand cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.CommandType = System.Data.CommandType.Text;
        cmd.CommandTimeout = TimeSpan.FromSeconds(5).Seconds;

        using var rd = await cmd.ExecuteReaderAsync();

        while (await rd.ReadAsync())
        {
            CryptoSymbol retVal = new CryptoSymbol
            {
                Symbol = rd.GetString(rd.GetOrdinal("Crypto"))
            };

            yield return retVal;
        }

        logger.LogInformation("Done!");
    }

    public async IAsyncEnumerable<CryptoPriceRec> GetCryptoPrice(string cryptoSymbol)
    {
        using var conn = await GetSqlConnection();

        string sql = CryptoPricesSql.GetCryptoPriceBySymbol;

        using SqlCommand cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.CommandType = System.Data.CommandType.Text;
        cmd.CommandTimeout = TimeSpan.FromSeconds(5).Seconds;

        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@crypto", cryptoSymbol);

        using var rd = await cmd.ExecuteReaderAsync();

        while (await rd.ReadAsync())
        {
            CryptoPriceRec retVal = new CryptoPriceRec
            {
                Id = rd.GetGuid(rd.GetOrdinal("Id")),
                Crypto = rd.GetString(rd.GetOrdinal("Crypto")), 
                Price = (double) rd.GetDecimal(rd.GetOrdinal("Price")), 
                TimeStamp = rd.GetDateTime(rd.GetOrdinal("TimeStamp"))
            };

            yield return retVal;
        }

        logger.LogInformation("Done!");
    }

    public async Task SaveCryptoPrices(IAsyncEnumerable<CryptoPriceRec> cryptoPrices)
    {
        using var conn = await GetSqlConnection();

        await foreach (var rec in cryptoPrices)
        {
            logger.LogInformation($"{rec}");

            string sql = CryptoPricesSql.InsertRecord;

            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandTimeout = TimeSpan.FromSeconds(5).Seconds;

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Crypto", rec.Crypto);
            cmd.Parameters.AddWithValue("@Price", rec.Price);
            cmd.Parameters.AddWithValue("@TimeStamp", rec.TimeStamp);

            var result = await cmd.ExecuteNonQueryAsync();

            logger.LogInformation("Inserted {Crypto}: {Result}", rec.Crypto, result);
        }

        logger.LogInformation("Save all to Db.");
    }

    private async Task<SqlConnection> GetSqlConnection()
    {
        var retVal = new SqlConnection(SqlConnectionString);

        await retVal.OpenAsync();

        return retVal;
    }
}
