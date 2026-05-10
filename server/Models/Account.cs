namespace server.Models;

public class Account
{
    public int Id { get; set; }

    public string AccountNumber { get; set; } = string.Empty;

    public string OwnerName { get; set; } = string.Empty;

    public decimal Balance { get; set; }
}