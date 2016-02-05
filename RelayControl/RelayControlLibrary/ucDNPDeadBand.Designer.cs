namespace RelayControlLibrary
{
    partial class ucDNPDeadBand
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
            this.labelUnits = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownValue)).BeginInit();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(3, 3);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(35, 13);
            this.labelName.TabIndex = 1;
            this.labelName.Text = "label1";
            // 
            // numericUpDownValue
            // 
            this.numericUpDownValue.Location = new System.Drawing.Point(147, 0);
            this.numericUpDownValue.Name = "numericUpDownValue";
            this.numericUpDownValue.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownValue.TabIndex = 2;
            // 
            // labelUnits
            // 
            this.labelUnits.AutoSize = true;
            this.labelUnits.Location = new System.Drawing.Point(271, 3);
            this.labelUnits.Name = "labelUnits";
            this.labelUnits.Size = new System.Drawing.Size(35, 13);
            this.labelUnits.TabIndex = 3;
            this.labelUnits.Text = "label2";
            // 
            // ucDNPDeadBand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelUnits);
            this.Controls.Add(this.numericUpDownValue);
            this.Controls.Add(this.labelName);
            this.Size = new System.Drawing.Size(333, 20);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.NumericUpDown numericUpDownValue;
        private System.Windows.Forms.Label labelUnits;
    }
}
