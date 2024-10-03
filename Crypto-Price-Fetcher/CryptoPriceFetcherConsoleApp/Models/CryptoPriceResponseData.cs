namespace CryptoPriceFetcherConsoleApp.Models;

public class CryptoPriceResponseData
{
    public string Symbol { get; set; } = string.Empty;
    public double Price { get; set; }
    public int Timestamp { get; set; }

    public DateTime DateTimeStamp
        => DateTimeOffset.FromUnixTimeSeconds(Timestamp).DateTime;

    override public string ToString()
        => $"Symbol: {Symbol}, Price: {Price}, Timestamp: {DateTimeStamp}";
}
