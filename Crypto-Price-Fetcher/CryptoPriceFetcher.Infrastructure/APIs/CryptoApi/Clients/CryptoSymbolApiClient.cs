using CryptoPriceFetcher.Infrastructure.Configurations;
using CryptoPriceFetcher.Domain.Interfaces;
using CryptoPriceFetcher.Domain.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace CryptoPriceFetcher.Infrastructure.APIs.CryptoApi.Clients;

public class CryptoSymbolApiClient(
    ILogger<CryptoSymbolApiClient> logger,
    IOptions<CryptoApiOptions> cryptoApiOptions,
    IHttpClientFactory httpClientFactory
    ) : ApiClientBase(logger, httpClientFactory, cryptoApiOptions.Value.ApiKey),
    ICryptoSymbolApiClient
{
    public async Task<CryptoSymbolApiResponseDto?> FetchCryptoSumbols(long startTime)
    {
        var httpClient = ConfigureHttpClient();

        // Call the API
        var response = await httpClient.GetAsync(cryptoApiOptions.Value.CryptoSymbolUrl);

        logger.LogInformation("Completed API call: {timeStamp}", Stopwatch.GetElapsedTime(startTime));

        return await HandleApiResponse<CryptoSymbolApiResponseDto>(response);
    }
}
