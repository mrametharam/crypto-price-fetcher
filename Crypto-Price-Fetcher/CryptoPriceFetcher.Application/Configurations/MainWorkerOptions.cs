namespace CryptoPriceFetcher.Application.Configurations;

public record MainWokerOptions
{
    public const string Section = "MainWorker";

    public int Interval { get; set; }
}
