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
            this.ucDNPSAv5OSName1 = new RelayDNPSecurity.ucDNPSAv5OSName();
            this.ucDNPSAv5AuthoritySym1 = new RelayDNPSecurity.ucDNPSAv5AuthoritySym();
            this.ucDNPSAv5User1 = new RelayDNPSecurity.ucDNPSAv5User();
            this.buttonLoadDefaultAuthorityKey = new System.Windows.Forms.Button();
            this.tab_subTabsDNPSAv5 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ucDNPSAv5Settings1 = new RelayDNPSecurity.ucDNPSAv5Settings();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tab_subTabsDNPSAv5.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonLoadDefaultUser
            // 
            this.buttonLoadDefaultUser.Location = new System.Drawing.Point(211, 650);
            this.buttonLoadDefaultUser.Name = "buttonLoadDefaultUser";
            this.buttonLoadDefaultUser.Size = new System.Drawing.Size(164, 39);
            this.buttonLoadDefaultUser.TabIndex = 7;
            this.buttonLoadDefaultUser.Text = "Load Default User";
            this.buttonLoadDefaultUser.UseVisualStyleBackColor = true;
            this.buttonLoadDefaultUser.Click += new System.EventHandler(this.buttonLoadDefaultUser_Click);
            // 
            // buttonGetLoadedUsers
            // 
            this.buttonGetLoadedUsers.Location = new System.Drawing.Point(346, 582);
            this.buttonGetLoadedUsers.Name = "buttonGetLoadedUsers";
            this.buttonGetLoadedUsers.Size = new System.Drawing.Size(181, 39);
            this.buttonGetLoadedUsers.TabIndex = 6;
            this.buttonGetLoadedUsers.Text = "Get Loaded Users";
            this.buttonGetLoadedUsers.UseVisualStyleBackColor = true;
            this.buttonGetLoadedUsers.Click += new System.EventHandler(this.buttonGetLoadedUsers_Click);
            // 
            // labelCurrentlyLoadedUsers
            // 
            this.labelCurrentlyLoadedUsers.AutoSize = true;
            this.labelCurrentlyLoadedUsers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelCurrentlyLoadedUsers.Location = new System.Drawing.Point(12, 592);
            this.labelCurrentlyLoadedUsers.Name = "labelCurrentlyLoadedUsers";
            this.labelCurrentlyLoadedUsers.Size = new System.Drawing.Size(2, 15);
            this.labelCurrentlyLoadedUsers.TabIndex = 5;
            // 
            // labelLoadedUsersNumbersLabel
            // 
            this.labelLoadedUsersNumbersLabel.AutoSize = true;
            this.labelLoadedUsersNumbersLabel.Location = new System.Drawing.Point(616, 592);
            this.labelLoadedUsersNumbersLabel.Name = "labelLoadedUsersNumbersLabel";
            this.labelLoadedUsersNumbersLabel.Size = new System.Drawing.Size(180, 19);
            this.labelLoadedUsersNumbersLabel.TabIndex = 4;
            this.labelLoadedUsersNumbersLabel.Text = "Currently Loaded Users:";
            // 
            // ucDNPSAv5OSName1
            // 
            this.ucDNPSAv5OSName1.Location = new System.Drawing.Point(200, 426);
            this.ucDNPSAv5OSName1.Margin = new System.Windows.Forms.Padding(6);
            this.ucDNPSAv5OSName1.Name = "ucDNPSAv5OSName1";
            this.ucDNPSAv5OSName1.OSName = "DIGITALGRID, INC. DNP Relay Serial Number: DIGITALGRID, INC. DNP Relay";
            this.ucDNPSAv5OSName1.RequestOSNameClicked = false;
            this.ucDNPSAv5OSName1.Size = new System.Drawing.Size(908, 123);
            this.ucDNPSAv5OSName1.TabIndex = 9;
            // 
            // ucDNPSAv5AuthoritySym1
            // 
            this.ucDNPSAv5AuthoritySym1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ucDNPSAv5AuthoritySym1.Location = new System.Drawing.Point(200, 275);
            this.ucDNPSAv5AuthoritySym1.Margin = new System.Windows.Forms.Padding(6);
            this.ucDNPSAv5AuthoritySym1.Name = "ucDNPSAv5AuthoritySym1";
            this.ucDNPSAv5AuthoritySym1.Size = new System.Drawing.Size(1500, 140);
            this.ucDNPSAv5AuthoritySym1.TabIndex = 8;
            // 
            // ucDNPSAv5User1
            // 
            this.ucDNPSAv5User1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ucDNPSAv5User1.Location = new System.Drawing.Point(60, 32);
            this.ucDNPSAv5User1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ucDNPSAv5User1.Name = "ucDNPSAv5User1";
            this.ucDNPSAv5User1.Size = new System.Drawing.Size(1375, 245);
            this.ucDNPSAv5User1.TabIndex = 2;
            // 
            // buttonLoadDefaultAuthorityKey
            // 
            this.buttonLoadDefaultAuthorityKey.Location = new System.Drawing.Point(19, 650);
            this.buttonLoadDefaultAuthorityKey.Name = "buttonLoadDefaultAuthorityKey";
            this.buttonLoadDefaultAuthorityKey.Size = new System.Drawing.Size(186, 39);
            this.buttonLoadDefaultAuthorityKey.TabIndex = 11;
            this.buttonLoadDefaultAuthorityKey.Text = "Load Default Authority Key";
            this.buttonLoadDefaultAuthorityKey.UseVisualStyleBackColor = true;
            this.buttonLoadDefaultAuthorityKey.Click += new System.EventHandler(this.buttonLoadDefaultAuthorityKey_Click);
            // 
            // tab_subTabsDNPSAv5
            // 
            this.tab_subTabsDNPSAv5.Controls.Add(this.tabPage1);
            this.tab_subTabsDNPSAv5.Controls.Add(this.tabPage2);
            this.tab_subTabsDNPSAv5.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tab_subTabsDNPSAv5.Location = new System.Drawing.Point(3, 3);
            this.tab_subTabsDNPSAv5.Name = "tab_subTabsDNPSAv5";
            this.tab_subTabsDNPSAv5.SelectedIndex = 0;
            this.tab_subTabsDNPSAv5.Size = new System.Drawing.Size(1360, 727);
            this.tab_subTabsDNPSAv5.TabIndex = 12;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ucDNPSAv5Settings1);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1352, 695);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "DNPSAv5 Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ucDNPSAv5Settings1
            // 
            this.ucDNPSAv5Settings1.AuthenticationEnabled = false;
            this.ucDNPSAv5Settings1.Location = new System.Drawing.Point(-242, -136);
            this.ucDNPSAv5Settings1.Margin = new System.Windows.Forms.Padding(6);
            this.ucDNPSAv5Settings1.Name = "ucDNPSAv5Settings1";
            this.ucDNPSAv5Settings1.Size = new System.Drawing.Size(1440, 555);
            this.ucDNPSAv5Settings1.TabIndex = 11;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ucDNPSAv5User1);
            this.tabPage2.Controls.Add(this.buttonGetLoadedUsers);
            this.tabPage2.Controls.Add(this.labelLoadedUsersNumbersLabel);
            this.tabPage2.Controls.Add(this.buttonLoadDefaultUser);
            this.tabPage2.Controls.Add(this.buttonLoadDefaultAuthorityKey);
            this.tabPage2.Controls.Add(this.ucDNPSAv5AuthoritySym1);
            this.tabPage2.Controls.Add(this.ucDNPSAv5OSName1);
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1352, 695);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "DNPSAv5 User Key";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ucDNPSAv5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tab_subTabsDNPSAv5);
            this.Controls.Add(this.labelCurrentlyLoadedUsers);
            this.Name = "ucDNPSAv5";
            this.Size = new System.Drawing.Size(978, 733);
            this.tab_subTabsDNPSAv5.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
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
        public System.Windows.Forms.Button buttonLoadDefaultAuthorityKey;
        private System.Windows.Forms.TabControl tab_subTabsDNPSAv5;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        public ucDNPSAv5Settings ucDNPSAv5Settings1;
    }
}
