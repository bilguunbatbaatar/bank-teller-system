namespace server.Models;

/// <summary>
/// Валютын ханшийн мэдээллийг төлөөлөх класс.
/// </summary>
public class ExchangeRate
{
    /// <summary>
    /// Систем дэх дахин давтагдашгүй ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Валютын код (жишээ нь: USD, EUR).
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// Банкны авах ханш.
    /// </summary>
    public decimal BuyRate { get; set; }

    /// <summary>
    /// Банкны зарах ханш.
    /// </summary>
    public decimal SellRate { get; set; }

    /// <summary>
    /// Ханш хамгийн сүүлд шинэчлэгдсэн огноо.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}