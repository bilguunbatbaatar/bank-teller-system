using server.Enums;

namespace server.Models;

/// <summary>
/// Очерын тасалбарын мэдээллийг төлөөлөх класс.
/// </summary>
public class Ticket
{
    /// <summary>
    /// Систем дэх дахин давтагдашгүй ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Тасалбарын дугаар (жишээ нь: A001).
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// Тасалбарын төлөв.
    /// </summary>
    public TicketStatus Status { get; set; } = TicketStatus.Waiting;

    /// <summary>
    /// Тасалбарыг дуудсан теллерийн дугаар.
    /// </summary>
    public int? TellerNumber { get; set; }

    /// <summary>
    /// Тасалбар үүсгэсэн огноо.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Теллер тасалбарыг дуудсан огноо.
    /// </summary>
    public DateTime? CalledAt { get; set; }

    /// <summary>
    /// Үйлчилгээ дууссан огноо.
    /// </summary>
    public DateTime? CompletedAt { get; set; }
}