using System.ComponentModel.DataAnnotations;

namespace server.Models;

/// <summary>
/// Харилцагчийн дансны мэдээллийг төлөөлөх класс.
/// </summary>
public class Account
{
    /// <summary>
    /// Дансны систем дэх дахин давтагдашгүй ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Дансны дугаар (жишээ нь: ACC000001).
    /// </summary>
    public string AccountNumber { get; set; } = string.Empty;

    /// <summary>
    /// Данс эзэмшигчийн нэр.
    /// </summary>
    public string OwnerName { get; set; } = string.Empty;

    /// <summary>
    /// Дансны үлдэгдэл.
    /// </summary>
    public decimal Balance { get; set; } = 0;

    /// <summary>
    /// Данс идэвхтэй эсэх.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Данс үүсгэсэн огноо.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}