using System.Net;
using System.Net.Sockets;
using System.Text;

namespace server.Services;

/// <summary>
/// Очерын мэдээллийг TCP сокетоор дамжуулах сервис.
/// </summary>
public class QueueSocketService
{
    private readonly
        List<TcpClient>
        _clients =
            new();

    /// <summary>
    /// QueueSocketService-ийн шинэ хувилбарыг үүсгэж, сокет серверийг эхлүүлнэ.
    /// </summary>
    public QueueSocketService()
    {
        Console.WriteLine(
            "Socket server starting...");

        Task.Run(
            StartServer);
    }

    /// <summary>
    /// TCP серверийг 5000 порт дээр ажиллуулж, холболтуудыг хүлээн авна.
    /// </summary>
    private async Task
        StartServer()
    {
        var listener =
            new TcpListener(
                IPAddress.Any,
                5000);

        listener.Start();

        Console.WriteLine(
            "Socket server running on port 5000.");

        while (true)
        {
            var client =
                await listener
                    .AcceptTcpClientAsync();

            Console.WriteLine(
                "Display connected.");

            lock (_clients)
            {
                _clients
                    .Add(client);
            }
        }
    }

    /// <summary>
    /// Холбогдсон бүх клиентүүд рүү (дэлгэцүүд рүү) мэдээлэл илгээнэ.
    /// </summary>
    /// <param name="message">Илгээх мессеж.</param>
    public async Task
        Broadcast(
            string message)
    {
        var bytes =
            Encoding.UTF8
                .GetBytes(
                    message);

        foreach (var client in _clients)
        {
            try
            {
                await client
                    .GetStream()
                    .WriteAsync(
                        bytes);
            }
            catch
            {
            }
        }
    }
}