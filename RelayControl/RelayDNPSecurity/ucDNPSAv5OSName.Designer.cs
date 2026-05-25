namespace RelayDNPSecurity
{
    partial class ucDNPSAv5OSName
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
        public void InitializeComponent()
        {
            this.textBoxOSName = new System.Windows.Forms.TextBox();
            this.buttonSendName = new System.Windows.Forms.Button();
            this.groupBoxMain = new System.Windows.Forms.GroupBox();
            this.panel_DNPoutstationName = new System.Windows.Forms.Panel();
            this.buttonRequestName = new System.Windows.Forms.Button();
            this.buttonGenerateName = new System.Windows.Forms.Button();
            this.groupBoxMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxOSName
            // 
            this.textBoxOSName.Location = new System.Drawing.Point(8, 27);
            this.textBoxOSName.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxOSName.Name = "textBoxOSName";
            this.textBoxOSName.Size = new System.Drawing.Size(671, 32);
            this.textBoxOSName.TabIndex = 0;
            this.textBoxOSName.TextChanged += new System.EventHandler(this.textBoxOSName_TextChanged);
            // 
            // buttonSendName
            // 
            this.buttonSendName.Location = new System.Drawing.Point(688, 23);
            this.buttonSendName.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSendName.Name = "buttonSendName";
            this.buttonSendName.Size = new System.Drawing.Size(100, 28);
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
            this.groupBoxMain.Controls.Add(this.panel_DNPoutstationName);
            this.groupBoxMain.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxMain.Location = new System.Drawing.Point(4, 4);
            this.groupBoxMain.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxMain.Name = "groupBoxMain";
            this.groupBoxMain.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxMain.Size = new System.Drawing.Size(796, 91);
            this.groupBoxMain.TabIndex = 2;
            this.groupBoxMain.TabStop = false;
            this.groupBoxMain.Text = "Outstation (Relay) Name";
            // 
            // panel_DNPoutstationName
            // 
            this.panel_DNPoutstationName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_DNPoutstationName.Location = new System.Drawing.Point(2, 2);
            this.panel_DNPoutstationName.Name = "panel_DNPoutstationName";
            this.panel_DNPoutstationName.Size = new System.Drawing.Size(62, 45);
            this.panel_DNPoutstationName.TabIndex = 4;
            // 
            // buttonRequestName
            // 
            this.buttonRequestName.Location = new System.Drawing.Point(667, 59);
            this.buttonRequestName.Margin = new System.Windows.Forms.Padding(4);
            this.buttonRequestName.Name = "buttonRequestName";
            this.buttonRequestName.Size = new System.Drawing.Size(121, 28);
            this.buttonRequestName.TabIndex = 3;
            this.buttonRequestName.Text = "Request Name";
            this.buttonRequestName.UseVisualStyleBackColor = true;
            this.buttonRequestName.Click += new System.EventHandler(this.buttonRequestName_Click);
            // 
            // buttonGenerateName
            // 
            this.buttonGenerateName.Location = new System.Drawing.Point(8, 59);
            this.buttonGenerateName.Margin = new System.Windows.Forms.Padding(4);
            this.buttonGenerateName.Name = "buttonGenerateName";
            this.buttonGenerateName.Size = new System.Drawing.Size(121, 28);
            this.buttonGenerateName.TabIndex = 2;
            this.buttonGenerateName.Text = "Generate Name";
            this.buttonGenerateName.UseVisualStyleBackColor = true;
            this.buttonGenerateName.Click += new System.EventHandler(this.buttonGenerateName_Click);
            // 
            // ucDNPSAv5OSName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxMain);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ucDNPSAv5OSName";
            this.Size = new System.Drawing.Size(807, 101);
            this.groupBoxMain.ResumeLayout(false);
            this.groupBoxMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TextBox textBoxOSName;
        public System.Windows.Forms.Button buttonSendName;
        public System.Windows.Forms.GroupBox groupBoxMain;
        public System.Windows.Forms.Button buttonGenerateName;
        public System.Windows.Forms.Button buttonRequestName;
        private System.Windows.Forms.Panel panel_DNPoutstationName;
    }
}
