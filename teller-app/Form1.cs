using System.Net.Http.Json;

namespace teller_app;

using System.ComponentModel;
using System.Text.Json;

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
            1,2,3
            });

        cmbTeller.SelectedIndex = 0;
        cmbTeller.SelectedIndexChanged +=
    cmbTeller_SelectedIndexChanged;


        SetupUI();
    }

    private int
        TellerNumber =>
            (int)cmbTeller
                .SelectedItem!;

    private async void
cmbTeller_SelectedIndexChanged(
    object sender,
    EventArgs e)
    {
        await LoadCurrent();
    }

    private async void
   btnNext_Click(
       object sender,
       EventArgs e)
    {
        if (lblTicket.Text != "----")
        {
            MessageBox.Show(
                "Одоогийн харилцагчийн үйлчилгээг дуусгана уу.",
                "Анхаар",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        await _http.PostAsync(
            $"/api/ticket/next/{TellerNumber}",
            null);

        await LoadCurrent();
    }

    private async void
        btnComplete_Click(
            object sender,
            EventArgs e)
    {
     
        await _http
            .PostAsync(
                $"/api/ticket/complete/{TellerNumber}",
                null);

        await LoadCurrent();
    }

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
    private void SetupUI()
    {
        // FORM
        BackColor =
            Color.FromArgb(
                10, 25, 47);

        Text =
            "BANK TELLER SYSTEM";

        WindowState =
            FormWindowState.Maximized;



        // =========================
        // QUEUE
        // =========================
        var panelQueue =
            new Panel();

        panelQueue.Size =
            new Size(
                400,
                550);

        panelQueue.Location =
            new Point(
                40,
                40);

        panelQueue.BackColor =
            Color.FromArgb(
                17, 34, 64);

        panelQueue.Anchor =
            AnchorStyles.Top |
            AnchorStyles.Left;

        Controls.Add(
            panelQueue);


        panelQueue.Controls.Add(
            cmbTeller);

        panelQueue.Controls.Add(
            lblTicket);

        panelQueue.Controls.Add(
            btnNext);

        panelQueue.Controls.Add(
            btnComplete);

        panelQueue.Controls.Add(
            btnCancel);


        cmbTeller.Location =
            new Point(
                80,
                30);

        cmbTeller.Size =
            new Size(
                220,
                45);


        lblTicket.Size =
            new Size(
                300,
                90);

        lblTicket.Location =
            new Point(
                50,
                110);

        lblTicket.TextAlign =
            ContentAlignment.MiddleCenter;

        lblTicket.Font =
            new Font(
                "Segoe UI",
                30,
                FontStyle.Bold);

        lblTicket.ForeColor =
            Color.Lime;


        btnNext.Location =
            new Point(
                110,
                230);

        btnComplete.Location =
            new Point(
                110,
                310);

        btnCancel.Location =
            new Point(
                110,
                390);



        // =========================
        // ACCOUNT
        // =========================
        var panelAccount =
            new Panel();

        panelAccount.Size =
            new Size(
                550,
                950);

        panelAccount.Location =
            new Point(
                470,
                40);

        panelAccount.BackColor =
            Color.FromArgb(
                17, 34, 64);

        panelAccount.Anchor =
            AnchorStyles.Top |
            AnchorStyles.Left;

        Controls.Add(
            panelAccount);


        panelAccount.Controls.Add(
            txtOwnerName);

        panelAccount.Controls.Add(
            btnCreateAccount);

        panelAccount.Controls.Add(
            txtAccountNumber);

        panelAccount.Controls.Add(
            btnSearch);

        panelAccount.Controls.Add(
            lblName);

        panelAccount.Controls.Add(
            lblBalance);

        panelAccount.Controls.Add(
            txtAmount);

        panelAccount.Controls.Add(
            txtToAccount);

        panelAccount.Controls.Add(
            btnDeposit);

        panelAccount.Controls.Add(
            btnWithdraw);

        panelAccount.Controls.Add(
            btnTransfer);

        panelAccount.Controls.Add(
            btnHistory);

        panelAccount.Controls.Add(
            btnCloseAccount);



        txtOwnerName.Location =
            new Point(
                70,
                40);

        btnCreateAccount.Location =
            new Point(
                70,
                110);


        txtAccountNumber.Location =
            new Point(
                70,
                220);

        btnSearch.Location =
            new Point(
                70,
                290);


        lblName.Location =
            new Point(
                70,
                390);

        lblBalance.Location =
            new Point(
                70,
                440);


        txtAmount.Location =
            new Point(
                70,
                540);

        txtToAccount.Location =
            new Point(
                70,
                610);


        btnDeposit.Location =
            new Point(
                70,
                690);

        btnHistory.Location =
            new Point(
                270,
                690);


        btnWithdraw.Location =
            new Point(
                70,
                760);

        btnCloseAccount.Location =
            new Point(
                270,
                760);


        btnTransfer.Location =
            new Point(
                170,
                830);



        // =========================
        // HISTORY
        // =========================
        gridHistory.Location =
            new Point(
                1080,
                40);

        gridHistory.Size =
            new Size(
                760,
                520);

        gridHistory.Anchor =
            AnchorStyles.Top |
            AnchorStyles.Left |
            AnchorStyles.Right;


        gridHistory.BackgroundColor =
            Color.FromArgb(
                17, 34, 64);

        gridHistory.BorderStyle =
            BorderStyle.None;

        gridHistory.EnableHeadersVisualStyles =
            false;

        gridHistory.ColumnHeadersDefaultCellStyle.BackColor =
            Color.FromArgb(
                0, 174, 239);

        gridHistory.ColumnHeadersDefaultCellStyle.ForeColor =
            Color.White;

        gridHistory.DefaultCellStyle.BackColor =
            Color.FromArgb(
                25, 45, 70);

        gridHistory.DefaultCellStyle.ForeColor =
            Color.White;

        gridHistory.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill;



        // =========================
        // EXCHANGE
        // =========================
        var panelExchange =
            new Panel();

        panelExchange.Size =
            new Size(
                450,
                350);

        panelExchange.Location =
            new Point(
                1200,
                620);

        panelExchange.BackColor =
            Color.FromArgb(
                17, 34, 64);

        panelExchange.Anchor =
            AnchorStyles.Top |
            AnchorStyles.Right;

        Controls.Add(
            panelExchange);


        panelExchange.Controls.Add(
            txtCurrency);

        panelExchange.Controls.Add(
            txtBuyRate);

        panelExchange.Controls.Add(
            txtSellRate);

        panelExchange.Controls.Add(
            btnUpdateRate);


        txtCurrency.Location =
            new Point(
                25,
                40);

        txtBuyRate.Location =
            new Point(
                25,
                110);

        txtSellRate.Location =
            new Point(
                25,
                180);

        btnUpdateRate.Location =
            new Point(
                130,
                260);



        // STYLING
        StyleButton(btnNext);
        StyleButton(btnComplete);
        StyleButton(btnCancel);

        StyleButton(btnCreateAccount);
        StyleButton(btnSearch);

        StyleButton(btnDeposit);
        StyleButton(btnWithdraw);
        StyleButton(btnTransfer);

        StyleButton(btnHistory);
        StyleButton(btnCloseAccount);

        StyleButton(btnUpdateRate);


        StyleTextBox(txtOwnerName);
        StyleTextBox(txtAccountNumber);

        StyleTextBox(txtAmount);
        StyleTextBox(txtToAccount);

        StyleTextBox(txtCurrency);
        StyleTextBox(txtBuyRate);
        StyleTextBox(txtSellRate);


        lblName.ForeColor =
            Color.White;

        lblBalance.ForeColor =
            Color.Gold;
    }
    private void StyleButton(
    Button btn)
    {
        btn.BackColor =
            Color.FromArgb(
                0, 174, 239);

        btn.ForeColor =
            Color.White;

        btn.FlatStyle =
            FlatStyle.Flat;

        btn.FlatAppearance.BorderSize =
            0;

        btn.Font =
            new Font(
                "Segoe UI",
                12,
                FontStyle.Bold);

        btn.Size =
            new Size(
                180,
                55);
    }


    private void StyleTextBox(
        TextBox txt)
    {
        txt.BackColor =
            Color.FromArgb(
                25, 45, 70);

        txt.ForeColor =
            Color.White;

        txt.BorderStyle =
            BorderStyle.FixedSingle;

        txt.Font =
            new Font(
                "Segoe UI",
                14,
                FontStyle.Bold);

        txt.Size =
            new Size(
                400,
                45);
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