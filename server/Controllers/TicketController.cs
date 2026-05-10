using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Enums;
using server.Models;

namespace server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketController : ControllerBase
{
    private static readonly Lock _ticketLock = new();

    private readonly AppDbContext _context;

    public TicketController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public ActionResult<Ticket> Create()
    {
        lock (_ticketLock)
        {
            var ticket = new Ticket();

            _context.Tickets.Add(ticket);

            _context.SaveChanges();

            ticket.Number =
                $"A{ticket.Id:000}";

            _context.SaveChanges();

            return ticket;
        }
    }

    [HttpPost("next")]
    public ActionResult<Ticket> Next()
    {
        lock (_ticketLock)
        {
            var ticket = _context.Tickets
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

            ticket.CalledAt =
                DateTime.UtcNow;

            _context.SaveChanges();

            return ticket;
        }
    }
    [HttpPost("complete")]
    public ActionResult<Ticket> Complete()
    {
        lock (_ticketLock)
        {
            var ticket = _context.Tickets
                .OrderBy(x => x.Id)
                .LastOrDefault(x =>
                    x.Status ==
                    TicketStatus.Called);

            if (ticket == null)
            {
                return NotFound();
            }

            ticket.Status =
                TicketStatus.Completed;

            ticket.CompletedAt =
                DateTime.UtcNow;

            _context.SaveChanges();

            return ticket;
        }
    }

    [HttpPost("cancel")]
    public ActionResult<Ticket> Cancel()
    {
        lock (_ticketLock)
        {
            var ticket = _context.Tickets
                .OrderBy(x => x.Id)
                .LastOrDefault(x =>
                    x.Status ==
                    TicketStatus.Called);

            if (ticket == null)
            {
                return NotFound();
            }

            ticket.Status =
                TicketStatus.Cancelled;

            _context.SaveChanges();

            return ticket;
        }
    }

    [HttpGet("current")]
    public ActionResult<Ticket> Current()
    {
        var ticket = _context.Tickets
            .OrderBy(x => x.Id)
            .LastOrDefault(x =>
                x.Status ==
                TicketStatus.Called);

        if (ticket == null)
        {
            return NotFound();
        }

        return ticket;
    }
}