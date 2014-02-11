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
            this.buttonSendKey = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonSendKey
            // 
            this.buttonSendKey.Location = new System.Drawing.Point(710, 3);
            this.buttonSendKey.Name = "buttonSendKey";
            this.buttonSendKey.Size = new System.Drawing.Size(108, 23);
            this.buttonSendKey.TabIndex = 0;
            this.buttonSendKey.Text = "Send Authority Key";
            this.buttonSendKey.UseVisualStyleBackColor = true;
            this.buttonSendKey.Click += new System.EventHandler(this.buttonSendKey_Click);
            // 
            // ucDNPSAv5AuthoritySym
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonSendKey);
            this.Name = "ucDNPSAv5AuthoritySym";
            this.Size = new System.Drawing.Size(821, 90);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSendKey;

    }
}
