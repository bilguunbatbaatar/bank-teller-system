using System.Net.Sockets;
using System.Text;

namespace queue_display;

public partial class Form1 : Form
{
    private readonly TcpClient _client =
        new();

    public Form1()
    {
        InitializeComponent();

        Load += Form1_Load;
    }

    private async void Form1_Load(
        object? sender,
        EventArgs e)
    {
        try
        {
            await _client
                .ConnectAsync(
                    "127.0.0.1",
                    5000);

            var stream =
                _client.GetStream();

            var buffer =
                new byte[1024];

            while (true)
            {
                var count =
                    await stream
                        .ReadAsync(
                            buffer);

                var message =
                    Encoding.UTF8
                        .GetString(
                            buffer,
                            0,
                            count);

                UpdateLabels(
                    message);
            }
        }
        catch
        {
            MessageBox.Show(
                "Cannot connect to server.");
        }
    }

    private void UpdateLabels(
        string message)
    {
        Invoke(() =>
        {
            lblTeller1.Text =
                "TELLER 1 : ---";

            lblTeller2.Text =
                "TELLER 2 : ---";

            lblTeller3.Text =
                "TELLER 3 : ---";

            var lines =
                message.Split(
                    Environment.NewLine);

            foreach (var line in lines)
            {
                if (line.StartsWith(
                    "Teller 1"))
                {
                    lblTeller1.Text =
                        line;
                }

                if (line.StartsWith(
                    "Teller 2"))
                {
                    lblTeller2.Text =
                        line;
                }

                if (line.StartsWith(
                    "Teller 3"))
                {
                    lblTeller3.Text =
                        line;
                }
            }
        });
    }
}