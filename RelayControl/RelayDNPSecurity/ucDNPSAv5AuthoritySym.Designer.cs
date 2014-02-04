namespace RelayDNPSecurity
{
    partial class ucDNPSAv5AuthoritySym
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
            this.groupBoxMain = new System.Windows.Forms.GroupBox();
            this.buttonSendKey = new System.Windows.Forms.Button();
            this.groupBoxMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxMain
            // 
            this.groupBoxMain.Controls.Add(this.buttonSendKey);
            this.groupBoxMain.Location = new System.Drawing.Point(3, 3);
            this.groupBoxMain.Name = "groupBoxMain";
            this.groupBoxMain.Size = new System.Drawing.Size(814, 101);
            this.groupBoxMain.TabIndex = 0;
            this.groupBoxMain.TabStop = false;
            this.groupBoxMain.Text = "Authority Symmetric Key Control";
            // 
            // buttonSendKey
            // 
            this.buttonSendKey.Location = new System.Drawing.Point(697, 19);
            this.buttonSendKey.Name = "buttonSendKey";
            this.buttonSendKey.Size = new System.Drawing.Size(108, 23);
            this.buttonSendKey.TabIndex = 0;
            this.buttonSendKey.Text = "Send Key";
            this.buttonSendKey.UseVisualStyleBackColor = true;
            this.buttonSendKey.Click += new System.EventHandler(this.buttonSendKey_Click);
            // 
            // ucDNPSAv5AuthoritySym
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxMain);
            this.Name = "ucDNPSAv5AuthoritySym";
            this.Size = new System.Drawing.Size(821, 107);
            this.groupBoxMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxMain;
        private System.Windows.Forms.Button buttonSendKey;
    }
}
