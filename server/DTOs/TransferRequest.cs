using System.ComponentModel.DataAnnotations;

namespace server.DTOs;

/// <summary>
/// Данс хооронд шилжүүлэг хийх хүсэлтийн мэдээллийг дамжуулах класс.
/// </summary>
public class TransferRequest
{
    /// <summary>
    /// Шилжүүлэг хийх эх сурвалж дансны дугаар.
    /// </summary>
    [Required]
    public string FromAccountNumber { get; set; } = string.Empty;

    /// <summary>
    /// Шилжүүлэг хүлээн авах дансны дугаар.
    /// </summary>
    [Required]
    public string ToAccountNumber { get; set; } = string.Empty;

    /// <summary>
    /// Шилжүүлгийн дүн.
    /// </summary>
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }
}