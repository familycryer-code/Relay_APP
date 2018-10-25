using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RelayControl
{
    public partial class CTRatioCaculator : Form
    {
        public CTRatioCaculator()
        {
            InitializeComponent();
            this.myInitialize();
        }

        private void myInitialize()
        {
            this.radioButtonCT450.Checked = true;
            this.radioButtonKVA150.Checked = true;
            this.radioButtonSec120.Checked = true;
            this.calculateCTRatio();
        }

        #region Calculation

        private void calculateCTRatio()
        {
            float cTValue, cTSize, secondaryVoltage, kVA;

            cTSize = this.getCTSize();
            secondaryVoltage = this.getSecondaryVoltage();
            kVA = this.getKVA();

            cTValue = 3f * secondaryVoltage;
            cTValue /= kVA * 1000f;
            cTValue *= cTSize * 100;

            cTValue = (float)Math.Round(cTValue, 0);

            this.textBoxCTRatioValue.Text = cTValue.ToString();
        }

        private float getCTSize()
        {
            if (this.radioButtonCT1200.Checked) return 1200f;
            if (this.radioButtonCT1500.Checked) return 1500f;
            if (this.radioButtonCT1600.Checked) return 1600f;
            if (this.radioButtonCT2000.Checked) return 2000f;
            if (this.radioButtonCT2500.Checked) return 2500f;
            if (this.radioButtonCT3000.Checked) return 3000f;
            if (this.radioButtonCT3500.Checked) return 3500f;
            if (this.radioButtonCT450.Checked) return 450f;
            if (this.radioButtonCT480.Checked) return 480f;
            if (this.radioButtonCT720.Checked) return 720f;
            if (this.radioButtonCT800.Checked) return 800f;
            if (this.radioButtonCT960.Checked) return 960f;

            return 450f;
        }

        private float getSecondaryVoltage()
        {
            if (this.radioButtonSec120.Checked) return 120f;
            if (this.radioButtonSec125.Checked) return 125f;
            if (this.radioButtonSec265.Checked) return 265f;
            if (this.radioButtonSec277.Checked) return 277f;
            if (this.radioButtonSec347.Checked) return 347f;

            return 120f;
        }

        private float getKVA()
        {
            if (this.radioButtonKVA1000.Checked) return 1000f;
            if (this.radioButtonKVA1120.Checked) return 1120f;
            if (this.radioButtonKVA1250.Checked) return 1250f;
            if (this.radioButtonKVA150.Checked) return 150f;
            if (this.radioButtonKVA1500.Checked) return 1500f;
            if (this.radioButtonKVA2000.Checked) return 2000f;
            if (this.radioButtonKVA2240.Checked) return 2240f;
            if (this.radioButtonKVA2500.Checked) return 2500f;
            if (this.radioButtonKVA2800.Checked) return 2800f;
            if (this.radioButtonKVA300.Checked) return 300f;
            if (this.radioButtonKVA3000.Checked) return 3000f;
            if (this.radioButtonKVA3750.Checked) return 3750f;
            if (this.radioButtonKVA450.Checked) return 450f;
            if (this.radioButtonKVA500.Checked) return 500f;
            if (this.radioButtonKVA560.Checked) return 560f;
            if (this.radioButtonKVA750.Checked) return 750f;

            return 150f;
        }

        #endregion

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.calculateCTRatio();
        }
    }
}