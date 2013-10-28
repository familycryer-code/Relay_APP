namespace RelayControl
{
    partial class ucForceCustomerSwitch
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
            this.buttonForceSwitch = new System.Windows.Forms.Button();
            this.groupBoxForceCustomerSwitch = new System.Windows.Forms.GroupBox();
            this.comboBoxCustomers = new System.Windows.Forms.ComboBox();
            this.groupBoxForceCustomerSwitch.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonForceSwitch
            // 
            this.buttonForceSwitch.Location = new System.Drawing.Point(133, 17);
            this.buttonForceSwitch.Name = "buttonForceSwitch";
            this.buttonForceSwitch.Size = new System.Drawing.Size(75, 23);
            this.buttonForceSwitch.TabIndex = 0;
            this.buttonForceSwitch.Text = "Force Switch";
            this.buttonForceSwitch.UseVisualStyleBackColor = true;
            this.buttonForceSwitch.Click += new System.EventHandler(this.buttonForceSwitch_Click);
            // 
            // groupBoxForceCustomerSwitch
            // 
            this.groupBoxForceCustomerSwitch.Controls.Add(this.comboBoxCustomers);
            this.groupBoxForceCustomerSwitch.Controls.Add(this.buttonForceSwitch);
            this.groupBoxForceCustomerSwitch.Location = new System.Drawing.Point(3, 3);
            this.groupBoxForceCustomerSwitch.Name = "groupBoxForceCustomerSwitch";
            this.groupBoxForceCustomerSwitch.Size = new System.Drawing.Size(215, 47);
            this.groupBoxForceCustomerSwitch.TabIndex = 1;
            this.groupBoxForceCustomerSwitch.TabStop = false;
            this.groupBoxForceCustomerSwitch.Text = "Force Customer Switch";
            // 
            // comboBoxCustomers
            // 
            this.comboBoxCustomers.FormattingEnabled = true;
            this.comboBoxCustomers.Location = new System.Drawing.Point(6, 19);
            this.comboBoxCustomers.Name = "comboBoxCustomers";
            this.comboBoxCustomers.Size = new System.Drawing.Size(121, 21);
            this.comboBoxCustomers.TabIndex = 2;
            // 
            // ucForceCustomerSwitch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxForceCustomerSwitch);
            this.Name = "ucForceCustomerSwitch";
            this.Size = new System.Drawing.Size(222, 52);
            this.groupBoxForceCustomerSwitch.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonForceSwitch;
        private System.Windows.Forms.GroupBox groupBoxForceCustomerSwitch;
        private System.Windows.Forms.ComboBox comboBoxCustomers;
    }
}
