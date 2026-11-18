namespace Calculatrice;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;
    private TextBox txtDisplay;
    private TableLayoutPanel keyPanel;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        SuspendLayout();

        Color dark = Color.FromArgb(30, 30, 46);
        Color num = Color.FromArgb(49, 50, 68);
        Color op = Color.FromArgb(69, 71, 90);
        Color special = Color.FromArgb(243, 139, 168);
        Color equals = Color.FromArgb(166, 227, 161);
        Color text = Color.FromArgb(205, 214, 244);
        Color opText = Color.FromArgb(137, 180, 250);

        txtDisplay = new TextBox
        {
            Dock = DockStyle.Top, Height = 90, Text = "0", ReadOnly = true,
            TextAlign = HorizontalAlignment.Right, BorderStyle = BorderStyle.None,
            BackColor = dark, ForeColor = Color.White,
            Font = new Font("Segoe UI", 32F, FontStyle.Bold),
            Padding = new Padding(0, 0, 16, 0)
        };

        keyPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 5,
            BackColor = dark, Padding = new Padding(10, 4, 10, 10)
        };
        for (int i = 0; i < 4; i++) keyPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        for (int i = 0; i < 5; i++) keyPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));

        // (texte, colonne, ligne, largeur, fond, texte)
        (string, int, int, int, Color, Color)[] keys =
        {
            ("C",  0,0,1, special, dark),   ("⌫", 1,0,1, special, dark),
            ("±",  2,0,1, special, dark),   ("÷", 3,0,1, op, opText),
            ("7",  0,1,1, num, text),       ("8", 1,1,1, num, text),
            ("9",  2,1,1, num, text),       ("×", 3,1,1, op, opText),
            ("4",  0,2,1, num, text),       ("5", 1,2,1, num, text),
            ("6",  2,2,1, num, text),       ("−", 3,2,1, op, opText),
            ("1",  0,3,1, num, text),       ("2", 1,3,1, num, text),
            ("3",  2,3,1, num, text),       ("+", 3,3,1, op, opText),
            ("0",  0,4,2, num, text),       (",", 2,4,1, num, text),
            ("=",  3,4,1, equals, dark)
        };

        foreach (var (label, col, row, span, back, fore) in keys)
        {
            Button b = new()
            {
                Text = label, Name = "btn" + label, Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat, BackColor = back, ForeColor = fore,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                Margin = new Padding(3), Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = ControlPaint.Light(back, 0.15f);
            b.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(back, 0.1f);
            keyPanel.Controls.Add(b, col, row);
            if (span > 1) keyPanel.SetColumnSpan(b, span);
        }

        Controls.Add(keyPanel);
        Controls.Add(txtDisplay);

        Text = "Calculatrice";
        ClientSize = MinimumSize = MaximumSize = new Size(340, 470);
        BackColor = dark;
        ForeColor = Color.White;
        Font = new Font("Segoe UI", 10F);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        KeyPreview = true;

        ResumeLayout(false);
    }
}
