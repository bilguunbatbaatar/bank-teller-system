using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using server.Data;
using server.Models;

namespace server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExchangeController
    : ControllerBase
{
    private readonly
        AppDbContext _context;

    public ExchangeController(
        AppDbContext context)
    {
        _context = context;
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

        var rate =
            await _context
                .ExchangeRates
                .FirstOrDefaultAsync(
                    x =>
                    x.CurrencyCode ==
                    currency);

        if (rate == null)
        {
            rate =
                new ExchangeRate
                {
                    CurrencyCode =
                        currency
                            .ToUpper()
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

        return rate;
    }
}