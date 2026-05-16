namespace server.Enums;

/// <summary>
/// Очерын тасалбарын төлөвийг илэрхийлэх жагсаалт.
/// </summary>
public enum TicketStatus
{
    /// <summary> Хүлээгдэж буй. </summary>
    Waiting = 0,
    /// <summary> Дуудагдсан. </summary>
    Called = 1,
    /// <summary> Үйлчилгээ дууссан. </summary>
    Completed = 2,
    /// <summary> Цуцлагдсан. </summary>
    Cancelled = 3
}