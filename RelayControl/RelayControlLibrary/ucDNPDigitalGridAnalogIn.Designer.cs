namespace RelayControlLibrary
{
    partial class ucDNPDIGITALGRIDAnalogIn
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
            this.textBoxPointValue = new System.Windows.Forms.TextBox();
            this.labelPointName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // 
            // textBoxPointValue
            // 
            this.textBoxPointValue.BackColor = System.Drawing.SystemColors.Window;
            //this.textBoxPointValue.Location = new System.Drawing.Point(48, 0);
            this.textBoxPointValue.Location = new System.Drawing.Point(20, 0);
            this.textBoxPointValue.Name = "textBoxPointValue";
            this.textBoxPointValue.ReadOnly = true;
            this.textBoxPointValue.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBoxPointValue.Size = new System.Drawing.Size(100, 20);
            this.textBoxPointValue.TabIndex = 12;
            this.textBoxPointValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelPointName
            // 
            this.labelPointName.AutoSize = true;
           // this.labelPointName.Location = new System.Drawing.Point(154, 3);
            this.labelPointName.Location = new System.Drawing.Point(124, 3);
            this.labelPointName.Name = "labelPointName";
            this.labelPointName.Size = new System.Drawing.Size(35, 13);
            this.labelPointName.TabIndex = 11;
            this.labelPointName.Text = "label1";

            // 
            // ucDNPDigitalGridAnalogIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxPointValue);
            this.Controls.Add(this.labelPointName);
            this.Name = "ucDNPDIGITALGRIDAnalogIn";
            this.Size = new System.Drawing.Size(437, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxPointValue;
        private System.Windows.Forms.Label labelPointName;
    }
}
