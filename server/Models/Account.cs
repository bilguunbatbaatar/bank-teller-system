using System.ComponentModel.DataAnnotations;

namespace server.Models;

public class Account
{
    public int Id { get; set; }

    public string AccountNumber { get; set; } = string.Empty;

    public string OwnerName { get; set; } = string.Empty;

    public decimal Balance { get; set; } = 0;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}