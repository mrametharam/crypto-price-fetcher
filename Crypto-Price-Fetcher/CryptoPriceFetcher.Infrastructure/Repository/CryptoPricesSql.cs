namespace CryptoPriceFetcher.Infrastructure.Repository;

internal class CryptoPricesSql
{
    internal static string InsertRecord
        => @"
INSERT INTO [CryptoPrices]
    ([Crypto], [Price], [TimeStamp])
--  OUTPUT INSERTED.*
  VALUES
    (@Crypto, @Price, @TimeStamp)";

}
