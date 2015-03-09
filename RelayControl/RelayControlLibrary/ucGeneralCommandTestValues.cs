using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RelayControlLibrary
{
    public partial class ucGeneralCommandTestValues : UserControl
    {
        public ucGeneralCommandTestValues()
        {
            InitializeComponent();
        }

        public ucGeneralCommandTestValues(int numberOfBoxes, Size s)
        {
            InitializeComponent();
            this.Size = s;
            this.generateBoxes(numberOfBoxes);
        }

        private void generateBoxes(int numberOfBoxes)
        {
            Point p = new Point(5, 5);
            Size s = new Size(100, 20);

            for (int i = 0; i < numberOfBoxes; i++)
            {
                TextBox workingBox = new TextBox();
                workingBox.Location = p;

                p = new Point(p.X, p.Y + s.Height);

                if (p.Y >= this.Height - s.Height)
                    p = new Point(5 + s.Width + p.X, 5);

                workingBox.Visible = true;
                this.Controls.Add(workingBox);
            }
        }

        public void SetValues(byte[] b)
        {
            int i = 0;
            foreach (TextBox tB in this.Controls)
            {
                UInt16 temp = b[i + 1];
                temp <<= 8;
                temp += b[i++];
                i++;

                tB.Text = temp.ToString();
            }
        }
    }
}
