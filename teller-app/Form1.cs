using System.Net.Http.Json;

namespace teller_app;

using System.ComponentModel;
using System.Text.Json;

/// <summary>
/// Банкны теллерийн үндсэн цонх. Очер удирдах болон банкны гүйлгээ хийх боломжтой.
/// </summary>
public partial class Form1 : Form
{
    private readonly
        HttpClient _http =
            new();

    public Form1()
    {
        InitializeComponent();

        _http.BaseAddress =
            new Uri(
                "https://localhost:7174");

        cmbTeller.Items.AddRange(
            new object[]
            {
                1,
                2,
                3
            });

        cmbTeller.SelectedIndex = 0;
    }

    private int
        TellerNumber =>
            (int)cmbTeller
                .SelectedItem!;

    /// <summary>
    /// Дараагийн очерын тасалбарыг дуудна.
    /// </summary>
    private async void
        btnNext_Click(
            object sender,
            EventArgs e)
    {
        var ticket =
           await _http
    .PostAsync(
        $"/api/ticket/next/{TellerNumber}",
        null);

        await LoadCurrent();
    }

    /// <summary>
    /// Тухайн тасалбарын үйлчилгээг дуусгана.
    /// </summary>
    private async void
        btnComplete_Click(
            object sender,
            EventArgs e)
    {
        MessageBox.Show(
    "Next clicked");
        await _http
            .PostAsync(
                $"/api/ticket/complete/{TellerNumber}",
                null);

        await LoadCurrent();
    }

    /// <summary>
    /// Тасалбарыг цуцална.
    /// </summary>
    private async void
        btnCancel_Click(
            object sender,
            EventArgs e)
    {
        await _http
            .PostAsync(
                $"/api/ticket/cancel/{TellerNumber}",
                null);

        await LoadCurrent();
    }

    /// <summary>
    /// Одоогийн дуудагдсан байгаа тасалбарын мэдээллийг серверээс ачаална.
    /// </summary>
    private async Task
    LoadCurrent()
    {
        var tickets =
            await _http
                .GetFromJsonAsync<
                    List<Ticket>>(
                        "/api/ticket/current",
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive =
                                true
                        })
            ?? new();

        var current =
            tickets
                .FirstOrDefault(
                    x =>
                    x.TellerNumber ==
                    TellerNumber);

        lblTicket.Text =
            current?.Number
            ?? "----";
    }

    public class Ticket
    {
        public string Number
        { get; set; }
            = string.Empty;

        public int?
            TellerNumber
        { get; set; }
    }

    /// <summary>
    /// Дансны дугаараар хайлт хийж харилцагчийн мэдээллийг харуулна.
    /// </summary>
    private async void
    btnSearch_Click(
        object sender,
        EventArgs e)
    {
        var accountNumber =
            txtAccountNumber
                .Text
                .Trim();

        if (string.IsNullOrWhiteSpace(
            accountNumber))
        {
            MessageBox.Show(
                "Дансны дугаар оруулна уу.",
                "Анхаар",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        try
        {
            var account =
                await _http
                    .GetFromJsonAsync<
                        Account>(
                            $"/api/account/{accountNumber}");

            if (account == null)
            {
                throw new Exception();
            }

            lblName.Text =
                account.OwnerName;

            lblBalance.Text =
                account.Balance
                    .ToString("N2");
        }
        catch
        {
            MessageBox.Show(
                "Данс олдсонгүй.",
                "Анхаар",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            lblName.Text =
                "----";

            lblBalance.Text =
                "----";
        }
    }
    /// <summary>
    /// Данс хоорондын шилжүүлэг хийнэ.
    /// </summary>
    private async void
    btnTransfer_Click(
        object sender,
        EventArgs e)
    {
        var request =
            new TransferRequest
            {
                FromAccountNumber =
                    txtAccountNumber.Text,

                ToAccountNumber =
                    txtToAccount.Text,

                Amount =
                    decimal.Parse(
                        txtAmount.Text)
            };

        var response =
            await _http
                .PostAsJsonAsync(
                    "/api/transaction/transfer",
                    request);

        if (!response.IsSuccessStatusCode)
        {
            MessageBox.Show(
                "Шилжүүлэг амжилтгүй.");
            return;
        }

        btnSearch_Click(
            null!,
            EventArgs.Empty);

        MessageBox.Show(
            "Шилжүүлэг амжилттай.");
    }

    /// <summary>
    /// Дансанд бэлэн мөнгөний орлого хийнэ.
    /// </summary>
    private async void
    btnDeposit_Click(
        object sender,
        EventArgs e)
    {
        var amount =
            decimal.Parse(
                txtAmount.Text);

        var accountNumber =
            txtAccountNumber.Text;

        var response =
            await _http
                .PostAsync(
                    $"/api/transaction/deposit" +
                    $"?accountNumber={accountNumber}" +
                    $"&amount={amount}",
                    null);

        if (!response.IsSuccessStatusCode)
        {
            MessageBox.Show(
                "Гүйлгээ амжилтгүй.");
            return;
        }

        btnSearch_Click(
            null!,
            EventArgs.Empty);

        MessageBox.Show(
            "Мөнгө амжилттай нэмэгдлээ.");
    }

    /// <summary>
    /// Дансны гүйлгээний түүхийг харуулна.
    /// </summary>
    private async void
    btnHistory_Click(
        object sender,
        EventArgs e)
    {
        var accountNumber =
            txtAccountNumber
                .Text
                .Trim();

        if (string.IsNullOrWhiteSpace(
            accountNumber))
        {
            MessageBox.Show(
                "Дансны дугаар оруулна уу.");

            return;
        }

        var history =
            await _http
                .GetFromJsonAsync<
                    List<Transaction>>(
                        $"/api/transaction/" +
                        $"{accountNumber}/history")
            ?? new();

        gridHistory.DataSource =
            history;
    }
    /// <summary>
    /// Шинэ харилцагчийн данс үүсгэнэ.
    /// </summary>
    private async void
    btnCreateAccount_Click(
        object sender,
        EventArgs e)
    {
        var request =
            new CreateAccountRequest
            {
                OwnerName =
                    txtOwnerName.Text
            };

        var response =
            await _http
                .PostAsJsonAsync(
                    "/api/account",
                    request);

        if (!response.IsSuccessStatusCode)
        {
            MessageBox.Show(
                "Данс үүсгэхэд алдаа гарлаа.");

            return;
        }

        var account =
            await response
                .Content
                .ReadFromJsonAsync<
                    Account>();

        txtAccountNumber.Text =
            account!
                .AccountNumber;

        lblName.Text =
            account.OwnerName;

        lblBalance.Text =
            account.Balance
                .ToString("N2");

        MessageBox.Show(
            "Данс амжилттай үүслээ.");
    }

    /// <summary>
    /// Валютын ханшийг шинэчилнэ.
    /// </summary>
    private async void
    btnUpdateRate_Click(
        object sender,
        EventArgs e)
    {
        var currency =
            txtCurrency
                .Text
                .Trim()
                .ToUpper();

        var buyRate =
            txtBuyRate
                .Text;

        var sellRate =
            txtSellRate
                .Text;

        var response =
            await _http
                .PutAsync(
                    $"/api/exchange/" +
                    $"{currency}" +
                    $"?buyRate={buyRate}" +
                    $"&sellRate={sellRate}",
                    null);

        if (!response.IsSuccessStatusCode)
        {
            MessageBox.Show(
                "Ханш шинэчлэхэд алдаа гарлаа.");

            return;
        }

        MessageBox.Show(
            "Ханш амжилттай шинэчлэгдлээ.");
    }

    /// <summary>
    /// Дансыг хаана.
    /// </summary>
    private async void
    btnCloseAccount_Click(
        object sender,
        EventArgs e)
    {
        var accountNumber =
            txtAccountNumber.Text;

        var response =
            await _http
                .PutAsync(
                    $"/api/account/close/" +
                    $"{accountNumber}",
                    null);

        if (!response.IsSuccessStatusCode)
        {
            MessageBox.Show(
                "Данс хаах боломжгүй.");

            return;
        }

        MessageBox.Show(
            "Данс амжилттай хаагдлаа.");

        txtAccountNumber.Text =
            "";

        lblName.Text =
            "----";

        lblBalance.Text =
            "----";
    }

    /// <summary>
    /// Данснаас бэлэн мөнгөний зарлага гаргана.
    /// </summary>
    private async void
    btnWithdraw_Click(
        object sender,
        EventArgs e)
    {
        var amount =
            decimal.Parse(
                txtAmount.Text);

        var accountNumber =
            txtAccountNumber.Text;

        var response =
            await _http
                .PostAsync(
                    $"/api/transaction/withdraw" +
                    $"?accountNumber={accountNumber}" +
                    $"&amount={amount}",
                    null);

        if (!response.IsSuccessStatusCode)
        {
            MessageBox.Show(
                "Үлдэгдэл хүрэлцэхгүй.");
            return;
        }

        btnSearch_Click(
            null!,
            EventArgs.Empty);

        MessageBox.Show(
            "Мөнгө амжилттай гарлаа.");
    }

 
}
public class Account
{
    public string AccountNumber
    { get; set; }
        = string.Empty;

    public string OwnerName
    { get; set; }
        = string.Empty;

    public decimal Balance
    { get; set; }
}
public class TransferRequest
{
    public string
        FromAccountNumber
    { get; set; }
        = string.Empty;

    public string
        ToAccountNumber
    { get; set; }
        = string.Empty;

    public decimal
        Amount
    { get; set; }
}
public class Transaction
{
    public TransactionType Type
    { get; set; }

    public decimal Amount
    { get; set; }

    public string
        FromAccountNumber
    { get; set; }
        = string.Empty;

    public string
        ToAccountNumber
    { get; set; }
        = string.Empty;

    public DateTime
        CreatedAt
    { get; set; }
}
public enum TransactionType
{
    Deposit = 0,
    Withdraw = 1,
    Transfer = 2
}
public class CreateAccountRequest
{
    public string OwnerName
    { get; set; }
        = string.Empty;
}