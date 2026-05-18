namespace terminal_app;

/// <summary>
/// Form1-ийн дизайнерын хэсэг.
/// Контролуудын тодорхойлолт, байршил, өнгө, фонтыг энд тохируулна.
/// </summary>
partial class Form1
{
    /// <summary>Дизайнерын шаардлагатай хувьсагч.</summary>
    private System.ComponentModel.IContainer? components = null;

    /// <summary>Тасалбар авах товч.</summary>
    private Button btnTakeTicket = null!;

    /// <summary>Тасалбарын дугаарыг харуулах том шошго.</summary>
    private Label lblTicket = null!;

    /// <summary>Банкны нэрийн гарчгийн шошго.</summary>
    private Label lblTitle = null!;

    /// <summary>
    /// Ашигласан нөөцийг чөлөөлнө.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    /// <summary>
    /// Бүх контролыг үүсгэж цонхны анхны тохиргоог хийнэ.
    /// </summary>
    private void InitializeComponent()
    {
        // ── Өнгөний тодорхойлолт ──────────────────────────────────
        var bgColor = Color.FromArgb(10, 18, 35);      // гүн хар-хөх дэвсгэр
        var cardColor = Color.FromArgb(16, 28, 54);      // картны дэвсгэр
        var accentColor = Color.FromArgb(74, 158, 255);    // тод цэнхэр акцент
        var textPrimary = Color.FromArgb(220, 235, 255);   // гол бичвэр
        var textMuted = Color.FromArgb(90, 120, 175);    // дэд бичвэр

        // ── Фонтын тодорхойлолт ───────────────────────────────────
        var fontTitle = new System.Drawing.Font("Arial", 13f, FontStyle.Bold, GraphicsUnit.Point);
        var fontSubtitle = new System.Drawing.Font("Arial", 9f, FontStyle.Regular, GraphicsUnit.Point);
        var fontNumber = new System.Drawing.Font("Impact", 52f, FontStyle.Regular, GraphicsUnit.Point);
        var fontButton = new System.Drawing.Font("Arial", 11f, FontStyle.Bold, GraphicsUnit.Point);

        SuspendLayout();

        // ── Банкны нэрийн гарчиг ──────────────────────────────────
        lblTitle = new Label
        {
            Text = "TICKET TERMINAL",
            Font = fontTitle,
            ForeColor = textPrimary,
            BackColor = Color.Transparent,
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Bounds = new Rectangle(0, 32, 480, 36)
        };

        // ── Тасалбарын дугаарын том шошго ─────────────────────────
        // Дугаар ирэхэд энэ шошгод харуулна; анхны утга "---"
        lblTicket = new Label
        {
            Text = "---",
            Font = fontNumber,
            ForeColor = accentColor,
            BackColor = Color.Transparent,
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Bounds = new Rectangle(80, 155, 320, 110)
        };

        // ── Тасалбар авах товч ────────────────────────────────────
        btnTakeTicket = new Button
        {
            Text = "TAKE TICKET",
            Font = fontButton,
            ForeColor = bgColor,
            BackColor = accentColor,
            FlatStyle = FlatStyle.Flat,
            AutoSize = false,
            Bounds = new Rectangle(140, 295, 200, 48),
            Cursor = Cursors.Hand,
            UseVisualStyleBackColor = false
        };
        btnTakeTicket.FlatAppearance.BorderSize = 0;
        btnTakeTicket.FlatAppearance.MouseOverBackColor = Color.FromArgb(100, 175, 255);
        btnTakeTicket.FlatAppearance.MouseDownBackColor = Color.FromArgb(50, 130, 220);
        btnTakeTicket.Click += btnTakeTicket_Click;

        // ── Цонхны ерөнхий тохиргоо ──────────────────────────────
        BackColor = bgColor;
        ClientSize = new Size(480, 380);
        AutoScaleDimensions = new SizeF(7f, 15f);
        AutoScaleMode = AutoScaleMode.Font;
        DoubleBuffered = true;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "Form1";
        Text = "Ticket Terminal";
        StartPosition = FormStartPosition.CenterScreen;

        Controls.AddRange(new Control[]
        {
            lblTitle,
            lblTicket,
            btnTakeTicket
        });

        ResumeLayout(false);
    }
}