using CryptoPriceFetcher.Domain.Entities;
using CryptoPriceFetcher.Domain.DTOs;

namespace CryptoPriceFetcher.Domain.Interfaces;

public interface ICryptoPriceApiClient
{
    IAsyncEnumerable<CryptoPriceRec> FetchCryptoPrices(long startTime, CryptoSymbolApiResponseDto? data);
}
