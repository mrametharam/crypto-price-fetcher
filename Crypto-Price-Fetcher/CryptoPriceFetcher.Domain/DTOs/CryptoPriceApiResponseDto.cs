namespace CryptoPriceFetcher.Domain.DTOs;

public record CryptoPriceApiResponseDto
{
    public string Symbol { get; set; } = string.Empty;
    public double Price { get; set; }
    public int Timestamp { get; set; }

    public DateTime DateTimeStamp
        => DateTimeOffset.FromUnixTimeSeconds(Timestamp).DateTime;
}
