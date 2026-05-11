using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Account> Accounts => Set<Account>();

    public DbSet<Ticket> Tickets => Set<Ticket>();

    public DbSet<Transaction> Transactions => Set<Transaction>();

    public DbSet<ExchangeRate> ExchangeRates => Set<ExchangeRate>();
   

    public DbSet<Teller> Tellers => Set<Teller>();

    protected override void OnModelCreating(
        ModelBuilder builder)
    {
        builder.Entity<Account>()
            .HasIndex(x => x.AccountNumber)
            .IsUnique();

        builder.Entity<Account>()
            .Property(x => x.Balance)
            .HasPrecision(18, 2);
    }
}