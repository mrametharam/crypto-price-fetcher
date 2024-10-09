using CryptoPriceFetcher.Domain.Interfaces;

namespace CryptoPriceFetcherConsoleApp.ApiEndpoints;

public static class CryptoPricesEndpoints
{
    public static void RegisterCryptoPricesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/crypto-symbols", (ICryptoPriceService cryptoPriceService) =>
        {
            var retVal = cryptoPriceService.GetCryptoSymbols();

            return Results.Ok(retVal);
        });

        app.MapGet("/crypto-prices/by-symbol/{id?}", (string? id, ICryptoPriceService cryptoPriceService) =>
        {
            if (id is null)
            {
                return Results.BadRequest("A crypto symbol is required");
            }

            var retVal = cryptoPriceService.GetCryptoPrice(id);

            if (retVal is null)
            {
                return Results.NotFound($"Crypt {id} not found!");
            }

            return Results.Ok(retVal);
        });
    }
}
