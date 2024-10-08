using CryptoPriceFetcher.Domain.DTOs;

namespace CryptoPriceFetcher.Domain.Interfaces;

public interface ICryptoSymbolApiClient
{
    Task<CryptoSymbolApiResponseDto?> FetchCryptoSumbols(long startTime);
}
