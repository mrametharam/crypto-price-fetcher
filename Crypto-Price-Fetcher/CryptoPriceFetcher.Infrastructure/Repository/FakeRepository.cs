using CryptoPriceFetcher.Domain.Entities;

namespace CryptoPriceFetcher.Infrastructure.Repository;

public class FakeRepository
{
    public async IAsyncEnumerable<CryptoSymbol> GetCryptoSymbols()
    {

        for (var i = 0; i < 10; i++)
        {
            await Task.Delay(500);
            yield return new CryptoSymbol { Symbol = $"Cryto-{i:0#}" };
        }
    }

    public async IAsyncEnumerable<CryptoPriceRec> GetCryptoPrice(string cryptoSymbol)
    {
        var rnd = new Random();

        for (var i = 0; i < 5; i++)
        {
            await Task.Delay(500);

            double price = rnd.NextDouble() * 1000;
            int secs = rnd.Next(0, 1000);

            yield return new CryptoPriceRec { Crypto = cryptoSymbol, Id = Guid.NewGuid(), Price = price, TimeStamp = DateTime.UtcNow.AddSeconds(-secs) };
        }
    }
}
