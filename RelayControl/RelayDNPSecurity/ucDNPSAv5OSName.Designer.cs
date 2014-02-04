namespace RelayDNPSecurity
{
    partial class ucDNPSAv5OSName
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
            this.textBoxOSName = new System.Windows.Forms.TextBox();
            this.buttonSendName = new System.Windows.Forms.Button();
            this.groupBoxMain = new System.Windows.Forms.GroupBox();
            this.buttonGenerateName = new System.Windows.Forms.Button();
            this.buttonRequestName = new System.Windows.Forms.Button();
            this.groupBoxMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxOSName
            // 
            this.textBoxOSName.Location = new System.Drawing.Point(6, 22);
            this.textBoxOSName.Name = "textBoxOSName";
            this.textBoxOSName.Size = new System.Drawing.Size(504, 20);
            this.textBoxOSName.TabIndex = 0;
            this.textBoxOSName.TextChanged += new System.EventHandler(this.textBoxOSName_TextChanged);
            // 
            // buttonSendName
            // 
            this.buttonSendName.Location = new System.Drawing.Point(516, 19);
            this.buttonSendName.Name = "buttonSendName";
            this.buttonSendName.Size = new System.Drawing.Size(75, 23);
            this.buttonSendName.TabIndex = 1;
            this.buttonSendName.Text = "Send Name";
            this.buttonSendName.UseVisualStyleBackColor = true;
            this.buttonSendName.Click += new System.EventHandler(this.buttonSendName_Click);
            // 
            // groupBoxMain
            // 
            this.groupBoxMain.Controls.Add(this.buttonRequestName);
            this.groupBoxMain.Controls.Add(this.buttonGenerateName);
            this.groupBoxMain.Controls.Add(this.buttonSendName);
            this.groupBoxMain.Controls.Add(this.textBoxOSName);
            this.groupBoxMain.Location = new System.Drawing.Point(3, 3);
            this.groupBoxMain.Name = "groupBoxMain";
            this.groupBoxMain.Size = new System.Drawing.Size(597, 74);
            this.groupBoxMain.TabIndex = 2;
            this.groupBoxMain.TabStop = false;
            this.groupBoxMain.Text = "Outstation (Relay) Name";
            // 
            // buttonGenerateName
            // 
            this.buttonGenerateName.Location = new System.Drawing.Point(6, 48);
            this.buttonGenerateName.Name = "buttonGenerateName";
            this.buttonGenerateName.Size = new System.Drawing.Size(91, 23);
            this.buttonGenerateName.TabIndex = 2;
            this.buttonGenerateName.Text = "Generate Name";
            this.buttonGenerateName.UseVisualStyleBackColor = true;
            this.buttonGenerateName.Click += new System.EventHandler(this.buttonGenerateName_Click);
            // 
            // buttonRequestName
            // 
            this.buttonRequestName.Location = new System.Drawing.Point(500, 48);
            this.buttonRequestName.Name = "buttonRequestName";
            this.buttonRequestName.Size = new System.Drawing.Size(91, 23);
            this.buttonRequestName.TabIndex = 3;
            this.buttonRequestName.Text = "Request Name";
            this.buttonRequestName.UseVisualStyleBackColor = true;
            this.buttonRequestName.Click += new System.EventHandler(this.buttonRequestName_Click);
            // 
            // ucDNPSAv5OSName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxMain);
            this.Name = "ucDNPSAv5OSName";
            this.Size = new System.Drawing.Size(605, 82);
            this.groupBoxMain.ResumeLayout(false);
            this.groupBoxMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxOSName;
        private System.Windows.Forms.Button buttonSendName;
        private System.Windows.Forms.GroupBox groupBoxMain;
        private System.Windows.Forms.Button buttonGenerateName;
        private System.Windows.Forms.Button buttonRequestName;
    }
}
