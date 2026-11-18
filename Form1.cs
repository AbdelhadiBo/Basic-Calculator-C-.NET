using System.Globalization;

namespace Calculatrice;

public partial class Form1 : Form
{
    private double _acc;
    private string? _op;
    private bool _fresh = true, _error;

    private static readonly Dictionary<string, Func<double, double, double>> Ops = new()
    {
        ["+"] = (a, b) => a + b,
        ["−"] = (a, b) => a - b,
        ["×"] = (a, b) => a * b,
        ["÷"] = (a, b) => b == 0 ? throw new DivideByZeroException() : a / b
    };

    public Form1()
    {
        InitializeComponent();
        KeyDown += OnKey;
        foreach (Button b in keyPanel.Controls.OfType<Button>())
            b.Click += (_, _) => Press(b.Text);
    }

    private void Press(string key)
    {
        if (key.Length == 1 && char.IsDigit(key[0])) Input(key);
        else if (key is "," or ".") Input(",");
        else if (Ops.ContainsKey(key)) Operation(key);
        else if (key == "=") Equals();
        else if (key == "C") Clear();
        else if (key == "⌫") Backspace();
        else if (key == "±") Negate();
    }

    private double Display => double.TryParse(txtDisplay.Text.Replace(',', '.'),
        NumberStyles.Any, CultureInfo.InvariantCulture, out double v) ? v : 0;

    private void Show(double v)
    {
        _error = false;
        txtDisplay.Text = v.ToString("G15", CultureInfo.InvariantCulture).Replace('.', ',');
    }

    private void Input(string s)
    {
        if (_error) Clear();
        if (_fresh)
        {
            txtDisplay.Text = s == "," ? "0," : s;
            _fresh = false;
        }
        else if (s == ",")
        {
            if (!txtDisplay.Text.Contains(',')) txtDisplay.Text += ",";
        }
        else if (txtDisplay.Text != "0" || s != "0")
            txtDisplay.Text = txtDisplay.Text == "0" ? s : txtDisplay.Text + s;
    }

    private void Operation(string op)
    {
        if (_error) return;
        if (_op is not null && !_fresh)
        {
            if (!Compute()) return;
        }
        else _acc = Display;
        _op = op;
        _fresh = true;
    }

    private void Equals()
    {
        if (!_error && _op is not null && Compute()) _op = null;
    }

    private bool Compute()
    {
        try
        {
            double r = Ops[_op!](_acc, Display);
            _acc = r;
            Show(r);
            return true;
        }
        catch
        {
            _error = true;
            txtDisplay.Text = "Erreur";
            _acc = 0;
            _op = null;
            _fresh = true;
            return false;
        }
    }

    private void Clear()
    {
        txtDisplay.Text = "0";
        _acc = 0;
        _op = null;
        _fresh = true;
        _error = false;
    }

    private void Backspace()
    {
        if (_error || _fresh) return;
        string t = txtDisplay.Text;
        t = t.Length > 1 ? t[..^1] : "0";
        if (t is "" or "-" or "0,") t = "0";
        txtDisplay.Text = t;
        if (t == "0") _fresh = true;
    }

    private void Negate()
    {
        if (_error || txtDisplay.Text == "0") return;
        string t = txtDisplay.Text;
        txtDisplay.Text = t.StartsWith('-') ? t[1..] : "-" + t;
    }

    private void OnKey(object? sender, KeyEventArgs e)
    {
        string? key = e.KeyCode switch
        {
            >= Keys.D0 and <= Keys.D9 => ((char)('0' + e.KeyCode - Keys.D0)).ToString(),
            >= Keys.NumPad0 and <= Keys.NumPad9 => ((char)('0' + e.KeyCode - Keys.NumPad0)).ToString(),
            Keys.OemPeriod or Keys.Decimal => ",",
            Keys.Add => "+",
            Keys.Oemplus when e.Shift => "+",
            Keys.Subtract or Keys.OemMinus => "−",
            Keys.Multiply => "×",
            Keys.Divide => "÷",
            Keys.Oemplus or Keys.Enter or Keys.Return => "=",
            Keys.Back => "⌫",
            Keys.Escape or Keys.Delete => "C",
            _ => null
        };
        if (key is not null) Press(key);
    }
}
