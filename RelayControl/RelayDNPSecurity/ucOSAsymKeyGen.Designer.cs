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
            this.buttonGetPublicKey = new System.Windows.Forms.Button();
            this.groupBoxMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxMain
            // 
            this.groupBoxMain.Controls.Add(this.buttonGenerateKey);
            this.groupBoxMain.Controls.Add(this.buttonGetPublicKey);
            this.groupBoxMain.Location = new System.Drawing.Point(3, 0);
            this.groupBoxMain.Name = "groupBoxMain";
            this.groupBoxMain.Size = new System.Drawing.Size(848, 97);
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
            // 
            // buttonGetPublicKey
            // 
            this.buttonGetPublicKey.Location = new System.Drawing.Point(731, 19);
            this.buttonGetPublicKey.Name = "buttonGetPublicKey";
            this.buttonGetPublicKey.Size = new System.Drawing.Size(111, 27);
            this.buttonGetPublicKey.TabIndex = 0;
            this.buttonGetPublicKey.Text = "Get Public Key";
            this.buttonGetPublicKey.UseVisualStyleBackColor = true;
            // 
            // ucOSAsymKeyGen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxMain);
            this.Name = "ucOSAsymKeyGen";
            this.Size = new System.Drawing.Size(854, 100);
            this.groupBoxMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonGetPublicKey;
        private System.Windows.Forms.Button buttonGenerateKey;
        private System.Windows.Forms.GroupBox groupBoxMain;
    }
}
