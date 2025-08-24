using GooseShared;
using System;
using System.Drawing;
using System.Threading.Tasks;

public static class SpeechBubble
{
    private static string _text = string.Empty;
    private static bool _visible = false;

    // Attach this renderer in ModMain
    public static void Draw(GooseEntity g, Graphics gfx)
    {
        if (!_visible || string.IsNullOrEmpty(_text))
            return;

        using (var font = new Font("Arial", 12))
        {
            SizeF size = gfx.MeasureString(_text, font);
            const int pad = 6;
            RectangleF rect = new RectangleF(
                g.position.x - size.Width / 2 - pad,
                g.position.y - 40 - size.Height - pad,
                size.Width + pad * 2,
                size.Height + pad * 2
            );

            using (var brush = new SolidBrush(Color.White))
                gfx.FillRectangle(brush, rect);
            using (var pen = new Pen(Color.Black, 2))
                gfx.DrawRectangle(pen, rect);
            using (var brush2 = new SolidBrush(Color.Black))
                gfx.DrawString(_text, font, brush2, rect.X + pad, rect.Y + pad);
        }
    }

    public static void Speak(string message)
    {
        _text = message;
        _visible = true;

        // Calculate duration: base 2 seconds + 0.05 seconds per character
        double seconds = 2 + (message.Length * 0.05);

        Task.Factory.StartNew(() =>
        {
            Task.Delay(TimeSpan.FromSeconds(seconds)).Wait();
            _visible = false;
        });
    }
}
