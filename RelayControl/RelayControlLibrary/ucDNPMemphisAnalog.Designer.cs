namespace RelayControlLibrary
{
    partial class ucDNPMemphisAnalog
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
            this.labelPointNumber = new System.Windows.Forms.Label();
            this.labelPointName = new System.Windows.Forms.Label();
            this.textBoxPointValue = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelPointNumber
            // 
            this.labelPointNumber.AutoSize = true;
            this.labelPointNumber.Location = new System.Drawing.Point(3, 3);
            this.labelPointNumber.Name = "labelPointNumber";
            this.labelPointNumber.Size = new System.Drawing.Size(35, 13);
            this.labelPointNumber.TabIndex = 0;
            this.labelPointNumber.Text = "label1";
            // 
            // labelPointName
            // 
            this.labelPointName.AutoSize = true;
            this.labelPointName.Location = new System.Drawing.Point(150, 3);
            this.labelPointName.Name = "labelPointName";
            this.labelPointName.Size = new System.Drawing.Size(35, 13);
            this.labelPointName.TabIndex = 1;
            this.labelPointName.Text = "label1";
            // 
            // textBoxPointValue
            // 
            this.textBoxPointValue.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxPointValue.Location = new System.Drawing.Point(44, 0);
            this.textBoxPointValue.Name = "textBoxPointValue";
            this.textBoxPointValue.ReadOnly = true;
            this.textBoxPointValue.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textBoxPointValue.Size = new System.Drawing.Size(100, 20);
            this.textBoxPointValue.TabIndex = 2;
            // 
            // ucDNPMemphisAnalog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxPointValue);
            this.Controls.Add(this.labelPointName);
            this.Controls.Add(this.labelPointNumber);
            this.Name = "ucDNPMemphisAnalog";
            this.Size = new System.Drawing.Size(346, 20);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelPointNumber;
        private System.Windows.Forms.Label labelPointName;
        private System.Windows.Forms.TextBox textBoxPointValue;
    }
}
