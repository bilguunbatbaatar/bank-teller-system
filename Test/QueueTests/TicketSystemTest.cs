using Microsoft.EntityFrameworkCore;
using server.Controllers;
using server.Data;
using server.Enums;
using server.Models;
using server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Test.QueueTests;

/// <summary>
/// Очерын системийн үйлдлүүдийг (тасалбар олгох, дуудах, дуусгах) шалгах тестүүд.
/// </summary>
[TestClass]
public class TicketSystemTest
{
    private (AppDbContext context, QueueSocketService socket) GetDependencies()
    {
        var connection = new Microsoft.Data.Sqlite.SqliteConnection("DataSource=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;
        var context = new AppDbContext(options);
        context.Database.EnsureCreated();
        return (context, new QueueSocketService());
    }

    /// <summary>
    /// Шинэ тасалбар амжилттай олгогдож байгааг шалгана.
    /// </summary>
    [TestMethod]
    public void Create_IncrementsTicketNumber()
    {
        // Arrange
        var (context, socket) = GetDependencies();
        var controller = new TicketController(context, socket);

        // Act
        var result1 = controller.Create();
        var result2 = controller.Create();

        // Assert
        Assert.AreEqual("A001", result1.Value?.Number);
        Assert.AreEqual("A002", result2.Value?.Number);
    }

    /// <summary>
    /// Теллер дараагийн тасалбарыг амжилттай дуудаж байгааг шалгана.
    /// </summary>
    [TestMethod]
    public void Next_AssignsWaitingTicketToTeller()
    {
        // Arrange
        var (context, socket) = GetDependencies();
        var controller = new TicketController(context, socket);
        controller.Create(); // A001

        // Act
        var result = controller.Next(1);

        // Assert
        Assert.AreEqual("A001", result.Value?.Number);
        Assert.AreEqual(TicketStatus.Called, result.Value?.Status);
        Assert.AreEqual(1, result.Value?.TellerNumber);
    }

    /// <summary>
    /// Хүлээгдэж буй тасалбар байхгүй үед NotFound буцааж байгааг шалгана.
    /// </summary>
    [TestMethod]
    public void Next_NoWaitingTickets_ReturnsNotFound()
    {
        // Arrange
        var (context, socket) = GetDependencies();
        var controller = new TicketController(context, socket);

        // Act
        var result = controller.Next(1);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
    }

    /// <summary>
    /// Теллер идэвхтэй тасалбартай байх үед дахиж дуудахыг хориглож байгааг шалгана.
    /// </summary>
    [TestMethod]
    public void Next_TellerAlreadyHasActiveTicket_ReturnsConflict()
    {
        // Arrange
        var (context, socket) = GetDependencies();
        var controller = new TicketController(context, socket);
        controller.Create();
        controller.Create();
        controller.Next(1); // Teller 1 takes A1

        // Act
        var result = controller.Next(1); // Teller 1 tries to take A2

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(ConflictObjectResult));
    }

    /// <summary>
    /// Тасалбарын үйлчилгээг амжилттай дуусгаж байгааг шалгана.
    /// </summary>
    [TestMethod]
    public void Complete_SetsStatusToCompleted()
    {
        // Arrange
        var (context, socket) = GetDependencies();
        var controller = new TicketController(context, socket);
        controller.Create();
        controller.Next(1);

        // Act
        var result = controller.Complete(1);

        // Assert
        Assert.AreEqual(TicketStatus.Completed, result.Value?.Status);
        Assert.IsNotNull(result.Value?.CompletedAt);
    }
}
