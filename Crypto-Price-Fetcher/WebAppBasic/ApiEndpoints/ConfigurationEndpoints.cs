using Microsoft.Extensions.Options;
using WebAppBasic.Configurations;

namespace WebAppBasic.ApiEndpoints;

public static class ConfigurationEndpoints
{
    public static void RegisterConfigurationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/configurations", (IOptions<CryptoApiOptions> cryptoApiOptions) =>
        {
            var retVal = cryptoApiOptions.Value;

            return Results.Ok(retVal);
        });
    }
}
