using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Data;

/// <summary>
/// Програмын өгөгдлийн сангийн контекст. Entity Framework Core-ийн тусламжтайгаар өгөгдлийн сантай харилцана.
/// </summary>

public class AppDbContext : DbContext
{
    /// <summary>
    /// AppDbContext-ийн шинэ хувилбарыг үүсгэнэ.
    /// </summary>
    /// <param name="options">Контекстийн тохиргоо.</param>
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Данснуудын хүснэгт.
    /// </summary>
    public DbSet<Account> Accounts => Set<Account>();

    /// <summary>
    /// Тасалбаруудын хүснэгт.
    /// </summary>
    public DbSet<Ticket> Tickets => Set<Ticket>();

    /// <summary>
    /// Гүйлгээнүүдийн хүснэгт.
    /// </summary>
    public DbSet<Transaction> Transactions => Set<Transaction>();

    /// <summary>
    /// Валютын ханшуудын хүснэгт.
    /// </summary>
    public DbSet<ExchangeRate> ExchangeRates => Set<ExchangeRate>();
   

    /// <summary>
    /// Теллерүүдийн хүснэгт.
    /// </summary>
    public DbSet<Teller> Tellers => Set<Teller>();

    /// <summary>
    /// Өгөгдлийн сангийн моделийг тохируулах функц.
    /// </summary>
    /// <param name="builder">Модель байгуулагч.</param>
    protected override void OnModelCreating(
        ModelBuilder builder)
    {
        builder.Entity<Account>()
            .HasIndex(x => x.AccountNumber)
            .IsUnique();

        builder.Entity<ExchangeRate>()
        .HasIndex(x =>
        x.CurrencyCode)
        .IsUnique();

        builder.Entity<Account>()
            .Property(x => x.Balance)
            .HasPrecision(18, 2);
    }
}