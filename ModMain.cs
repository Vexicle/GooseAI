using GooseShared;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;   // for Cursor

public class ModMain : IMod
{
    private readonly TimeSpan _idleThreshold = TimeSpan.FromMinutes(2);
    private bool _aiTriggered = false, _bubbleAttached = false;

    // — click‐count fields —
    private int _clickCount = 0;
    private DateTime _lastClickTime = DateTime.MinValue;
    private const int MaxClickIntervalMs = 500;   // max ms between taps
    private const float ClickRadius = 50f;        // px tolerance

    [DllImport("User32.dll")]
    private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);

    [StructLayout(LayoutKind.Sequential)]
    private struct LASTINPUTINFO { public uint cbSize, dwTime; }

    public void Init()
    {
        InjectionPoints.PostTickEvent += OnTick;
    }

    private void OnTick(GooseEntity goose)
    {
        // Attach speech‐bubble renderer once
        if (!_bubbleAttached)
        {
            goose.render += SpeechBubble.Draw;
            _bubbleAttached = true;
        }

        // Idle trigger
        CheckIdle(goose);

        // Triple‐click trigger
        CheckClicks(goose);
    }

    private void CheckIdle(GooseEntity goose)
    {
        if (!_aiTriggered && GetIdleTime() > _idleThreshold)
        {
            new TaskAIInteraction().RunTask(goose);
            _aiTriggered = true;
        }
        else if (GetIdleTime() < TimeSpan.FromSeconds(10))
        {
            _aiTriggered = false;
        }
    }

    private void CheckClicks(GooseEntity goose)
    {
        // Only on mouse‐down edge
        const int VK_LBUTTON = 0x01;
        bool isDown = (GetAsyncKeyState(VK_LBUTTON) & 0x8000) != 0;
        if (isDown && _lastClickTime != DateTime.MinValue && (DateTime.Now - _lastClickTime).TotalMilliseconds < 50)
            return;  // still held

        if (isDown)
        {
            var now = DateTime.Now;
            // reset if too slow
            if ((now - _lastClickTime).TotalMilliseconds > MaxClickIntervalMs)
                _clickCount = 0;

            // get cursor pos in screen coords
            var cursor = Cursor.Position;
            // game coords are also screen coords for Desktop Goose
            float dx = cursor.X - goose.position.x;
            float dy = cursor.Y - goose.position.y;
            if (dx * dx + dy * dy < ClickRadius * ClickRadius)
            {
                _clickCount++;
                if (_clickCount >= 3)
                {
                    new TaskAIInteraction().RunTask(goose);
                    _clickCount = 0;
                }
            }

            _lastClickTime = now;
        }
    }

    private TimeSpan GetIdleTime()
    {
        var lastIn = new LASTINPUTINFO { cbSize = (uint)Marshal.SizeOf(typeof(LASTINPUTINFO)) };
        GetLastInputInfo(ref lastIn);
        uint idleMs = (uint)(Environment.TickCount - lastIn.dwTime);
        return TimeSpan.FromMilliseconds(idleMs);
    }
}
