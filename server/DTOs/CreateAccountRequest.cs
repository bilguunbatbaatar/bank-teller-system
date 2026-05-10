using System.ComponentModel.DataAnnotations;

namespace server.DTOs;

public class CreateAccountRequest
{
    [Required]
    [StringLength(100)]
    [RegularExpression(@"^[a-zA-Z\s\-]+$")]
    public string OwnerName { get; set; } = string.Empty;
}