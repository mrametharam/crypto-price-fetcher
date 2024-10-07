namespace CryptoPriceFetcherConsoleApp.Configuration.Options;

public class CryptoApiOptions
{
    public const string Section = "CryptoApi";

    public string ApiKey { get; set; } = string.Empty;
    public string CryptoSymbolUrl { get; set; } = string.Empty;
    public string CryptoPriceUrl { get; set; } = string.Empty;
}
