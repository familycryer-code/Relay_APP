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
            this.textBoxIPLabel = new System.Windows.Forms.TextBox();
            this.labelIPLabel = new System.Windows.Forms.Label();
            this.buttonSaveIP = new System.Windows.Forms.Button();
            this.comboBoxIPAddresses = new System.Windows.Forms.ComboBox();
            this.labelSavedIPs = new System.Windows.Forms.Label();
            this.buttonDeleteIP = new System.Windows.Forms.Button();
            this.ipAddressControl = new IPAddressControlLib.IPAddressControl();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonSetIP
            // 
            this.buttonSetIP.Location = new System.Drawing.Point(326, 36);
            this.buttonSetIP.Name = "buttonSetIP";
            this.buttonSetIP.Size = new System.Drawing.Size(75, 23);
            this.buttonSetIP.TabIndex = 2;
            this.buttonSetIP.Text = "Set IP";
            this.buttonSetIP.UseVisualStyleBackColor = true;
            this.buttonSetIP.Click += new System.EventHandler(this.buttonSetIP_Click);
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
            this.labelPort.Location = new System.Drawing.Point(7, 41);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(29, 13);
            this.labelPort.TabIndex = 2;
            this.labelPort.Text = "Port:";
            // 
            // numericUpDownPort
            // 
            this.numericUpDownPort.Location = new System.Drawing.Point(77, 39);
            this.numericUpDownPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDownPort.Name = "numericUpDownPort";
            this.numericUpDownPort.Size = new System.Drawing.Size(114, 20);
            this.numericUpDownPort.TabIndex = 1;
            // 
            // textBoxIPLabel
            // 
            this.textBoxIPLabel.Location = new System.Drawing.Point(77, 65);
            this.textBoxIPLabel.Name = "textBoxIPLabel";
            this.textBoxIPLabel.Size = new System.Drawing.Size(100, 20);
            this.textBoxIPLabel.TabIndex = 3;
            // 
            // labelIPLabel
            // 
            this.labelIPLabel.AutoSize = true;
            this.labelIPLabel.Location = new System.Drawing.Point(7, 68);
            this.labelIPLabel.Name = "labelIPLabel";
            this.labelIPLabel.Size = new System.Drawing.Size(64, 13);
            this.labelIPLabel.TabIndex = 4;
            this.labelIPLabel.Text = "Save Label:";
            // 
            // buttonSaveIP
            // 
            this.buttonSaveIP.Location = new System.Drawing.Point(326, 62);
            this.buttonSaveIP.Name = "buttonSaveIP";
            this.buttonSaveIP.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveIP.TabIndex = 5;
            this.buttonSaveIP.Text = "Save IP";
            this.buttonSaveIP.UseVisualStyleBackColor = true;
            this.buttonSaveIP.Click += new System.EventHandler(this.buttonSaveIP_Click);
            // 
            // comboBoxIPAddresses
            // 
            this.comboBoxIPAddresses.FormattingEnabled = true;
            this.comboBoxIPAddresses.Location = new System.Drawing.Point(77, 91);
            this.comboBoxIPAddresses.Name = "comboBoxIPAddresses";
            this.comboBoxIPAddresses.Size = new System.Drawing.Size(240, 21);
            this.comboBoxIPAddresses.TabIndex = 6;
            this.comboBoxIPAddresses.SelectedIndexChanged += new System.EventHandler(this.comboBoxIPAddresses_SelectedIndexChanged);
            // 
            // labelSavedIPs
            // 
            this.labelSavedIPs.AutoSize = true;
            this.labelSavedIPs.Location = new System.Drawing.Point(7, 94);
            this.labelSavedIPs.Name = "labelSavedIPs";
            this.labelSavedIPs.Size = new System.Drawing.Size(59, 13);
            this.labelSavedIPs.TabIndex = 7;
            this.labelSavedIPs.Text = "Saved IPs:";
            // 
            // buttonDeleteIP
            // 
            this.buttonDeleteIP.Location = new System.Drawing.Point(326, 89);
            this.buttonDeleteIP.Name = "buttonDeleteIP";
            this.buttonDeleteIP.Size = new System.Drawing.Size(75, 23);
            this.buttonDeleteIP.TabIndex = 8;
            this.buttonDeleteIP.Text = "Delete IP";
            this.buttonDeleteIP.UseVisualStyleBackColor = true;
            this.buttonDeleteIP.Click += new System.EventHandler(this.buttonDeleteIP_Click);
            // 
            // ipAddressControl
            // 
            this.ipAddressControl.AllowInternalTab = true;
            this.ipAddressControl.AutoHeight = true;
            this.ipAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.ipAddressControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ipAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ipAddressControl.Location = new System.Drawing.Point(77, 12);
            this.ipAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.ipAddressControl.Name = "ipAddressControl";
            this.ipAddressControl.ReadOnly = false;
            this.ipAddressControl.Size = new System.Drawing.Size(114, 20);
            this.ipAddressControl.TabIndex = 9;
            this.ipAddressControl.Text = "...";
            // 
            // TCPConnectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 116);
            this.Controls.Add(this.ipAddressControl);
            this.Controls.Add(this.buttonDeleteIP);
            this.Controls.Add(this.labelSavedIPs);
            this.Controls.Add(this.comboBoxIPAddresses);
            this.Controls.Add(this.buttonSaveIP);
            this.Controls.Add(this.labelIPLabel);
            this.Controls.Add(this.textBoxIPLabel);
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
        private System.Windows.Forms.TextBox textBoxIPLabel;
        private System.Windows.Forms.Label labelIPLabel;
        private System.Windows.Forms.Button buttonSaveIP;
        private System.Windows.Forms.ComboBox comboBoxIPAddresses;
        private System.Windows.Forms.Label labelSavedIPs;
        private System.Windows.Forms.Button buttonDeleteIP;
        private IPAddressControlLib.IPAddressControl ipAddressControl;
    }
}