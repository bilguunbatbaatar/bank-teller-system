using Microsoft.EntityFrameworkCore;
using server.Controllers;
using server.Data;
using server.Hubs;
using server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace Test.TellerTests;

/// <summary>
/// Валютын ханшийн шинэчлэл болон бодит хугацааны мэдэгдлийг шалгах тестүүд.
/// </summary>
[TestClass]
public class ExchangeRatesTest
{
    private (AppDbContext context, Mock<IHubContext<ExchangeHub>> hub, Mock<IClientProxy> proxy) GetDependencies()
    {
        var connection = new Microsoft.Data.Sqlite.SqliteConnection("DataSource=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;
        var context = new AppDbContext(options);
        context.Database.EnsureCreated();
        
        var mockHub = new Mock<IHubContext<ExchangeHub>>();
        var mockClients = new Mock<IHubClients>();
        var mockClientProxy = new Mock<IClientProxy>();
        
        mockHub.Setup(h => h.Clients).Returns(mockClients.Object);
        mockClients.Setup(c => c.All).Returns(mockClientProxy.Object);
        
        return (context, mockHub, mockClientProxy);
    }

    /// <summary>
    /// Валютын ханшийг амжилттай шинэчилж, SignalR-ээр мэдэгдэл илгээж байгааг шалгана.
    /// </summary>
    [TestMethod]
    public async Task Update_ExistingRate_UpdatesValueAndBroadcasts()
    {
        // Arrange
        var (context, mockHub, mockProxy) = GetDependencies();
        var controller = new ExchangeController(context, mockHub.Object);
        var rate = new ExchangeRate { CurrencyCode = "USD", BuyRate = 3400, SellRate = 3450 };
        context.ExchangeRates.Add(rate);
        context.SaveChanges();

        // Act
        var result = await controller.Update("USD", 3410, 3460);

        // Assert
        Assert.AreEqual(3410, rate.BuyRate);
        Assert.AreEqual(3460, rate.SellRate);
        
        // Verify SignalR broadcast was called via SendCoreAsync
        mockProxy.Verify(p => p.SendCoreAsync("ExchangeUpdated", It.IsAny<object[]>(), default), Times.Once);
    }

    /// <summary>
    /// Шинэ валютын ханшийг амжилттай үүсгэж байгааг шалгана (Upsert).
    /// </summary>
    [TestMethod]
    public async Task Update_NewCurrency_CreatesRate()
    {
        // Arrange
        var (context, mockHub, _) = GetDependencies();
        var controller = new ExchangeController(context, mockHub.Object);

        // Act
        await controller.Update("EUR", 3700, 3750);

        // Assert
        var rate = await context.ExchangeRates.FirstOrDefaultAsync(x => x.CurrencyCode == "EUR");
        Assert.IsNotNull(rate);
        Assert.AreEqual(3700, rate.BuyRate);
    }

    /// <summary>
    /// Сөрөг утгатай ханшийг хориглож байгааг шалгана.
    /// </summary>
    [TestMethod]
    public async Task Update_InvalidRates_ReturnsBadRequest()
    {
        // Arrange
        var (context, mockHub, _) = GetDependencies();
        var controller = new ExchangeController(context, mockHub.Object);

        // Act
        var result = await controller.Update("USD", -10, 3400);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
    }
}
