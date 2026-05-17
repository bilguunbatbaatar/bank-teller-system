using Microsoft.EntityFrameworkCore;
using server.Controllers;
using server.Data;
using server.DTOs;
using server.Models;
using Microsoft.AspNetCore.Mvc;

namespace Test.AccountTests;

/// <summary>
/// Банкны дансны бүртгэл, хаалт, хайлт зэрэг үйлдлүүдийг шалгах тестүүд.
/// </summary>
[TestClass]
public class AccountManagementTest
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
    /// Шинэ данс амжилттай үүсэхийг шалгана.
    /// </summary>
    [TestMethod]
    public void Create_ValidRequest_CreatesAccount()
    {
        // Arrange
        using var context = GetContext();
        var controller = new AccountController(context);
        var request = new CreateAccountRequest { OwnerName = "Bat-Erdene" };

        // Act
        var result = controller.Create(request);

        // Assert
        var createdAtActionResult = result.Result as CreatedAtActionResult;
        var account = createdAtActionResult?.Value as Account;
        
        Assert.IsNotNull(account);
        Assert.AreEqual("BAT-ERDENE", account.OwnerName);
        Assert.IsTrue(account.AccountNumber.StartsWith("ACC"), "Account number should start with ACC");
    }

    /// <summary>
    /// Дансыг амжилттай хааж байгааг шалгана.
    /// </summary>
    [TestMethod]
    public async Task Close_ExistingAccount_SetsIsActiveFalse()
    {
        // Arrange
        using var context = GetContext();
        var controller = new AccountController(context);
        var account = new Account { AccountNumber = "ACC001", OwnerName = "TEST", IsActive = true };
        context.Accounts.Add(account);
        context.SaveChanges();

        // Act
        await controller.Close("ACC001");

        // Assert
        Assert.IsFalse(account.IsActive);
    }

    /// <summary>
    /// Аль хэдийн хаагдсан дансыг дахин хаах үед Conflict буцааж байгааг шалгана.
    /// </summary>
    [TestMethod]
    public async Task Close_AlreadyClosed_ReturnsConflict()
    {
        // Arrange
        using var context = GetContext();
        var controller = new AccountController(context);
        var account = new Account { AccountNumber = "ACC001", OwnerName = "TEST", IsActive = false };
        context.Accounts.Add(account);
        context.SaveChanges();

        // Act
        var result = await controller.Close("ACC001");

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(ConflictObjectResult));
    }

    /// <summary>
    /// Байхгүй дансны дугаараар хайхад NotFound буцааж байгааг шалгана.
    /// </summary>
    [TestMethod]
    public async Task GetByNumber_NonExistent_ReturnsNotFound()
    {
        // Arrange
        using var context = GetContext();
        var controller = new AccountController(context);

        // Act
        var result = await controller.GetByNumber("NONEXISTENT");

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
    }
}
