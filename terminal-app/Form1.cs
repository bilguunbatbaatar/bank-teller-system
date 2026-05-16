using System.Net.Http.Json;

namespace terminal_app;

/// <summary>
/// Харилцагчийн очерын тасалбар авах терминалын цонх.
/// </summary>
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

    /// <summary>
    /// Очерын тасалбар авах товчлуур дээр дарахад шинэ тасалбар үүсгэж дугаарыг харуулна.
    /// </summary>
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

/// <summary>
/// Серверээс ирэх тасалбарын хариу мэдээллийг төлөөлөх класс.
/// </summary>
public class TicketResponse
{
    public string Number { get; set; }
        = string.Empty;
}