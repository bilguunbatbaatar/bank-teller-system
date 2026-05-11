using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Enums;
using server.Models;
using Microsoft.AspNetCore.SignalR;
using server.Hubs;

namespace server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketController : ControllerBase
{
    private static readonly Lock _ticketLock = new();

    private readonly AppDbContext _context;
    private readonly
    IHubContext<QueueHub>
    _hub;

    public TicketController(
     AppDbContext context,
     IHubContext<QueueHub> hub)
    {
        _context = context;
        _hub = hub;
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

    [HttpPost("next/{tellerNumber}")]
    public ActionResult<Ticket>
      Next(int tellerNumber)
    {
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

            var ticket =
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
            _hub.Clients.All.SendAsync(
    "QueueUpdated");

            return ticket;
        }
    }
    [HttpPost("complete/{tellerNumber}")]
    public ActionResult<Ticket>
     Complete(int tellerNumber)
    {
        lock (_ticketLock)
        {
            var ticket =
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
            _hub.Clients.All.SendAsync(
    "QueueUpdated");

            return ticket;
        }
    }

    [HttpPost("cancel/{tellerNumber}")]
    public ActionResult<Ticket>
     Cancel(int tellerNumber)
    {
        lock (_ticketLock)
        {
            var ticket =
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
            _hub.Clients.All.SendAsync(
    "QueueUpdated");

            return ticket;
        }
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
}