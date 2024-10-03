namespace CryptoPriceFetcherConsoleApp.Models;

public class CryptoPriceRec
{
    public Guid Id { get; set; }
    public string? Crypto { get; set; } = string.Empty;
    public double? Price { get; set; }
    public DateTime? TimeStamp { get; set; }

    override public string ToString()
        => $"Crypto: {Crypto}, Price: {Price}, Timestamp: {TimeStamp}";
}