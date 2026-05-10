using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;

namespace server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketController : ControllerBase
{
    private readonly AppDbContext _context;

    public TicketController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<Ticket> Create()
    {
        var count = await _context.Tickets.CountAsync();

        var ticket = new Ticket
        {
            Number = $"A{(count + 1):000}"
        };

        _context.Tickets.Add(ticket);

        await _context.SaveChangesAsync();

        return ticket;
    }
}