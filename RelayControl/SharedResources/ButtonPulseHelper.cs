using System;
using System.Drawing;
using System.Windows.Forms;

namespace SharedResources
{
    public sealed class ButtonPulseHelper : IDisposable
    {
        private readonly Button _button;
        private readonly Color _baseColor;
        private readonly Color _highlightColor;
        private readonly Timer _pulseTimer;
        private readonly Timer _restoreTimer;
        private readonly bool _setFlatStyle;
        private readonly int _minimumVisibleMs;
        private readonly int _normalRestoreDelayMs;
        private bool _disposed;

        private float _t;
        private bool _forward = true;
        private DateTime _pulseStartedAtUtc;

        public ButtonPulseHelper(
            Button button,
            Color baseColor,
            Color highlightColor,
            int intervalMs = 80,
            int restoreDelayMs = 300,
            bool setFlatStyle = false,
            int minimumVisibleMs = 350)
        {
            _button = button ?? throw new ArgumentNullException(nameof(button));
            _baseColor = baseColor;
            _highlightColor = highlightColor;
            _setFlatStyle = setFlatStyle;
            _minimumVisibleMs = Math.Max(0, minimumVisibleMs);
            _normalRestoreDelayMs = Math.Max(1, restoreDelayMs);

            _pulseTimer = new Timer { Interval = Math.Max(1, intervalMs) };
            _pulseTimer.Tick += PulseTimer_Tick;

            _restoreTimer = new Timer { Interval = _normalRestoreDelayMs };
            _restoreTimer.Tick += RestoreTimer_Tick;

            _button.Disposed += Button_Disposed;
        }

        public void Start()
        {
            if (_disposed || _button.IsDisposed) return;
            if (!_button.IsHandleCreated) return;

            _t = 0f;
            _forward = true;
            _pulseStartedAtUtc = DateTime.UtcNow;

            _button.Enabled = true;
            _button.UseVisualStyleBackColor = false;
            if (_setFlatStyle) _button.FlatStyle = FlatStyle.Flat;

            _restoreTimer.Stop();
            _restoreTimer.Interval = _normalRestoreDelayMs; // reset any temporary interval
            _pulseTimer.Start();
        }

        public void Stop()
        {
            if (_disposed) return;

            _pulseTimer.Stop();
            _restoreTimer.Stop();
            _restoreTimer.Interval = _normalRestoreDelayMs;

            if (_button.IsDisposed) return;

            _button.Enabled = true;
            _button.UseVisualStyleBackColor = false;
            if (_setFlatStyle) _button.FlatStyle = FlatStyle.Flat;
            _button.BackColor = _baseColor;
            _button.Refresh();
        }

        private void PulseTimer_Tick(object sender, EventArgs e)
        {
            if (_disposed || _button.IsDisposed)
            {
                Dispose();
                return;
            }

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

            if (_t >= 1f)
            {
                _pulseTimer.Stop();

                var elapsedMs = (int)(DateTime.UtcNow - _pulseStartedAtUtc).TotalMilliseconds;
                var remainingForMinimum = _minimumVisibleMs - elapsedMs;

                if (remainingForMinimum > 0)
                {
                    _restoreTimer.Interval = Math.Max(50, remainingForMinimum);
                }
                else
                {
                    _restoreTimer.Interval = _normalRestoreDelayMs;
                }

                _restoreTimer.Start();
            }
        }

        private void RestoreTimer_Tick(object sender, EventArgs e)
        {
            if (_disposed || _button.IsDisposed)
            {
                Dispose();
                return;
            }

            _restoreTimer.Stop();
            _restoreTimer.Interval = _normalRestoreDelayMs; // restore default
            _button.BackColor = _baseColor;
            _button.Refresh();
        }

        private void Button_Disposed(object sender, EventArgs e)
        {
            Dispose();
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
            if (_disposed) return;
            _disposed = true;

            _pulseTimer.Stop();
            _restoreTimer.Stop();

            _pulseTimer.Tick -= PulseTimer_Tick;
            _restoreTimer.Tick -= RestoreTimer_Tick;
            _button.Disposed -= Button_Disposed;

            _pulseTimer.Dispose();
            _restoreTimer.Dispose();
        }
    }
}