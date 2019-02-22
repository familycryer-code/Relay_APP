namespace RelayControl
{
    partial class TCPConnectionForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonSetIP = new System.Windows.Forms.Button();
            this.labelIP = new System.Windows.Forms.Label();
            this.labelPort = new System.Windows.Forms.Label();
            this.numericUpDownPort = new System.Windows.Forms.NumericUpDown();
            this.maskedTextBoxIPAddress = new System.Windows.Forms.MaskedTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonSetIP
            // 
            this.buttonSetIP.Location = new System.Drawing.Point(206, 32);
            this.buttonSetIP.Name = "buttonSetIP";
            this.buttonSetIP.Size = new System.Drawing.Size(75, 23);
            this.buttonSetIP.TabIndex = 2;
            this.buttonSetIP.Text = "Set IP";
            this.buttonSetIP.UseVisualStyleBackColor = true;
            this.buttonSetIP.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelIP
            // 
            this.labelIP.AutoSize = true;
            this.labelIP.Location = new System.Drawing.Point(7, 16);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(61, 13);
            this.labelIP.TabIndex = 1;
            this.labelIP.Text = "IP Address:";
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(7, 37);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(29, 13);
            this.labelPort.TabIndex = 2;
            this.labelPort.Text = "Port:";
            // 
            // numericUpDownPort
            // 
            this.numericUpDownPort.Location = new System.Drawing.Point(77, 35);
            this.numericUpDownPort.Name = "numericUpDownPort";
            this.numericUpDownPort.Size = new System.Drawing.Size(114, 20);
            this.numericUpDownPort.TabIndex = 1;
            // 
            // maskedTextBoxIPAddress
            // 
            this.maskedTextBoxIPAddress.Location = new System.Drawing.Point(77, 13);
            this.maskedTextBoxIPAddress.Mask = "000.000.000.000";
            this.maskedTextBoxIPAddress.Name = "maskedTextBoxIPAddress";
            this.maskedTextBoxIPAddress.Size = new System.Drawing.Size(100, 20);
            this.maskedTextBoxIPAddress.TabIndex = 0;
            // 
            // TCPConnectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 63);
            this.Controls.Add(this.maskedTextBoxIPAddress);
            this.Controls.Add(this.numericUpDownPort);
            this.Controls.Add(this.labelPort);
            this.Controls.Add(this.labelIP);
            this.Controls.Add(this.buttonSetIP);
            this.Name = "TCPConnectionForm";
            this.Text = "TCP Connection";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSetIP;
        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.NumericUpDown numericUpDownPort;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxIPAddress;
    }
}