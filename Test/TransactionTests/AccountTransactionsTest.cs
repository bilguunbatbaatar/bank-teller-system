using Microsoft.EntityFrameworkCore;
using server.Controllers;
using server.Data;
using server.DTOs;
using server.Models;
using Microsoft.AspNetCore.Mvc;

namespace Test.TransactionTests;

/// <summary>
/// Дансны гүйлгээний үйлдлүүдийг (орлого, зарлага, шилжүүлэг) шалгах тестүүд.
/// </summary>
[TestClass]
public class AccountTransactionsTest
{
    private AppDbContext GetContext()
    {
        var connection = new Microsoft.Data.Sqlite.SqliteConnection("DataSource=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;
        var context = new AppDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    /// <summary>
    /// Дансанд орлого амжилттай орохыг шалгана.
    /// </summary>
    [TestMethod]
    public void Deposit_ValidAmount_UpdatesBalance()
    {
        // Arrange
        using var context = GetContext();
        var controller = new TransactionController(context);
        var account = new Account { AccountNumber = "ACC001", Balance = 100, IsActive = true };
        context.Accounts.Add(account);
        context.SaveChanges();

        // Act
        var result = controller.Deposit("ACC001", 50);

        // Assert
        Assert.IsNotNull(result.Value);
        Assert.AreEqual(150, account.Balance);
    }

    /// <summary>
    /// Данснаас зарлага амжилттай гарахыг шалгана.
    /// </summary>
    [TestMethod]
    public void Withdraw_ValidAmount_DecreasesBalance()
    {
        // Arrange
        using var context = GetContext();
        var controller = new TransactionController(context);
        var account = new Account { AccountNumber = "ACC001", Balance = 100, IsActive = true };
        context.Accounts.Add(account);
        context.SaveChanges();

        // Act
        var result = controller.Withdraw("ACC001", 30);

        // Assert
        Assert.AreEqual(70, account.Balance);
    }

    /// <summary>
    /// Үлдэгдэл хүрэлцэхгүй үед зарлага гаргахыг хориглож байгааг шалгана.
    /// </summary>
    [TestMethod]
    public void Withdraw_InsufficientFunds_ReturnsConflict()
    {
        // Arrange
        using var context = GetContext();
        var controller = new TransactionController(context);
        var account = new Account { AccountNumber = "ACC001", Balance = 10, IsActive = true };
        context.Accounts.Add(account);
        context.SaveChanges();

        // Act
        var result = controller.Withdraw("ACC001", 100);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(ConflictObjectResult));
        Assert.AreEqual(10, account.Balance);
    }

    /// <summary>
    /// Хоёр дансны хооронд шилжүүлэг амжилттай хийгдэхийг шалгана.
    /// </summary>
    [TestMethod]
    public void Transfer_ValidRequest_UpdatesBothBalances()
    {
        // Arrange
        using var context = GetContext();
        var controller = new TransactionController(context);
        var from = new Account { AccountNumber = "ACC001", Balance = 100, IsActive = true };
        var to = new Account { AccountNumber = "ACC002", Balance = 50, IsActive = true };
        context.Accounts.AddRange(from, to);
        context.SaveChanges();

        var request = new TransferRequest { FromAccountNumber = "ACC001", ToAccountNumber = "ACC002", Amount = 40 };

        // Act
        var result = controller.Transfer(request);

        // Assert
        Assert.AreEqual(60, from.Balance);
        Assert.AreEqual(90, to.Balance);
    }

    /// <summary>
    /// Шилжүүлэг хийхэд үлдэгдэл хүрэлцэхгүй байгааг шалгана.
    /// </summary>
    [TestMethod]
    public void Transfer_InsufficientFunds_ReturnsConflict()
    {
        // Arrange
        using var context = GetContext();
        var controller = new TransactionController(context);
        var from = new Account { AccountNumber = "ACC001", Balance = 10, IsActive = true };
        var to = new Account { AccountNumber = "ACC002", Balance = 50, IsActive = true };
        context.Accounts.AddRange(from, to);
        context.SaveChanges();

        var request = new TransferRequest { FromAccountNumber = "ACC001", ToAccountNumber = "ACC002", Amount = 100 };

        // Act
        var result = controller.Transfer(request);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(ConflictObjectResult));
        Assert.AreEqual(10, from.Balance);
        Assert.AreEqual(50, to.Balance);
    }
}
