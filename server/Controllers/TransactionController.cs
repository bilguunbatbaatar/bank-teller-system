using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOs;
using server.Enums;
using server.Models;

namespace server.Controllers;

/// <summary>
/// Банкны гүйлгээний үйлдлүүдийг (орлого, зарлага, шилжүүлэг) удирдах контроллер.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private static readonly Lock _transferLock = new();

    private readonly AppDbContext _context;

    /// <summary>
    /// TransactionController-ийн шинэ хувилбарыг үүсгэнэ.
    /// </summary>
    /// <param name="context">Өгөгдлийн сангийн контекст.</param>
    public TransactionController(
        AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Дансанд орлого хийнэ.
    /// </summary>
    /// <param name="accountNumber">Орлого хийх дансны дугаар.</param>
    /// <param name="amount">Орлогын дүн.</param>
    /// <returns>Гүйлгээний мэдээлэл.</returns>
    [HttpPost("deposit")]
    public ActionResult<Transaction>
        Deposit(string accountNumber,
                decimal amount)
    {
        if (amount <= 0)
        {
            return BadRequest();
        }

        lock (_transferLock)
        {
            using var dbTransaction =
                _context.Database.BeginTransaction();

            var account =
                _context.Accounts
                    .FirstOrDefault(x =>
                        x.AccountNumber ==
                        accountNumber &&
                        x.IsActive);

            if (account == null)
            {
                return NotFound();
            }

            account.Balance += amount;

            var transaction =
                new Transaction
                {
                    FromAccountNumber = "CASH",
                    ToAccountNumber =
                        account.AccountNumber,
                    Amount = amount,
                    Type =
                        TransactionType.Deposit
                };

            _context.Transactions
                .Add(transaction);

            _context.SaveChanges();

            dbTransaction.Commit();

            return transaction;
        }
    }

    /// <summary>
    /// Данснаас зарлага гаргана.
    /// </summary>
    /// <param name="accountNumber">Зарлага гаргах дансны дугаар.</param>
    /// <param name="amount">Зарлагын дүн.</param>
    /// <returns>Гүйлгээний мэдээлэл.</returns>
    [HttpPost("withdraw")]
    public ActionResult<Transaction>
        Withdraw(string accountNumber,
                 decimal amount)
    {
        if (amount <= 0)
        {
            return BadRequest();
        }

        lock (_transferLock)
        {
            using var dbTransaction =
                _context.Database.BeginTransaction();

            var account =
                _context.Accounts
                    .FirstOrDefault(x =>
                        x.AccountNumber ==
                        accountNumber &&
                        x.IsActive);

            if (account == null)
            {
                return NotFound();
            }

            if (account.Balance < amount)
            {
                return Conflict(
                    "Insufficient funds.");
            }

            account.Balance -= amount;

            var transaction =
                new Transaction
                {
                    FromAccountNumber =
                        account.AccountNumber,
                    ToAccountNumber = "CASH",
                    Amount = amount,
                    Type =
                        TransactionType.Withdraw
                };

            _context.Transactions
                .Add(transaction);

            _context.SaveChanges();

            dbTransaction.Commit();

            return transaction;
        }
    }

    /// <summary>
    /// Хоёр дансны хооронд шилжүүлэг хийнэ.
    /// </summary>
    /// <param name="request">Шилжүүлгийн хүсэлтийн мэдээлэл.</param>
    /// <returns>Гүйлгээний мэдээлэл.</returns>
    [HttpPost("transfer")]
    public ActionResult<Transaction>
        Transfer(TransferRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        lock (_transferLock)
        {
            using var dbTransaction =
                _context.Database.BeginTransaction();

            var from =
                _context.Accounts
                    .FirstOrDefault(x =>
                        x.AccountNumber ==
                        request.FromAccountNumber &&
                        x.IsActive);

            var to =
                _context.Accounts
                    .FirstOrDefault(x =>
                        x.AccountNumber ==
                        request.ToAccountNumber &&
                        x.IsActive);

            if (from == null || to == null)
            {
                return NotFound();
            }

            if (from.Balance <
                request.Amount)
            {
                return Conflict(
                    "Insufficient funds.");
            }

            from.Balance -=
                request.Amount;

            to.Balance +=
                request.Amount;

            var transaction =
                new Transaction
                {
                    FromAccountNumber =
                        from.AccountNumber,

                    ToAccountNumber =
                        to.AccountNumber,

                    Amount =
                        request.Amount,

                    Type =
                        TransactionType.Transfer
                };

            _context.Transactions
                .Add(transaction);

            _context.SaveChanges();

            dbTransaction.Commit();

            return transaction;
        }
    }

    /// <summary>
    /// Тухайн дансны гүйлгээний түүхийг авна.
    /// </summary>
    /// <param name="accountNumber">Дансны дугаар.</param>
    /// <returns>Гүйлгээний жагсаалт.</returns>
    [HttpGet("{accountNumber}/history")]
    public async Task<
        ActionResult<List<Transaction>>>
        History(string accountNumber)
    {
        var exists =
            await _context.Accounts
                .AnyAsync(x =>
                    x.AccountNumber ==
                    accountNumber);

        if (!exists)
        {
            return NotFound();
        }

        var history =
            await _context.Transactions
                .Where(x =>
                    x.FromAccountNumber ==
                    accountNumber ||
                    x.ToAccountNumber ==
                    accountNumber)
                .OrderByDescending(x =>
                    x.CreatedAt)
                .ToListAsync();

        return history;
    }
}