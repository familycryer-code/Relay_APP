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
            this.groupBoxOSName = new System.Windows.Forms.GroupBox();
            this.ucDNPSAv5AuthoritySym1 = new RelayDNPSecurity.ucDNPSAv5AuthoritySym();
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
            // groupBoxOSName
            // 
            this.groupBoxOSName.Location = new System.Drawing.Point(42, 491);
            this.groupBoxOSName.Name = "groupBoxOSName";
            this.groupBoxOSName.Size = new System.Drawing.Size(415, 100);
            this.groupBoxOSName.TabIndex = 0;
            this.groupBoxOSName.TabStop = false;
            this.groupBoxOSName.Text = "Relay (Outstation) Name";
            // 
            // ucDNPSAv5AuthoritySym1
            // 
            this.ucDNPSAv5AuthoritySym1.Location = new System.Drawing.Point(17, 178);
            this.ucDNPSAv5AuthoritySym1.Name = "ucDNPSAv5AuthoritySym1";
            this.ucDNPSAv5AuthoritySym1.Size = new System.Drawing.Size(862, 109);
            this.ucDNPSAv5AuthoritySym1.TabIndex = 8;
            // 
            // ucDNPSAv5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ucDNPSAv5AuthoritySym1);
            this.Controls.Add(this.groupBoxOSName);
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
        private System.Windows.Forms.GroupBox groupBoxOSName;
        private ucDNPSAv5AuthoritySym ucDNPSAv5AuthoritySym1;
    }
}
