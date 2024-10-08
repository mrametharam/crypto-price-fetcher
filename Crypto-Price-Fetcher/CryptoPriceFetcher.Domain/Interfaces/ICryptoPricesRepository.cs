using CryptoPriceFetcher.Domain.Entities;

namespace CryptoPriceFetcher.Domain.Interfaces;

public interface ICryptoPricesRepository
{
    Task SaveCryptoPrices(long startTime, IAsyncEnumerable<CryptoPriceRec> cryptoPrices);
}
