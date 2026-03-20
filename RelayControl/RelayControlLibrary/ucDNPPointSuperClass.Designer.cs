namespace RelayControlLibrary
{
    partial class ucDNPPointSuperClass
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
            this.labelEventEnable = new System.Windows.Forms.Label();
            this.checkBoxEventEnabled = new System.Windows.Forms.CheckBox();
            this.labelPointNumber = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelEventEnable
            // 
            this.labelEventEnable.AutoSize = true;
            this.labelEventEnable.Location = new System.Drawing.Point(339, 3);
           // this.labelEventEnable.Location = new System.Drawing.Point(349, 3);
            this.labelEventEnable.Name = "labelEventEnable";
            this.labelEventEnable.Size = new System.Drawing.Size(74, 13);
            this.labelEventEnable.TabIndex = 9;
            this.labelEventEnable.Text = "Enable Event:";
            this.labelEventEnable.Enabled = true;// false;
            this.labelEventEnable.Visible = true;// false;
            // 
            // checkBoxEventEnabled
            // 
            this.checkBoxEventEnabled.AutoSize = true;
            this.checkBoxEventEnabled.Location = new System.Drawing.Point(419, 3);
            this.checkBoxEventEnabled.Name = "checkBoxEventEnabled";
            this.checkBoxEventEnabled.Size = new System.Drawing.Size(15, 14);
            this.checkBoxEventEnabled.TabIndex = 8;
            this.checkBoxEventEnabled.UseVisualStyleBackColor = true;
            this.checkBoxEventEnabled.Click += new System.EventHandler(this.checkBoxEventEnabled_Click);
            this.checkBoxEventEnabled.Enabled = true;// false;
            this.checkBoxEventEnabled.Visible = true;// false;
            // 
            // labelPointNumber
            // 
            this.labelPointNumber.AutoSize = true;
           // this.labelPointNumber.Location = new System.Drawing.Point(7, 3);
            this.labelPointNumber.Location = new System.Drawing.Point(3, 3);
            this.labelPointNumber.Name = "labelPointNumber";
            this.labelPointNumber.Size = new System.Drawing.Size(35, 13);
            this.labelPointNumber.TabIndex = 10;
            this.labelPointNumber.Text = "label1";
            // 
            // ucDNPPointSuperClass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelPointNumber);
            this.Controls.Add(this.labelEventEnable);
            this.Controls.Add(this.checkBoxEventEnabled);
            this.Name = "ucDNPPointSuperClass";
            this.Size = new System.Drawing.Size(437, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        protected System.Windows.Forms.Label labelEventEnable;
        protected System.Windows.Forms.CheckBox checkBoxEventEnabled;
        protected System.Windows.Forms.Label labelPointNumber;

        #endregion
    }
}
