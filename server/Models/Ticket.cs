namespace server.Models;

public class Ticket
{
    public int Id { get; set; }

    public string Number { get; set; } = string.Empty;

    public string Status { get; set; } = "Waiting";

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? CalledAt { get; set; }
}