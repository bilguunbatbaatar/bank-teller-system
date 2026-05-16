namespace server.Models;

/// <summary>
/// Теллерийн мэдээллийг төлөөлөх класс.
/// </summary>
public class Teller
{
    /// <summary>
    /// Систем дэх дахин давтагдашгүй ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Теллерийн дугаар.
    /// </summary>
    public int? TellerNumber { get; set; }

    /// <summary>
    /// Теллерийн нэр.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Кассын дугаар.
    /// </summary>
    public int CounterNo { get; set; }
}