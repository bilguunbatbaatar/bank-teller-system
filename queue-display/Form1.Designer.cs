namespace queue_display;

/// <summary>
/// Form1-ийн дизайнерын хэсэг.
/// Контролуудын тодорхойлолт, байршил, фонт,
/// болон цонхны анхны тохиргоо агуулагдана.
/// </summary>
partial class Form1
{
    // Дизайнерын шаардлагатай хувьсагч
    private System.ComponentModel.IContainer? components = null;

    // Гарчгийн контролууд

    /// <summary>Бодит цагийн цагийн шошго (баруун дээр).</summary>
    private Label lblClock = null!;

    // Кассир 1-ийн контролууд

    /// <summary>Кассир 1-ийн гарчгийн шошго.</summary>
    private Label lblTellerHead1 = null!;

    /// <summary>Кассир 1-ийн одоогийн дугаарын том шошго.</summary>
    private Label lblNum1 = null!;

    /// <summary>Кассир 1-ийн төлвийн шошго (SERVING / AVAILABLE).</summary>
    private Label lblBadge1 = null!;

    // Кассир 2-ийн контролууд

    /// <summary>Кассир 2-ийн гарчгийн шошго.</summary>
    private Label lblTellerHead2 = null!;

    /// <summary>Кассир 2-ийн одоогийн дугаарын том шошго.</summary>
    private Label lblNum2 = null!;

    /// <summary>Кассир 2-ийн төлвийн шошго (SERVING / AVAILABLE).</summary>
    private Label lblBadge2 = null!;

    // Кассир 3-ийн контролууд

    /// <summary>Кассир 3-ийн гарчгийн шошго.</summary>
    private Label lblTellerHead3 = null!;

    /// <summary>Кассир 3-ийн одоогийн дугаарын том шошго.</summary>
    private Label lblNum3 = null!;

    /// <summary>Кассир 3-ийн төлвийн шошго (SERVING / AVAILABLE).</summary>
    private Label lblBadge3 = null!;

    // Хөлийн контролууд

    /// <summary>Хөлийн баруун шошго (зааварчилгааны бичвэр).</summary>
    private Label lblFooterRight = null!;

    /// <summary>
    /// Ашигласан нөөцийг чөлөөлнө.
    /// </summary>
    /// <param name="disposing">
    /// True бол удирдагдах нөөцийг устгана; False бол зөвхөн удирдагдахгүй нөөцийг устгана.
    /// </param>
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    /// <summary>
    /// Дизайнерын шаардлагатай арга.
    /// Бүх контролыг үүсгэж, фонт, өнгө, байршлыг анх тохируулна.
    /// Цонхны хэмжээ өөрчлөгдөх үед LayoutControls() дуудагдана.
    /// </summary>
    private void InitializeComponent()
    {
        SuspendLayout();

        // Фонтын тодорхойлолт
        // fontNumber : кассирын дугаарт зориулсан том Impact фонт
        // fontHead   : кассирын гарчигт зориулсан Bold Arial
        // fontBadge  : SERVING/AVAILABLE шошго болон хөлд зориулсан жижиг Bold Arial
        // fontClock  : моноспейс Courier New цагт
        var fontNumber = new System.Drawing.Font("Impact", 72f, FontStyle.Regular, GraphicsUnit.Point);
        var fontHead = new System.Drawing.Font("Arial", 11f, FontStyle.Bold, GraphicsUnit.Point);
        var fontBadge = new System.Drawing.Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point);
        var fontClock = new System.Drawing.Font("Courier New", 14f, FontStyle.Regular, GraphicsUnit.Point);

        // Туслах функц: Label үүсгэж Controls-д нэмнэ
        // Давтагдах кодыг багасгах зорилгоор дотоод функц ашиглав.
        Label Make(
            string text,
            System.Drawing.Font f,
            Color fg,
            ContentAlignment align = ContentAlignment.MiddleLeft)
        {
            var l = new Label
            {
                Text = text,
                Font = f,
                ForeColor = fg,
                BackColor = Color.Transparent,
                AutoSize = false,
                TextAlign = align
            };
            Controls.Add(l);
            return l;
        }

        // Цагийн шошго (гарчгийн баруун тал)
        lblClock = Make(DateTime.Now.ToString("HH:mm:ss"), fontClock, AccentBlue,
                        ContentAlignment.MiddleRight);

        // Кассир 1-ийн контролуудыг үүсгэнэ
        lblTellerHead1 = Make("TELLER  1", fontHead, TextMuted);
        lblNum1 = Make("---", fontNumber, AccentGreen, ContentAlignment.MiddleLeft);
        lblBadge1 = Make("AVAILABLE", fontBadge, AccentGreen);

        // Кассир 2-ийн контролуудыг үүсгэнэ
        lblTellerHead2 = Make("TELLER  2", fontHead, TextMuted);
        lblNum2 = Make("---", fontNumber, AccentGreen, ContentAlignment.MiddleLeft);
        lblBadge2 = Make("AVAILABLE", fontBadge, AccentGreen);

        // Кассир 3-ийн контролуудыг үүсгэнэ
        lblTellerHead3 = Make("TELLER  3", fontHead, TextMuted);
        lblNum3 = Make("---", fontNumber, AccentGreen, ContentAlignment.MiddleLeft);
        lblBadge3 = Make("AVAILABLE", fontBadge, AccentGreen);

        // Хөлийн баруун шошго
        lblFooterRight = Make("PLEASE WAIT FOR YOUR NUMBER TO BE CALLED",
                               fontBadge, TextFaint, ContentAlignment.MiddleRight);

        // Цонхны ерөнхий тохиргоо
        AutoScaleDimensions = new SizeF(7f, 15f);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = BgDeep;
        ClientSize = new Size(1280, 480);
        DoubleBuffered = true;
        FormBorderStyle = FormBorderStyle.None;
        Name = "Form1";
        Text = "Queue Display";

        // Цонхны хэмжээ өөрчлөгдөх бүрт байршлыг дахин тооцоолно
        Resize += (_, _) => LayoutControls();

        // Escape товчоор цонхыг хаана
        KeyPreview = true;
        KeyDown += (_, e) => { if (e.KeyCode == Keys.Escape) Close(); };

        ResumeLayout(false);
        LayoutControls();
    }

    /// <summary>
    /// Бүх контролын байршил, хэмжээг цонхны одоогийн хэмжээнд
    /// тулгуурлан динамикаар тооцоолж тохируулна.
    /// InitializeComponent()-ээс болон Resize үйл явдлаас дуудагдана.
    /// </summary>
    private void LayoutControls()
    {
        int W = ClientSize.Width;
        int H = ClientSize.Height;
        int pad = 28;
        int col = W / 3;

        // Гарчгийн мөр
        lblClock.SetBounds(W - 175, 14, 148, 44);

        // Агуулгын талбарын үндсэн координатууд
        int contentY = 72;
        int contentH = H - 60 - contentY;

        // Нэг кассирын гурван контролыг байрлуулах туслах функц
        void PositionTeller(Label head, Label num, Label badge, int colIdx)
        {
            int x = col * colIdx + pad;
            head.SetBounds(x, contentY + 12, col - pad * 2, 24);
            num.SetBounds(x, contentY + 38, col - pad, contentH - 72);
            badge.SetBounds(x, contentY + contentH - 34, col - pad * 2, 22);
        }

        PositionTeller(lblTellerHead1, lblNum1, lblBadge1, 0);
        PositionTeller(lblTellerHead2, lblNum2, lblBadge2, 1);
        PositionTeller(lblTellerHead3, lblNum3, lblBadge3, 2);

        // Хөлийн мөр
        lblFooterRight.SetBounds(pad, H - 52, W - pad * 2, 40);

        // Арын зургийг дахин зурна
        Invalidate();
    }
}