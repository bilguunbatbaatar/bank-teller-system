using System.Net.Http.Json;

namespace terminal_app;

public partial class Form1 : Form
{
    private readonly HttpClient _http =
        new HttpClient();

    public Form1()
    {
        InitializeComponent();

        _http.BaseAddress =
            new Uri(
                "https://localhost:7174");
    }

    private async void btnTakeTicket_Click(
        object sender,
        EventArgs e)
    {
        btnTakeTicket.Enabled = false;

        try
        {
            var response =
                await _http
                    .PostAsJsonAsync(
                        "/api/ticket",
                        new { });

            response
                .EnsureSuccessStatusCode();

            var ticket =
                await response.Content
                    .ReadFromJsonAsync<
                        TicketResponse>();

            lblTicket.Text =
                ticket!.Number;
        }
        catch
        {
            MessageBox.Show(
                "Server connection failed.");
        }
        finally
        {
            btnTakeTicket.Enabled = true;
        }
    }
}

public class TicketResponse
{
    public string Number { get; set; }
        = string.Empty;
}