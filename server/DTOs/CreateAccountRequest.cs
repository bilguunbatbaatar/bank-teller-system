using System.ComponentModel.DataAnnotations;

namespace server.DTOs;

/// <summary>
/// Шинэ данс үүсгэх хүсэлтийн мэдээллийг дамжуулах класс.
/// </summary>
public class CreateAccountRequest
{
    /// <summary>
    /// Данс эзэмшигчийн нэр.
    /// </summary>
    [Required]
    [StringLength(100)]
    [RegularExpression(@"^[a-zA-Z\s\-]+$")]
    public string OwnerName { get; set; } = string.Empty;
}