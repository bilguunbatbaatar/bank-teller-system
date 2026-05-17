using server.Enums;

namespace server.Models;

/// <summary>
/// Санхүүгийн гүйлгээний мэдээллийг төлөөлөх класс.
/// </summary>
public class Transaction
{
    /// <summary>
    /// Систем дэх дахин давтагдашгүй ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Шилжүүлэг хийж буй дансны дугаар.
    /// </summary>
    public string FromAccountNumber { get; set; } = string.Empty;

    /// <summary>
    /// Хүлээн авагч дансны дугаар.
    /// </summary>
    public string ToAccountNumber { get; set; } = string.Empty;

    /// <summary>
    /// Гүйлгээний дүн.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Гүйлгээний төрөл.
    /// </summary>
    public TransactionType Type { get; set; }

    /// <summary>
    /// Гүйлгээ хийгдсэн огноо.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}