using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RelayDNPSecurity
{
    public partial class ucKeyValuesInputControl : UserControl
    {
        public ucKeyValuesInputControl(int numberOfValues, string groupBoxName)
        {
            InitializeComponent();
            if(numberOfValues % 16 != 0)
                throw new Exception("Number of Values must be divisible by 16");

            this.generateBoxes(numberOfValues);
            this.setGroupBox(numberOfValues, groupBoxName);
        }

        private static int _boxWidth = 30;
        private static int _boxHeight = 20;
        private static int _locationOffset = 5;
        private GroupBox groupBox;

        private void generateBoxes(int numberOfValues)
        {
            for (int i = 0; i < numberOfValues; ++i)
            {
                TextBox workingTB;
                Point boxLocation = new Point((i%16 + _locationOffset) + (i%16 * _boxWidth), (i/16) + _locationOffset + ((i / 16) * _boxHeight));

                workingTB = new TextBox();
                workingTB.Height = _boxHeight;
                workingTB.Width = _boxWidth;

                this.Controls.Add(workingTB);
            }
        }

        private void setGroupBox(int numberOfValues, string groupBoxName)
        {
            this.groupBox = new GroupBox();
            this.groupBox.Text = groupBoxName;
            this.groupBox.Height = ((numberOfValues / 16) + 1) * _boxHeight + _locationOffset + _locationOffset;
            this.groupBox.Width = _boxWidth * 16 + _locationOffset + _locationOffset;
            this.groupBox.Location = new Point(0, 0);
        }
    }
}
