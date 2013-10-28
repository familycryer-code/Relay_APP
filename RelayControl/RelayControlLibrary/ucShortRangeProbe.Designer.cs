namespace RelayControlLibrary
{
    partial class ucShortRangeProbe
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
            this.numericUpDownIDNumber = new System.Windows.Forms.NumericUpDown();
            this.labelID = new System.Windows.Forms.Label();
            this.textBoxSignal133KHz = new System.Windows.Forms.TextBox();
            this.textBoxAge133KHz = new System.Windows.Forms.TextBox();
            this.labelSignal133KHz = new System.Windows.Forms.Label();
            this.labelAge133KHz = new System.Windows.Forms.Label();
            this.labelAge153KHz = new System.Windows.Forms.Label();
            this.label153KHz = new System.Windows.Forms.Label();
            this.textBoxAge153KHz = new System.Windows.Forms.TextBox();
            this.textBoxSignal153KHz = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIDNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDownIDNumber
            // 
            this.numericUpDownIDNumber.Location = new System.Drawing.Point(30, 3);
            this.numericUpDownIDNumber.Maximum = new decimal(new int[] {
            1023,
            0,
            0,
            0});
            this.numericUpDownIDNumber.Name = "numericUpDownIDNumber";
            this.numericUpDownIDNumber.Size = new System.Drawing.Size(90, 20);
            this.numericUpDownIDNumber.TabIndex = 1;
            //this.numericUpDownIDNumber.Leave += new System.EventHandler(this.numericUpDownIDNumber_Leave);
            // 
            // labelID
            // 
            this.labelID.AutoSize = true;
            this.labelID.Location = new System.Drawing.Point(3, 5);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(21, 13);
            this.labelID.TabIndex = 3;
            this.labelID.Text = "ID:";
            // 
            // textBoxSignal133KHz
            // 
            this.textBoxSignal133KHz.Location = new System.Drawing.Point(186, 2);
            this.textBoxSignal133KHz.Name = "textBoxSignal133KHz";
            this.textBoxSignal133KHz.Size = new System.Drawing.Size(41, 20);
            this.textBoxSignal133KHz.TabIndex = 4;
            // 
            // textBoxAge133KHz
            // 
            this.textBoxAge133KHz.Location = new System.Drawing.Point(268, 2);
            this.textBoxAge133KHz.Name = "textBoxAge133KHz";
            this.textBoxAge133KHz.Size = new System.Drawing.Size(35, 20);
            this.textBoxAge133KHz.TabIndex = 5;
            // 
            // labelSignal133KHz
            // 
            this.labelSignal133KHz.AutoSize = true;
            this.labelSignal133KHz.Location = new System.Drawing.Point(129, 5);
            this.labelSignal133KHz.Name = "labelSignal133KHz";
            this.labelSignal133KHz.Size = new System.Drawing.Size(44, 13);
            this.labelSignal133KHz.TabIndex = 6;
            this.labelSignal133KHz.Text = "Chan 4:";
            // 
            // labelAge133KHz
            // 
            this.labelAge133KHz.AutoSize = true;
            this.labelAge133KHz.Location = new System.Drawing.Point(233, 5);
            this.labelAge133KHz.Name = "labelAge133KHz";
            this.labelAge133KHz.Size = new System.Drawing.Size(29, 13);
            this.labelAge133KHz.TabIndex = 7;
            this.labelAge133KHz.Text = "Age:";
            // 
            // labelAge153KHz
            // 
            this.labelAge153KHz.AutoSize = true;
            this.labelAge153KHz.Location = new System.Drawing.Point(460, 5);
            this.labelAge153KHz.Name = "labelAge153KHz";
            this.labelAge153KHz.Size = new System.Drawing.Size(29, 13);
            this.labelAge153KHz.TabIndex = 11;
            this.labelAge153KHz.Text = "Age:";
            // 
            // label153KHz
            // 
            this.label153KHz.AutoSize = true;
            this.label153KHz.Location = new System.Drawing.Point(356, 5);
            this.label153KHz.Name = "label153KHz";
            this.label153KHz.Size = new System.Drawing.Size(44, 13);
            this.label153KHz.TabIndex = 10;
            this.label153KHz.Text = "Chan 5:";
            // 
            // textBoxAge153KHz
            // 
            this.textBoxAge153KHz.Location = new System.Drawing.Point(495, 2);
            this.textBoxAge153KHz.Name = "textBoxAge153KHz";
            this.textBoxAge153KHz.Size = new System.Drawing.Size(35, 20);
            this.textBoxAge153KHz.TabIndex = 9;
            // 
            // textBoxSignal153KHz
            // 
            this.textBoxSignal153KHz.Location = new System.Drawing.Point(413, 2);
            this.textBoxSignal153KHz.Name = "textBoxSignal153KHz";
            this.textBoxSignal153KHz.Size = new System.Drawing.Size(41, 20);
            this.textBoxSignal153KHz.TabIndex = 8;
            // 
            // ucShortRangeProbe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelAge153KHz);
            this.Controls.Add(this.label153KHz);
            this.Controls.Add(this.textBoxAge153KHz);
            this.Controls.Add(this.textBoxSignal153KHz);
            this.Controls.Add(this.labelAge133KHz);
            this.Controls.Add(this.labelSignal133KHz);
            this.Controls.Add(this.textBoxAge133KHz);
            this.Controls.Add(this.textBoxSignal133KHz);
            this.Controls.Add(this.labelID);
            this.Controls.Add(this.numericUpDownIDNumber);
            this.Name = "ucShortRangeProbe";
            this.Size = new System.Drawing.Size(536, 26);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIDNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numericUpDownIDNumber;
        private System.Windows.Forms.Label labelID;
        private System.Windows.Forms.TextBox textBoxSignal133KHz;
        private System.Windows.Forms.TextBox textBoxAge133KHz;
        private System.Windows.Forms.Label labelSignal133KHz;
        private System.Windows.Forms.Label labelAge133KHz;
        private System.Windows.Forms.Label labelAge153KHz;
        private System.Windows.Forms.Label label153KHz;
        private System.Windows.Forms.TextBox textBoxAge153KHz;
        private System.Windows.Forms.TextBox textBoxSignal153KHz;
    }
}
