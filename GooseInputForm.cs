using System;
using System.Drawing;
using System.Windows.Forms;

public class GooseInputForm : Form
{
    private readonly TextBox _input;
    private readonly Button _ok;

    private static GooseInputForm _instance;

    public string UserInput => _input.Text;

    private GooseInputForm(Point screenPos, string prompt)
    {
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.Manual;
        BackColor = Color.White;
        Padding = new Padding(10);
        Location = screenPos;

        var label = new Label
        {
            Text = prompt,
            AutoSize = true,
            Parent = this,
            Location = new Point(Padding.Left, Padding.Top)
        };

        _input = new TextBox
        {
            Width = 200,
            Parent = this,
            Location = new Point(Padding.Left, label.Bottom + 5)
        };

        _ok = new Button
        {
            Text = "Goose!",
            Parent = this,
            Location = new Point(Padding.Left, _input.Bottom + 8),
            DialogResult = DialogResult.OK
        };
        _ok.Click += (s, e) => Close();

        _input.KeyDown += (s, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                DialogResult = DialogResult.OK;
                Close();
            }
        };

        Resize += (s, e) =>
        {
            int radius = 20;
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(Width - radius, Height - radius, radius, radius, 0, 90);
            path.AddArc(0, Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            Region = new Region(path);
        };

        Width = Padding.Horizontal + Math.Max(label.PreferredWidth, _input.Width);
        Height = _ok.Bottom + Padding.Bottom;
    }

    public static string ShowAt(Point screenPos, string prompt)
    {
        if (_instance != null)
        {
            _instance.BringToFront();
            _instance.Focus();
            return string.Empty;
        }

        _instance = new GooseInputForm(screenPos, prompt);

        _instance.Shown += (s, e) => _instance._input.Focus();

        string input = string.Empty;
        if (_instance.ShowDialog() == DialogResult.OK)
            input = _instance.UserInput;

        _instance.Dispose();
        _instance = null;

        return input;
    }
}
