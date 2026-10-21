using System;
using System.Drawing;
using System.Windows.Forms;

namespace RelayControl
{
    public class ButtonPulseHelper : IDisposable
    {
        private readonly Button _button;
        private readonly Color _baseColor;
        private readonly Color _highlightColor;
        private readonly Timer _pulseTimer;
        private readonly Timer _restoreTimer;
        private readonly int _restoreDelayMs;

        private float _t;
        private bool _forward = true;

        public ButtonPulseHelper(
            Button button,
            Color baseColor,
            Color highlightColor,
            int intervalMs = 80,
            int restoreDelayMs = 300)
        {
            _button = button;
            _baseColor = baseColor;
            _highlightColor = highlightColor;
            _restoreDelayMs = restoreDelayMs;

            _pulseTimer = new Timer
            {
                Interval = intervalMs
            };
            _pulseTimer.Tick += PulseTimer_Tick;

            _restoreTimer = new Timer
            {
                Interval = restoreDelayMs
            };
            _restoreTimer.Tick += RestoreTimer_Tick;
        }

        public void Start()
        {
            _t = 0f;
            _forward = true;

            _button.Enabled = true;
            _button.UseVisualStyleBackColor = false;
            _button.FlatStyle = FlatStyle.Flat;

            _restoreTimer.Stop();
            _pulseTimer.Start();
        }

        public void Stop()
        {
            _pulseTimer.Stop();
            _restoreTimer.Stop();

            _button.Enabled = true;
            _button.UseVisualStyleBackColor = false;
            _button.FlatStyle = FlatStyle.Flat;
            _button.BackColor = _baseColor;
            _button.Refresh();
        }

        private void PulseTimer_Tick(object sender, EventArgs e)
        {
            const float step = 0.06f;

            _t += _forward ? step : -step;

            if (_t >= 1f)
            {
                _t = 1f;
                _forward = false;
            }
            else if (_t <= 0f)
            {
                _t = 0f;
                _forward = true;
            }

            _button.BackColor = LerpColor(_baseColor, _highlightColor, _t);

            // once it reaches full highlight, wait a moment, then restore
            if (_t >= 1f)
            {
                _pulseTimer.Stop();
                _restoreTimer.Start();
            }
        }

        private void RestoreTimer_Tick(object sender, EventArgs e)
        {
            _restoreTimer.Stop();
            _button.BackColor = _baseColor;
            _button.Refresh();
        }

        private static Color LerpColor(Color from, Color to, float t)
        {
            int r = (int)(from.R + (to.R - from.R) * t);
            int g = (int)(from.G + (to.G - from.G) * t);
            int b = (int)(from.B + (to.B - from.B) * t);
            return Color.FromArgb(r, g, b);
        }

        public void Dispose()
        {
            Stop();
            _pulseTimer.Dispose();
            _restoreTimer.Dispose();
        }
    }
}