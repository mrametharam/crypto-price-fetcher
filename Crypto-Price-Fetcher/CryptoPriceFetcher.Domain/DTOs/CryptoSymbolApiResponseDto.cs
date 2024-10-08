namespace CryptoPriceFetcher.Domain.DTOs;

public class CryptoSymbolApiResponseDto
{
    public string[] Symbols { get; set; } = [];
    public int Timestamp { get; set; }

    public DateTime DateTimeStamp
        => DateTimeOffset.FromUnixTimeSeconds(Timestamp).DateTime;

}
