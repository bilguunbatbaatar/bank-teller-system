using System.ComponentModel.DataAnnotations;

namespace server.DTOs;

public class TransferRequest
{
    [Required]
    public string FromAccountNumber { get; set; }
        = string.Empty;

    [Required]
    public string ToAccountNumber { get; set; }
        = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }
}