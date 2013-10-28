namespace RelayControlLibrary
{
    partial class ucDNPMemphisBinary
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
            this.checkBoxPointName = new System.Windows.Forms.CheckBox();
            this.checkBoxEventEnabled = new System.Windows.Forms.CheckBox();
            this.labelEventEnable = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelPointNumber
            // 
            this.labelPointNumber.AutoSize = true;
            this.labelPointNumber.Location = new System.Drawing.Point(3, 4);
            this.labelPointNumber.Name = "labelPointNumber";
            this.labelPointNumber.Size = new System.Drawing.Size(13, 13);
            this.labelPointNumber.TabIndex = 1;
            this.labelPointNumber.Text = "0";
            // 
            // checkBoxPointName
            // 
            this.checkBoxPointName.AutoCheck = false;
            this.checkBoxPointName.AutoSize = true;
            this.checkBoxPointName.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxPointName.ForeColor = System.Drawing.SystemColors.InfoText;
            this.checkBoxPointName.Location = new System.Drawing.Point(34, 3);
            this.checkBoxPointName.Name = "checkBoxPointName";
            this.checkBoxPointName.Size = new System.Drawing.Size(80, 17);
            this.checkBoxPointName.TabIndex = 3;
            this.checkBoxPointName.Text = "checkBox1";
            this.checkBoxPointName.UseVisualStyleBackColor = false;
            // 
            // checkBoxEventEnabled
            // 
            this.checkBoxEventEnabled.AutoSize = true;
            this.checkBoxEventEnabled.Location = new System.Drawing.Point(419, 3);
            this.checkBoxEventEnabled.Name = "checkBoxEventEnabled";
            this.checkBoxEventEnabled.Size = new System.Drawing.Size(15, 14);
            this.checkBoxEventEnabled.TabIndex = 4;
            this.checkBoxEventEnabled.UseVisualStyleBackColor = true;
            this.checkBoxEventEnabled.CheckedChanged += new System.EventHandler(this.checkBoxEventEnabled_CheckedChanged);
            // 
            // labelEventEnable
            // 
            this.labelEventEnable.AutoSize = true;
            this.labelEventEnable.Location = new System.Drawing.Point(339, 3);
            this.labelEventEnable.Name = "labelEventEnable";
            this.labelEventEnable.Size = new System.Drawing.Size(74, 13);
            this.labelEventEnable.TabIndex = 5;
            this.labelEventEnable.Text = "Enable Event:";
            // 
            // ucDNPMemphisBinary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelEventEnable);
            this.Controls.Add(this.checkBoxEventEnabled);
            this.Controls.Add(this.checkBoxPointName);
            this.Controls.Add(this.labelPointNumber);
            this.Name = "ucDNPMemphisBinary";
            this.Size = new System.Drawing.Size(437, 22);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelPointNumber;
        private System.Windows.Forms.CheckBox checkBoxPointName;
        private System.Windows.Forms.CheckBox checkBoxEventEnabled;
        private System.Windows.Forms.Label labelEventEnable;
    }
}
