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
            this.panel_dnpAuthKey = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // buttonSendKey
            // 
            this.buttonSendKey.Location = new System.Drawing.Point(1065, 4);
            this.buttonSendKey.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSendKey.Name = "buttonSendKey";
            this.buttonSendKey.Size = new System.Drawing.Size(162, 34);
            this.buttonSendKey.TabIndex = 0;
            this.buttonSendKey.Text = "Send Authority Key";
            this.buttonSendKey.UseVisualStyleBackColor = true;
            this.buttonSendKey.Click += new System.EventHandler(this.buttonSendKey_Click);
            // 
            // panel_dnpAuthKey
            // 
            this.panel_dnpAuthKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_dnpAuthKey.Location = new System.Drawing.Point(29, 31);
            this.panel_dnpAuthKey.Name = "panel_dnpAuthKey";
            this.panel_dnpAuthKey.Size = new System.Drawing.Size(99, 48);
            this.panel_dnpAuthKey.TabIndex = 1;
            // 
            // ucDNPSAv5AuthoritySym
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_dnpAuthKey);
            this.Controls.Add(this.buttonSendKey);
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ucDNPSAv5AuthoritySym";
            this.Size = new System.Drawing.Size(1232, 132);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSendKey;
        private System.Windows.Forms.Panel panel_dnpAuthKey;
    }
}
