using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using server.Services;
using server.Data;
using server.Enums;
using server.Models;

namespace server.Controllers;

/// <summary>
/// Очерын тасалбар болон теллерийн үйлчилгээг удирдах контроллер.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TicketController : ControllerBase
{
    private static readonly Lock _ticketLock =
        new();

    private readonly
        QueueSocketService
        _socket;

    private readonly
        AppDbContext
        _context;

    /// <summary>
    /// TicketController-ийн шинэ хувилбарыг үүсгэнэ.
    /// </summary>
    /// <param name="context">Өгөгдлийн сангийн контекст.</param>
    /// <param name="socket">Очерын мэдээллийг TCP-ээр дамжуулах сервис.</param>
    public TicketController(
        AppDbContext context,
        QueueSocketService socket)
    {
        _context = context;
        _socket = socket;
    }

    /// <summary>
    /// Шинэ очерын тасалбар үүсгэнэ.
    /// </summary>
    /// <returns>Үүсгэсэн тасалбарын мэдээлэл.</returns>
    [HttpPost]
    public ActionResult<Ticket>
        Create()
    {
        lock (_ticketLock)
        {
            var ticket =
                new Ticket();

            _context.Tickets
                .Add(ticket);

            _context.SaveChanges();
            //ciruclar when it A999 restart from A001
            var nextNumber =
    ((_context.Tickets.Count() % 999) + 1);
            ticket.Number =
                $"A{ticket.Id:000}";

            _context.SaveChanges();

            return ticket;
        }
    }

    /// <summary>
    /// Дараагийн хүлээгдэж буй тасалбарыг теллерт хуваарилна.
    /// </summary>
    /// <param name="tellerNumber">Тасалбар дуудаж буй теллерийн дугаар.</param>
    /// <returns>Дуудагдсан тасалбарын мэдээлэл.</returns>
    [HttpPost("next/{tellerNumber}")]
    public ActionResult<Ticket>
        Next(int tellerNumber)
    {
        Ticket? ticket;

        lock (_ticketLock)
        {
            var activeTicket =
                _context.Tickets
                    .Any(x =>
                        x.Status ==
                        TicketStatus.Called &&
                        x.TellerNumber ==
                        tellerNumber);

            if (activeTicket)
            {
                return Conflict(
                    $"Teller {tellerNumber} already has an active ticket.");
            }

            ticket =
                _context.Tickets
                    .OrderBy(x => x.Id)
                    .FirstOrDefault(x =>
                        x.Status ==
                        TicketStatus.Waiting);

            if (ticket == null)
            {
                return NotFound();
            }

            ticket.Status =
                TicketStatus.Called;

            ticket.TellerNumber =
                tellerNumber;

            ticket.CalledAt =
                DateTime.UtcNow;

            _context.SaveChanges();
        }

        BroadcastQueue();

        return ticket;
    }

    /// <summary>
    /// Тасалбарын үйлчилгээг амжилттай дууссан төлөвт оруулна.
    /// </summary>
    /// <param name="tellerNumber">Теллерийн дугаар.</param>
    /// <returns>Дууссан тасалбарын мэдээлэл.</returns>
    [HttpPost("complete/{tellerNumber}")]
    public ActionResult<Ticket>
        Complete(int tellerNumber)
    {
        Ticket? ticket;

        lock (_ticketLock)
        {
            ticket =
                _context.Tickets
                    .FirstOrDefault(x =>
                        x.Status ==
                        TicketStatus.Called &&
                        x.TellerNumber ==
                        tellerNumber);

            if (ticket == null)
            {
                return NotFound();
            }

            ticket.Status =
                TicketStatus.Completed;

            ticket.CompletedAt =
                DateTime.UtcNow;

            _context.SaveChanges();
        }

        BroadcastQueue();

        return ticket;
    }

    /// <summary>
    /// Тасалбарын үйлчилгээг цуцалсан төлөвт оруулна.
    /// </summary>
    /// <param name="tellerNumber">Теллерийн дугаар.</param>
    /// <returns>Цуцлагдсан тасалбарын мэдээлэл.</returns>
    [HttpPost("cancel/{tellerNumber}")]
    public ActionResult<Ticket>
        Cancel(int tellerNumber)
    {
        Ticket? ticket;

        lock (_ticketLock)
        {
            ticket =
                _context.Tickets
                    .FirstOrDefault(x =>
                        x.Status ==
                        TicketStatus.Called &&
                        x.TellerNumber ==
                        tellerNumber);

            if (ticket == null)
            {
                return NotFound();
            }

            ticket.Status =
                TicketStatus.Cancelled;

            _context.SaveChanges();
        }

        BroadcastQueue();

        return ticket;
    }

    /// <summary>
    /// Одоо теллерүүд дээр дуудагдсан байгаа бүх тасалбаруудыг авна.
    /// </summary>
    /// <returns>Идэвхтэй тасалбаруудын жагсаалт.</returns>
    [HttpGet("current")]
    public ActionResult<List<Ticket>>
        Current()
    {
        var tickets =
            _context.Tickets
                .Where(x =>
                    x.Status ==
                    TicketStatus.Called)
                .OrderBy(x =>
                    x.TellerNumber)
                .ToList();

        return tickets;
    }

    /// <summary>
    /// Очерын мэдээллийг TCP-ээр дамжуулж бүх дэлгэцүүдийг шинэчилнэ.
    /// </summary>
    private void
        BroadcastQueue()
    {
        var message =
            string.Join(
                Environment.NewLine,
                _context.Tickets
                    .Where(x =>
                        x.Status ==
                        TicketStatus.Called)
                    .OrderBy(x =>
                        x.TellerNumber)
                    .Select(x =>
                        $"Teller {x.TellerNumber} => {x.Number}"));
        
        _socket
            .Broadcast(
                message)
            .Wait();
    }
}