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
            this.ucDNPSAv5User1 = new RelayDNPSecurity.ucDNPSAv5User();
            this.labelLoadedUsersNumbersLabel = new System.Windows.Forms.Label();
            this.labelCurrentlyLoadedUsers = new System.Windows.Forms.Label();
            this.buttonGetLoadedUsers = new System.Windows.Forms.Button();
            this.ucOSAsymKeyGen1 = new RelayDNPSecurity.ucOSAsymKeyGen();
            this.groupBox1.SuspendLayout();
            this.groupBoxOSPubKey.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBoxOSPubKey);
            this.groupBox1.Controls.Add(this.groupBoxAuthorityPubKey);
            this.groupBox1.Controls.Add(this.groupBoxOSName);
            this.groupBox1.Location = new System.Drawing.Point(17, 351);
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
            // ucDNPSAv5User1
            // 
            this.ucDNPSAv5User1.Location = new System.Drawing.Point(3, 0);
            this.ucDNPSAv5User1.Name = "ucDNPSAv5User1";
            this.ucDNPSAv5User1.Size = new System.Drawing.Size(843, 121);
            this.ucDNPSAv5User1.TabIndex = 2;
            // 
            // labelLoadedUsersNumbersLabel
            // 
            this.labelLoadedUsersNumbersLabel.AutoSize = true;
            this.labelLoadedUsersNumbersLabel.Location = new System.Drawing.Point(14, 124);
            this.labelLoadedUsersNumbersLabel.Name = "labelLoadedUsersNumbersLabel";
            this.labelLoadedUsersNumbersLabel.Size = new System.Drawing.Size(160, 13);
            this.labelLoadedUsersNumbersLabel.TabIndex = 4;
            this.labelLoadedUsersNumbersLabel.Text = "Currently Loaded User Numbers:";
            // 
            // labelCurrentlyLoadedUsers
            // 
            this.labelCurrentlyLoadedUsers.AutoSize = true;
            this.labelCurrentlyLoadedUsers.Location = new System.Drawing.Point(181, 124);
            this.labelCurrentlyLoadedUsers.Name = "labelCurrentlyLoadedUsers";
            this.labelCurrentlyLoadedUsers.Size = new System.Drawing.Size(0, 13);
            this.labelCurrentlyLoadedUsers.TabIndex = 5;
            // 
            // buttonGetLoadedUsers
            // 
            this.buttonGetLoadedUsers.Location = new System.Drawing.Point(13, 140);
            this.buttonGetLoadedUsers.Name = "buttonGetLoadedUsers";
            this.buttonGetLoadedUsers.Size = new System.Drawing.Size(112, 23);
            this.buttonGetLoadedUsers.TabIndex = 6;
            this.buttonGetLoadedUsers.Text = "Get Loaded Users";
            this.buttonGetLoadedUsers.UseVisualStyleBackColor = true;
            this.buttonGetLoadedUsers.Click += new System.EventHandler(this.buttonGetLoadedUsers_Click);
            // 
            // ucOSAsymKeyGen1
            // 
            this.ucOSAsymKeyGen1.Location = new System.Drawing.Point(3, 169);
            this.ucOSAsymKeyGen1.Name = "ucOSAsymKeyGen1";
            this.ucOSAsymKeyGen1.Size = new System.Drawing.Size(854, 80);
            this.ucOSAsymKeyGen1.TabIndex = 7;
            // 
            // ucDNPSAv5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ucOSAsymKeyGen1);
            this.Controls.Add(this.buttonGetLoadedUsers);
            this.Controls.Add(this.labelCurrentlyLoadedUsers);
            this.Controls.Add(this.labelLoadedUsersNumbersLabel);
            this.Controls.Add(this.ucDNPSAv5User1);
            this.Controls.Add(this.groupBox1);
            this.Name = "ucDNPSAv5";
            this.Size = new System.Drawing.Size(978, 733);
            this.groupBox1.ResumeLayout(false);
            this.groupBoxOSPubKey.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBoxOSPubKey;
        private System.Windows.Forms.Button buttonFetchOSPubKey;
        private System.Windows.Forms.Button buttonGenerateOSAsymKey;
        private System.Windows.Forms.GroupBox groupBoxAuthorityPubKey;
        private System.Windows.Forms.GroupBox groupBoxOSName;
        private ucDNPSAv5User ucDNPSAv5User1;
        private System.Windows.Forms.Label labelLoadedUsersNumbersLabel;
        private System.Windows.Forms.Label labelCurrentlyLoadedUsers;
        private System.Windows.Forms.Button buttonGetLoadedUsers;
        private ucOSAsymKeyGen ucOSAsymKeyGen1;
    }
}
