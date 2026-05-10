namespace server.Models;

public class ExchangeRate
{
    public int Id { get; set; }

    public string CurrencyCode { get; set; } = string.Empty;

    public decimal BuyRate { get; set; }

    public decimal SellRate { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}