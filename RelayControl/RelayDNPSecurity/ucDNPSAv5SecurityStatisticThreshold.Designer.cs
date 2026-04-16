namespace RelayDNPSecurity
{
    partial class ucDNPSAv5SecurityStatisticThreshold
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
            this.labelName = new System.Windows.Forms.Label();
            this.numericUpDownValue = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownValue)).BeginInit();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(13, 6);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(57, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "labelName";
            // 
            // numericUpDownValue
            // 
            this.numericUpDownValue.Location = new System.Drawing.Point(154, 3);
            this.numericUpDownValue.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDownValue.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownValue.Name = "numericUpDownValue";
            this.numericUpDownValue.Size = new System.Drawing.Size(51, 20);
            this.numericUpDownValue.TabIndex = 1;
            this.numericUpDownValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownValue.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ucDNPSAv5SecurityStatisticThreshold
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.numericUpDownValue);
            this.Controls.Add(this.labelName);
            this.Name = "ucDNPSAv5SecurityStatisticThreshold";
            this.Size = new System.Drawing.Size(210, 26);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.NumericUpDown numericUpDownValue;
    }
}
