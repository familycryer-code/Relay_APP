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
            this.ucDNPSAv5User1 = new RelayDNPSecurity.ucDNPSAv5User();
            this.labelLoadedUsersNumbersLabel = new System.Windows.Forms.Label();
            this.labelCurrentlyLoadedUsers = new System.Windows.Forms.Label();
            this.buttonGetLoadedUsers = new System.Windows.Forms.Button();
            this.buttonLoadDefaultUser = new System.Windows.Forms.Button();
            this.ucDNPSAv5AuthoritySym1 = new RelayDNPSecurity.ucDNPSAv5AuthoritySym();
            this.ucDNPSAv5OSName1 = new RelayDNPSecurity.ucDNPSAv5OSName();
            this.SuspendLayout();
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
            // buttonLoadDefaultUser
            // 
            this.buttonLoadDefaultUser.Location = new System.Drawing.Point(734, 124);
            this.buttonLoadDefaultUser.Name = "buttonLoadDefaultUser";
            this.buttonLoadDefaultUser.Size = new System.Drawing.Size(112, 23);
            this.buttonLoadDefaultUser.TabIndex = 7;
            this.buttonLoadDefaultUser.Text = "Load Default User";
            this.buttonLoadDefaultUser.UseVisualStyleBackColor = true;
            this.buttonLoadDefaultUser.Click += new System.EventHandler(this.buttonLoadDefaultUser_Click);
            // 
            // ucDNPSAv5AuthoritySym1
            // 
            this.ucDNPSAv5AuthoritySym1.Location = new System.Drawing.Point(3, 180);
            this.ucDNPSAv5AuthoritySym1.Name = "ucDNPSAv5AuthoritySym1";
            this.ucDNPSAv5AuthoritySym1.Size = new System.Drawing.Size(862, 109);
            this.ucDNPSAv5AuthoritySym1.TabIndex = 8;
            // 
            // ucDNPSAv5OSName1
            // 
            this.ucDNPSAv5OSName1.Location = new System.Drawing.Point(3, 292);
            this.ucDNPSAv5OSName1.Name = "ucDNPSAv5OSName1";
            this.ucDNPSAv5OSName1.Size = new System.Drawing.Size(605, 84);
            this.ucDNPSAv5OSName1.TabIndex = 9;
            // 
            // ucDNPSAv5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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

        private ucDNPSAv5User ucDNPSAv5User1;
        private System.Windows.Forms.Label labelLoadedUsersNumbersLabel;
        private System.Windows.Forms.Label labelCurrentlyLoadedUsers;
        private System.Windows.Forms.Button buttonGetLoadedUsers;
        private System.Windows.Forms.Button buttonLoadDefaultUser;
        private ucDNPSAv5AuthoritySym ucDNPSAv5AuthoritySym1;
        private ucDNPSAv5OSName ucDNPSAv5OSName1;
    }
}
