namespace RelayDNPSecurity
{
   partial class ucDNPSAv5
   {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        public System.ComponentModel.IContainer components = null;
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
            this.buttonLoadDefaultUser = new System.Windows.Forms.Button();
            this.buttonGetLoadedUsers = new System.Windows.Forms.Button();
            this.labelCurrentlyLoadedUsers = new System.Windows.Forms.Label();
            this.labelLoadedUsersNumbersLabel = new System.Windows.Forms.Label();
            this.ucDNPSAv5Settings1 = new RelayDNPSecurity.ucDNPSAv5Settings();
            this.ucDNPSAv5OSName1 = new RelayDNPSecurity.ucDNPSAv5OSName();
            this.ucDNPSAv5AuthoritySym1 = new RelayDNPSecurity.ucDNPSAv5AuthoritySym();
            this.ucDNPSAv5User1 = new RelayDNPSecurity.ucDNPSAv5User();
            this.buttonLoadDefaultAuthorityKey = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonLoadDefaultUser
            // 
            this.buttonLoadDefaultUser.Location = new System.Drawing.Point(756, 133);
            this.buttonLoadDefaultUser.Name = "buttonLoadDefaultUser";
            this.buttonLoadDefaultUser.Size = new System.Drawing.Size(112, 23);
            this.buttonLoadDefaultUser.TabIndex = 7;
            this.buttonLoadDefaultUser.Text = "Load Default User";
            this.buttonLoadDefaultUser.UseVisualStyleBackColor = true;
            this.buttonLoadDefaultUser.Click += new System.EventHandler(this.buttonLoadDefaultUser_Click);
            // 
            // buttonGetLoadedUsers
            // 
            this.buttonGetLoadedUsers.Location = new System.Drawing.Point(18, 133);
            this.buttonGetLoadedUsers.Name = "buttonGetLoadedUsers";
            this.buttonGetLoadedUsers.Size = new System.Drawing.Size(112, 23);
            this.buttonGetLoadedUsers.TabIndex = 6;
            this.buttonGetLoadedUsers.Text = "Get Loaded Users";
            this.buttonGetLoadedUsers.UseVisualStyleBackColor = true;
            this.buttonGetLoadedUsers.Click += new System.EventHandler(this.buttonGetLoadedUsers_Click);
            // 
            // labelCurrentlyLoadedUsers
            // 
            this.labelCurrentlyLoadedUsers.AutoSize = true;
            this.labelCurrentlyLoadedUsers.Location = new System.Drawing.Point(275, 138);
            this.labelCurrentlyLoadedUsers.Name = "labelCurrentlyLoadedUsers";
            this.labelCurrentlyLoadedUsers.Size = new System.Drawing.Size(0, 13);
            this.labelCurrentlyLoadedUsers.TabIndex = 5;
            // 
            // labelLoadedUsersNumbersLabel
            // 
            this.labelLoadedUsersNumbersLabel.AutoSize = true;
            this.labelLoadedUsersNumbersLabel.Location = new System.Drawing.Point(149, 138);
            this.labelLoadedUsersNumbersLabel.Name = "labelLoadedUsersNumbersLabel";
            this.labelLoadedUsersNumbersLabel.Size = new System.Drawing.Size(120, 13);
            this.labelLoadedUsersNumbersLabel.TabIndex = 4;
            this.labelLoadedUsersNumbersLabel.Text = "Currently Loaded Users:";
            // 
            // ucDNPSAv5Settings1
            // 
            this.ucDNPSAv5Settings1.AuthenticationEnabled = false;
            this.ucDNPSAv5Settings1.Location = new System.Drawing.Point(8, 316);
            this.ucDNPSAv5Settings1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ucDNPSAv5Settings1.Name = "ucDNPSAv5Settings1";
            this.ucDNPSAv5Settings1.Size = new System.Drawing.Size(960, 380);
            this.ucDNPSAv5Settings1.TabIndex = 10;
            // 
            // ucDNPSAv5OSName1
            // 
            this.ucDNPSAv5OSName1.Location = new System.Drawing.Point(8, 239);
            this.ucDNPSAv5OSName1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ucDNPSAv5OSName1.Name = "ucDNPSAv5OSName1";
            this.ucDNPSAv5OSName1.OSName = "DIGITALGRID, INC. DNP Relay Serial Number: DIGITALGRID, INC. DNP Relay";
            this.ucDNPSAv5OSName1.RequestOSNameClicked = false;
            this.ucDNPSAv5OSName1.Size = new System.Drawing.Size(605, 84);
            this.ucDNPSAv5OSName1.TabIndex = 9;
            // 
            // ucDNPSAv5AuthoritySym1
            // 
            this.ucDNPSAv5AuthoritySym1.Location = new System.Drawing.Point(6, 162);
            this.ucDNPSAv5AuthoritySym1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ucDNPSAv5AuthoritySym1.Name = "ucDNPSAv5AuthoritySym1";
            this.ucDNPSAv5AuthoritySym1.Size = new System.Drawing.Size(862, 82);
            this.ucDNPSAv5AuthoritySym1.TabIndex = 8;
            // 
            // ucDNPSAv5User1
            // 
            this.ucDNPSAv5User1.Location = new System.Drawing.Point(8, 3);
            this.ucDNPSAv5User1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ucDNPSAv5User1.Name = "ucDNPSAv5User1";
            this.ucDNPSAv5User1.Size = new System.Drawing.Size(876, 132);
            this.ucDNPSAv5User1.TabIndex = 2;
            // 
            // buttonLoadDefaultAuthorityKey
            // 
            this.buttonLoadDefaultAuthorityKey.Location = new System.Drawing.Point(756, 221);
            this.buttonLoadDefaultAuthorityKey.Name = "buttonLoadDefaultAuthorityKey";
            this.buttonLoadDefaultAuthorityKey.Size = new System.Drawing.Size(112, 39);
            this.buttonLoadDefaultAuthorityKey.TabIndex = 11;
            this.buttonLoadDefaultAuthorityKey.Text = "Load Default Authority Key";
            this.buttonLoadDefaultAuthorityKey.UseVisualStyleBackColor = true;
            this.buttonLoadDefaultAuthorityKey.Click += new System.EventHandler(this.buttonLoadDefaultAuthorityKey_Click);
            // 
            // ucDNPSAv5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonLoadDefaultAuthorityKey);
            this.Controls.Add(this.ucDNPSAv5Settings1);
            this.Controls.Add(this.ucDNPSAv5OSName1);
            this.Controls.Add(this.ucDNPSAv5AuthoritySym1);
            this.Controls.Add(this.buttonLoadDefaultUser);
            this.Controls.Add(this.buttonGetLoadedUsers);
            this.Controls.Add(this.labelCurrentlyLoadedUsers);
            this.Controls.Add(this.labelLoadedUsersNumbersLabel);
            this.Controls.Add(this.ucDNPSAv5User1);
            this.Name = "ucDNPSAv5";
            this.Size = new System.Drawing.Size(978, 733);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public ucDNPSAv5User ucDNPSAv5User1;
        public System.Windows.Forms.Label labelLoadedUsersNumbersLabel;
        public System.Windows.Forms.Label labelCurrentlyLoadedUsers;
        public System.Windows.Forms.Button buttonGetLoadedUsers;
        public System.Windows.Forms.Button buttonLoadDefaultUser;
        public ucDNPSAv5AuthoritySym ucDNPSAv5AuthoritySym1;
        public ucDNPSAv5OSName ucDNPSAv5OSName1;
        public ucDNPSAv5Settings ucDNPSAv5Settings1;
        public System.Windows.Forms.Button buttonLoadDefaultAuthorityKey;
    }
}
