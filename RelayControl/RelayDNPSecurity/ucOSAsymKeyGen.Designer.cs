namespace RelayDNPSecurity
{
    partial class ucOSAsymKeyGen
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
            this.buttonSendKeyPair = new System.Windows.Forms.Button();
            this.buttonGenerateKey = new System.Windows.Forms.Button();
            this.buttonGetKeyPair = new System.Windows.Forms.Button();
            this.groupBoxMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxMain
            // 
            this.groupBoxMain.Controls.Add(this.buttonSendKeyPair);
            this.groupBoxMain.Controls.Add(this.buttonGenerateKey);
            this.groupBoxMain.Controls.Add(this.buttonGetKeyPair);
            this.groupBoxMain.Location = new System.Drawing.Point(4, 0);
            this.groupBoxMain.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxMain.Name = "groupBoxMain";
            this.groupBoxMain.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxMain.Size = new System.Drawing.Size(1131, 215);
            this.groupBoxMain.TabIndex = 2;
            this.groupBoxMain.TabStop = false;
            this.groupBoxMain.Text = "Relay (Outstation) Asymmetric Key Control";
            // 
            // buttonSendKeyPair
            // 
            this.buttonSendKeyPair.Location = new System.Drawing.Point(975, 105);
            this.buttonSendKeyPair.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSendKeyPair.Name = "buttonSendKeyPair";
            this.buttonSendKeyPair.Size = new System.Drawing.Size(148, 33);
            this.buttonSendKeyPair.TabIndex = 2;
            this.buttonSendKeyPair.Text = "Send Key Pair";
            this.buttonSendKeyPair.UseVisualStyleBackColor = true;
            this.buttonSendKeyPair.Click += new System.EventHandler(this.buttonSendKeyPair_Click);
            // 
            // buttonGenerateKey
            // 
            this.buttonGenerateKey.Location = new System.Drawing.Point(975, 64);
            this.buttonGenerateKey.Margin = new System.Windows.Forms.Padding(4);
            this.buttonGenerateKey.Name = "buttonGenerateKey";
            this.buttonGenerateKey.Size = new System.Drawing.Size(148, 33);
            this.buttonGenerateKey.TabIndex = 1;
            this.buttonGenerateKey.Text = "Generate Key Pair";
            this.buttonGenerateKey.UseVisualStyleBackColor = true;
            this.buttonGenerateKey.Click += new System.EventHandler(this.buttonGenerateKey_Click);
            // 
            // buttonGetKeyPair
            // 
            this.buttonGetKeyPair.Location = new System.Drawing.Point(975, 23);
            this.buttonGetKeyPair.Margin = new System.Windows.Forms.Padding(4);
            this.buttonGetKeyPair.Name = "buttonGetKeyPair";
            this.buttonGetKeyPair.Size = new System.Drawing.Size(148, 33);
            this.buttonGetKeyPair.TabIndex = 0;
            this.buttonGetKeyPair.Text = "Get Key Pair";
            this.buttonGetKeyPair.UseVisualStyleBackColor = true;
            this.buttonGetKeyPair.Click += new System.EventHandler(this.buttonGetKeyPair_Click);
            // 
            // ucOSAsymKeyGen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxMain);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ucOSAsymKeyGen";
            this.Size = new System.Drawing.Size(1139, 215);
            this.groupBoxMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonGenerateKey;
        private System.Windows.Forms.GroupBox groupBoxMain;
        private System.Windows.Forms.Button buttonGetKeyPair;
        private System.Windows.Forms.Button buttonSendKeyPair;
    }
}
