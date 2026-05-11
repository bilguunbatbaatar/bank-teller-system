using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using server.Services;
using server.Data;
using server.Enums;
using server.Models;

namespace server.Controllers;

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

    public TicketController(
        AppDbContext context,
        QueueSocketService socket)
    {
        _context = context;
        _socket = socket;
    }

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

            ticket.Number =
                $"A{ticket.Id:000}";

            _context.SaveChanges();

            return ticket;
        }
    }

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