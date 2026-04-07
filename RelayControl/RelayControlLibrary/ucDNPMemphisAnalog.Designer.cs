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
            this.labelPointName = new System.Windows.Forms.Label();
            this.textBoxPointValue = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelEventEnable
            // 
            this.labelEventEnable.Location = new System.Drawing.Point(342, 3);
            // 
            // checkBoxEventEnabled
            // 
            this.checkBoxEventEnabled.Location = new System.Drawing.Point(422, 3);
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
            this.textBoxPointValue.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBoxPointValue.Size = new System.Drawing.Size(100, 20);
            this.textBoxPointValue.TabIndex = 2;
            this.textBoxPointValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center; //System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ucDNPMemphisAnalog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelPointName);
            this.Controls.Add(this.textBoxPointValue);
            this.Name = "ucDNPMemphisAnalog";
            this.Size = new System.Drawing.Size(345, 20);
            this.Controls.SetChildIndex(this.textBoxPointValue, 0);
            this.Controls.SetChildIndex(this.labelPointName, 0);
            this.Controls.SetChildIndex(this.checkBoxEventEnabled, 0);
            this.Controls.SetChildIndex(this.labelEventEnable, 0);
            this.Controls.SetChildIndex(this.labelPointNumber, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelPointName;
        private System.Windows.Forms.TextBox textBoxPointValue;
    }
}
