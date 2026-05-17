using System.Net.Http.Json;

namespace terminal_app;

/// <summary>
/// Харилцагчийн очерын тасалбар авах терминалын цонх.
/// Энэ цонх нь REST API-аар дамжуулан шинэ тасалбар үүсгэж,
/// харилцагчид дугаарыг харуулна.
/// </summary>
public partial class Form1 : Form
{
    // REST API-тай харилцах HTTP клиент
    private readonly HttpClient _http = new HttpClient();

    /// <summary>
    /// Конструктор: контролуудыг үүсгэж, HTTP клиентийн суурь хаягийг тохируулна.
    /// </summary>
    public Form1()
    {
        InitializeComponent();

        _http.BaseAddress = new Uri("https://localhost:7174");
    }

    /// <summary>
    /// "Тасалбар авах" товч дарахад POST хүсэлт илгээж шинэ дугаар авна.
    /// Хүсэлт явагдах хугацаанд товчийг идэвхгүй болгоно.
    /// Амжилттай бол lblTicket-д дугаарыг харуулна.
    /// Алдаа гарвал MessageBox мэдэгдэл өгнө.
    /// </summary>
    private async void btnTakeTicket_Click(
        object sender,
        EventArgs e)
    {
        btnTakeTicket.Enabled = false;
        lblTicket.Text = "...";

        try
        {
            var response =
                await _http
                    .PostAsJsonAsync(
                        "/api/ticket",
                        new { });

            response.EnsureSuccessStatusCode();

            var ticket =
                await response.Content
                    .ReadFromJsonAsync<TicketResponse>();

            lblTicket.Text = ticket!.Number;
        }
        catch
        {
            lblTicket.Text = "---";
            MessageBox.Show(
                "Server connection failed.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
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
    /// <summary>Серверээс өгсөн тасалбарын дугаар. Жишээ: "A042"</summary>
    public string Number { get; set; } = string.Empty;
}