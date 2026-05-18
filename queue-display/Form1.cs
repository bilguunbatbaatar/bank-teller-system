using System.Drawing.Drawing2D;
using System.Net.Sockets;
using System.Text;

namespace queue_display;

/// <summary>
/// Очерын мэдээллийг харуулах нийтийн дэлгэцийн цонх.
/// Энэ цонх нь TCP протоколоор серверт холбогдож, кассирдуудын
/// дугаарыг бодит цагт хүлээн авч дэлгэцэнд харуулна.
/// </summary>
public partial class Form1 : Form
{
    // Серверт холбогдох TCP клиент
    private readonly TcpClient _client = new();

    // Цагийн тоолуур — секунд тутам цагийн шошгыг шинэчилнэ
    private readonly System.Windows.Forms.Timer _clockTimer = new();

    // Өнгөний палитр — бүх контрол эдгээр өнгийг ашиглана

    /// <summary>Хамгийн гүн хар-хөх арын дэвсгэр өнгө.</summary>
    internal static readonly Color BgDeep = Color.FromArgb(8, 14, 26);

    /// <summary>Завгүй кассирын улаан өнгө (дугаар харагдаж байх үед).</summary>
    internal static readonly Color AccentRed = Color.FromArgb(220, 70, 70);

    /// <summary>Чөлөөт кассирын ногоон өнгө (--- харагдаж байх үед).</summary>
    internal static readonly Color AccentGreen = Color.FromArgb(34, 200, 100);

    /// <summary>Дээрх градиент зурааст харанхуй цэнхэр.</summary>
    internal static readonly Color AccentDim = Color.FromArgb(30, 80, 180);

    /// <summary>Дээрх градиент зурааст тод цэнхэр.</summary>
    internal static readonly Color AccentBlue = Color.FromArgb(74, 158, 255);

    /// <summary>Кассирын гарчгийн дунд зэргийн цэнхэр өнгө.</summary>
    internal static readonly Color TextMuted = Color.FromArgb(100, 140, 200);

    /// <summary>Идэвхгүй кассирын бараан цэнхэр өнгө.</summary>
    internal static readonly Color TextFaint = Color.FromArgb(50, 80, 140);

    /// <summary>
    /// Конструктор: контролуудыг үүсгэж, цагийн тоолуурыг эхлүүлнэ.
    /// </summary>
    public Form1()
    {
        InitializeComponent();

        // Секунд тутам цагийн шошгыг шинэчлэх тоолуур
        _clockTimer.Interval = 1000;
        _clockTimer.Tick += (_, _) =>
            lblClock.Text = DateTime.Now.ToString("HH:mm:ss");
        _clockTimer.Start();

        Load += Form1_Load;
    }

    /// <summary>
    /// Цонх ачаалагдах үед сервертэй TCP холболт үүсгэж,
    /// мэдээллийг тасралтгүй хүлээн авч эхэлнэ.
    /// Алдаа гарвал MessageBox мэдэгдэл өгнө.
    /// </summary>
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

            var stream = _client.GetStream();
            var buffer = new byte[1024];

            // Серверээс мэдээлэл тасралтгүй хүлээн авах давталт
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

                UpdateLabels(message);
            }
        }
        catch
        {
            // Холболт тасарсан эсвэл сервер байхгүй
            MessageBox.Show(
                "Cannot connect to server.",
                "Connection Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }

    /// <summary>
    /// Серверээс ирсэн очерын мэдээллээр дэлгэц дээрх
    /// бүх кассирын шошгыг шинэчилнэ.
    /// Эхлээд бүх кассирыг "---" (ногоон/чөлөөт) болгож,
    /// дараа нь мессежийг мөрөөр задлан тохирох кассирт тохируулна.
    /// </summary>
    /// <param name="message">
    /// Серверээс ирсэн мэдээлэл.
    /// Жишээ: "Teller 1: A042\nTeller 2: B017"
    /// </param>
    private void UpdateLabels(
        string message)
    {
        Invoke(() =>
        {
            // Бүх кассирыг чөлөөт байдалд оруулна (ногоон)
            SetTeller(lblNum1, lblBadge1, "---", false);
            SetTeller(lblNum2, lblBadge2, "---", false);
            SetTeller(lblNum3, lblBadge3, "---", false);

            // Мессежийг мөрөөр задлан кассир тус бүрт тохируулна
            foreach (var line in message.Split(Environment.NewLine))
            {
                if (line.StartsWith("Teller 1"))
                    SetTeller(lblNum1, lblBadge1, ExtractNumber(line), true);

                if (line.StartsWith("Teller 2"))
                    SetTeller(lblNum2, lblBadge2, ExtractNumber(line), true);

                if (line.StartsWith("Teller 3"))
                    SetTeller(lblNum3, lblBadge3, ExtractNumber(line), true);
            }
        });
    }

    /// <summary>
    /// Нэг кассирын дугаарын шошго болон төлвийн шошгыг шинэчилнэ.
    /// </summary>
    /// <param name="numLabel">Дугаарыг харуулах том шошго.</param>
    /// <param name="badgeLabel">"SERVING" / "AVAILABLE" харуулах жижиг шошго.</param>
    /// <param name="number">Харуулах дугаар (жишээ: "A042" эсвэл "---").</param>
    /// <param name="active">
    /// True бол кассир үйлчилж байна → улаан (завгүй),
    /// False бол хүлээж байна → ногоон (чөлөөт).
    /// </param>
    private static void SetTeller(
        Label numLabel,
        Label badgeLabel,
        string number,
        bool active)
    {
        numLabel.Text = number;
        numLabel.ForeColor = active ? AccentRed : AccentGreen;
        badgeLabel.Text = active ? "SERVING" : "AVAILABLE";
        badgeLabel.ForeColor = active ? AccentRed : AccentGreen;
    }

    /// <summary>
    /// "Teller N: A042" хэлбэрийн мөрөөс зөвхөн дугаарыг гаргаж авна.
    /// Хоёр цэгний ард байгаа хэсгийг буцаана.
    /// Хоёр цэг байхгүй бол "---" буцаана.
    /// </summary>
    /// <param name="line">Задлах мөр.</param>
    /// <returns>Цэвэрлэсэн дугаарын мөр.</returns>
    private static string ExtractNumber(string line)
    {
        var parts = line.Split("=>", 2, StringSplitOptions.TrimEntries);

        return parts.Length > 1
            ? parts[1]
            : "---";
    }
    /// <summary>
    /// Цонхны арын дэвсгэрийг зурна:
    /// дээрх градиент цэнхэр зураас болон
    /// гарчиг, хөл, кассирын баганыг хуваах шугамууд.
    /// </summary>
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // Дээрх градиент цэнхэр зураас (2px өндөр)
        using var accentBrush = new LinearGradientBrush(
            new Point(0, 0), new Point(Width, 0),
            Color.Transparent, Color.Transparent);
        accentBrush.InterpolationColors = new ColorBlend
        {
            Colors = new[] { Color.Transparent, AccentDim, AccentBlue, AccentDim, Color.Transparent },
            Positions = new[] { 0f, 0.25f, 0.5f, 0.75f, 1f }
        };
        g.FillRectangle(accentBrush, 0, 0, Width, 2);

        // Гарчиг болон хөлийн хэвтээ хуваах шугамууд
        using var divPen = new Pen(Color.FromArgb(40, 60, 120, 200), 1f);
        g.DrawLine(divPen, 28, 66, Width - 28, 66);
        g.DrawLine(divPen, 28, Height - 56, Width - 28, Height - 56);

        // Кассирын баганыг хуваах босоо шугамууд
        int col = Width / 3;
        g.DrawLine(divPen, col, 80, col, Height - 60);
        g.DrawLine(divPen, col * 2, 80, col * 2, Height - 60);
    }
}