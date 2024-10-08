using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CryptoPriceFetcher.Infrastructure.APIs;

public abstract class ApiClientBase(
    ILogger logger, 
    IHttpClientFactory httpClientFactory, 
    string apiKey)
{
    protected HttpClient ConfigureHttpClient()
    {
        var retVal = httpClientFactory.CreateClient();
        retVal.DefaultRequestHeaders.Add("Accept", "application/json");
        retVal.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
        retVal.Timeout = TimeSpan.FromSeconds(10);

        return retVal;
    }

    protected async Task<T?> HandleApiResponse<T>(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Error: {ErrorCode}", response.StatusCode);

            return default;
        }

        var responseData = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<T>(responseData);
    }
}
