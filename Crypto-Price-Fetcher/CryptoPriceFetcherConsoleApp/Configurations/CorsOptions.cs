namespace CryptoPriceFetcherConsoleApp.Configurations;

public record CorsOptions
{
    public const string Section = "CorsSettings";

    public string[] AllowedOrigins { get; set; } = [];
}
