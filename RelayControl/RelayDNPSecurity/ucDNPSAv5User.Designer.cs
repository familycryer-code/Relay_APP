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
            this.labelUserRole = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.buttonAddUser = new System.Windows.Forms.Button();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonDeleteUser = new System.Windows.Forms.Button();
            this.comboBoxUserRole = new System.Windows.Forms.ComboBox();
            this.groupBoxUserControl = new System.Windows.Forms.GroupBox();
            this.panel_DNPuserName = new System.Windows.Forms.Panel();
            this.groupBoxUserControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelUserNumber
            // 
            this.labelUserNumber.AutoSize = true;
            this.labelUserNumber.Location = new System.Drawing.Point(12, 27);
            this.labelUserNumber.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelUserNumber.Name = "labelUserNumber";
            this.labelUserNumber.Size = new System.Drawing.Size(93, 24);
            this.labelUserNumber.TabIndex = 0;
            this.labelUserNumber.Text = "User No.:";
            // 
            // textBoxUserNumber
            // 
            this.textBoxUserNumber.Location = new System.Drawing.Point(89, 23);
            this.textBoxUserNumber.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxUserNumber.Name = "textBoxUserNumber";
            this.textBoxUserNumber.Size = new System.Drawing.Size(105, 32);
            this.textBoxUserNumber.TabIndex = 1;
            // 
            // labelUserRole
            // 
            this.labelUserRole.AutoSize = true;
            this.labelUserRole.Location = new System.Drawing.Point(12, 62);
            this.labelUserRole.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelUserRole.Name = "labelUserRole";
            this.labelUserRole.Size = new System.Drawing.Size(102, 24);
            this.labelUserRole.TabIndex = 2;
            this.labelUserRole.Text = "User Role:";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // buttonAddUser
            // 
            this.buttonAddUser.Location = new System.Drawing.Point(993, 20); 
            this.buttonAddUser.Margin = new System.Windows.Forms.Padding(4);
            this.buttonAddUser.Name = "buttonAddUser";
            this.buttonAddUser.Size = new System.Drawing.Size(113, 43);
            this.buttonAddUser.TabIndex = 6;
            this.buttonAddUser.Text = "Add/Overwrite User #";
            this.buttonAddUser.UseVisualStyleBackColor = true;
            this.buttonAddUser.Click += new System.EventHandler(this.buttonAddUser_Click);
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.Location = new System.Drawing.Point(131, 114);
            this.textBoxUserName.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Size = new System.Drawing.Size(493, 32);
            this.textBoxUserName.TabIndex = 8;
            this.textBoxUserName.TextChanged += new System.EventHandler(this.textBoxUserName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 118);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 24);
            this.label1.TabIndex = 7;
            this.label1.Text = "User Name:";
            // 
            // buttonDeleteUser
            // 
            this.buttonDeleteUser.Location = new System.Drawing.Point(993, 78);
            this.buttonDeleteUser.Margin = new System.Windows.Forms.Padding(4);
            this.buttonDeleteUser.Name = "buttonDeleteUser";
            this.buttonDeleteUser.Size = new System.Drawing.Size(113, 43);
            this.buttonDeleteUser.TabIndex = 9;
            this.buttonDeleteUser.Text = "Delete User #";
            this.buttonDeleteUser.UseVisualStyleBackColor = true;
            this.buttonDeleteUser.Click += new System.EventHandler(this.buttonDeleteUser_Click);
            // 
            // comboBoxUserRole
            // 
            this.comboBoxUserRole.FormattingEnabled = true;
            this.comboBoxUserRole.Items.AddRange(new object[] {
            "VIEWER",
            "OPERATOR",
            "ENGINEER",
            "INSTALLER",
            "SECADM",
            "SECAUD",
            "RBACMNT"});
            this.comboBoxUserRole.Location = new System.Drawing.Point(89, 55);
            this.comboBoxUserRole.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxUserRole.Name = "comboBoxUserRole";
            this.comboBoxUserRole.Size = new System.Drawing.Size(105, 32);
            this.comboBoxUserRole.TabIndex = 10;
            // 
            // groupBoxUserControl
            // 
            this.groupBoxUserControl.Controls.Add(this.labelUserNumber);
            this.groupBoxUserControl.Controls.Add(this.textBoxUserNumber);
            this.groupBoxUserControl.Controls.Add(this.comboBoxUserRole);
            this.groupBoxUserControl.Controls.Add(this.labelUserRole);
            this.groupBoxUserControl.Controls.Add(this.buttonDeleteUser);
            this.groupBoxUserControl.Controls.Add(this.buttonAddUser);
            this.groupBoxUserControl.Controls.Add(this.textBoxUserName);
            this.groupBoxUserControl.Controls.Add(this.label1);
            this.groupBoxUserControl.Controls.Add(this.panel_DNPuserName);
            this.groupBoxUserControl.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxUserControl.Location = new System.Drawing.Point(3, 4);
            this.groupBoxUserControl.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxUserControl.Name = "groupBoxUserControl";
            this.groupBoxUserControl.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxUserControl.Size = new System.Drawing.Size(1119, 194); //(1159, 194);
            this.groupBoxUserControl.TabIndex = 11;
            this.groupBoxUserControl.TabStop = false;
            this.groupBoxUserControl.Text = "User Control";
            // 
            // panel_DNPuserName
            // 
            this.panel_DNPuserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_DNPuserName.Location = new System.Drawing.Point(332, 32);
            this.panel_DNPuserName.Name = "panel_DNPuserName";
            this.panel_DNPuserName.Size = new System.Drawing.Size(117, 55);
            this.panel_DNPuserName.TabIndex = 11;
            // 
            // ucDNPSAv5User
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxUserControl);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ucDNPSAv5User";
            this.Size = new System.Drawing.Size(1163, 158);
            this.groupBoxUserControl.ResumeLayout(false);
            this.groupBoxUserControl.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelUserNumber;
        private System.Windows.Forms.TextBox textBoxUserNumber;
        private System.Windows.Forms.Label labelUserRole;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Button buttonAddUser;
        private System.Windows.Forms.TextBox textBoxUserName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonDeleteUser;
        private System.Windows.Forms.ComboBox comboBoxUserRole;
        private System.Windows.Forms.GroupBox groupBoxUserControl;
        private System.Windows.Forms.Panel panel_DNPuserName;
    }
}
