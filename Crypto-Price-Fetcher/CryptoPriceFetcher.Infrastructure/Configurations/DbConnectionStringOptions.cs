namespace CryptoPriceFetcher.Infrastructure.Configurations;

public record DbConnectionStringOptions
{
    public const string Section = "ConnectionStrings";

    public string DefaultConnection { get; set; } = string.Empty;
}
