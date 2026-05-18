using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOs;
using server.Enums;
using server.Models;

namespace server.Controllers;

/// <summary>
/// Банкны дансны үйлдлүүдийг (үүсгэх, хаах, хайх) удирдах контроллер.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private static readonly Lock _accountLock = new();

    private readonly AppDbContext _context;

    /// <summary>
    /// AccountController-ийн шинэ хувилбарыг үүсгэнэ.
    /// </summary>
    /// <param name="context">Өгөгдлийн сангийн контекст.</param>
    public AccountController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Шинэ данс үүсгэнэ.
    /// </summary>
    /// <param name="request">Данс үүсгэх хүсэлтийн мэдээлэл.</param>
    /// <returns>Үүсгэсэн дансны мэдээлэл.</returns>
    [HttpPost]
    public ActionResult<Account> Create(
        CreateAccountRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var ownerName = request.OwnerName
            .Trim()
            .ToUpper();

        if (string.IsNullOrWhiteSpace(ownerName))
        {
            return BadRequest("Invalid owner name.");
        }

        lock (_accountLock)
        {
            var account = new Account
            {
                OwnerName = ownerName
            };

            _context.Accounts.Add(account);

            _context.SaveChanges();

            account.AccountNumber =
                $"ACC{account.Id:000000}";

            _context.SaveChanges();

            return CreatedAtAction(
                nameof(GetByNumber),
                new
                {
                    accountNumber =
                        account.AccountNumber
                },
                account);
        }
    }

    /// <summary>
    /// Бүх идэвхтэй данснуудын жагсаалтыг авна.
    /// </summary>
    /// <returns>Идэвхтэй дансны жагсаалт.</returns>
    [HttpGet]
    public async Task<List<Account>> GetAll()
    {
        return await _context.Accounts
            .Where(x => x.IsActive)
            .OrderBy(x => x.AccountNumber)
            .ToListAsync();
    }

    /// <summary>
    /// Дансны дугаараар дансны мэдээллийг хайна.
    /// </summary>
    /// <param name="accountNumber">Дансны дугаар.</param>
    /// <returns>Олдсон дансны мэдээлэл.</returns>
    [HttpGet("{accountNumber}")]
    public async Task<ActionResult<Account>>
        GetByNumber(string accountNumber)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(x =>
                x.AccountNumber == accountNumber &&
                x.IsActive);

        if (account == null)
        {
            return NotFound();
        }

        return account;
    }
    /// <summary>
    /// Дансыг хаана (идэвхгүй төлөвт оруулна).
    /// </summary>
    /// <param name="accountNumber">Хаах дансны дугаар.</param>
    /// <returns>Хаагдсан дансны мэдээлэл.</returns>
    [HttpPut("close/{accountNumber}")]
    public async Task<ActionResult<Account>>
    Close(string accountNumber)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(x =>
                x.AccountNumber == accountNumber);

        if (account == null)
        {
            return NotFound();
        }

        if (!account.IsActive)
        {
            return Conflict(
                "Account already closed.");
        }

        account.IsActive = false;

        await _context.SaveChangesAsync();

        return account;
    }



}