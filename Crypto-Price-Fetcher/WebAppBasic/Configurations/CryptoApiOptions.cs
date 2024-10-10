namespace WebAppBasic.Configurations;

public record CryptoApiOptions
{
    public const string Section = "CryptoApi";

    public string CryptoSymbolsUrl { get; set; } = string.Empty;
    public string CryptoPriceUrl { get; set; } = string.Empty;
}
