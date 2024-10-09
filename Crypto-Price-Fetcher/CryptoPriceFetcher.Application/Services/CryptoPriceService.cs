using CryptoPriceFetcher.Domain.Interfaces;
using CryptoPriceFetcher.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace CryptoPriceFetcher.Application.Services;

public class CryptoPriceService(
    ILogger<CryptoPriceService> logger,
    ICryptoPricesRepository cryptoPricesRepository
    ) : ICryptoPriceService
{
    public IAsyncEnumerable<CryptoSymbol> GetCryptoSymbols()
    {
        return cryptoPricesRepository.GetCryptoSymbols();
    }

    public IAsyncEnumerable<CryptoPriceRec> GetCryptoPrice(string cryptoSymbol)
    {
        return cryptoPricesRepository.GetCryptoPrice(cryptoSymbol);
    }
}
