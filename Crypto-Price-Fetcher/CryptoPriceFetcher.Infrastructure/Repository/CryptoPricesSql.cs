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


    internal static string GetCryptoSumbols
        => @"
SELECT distinct Crypto
  From [CryptoPrices] with (nolock)
  Where 1 = 1
  Order by Crypto";


    internal static string GetCryptoPriceBySymbol
        => @"
Select *
  From CryptoPrices with (nolock)
  Where 1 = 1
    and Crypto = @crypto
  Order by TimeStamp desc";
}
