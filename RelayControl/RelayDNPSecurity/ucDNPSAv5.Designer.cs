namespace RelayDNPSecurity
{
    partial class ucDNPSAv5
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBoxOSPubKey = new System.Windows.Forms.GroupBox();
            this.buttonFetchOSPubKey = new System.Windows.Forms.Button();
            this.buttonGenerateOSAsymKey = new System.Windows.Forms.Button();
            this.groupBoxAuthorityPubKey = new System.Windows.Forms.GroupBox();
            this.groupBoxOSName = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBoxOSPubKey.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBoxOSPubKey);
            this.groupBox1.Controls.Add(this.groupBoxAuthorityPubKey);
            this.groupBox1.Controls.Add(this.groupBoxOSName);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(975, 733);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // groupBoxOSPubKey
            // 
            this.groupBoxOSPubKey.Controls.Add(this.buttonFetchOSPubKey);
            this.groupBoxOSPubKey.Controls.Add(this.buttonGenerateOSAsymKey);
            this.groupBoxOSPubKey.Location = new System.Drawing.Point(6, 231);
            this.groupBoxOSPubKey.Name = "groupBoxOSPubKey";
            this.groupBoxOSPubKey.Size = new System.Drawing.Size(969, 100);
            this.groupBoxOSPubKey.TabIndex = 2;
            this.groupBoxOSPubKey.TabStop = false;
            this.groupBoxOSPubKey.Text = "Relay Public Key";
            // 
            // buttonFetchOSPubKey
            // 
            this.buttonFetchOSPubKey.Location = new System.Drawing.Point(844, 49);
            this.buttonFetchOSPubKey.Name = "buttonFetchOSPubKey";
            this.buttonFetchOSPubKey.Size = new System.Drawing.Size(111, 23);
            this.buttonFetchOSPubKey.TabIndex = 1;
            this.buttonFetchOSPubKey.Text = "Fetch Key";
            this.buttonFetchOSPubKey.UseVisualStyleBackColor = true;
            // 
            // buttonGenerateOSAsymKey
            // 
            this.buttonGenerateOSAsymKey.Location = new System.Drawing.Point(844, 20);
            this.buttonGenerateOSAsymKey.Name = "buttonGenerateOSAsymKey";
            this.buttonGenerateOSAsymKey.Size = new System.Drawing.Size(111, 23);
            this.buttonGenerateOSAsymKey.TabIndex = 0;
            this.buttonGenerateOSAsymKey.Text = "Generate Key";
            this.buttonGenerateOSAsymKey.UseVisualStyleBackColor = true;
            // 
            // groupBoxAuthorityPubKey
            // 
            this.groupBoxAuthorityPubKey.Location = new System.Drawing.Point(6, 125);
            this.groupBoxAuthorityPubKey.Name = "groupBoxAuthorityPubKey";
            this.groupBoxAuthorityPubKey.Size = new System.Drawing.Size(415, 100);
            this.groupBoxAuthorityPubKey.TabIndex = 1;
            this.groupBoxAuthorityPubKey.TabStop = false;
            this.groupBoxAuthorityPubKey.Text = "Authority Public Key";
            // 
            // groupBoxOSName
            // 
            this.groupBoxOSName.Location = new System.Drawing.Point(442, 125);
            this.groupBoxOSName.Name = "groupBoxOSName";
            this.groupBoxOSName.Size = new System.Drawing.Size(415, 100);
            this.groupBoxOSName.TabIndex = 0;
            this.groupBoxOSName.TabStop = false;
            this.groupBoxOSName.Text = "Relay (Outstation) Name";
            // 
            // ucDNPSAv5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "ucDNPSAv5";
            this.Size = new System.Drawing.Size(978, 733);
            this.groupBox1.ResumeLayout(false);
            this.groupBoxOSPubKey.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBoxOSPubKey;
        private System.Windows.Forms.Button buttonFetchOSPubKey;
        private System.Windows.Forms.Button buttonGenerateOSAsymKey;
        private System.Windows.Forms.GroupBox groupBoxAuthorityPubKey;
        private System.Windows.Forms.GroupBox groupBoxOSName;
    }
}
