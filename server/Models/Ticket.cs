using server.Enums;

namespace server.Models;

public class Ticket
{
    public int Id { get; set; }

    public string Number { get; set; } = string.Empty;

    public TicketStatus Status { get; set; }
        = TicketStatus.Waiting;

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;

    public DateTime? CalledAt { get; set; }

    public DateTime? CompletedAt { get; set; }
}