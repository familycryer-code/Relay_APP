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
            this.buttonGenerateKey = new System.Windows.Forms.Button();
            this.buttonGetKeyPair = new System.Windows.Forms.Button();
            this.buttonSendKeyPair = new System.Windows.Forms.Button();
            this.groupBoxMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxMain
            // 
            this.groupBoxMain.Controls.Add(this.buttonSendKeyPair);
            this.groupBoxMain.Controls.Add(this.buttonGenerateKey);
            this.groupBoxMain.Controls.Add(this.buttonGetKeyPair);
            this.groupBoxMain.Location = new System.Drawing.Point(3, 0);
            this.groupBoxMain.Name = "groupBoxMain";
            this.groupBoxMain.Size = new System.Drawing.Size(848, 175);
            this.groupBoxMain.TabIndex = 2;
            this.groupBoxMain.TabStop = false;
            this.groupBoxMain.Text = "Relay (Outstation) Asymmetric Key Control";
            // 
            // buttonGenerateKey
            // 
            this.buttonGenerateKey.Location = new System.Drawing.Point(731, 52);
            this.buttonGenerateKey.Name = "buttonGenerateKey";
            this.buttonGenerateKey.Size = new System.Drawing.Size(111, 27);
            this.buttonGenerateKey.TabIndex = 1;
            this.buttonGenerateKey.Text = "Generate Key Pair";
            this.buttonGenerateKey.UseVisualStyleBackColor = true;
            this.buttonGenerateKey.Click += new System.EventHandler(this.buttonGenerateKey_Click);
            // 
            // buttonGetKeyPair
            // 
            this.buttonGetKeyPair.Location = new System.Drawing.Point(731, 19);
            this.buttonGetKeyPair.Name = "buttonGetKeyPair";
            this.buttonGetKeyPair.Size = new System.Drawing.Size(111, 27);
            this.buttonGetKeyPair.TabIndex = 0;
            this.buttonGetKeyPair.Text = "Get Key Pair";
            this.buttonGetKeyPair.UseVisualStyleBackColor = true;
            this.buttonGetKeyPair.Click += new System.EventHandler(this.buttonGetKeyPair_Click);
            // 
            // buttonSendKeyPair
            // 
            this.buttonSendKeyPair.Location = new System.Drawing.Point(731, 85);
            this.buttonSendKeyPair.Name = "buttonSendKeyPair";
            this.buttonSendKeyPair.Size = new System.Drawing.Size(111, 27);
            this.buttonSendKeyPair.TabIndex = 2;
            this.buttonSendKeyPair.Text = "Send Key Pair";
            this.buttonSendKeyPair.UseVisualStyleBackColor = true;
            this.buttonSendKeyPair.Click += new System.EventHandler(this.buttonSendKeyPair_Click);
            // 
            // ucOSAsymKeyGen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxMain);
            this.Name = "ucOSAsymKeyGen";
            this.Size = new System.Drawing.Size(854, 175);
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
