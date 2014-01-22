namespace RelayDNPSecurity
{
    partial class ucDNPSAv5User
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
            this.components = new System.ComponentModel.Container();
            this.labelUserNumber = new System.Windows.Forms.Label();
            this.textBoxUserNumber = new System.Windows.Forms.TextBox();
            this.textBoxUserRole = new System.Windows.Forms.TextBox();
            this.labelUserRole = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.buttonAddUser = new System.Windows.Forms.Button();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonDeleteUser = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelUserNumber
            // 
            this.labelUserNumber.AutoSize = true;
            this.labelUserNumber.Location = new System.Drawing.Point(3, 10);
            this.labelUserNumber.Name = "labelUserNumber";
            this.labelUserNumber.Size = new System.Drawing.Size(52, 13);
            this.labelUserNumber.TabIndex = 0;
            this.labelUserNumber.Text = "User No.:";
            // 
            // textBoxUserNumber
            // 
            this.textBoxUserNumber.Location = new System.Drawing.Point(61, 7);
            this.textBoxUserNumber.Name = "textBoxUserNumber";
            this.textBoxUserNumber.Size = new System.Drawing.Size(70, 20);
            this.textBoxUserNumber.TabIndex = 1;
            // 
            // textBoxUserRole
            // 
            this.textBoxUserRole.Location = new System.Drawing.Point(61, 35);
            this.textBoxUserRole.Name = "textBoxUserRole";
            this.textBoxUserRole.Size = new System.Drawing.Size(70, 20);
            this.textBoxUserRole.TabIndex = 3;
            // 
            // labelUserRole
            // 
            this.labelUserRole.AutoSize = true;
            this.labelUserRole.Location = new System.Drawing.Point(3, 38);
            this.labelUserRole.Name = "labelUserRole";
            this.labelUserRole.Size = new System.Drawing.Size(57, 13);
            this.labelUserRole.TabIndex = 2;
            this.labelUserRole.Text = "User Role:";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // buttonAddUser
            // 
            this.buttonAddUser.Location = new System.Drawing.Point(737, 10);
            this.buttonAddUser.Name = "buttonAddUser";
            this.buttonAddUser.Size = new System.Drawing.Size(96, 45);
            this.buttonAddUser.TabIndex = 6;
            this.buttonAddUser.Text = "Add/Overwrite User #";
            this.buttonAddUser.UseVisualStyleBackColor = true;
            this.buttonAddUser.Click += new System.EventHandler(this.buttonAddUser_Click);
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.Location = new System.Drawing.Point(72, 90);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Size = new System.Drawing.Size(465, 20);
            this.textBoxUserName.TabIndex = 8;
            this.textBoxUserName.TextChanged += new System.EventHandler(this.textBoxUserName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "User Name:";
            // 
            // buttonDeleteUser
            // 
            this.buttonDeleteUser.Location = new System.Drawing.Point(737, 61);
            this.buttonDeleteUser.Name = "buttonDeleteUser";
            this.buttonDeleteUser.Size = new System.Drawing.Size(96, 45);
            this.buttonDeleteUser.TabIndex = 9;
            this.buttonDeleteUser.Text = "Delete User #";
            this.buttonDeleteUser.UseVisualStyleBackColor = true;
            this.buttonDeleteUser.Click += new System.EventHandler(this.buttonDeleteUser_Click);
            // 
            // ucDNPSAv5User
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonDeleteUser);
            this.Controls.Add(this.textBoxUserName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonAddUser);
            this.Controls.Add(this.textBoxUserRole);
            this.Controls.Add(this.labelUserRole);
            this.Controls.Add(this.textBoxUserNumber);
            this.Controls.Add(this.labelUserNumber);
            this.Name = "ucDNPSAv5User";
            this.Size = new System.Drawing.Size(840, 124);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelUserNumber;
        private System.Windows.Forms.TextBox textBoxUserNumber;
        private System.Windows.Forms.TextBox textBoxUserRole;
        private System.Windows.Forms.Label labelUserRole;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Button buttonAddUser;
        private System.Windows.Forms.TextBox textBoxUserName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonDeleteUser;
    }
}
