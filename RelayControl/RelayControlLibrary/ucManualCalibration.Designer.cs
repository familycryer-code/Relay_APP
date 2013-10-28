namespace RelayControlLibrary
{
    partial class ucManualCalibration
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBoxPhase = new System.Windows.Forms.ComboBox();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonMinus = new System.Windows.Forms.Button();
            this.buttonPlus = new System.Windows.Forms.Button();
            this.numericUpDownCalAmount = new System.Windows.Forms.NumericUpDown();
            this.groupBoxManualCalibration = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCalAmount)).BeginInit();
            this.groupBoxManualCalibration.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxPhase
            // 
            this.comboBoxPhase.FormattingEnabled = true;
            this.comboBoxPhase.Location = new System.Drawing.Point(49, 13);
            this.comboBoxPhase.Name = "comboBoxPhase";
            this.comboBoxPhase.Size = new System.Drawing.Size(121, 21);
            this.comboBoxPhase.TabIndex = 2;
            this.comboBoxPhase.SelectedIndexChanged += new System.EventHandler(this.comboBoxPhase_SelectedIndexChanged);
            // 
            // comboBoxType
            // 
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Location = new System.Drawing.Point(49, 40);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(121, 21);
            this.comboBoxType.TabIndex = 3;
            this.comboBoxType.SelectedIndexChanged += new System.EventHandler(this.comboBoxType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Phase";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Type";
            // 
            // buttonMinus
            // 
            this.buttonMinus.Location = new System.Drawing.Point(176, 40);
            this.buttonMinus.Name = "buttonMinus";
            this.buttonMinus.Size = new System.Drawing.Size(23, 23);
            this.buttonMinus.TabIndex = 6;
            this.buttonMinus.Text = "-";
            this.buttonMinus.UseVisualStyleBackColor = true;
            this.buttonMinus.Click += new System.EventHandler(this.buttonMinus_Click);
            // 
            // buttonPlus
            // 
            this.buttonPlus.Location = new System.Drawing.Point(176, 13);
            this.buttonPlus.Name = "buttonPlus";
            this.buttonPlus.Size = new System.Drawing.Size(23, 23);
            this.buttonPlus.TabIndex = 7;
            this.buttonPlus.Text = "+";
            this.buttonPlus.UseVisualStyleBackColor = true;
            this.buttonPlus.Click += new System.EventHandler(this.buttonPlus_Click);
            // 
            // numericUpDownCalAmount
            // 
            this.numericUpDownCalAmount.Location = new System.Drawing.Point(206, 28);
            this.numericUpDownCalAmount.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.numericUpDownCalAmount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownCalAmount.Name = "numericUpDownCalAmount";
            this.numericUpDownCalAmount.Size = new System.Drawing.Size(79, 20);
            this.numericUpDownCalAmount.TabIndex = 8;
            this.numericUpDownCalAmount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBoxManualCalibration
            // 
            this.groupBoxManualCalibration.Controls.Add(this.label1);
            this.groupBoxManualCalibration.Controls.Add(this.numericUpDownCalAmount);
            this.groupBoxManualCalibration.Controls.Add(this.label2);
            this.groupBoxManualCalibration.Controls.Add(this.comboBoxPhase);
            this.groupBoxManualCalibration.Controls.Add(this.buttonMinus);
            this.groupBoxManualCalibration.Controls.Add(this.buttonPlus);
            this.groupBoxManualCalibration.Controls.Add(this.comboBoxType);
            this.groupBoxManualCalibration.Location = new System.Drawing.Point(3, 3);
            this.groupBoxManualCalibration.Name = "groupBoxManualCalibration";
            this.groupBoxManualCalibration.Size = new System.Drawing.Size(296, 78);
            this.groupBoxManualCalibration.TabIndex = 10;
            this.groupBoxManualCalibration.TabStop = false;
            this.groupBoxManualCalibration.Text = "Manual Calibration";
            // 
            // ucManualCalibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxManualCalibration);
            this.Name = "ucManualCalibration";
            this.Size = new System.Drawing.Size(304, 84);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCalAmount)).EndInit();
            this.groupBoxManualCalibration.ResumeLayout(false);
            this.groupBoxManualCalibration.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxPhase;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonMinus;
        private System.Windows.Forms.Button buttonPlus;
        private System.Windows.Forms.NumericUpDown numericUpDownCalAmount;
        private System.Windows.Forms.GroupBox groupBoxManualCalibration;

    }
}
