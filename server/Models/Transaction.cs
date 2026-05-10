using server.Enums;

namespace server.Models;

public class Transaction
{
    public int Id { get; set; }

    public string FromAccountNumber { get; set; }
        = string.Empty;

    public string ToAccountNumber { get; set; }
        = string.Empty;

    public decimal Amount { get; set; }

    public TransactionType Type { get; set; }

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;
}