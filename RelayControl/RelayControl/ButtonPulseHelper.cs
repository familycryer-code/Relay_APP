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
        private readonly Timer _timer;
        private float _t;
        private bool _forward = true;

        public ButtonPulseHelper(Button button, Color baseColor, Color highlightColor, int intervalMs = 40)
        {
            _button = button;
            _baseColor = baseColor;
            _highlightColor = highlightColor;

            _timer = new Timer
            {
                Interval = intervalMs
            };
            _timer.Tick += Timer_Tick;
        }

        public void Start()
        {
            _t = 0f;
            _forward = true;
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
            _button.BackColor = _baseColor;
        }

        private void Timer_Tick(object sender, EventArgs e)
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
            _timer.Dispose();
        }
    }
}
