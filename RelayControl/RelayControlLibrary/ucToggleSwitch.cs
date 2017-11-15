using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace RelayControlLibrary
{
    public partial class ucToggleSwitch : CheckBox
    {
        public ucToggleSwitch()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            Padding = new Padding(6);
            InitializeComponent();
        }

        public Color CheckedColor = Color.Green;
        public Color UncheckedColor = Color.WhiteSmoke;

        protected override void OnPaint(PaintEventArgs pEA)
        {
            OnPaintBackground(pEA);
            pEA.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = new GraphicsPath())
            {
                var d = Padding.All;
                var r = this.Height - 2 * d;
                path.AddArc(d, d, r, r, 90, 180);
                path.AddArc(this.Width - r - d, d, r, r, -90, 180);
                path.CloseFigure();
                pEA.Graphics.FillPath(Checked ? Brushes.DarkGray : Brushes.LightGray, path);
                r = Height -  1;
                var rect = Checked ? new Rectangle(Width - r - 1, 0, r, r)
                    : new Rectangle(0, 0, r, r);
                pEA.Graphics.FillEllipse(Checked ? new SolidBrush(CheckedColor): new SolidBrush(UncheckedColor), rect);
            }
        }
    }
}
