using CryptoPriceFetcher.Domain.Entities;

namespace CryptoPriceFetcher.Domain.Interfaces;

public interface ICryptoPriceService
{
    IAsyncEnumerable<CryptoSymbol> GetCryptoSymbols();
    IAsyncEnumerable<CryptoPriceRec> GetCryptoPrice(string cryptoSymbol);
}
