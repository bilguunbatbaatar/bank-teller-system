using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using server.Hubs;

using server.Data;
using server.Models;

namespace server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExchangeController
    : ControllerBase
{
    private readonly
    IHubContext<ExchangeHub>
    _hub;
    private readonly
        AppDbContext _context;

    public ExchangeController(
      AppDbContext context,
      IHubContext<ExchangeHub> hub)
    {
        _context = context;
        _hub = hub;
    }

    [HttpGet]
    public async Task<
        List<ExchangeRate>>
        GetAll()
    {
        return await _context
            .ExchangeRates
            .OrderBy(x =>
                x.CurrencyCode)
            .ToListAsync();
    }

    [HttpPut("{currency}")]
    public async Task<
    ActionResult<ExchangeRate>>
    Update(
        string currency,
        decimal buyRate,
        decimal sellRate)
    {
        if (buyRate <= 0 ||
            sellRate <= 0)
        {
            return BadRequest();
        }

        currency =
            currency
                .Trim()
                .ToUpper();

        var rate =
            await _context
                .ExchangeRates
                .FirstOrDefaultAsync(
                    x =>
                    x.CurrencyCode
                        .ToUpper() ==
                    currency);

        if (rate == null)
        {
            rate =
                new ExchangeRate
                {
                    CurrencyCode =
                        currency
                };

            _context
                .ExchangeRates
                .Add(rate);
        }

        rate.BuyRate =
            buyRate;

        rate.SellRate =
            sellRate;

        rate.UpdatedAt =
            DateTime.UtcNow;

        await _context
            .SaveChangesAsync();

        Console.WriteLine(
    "Broadcasting exchange update...");
        await _hub.Clients
            .All
            .SendAsync(
                "ExchangeUpdated");

        return rate;
    }
}