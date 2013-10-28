namespace TransmitterLibrary
{
  partial class About
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
      this.GroupBox1 = new System.Windows.Forms.GroupBox();
      this.lblCopyright = new System.Windows.Forms.Label();
      this.lblVersion = new System.Windows.Forms.Label();
      this.btnClose = new System.Windows.Forms.Button();
      this.lblProgram = new System.Windows.Forms.Label();
      this.lblLaw = new System.Windows.Forms.Label();
      this.PictureBox1 = new System.Windows.Forms.PictureBox();
      this.GroupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
      this.SuspendLayout();
      // 
      // GroupBox1
      // 
      this.GroupBox1.Controls.Add(this.lblCopyright);
      this.GroupBox1.Controls.Add(this.lblVersion);
      this.GroupBox1.Controls.Add(this.btnClose);
      this.GroupBox1.Controls.Add(this.lblProgram);
      this.GroupBox1.Controls.Add(this.lblLaw);
      this.GroupBox1.Controls.Add(this.PictureBox1);
      this.GroupBox1.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.GroupBox1.Location = new System.Drawing.Point(4, -4);
      this.GroupBox1.Name = "GroupBox1";
      this.GroupBox1.Size = new System.Drawing.Size(432, 272);
      this.GroupBox1.TabIndex = 2;
      this.GroupBox1.TabStop = false;
      // 
      // lblCopyright
      // 
      this.lblCopyright.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
      this.lblCopyright.Location = new System.Drawing.Point(16, 224);
      this.lblCopyright.Name = "lblCopyright";
      this.lblCopyright.Size = new System.Drawing.Size(272, 32);
      this.lblCopyright.TabIndex = 6;
      // 
      // lblVersion
      // 
      this.lblVersion.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblVersion.Location = new System.Drawing.Point(176, 96);
      this.lblVersion.Name = "lblVersion";
      this.lblVersion.Size = new System.Drawing.Size(252, 40);
      this.lblVersion.TabIndex = 5;
      // 
      // btnClose
      // 
      this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
      this.btnClose.Location = new System.Drawing.Point(320, 240);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(96, 23);
      this.btnClose.TabIndex = 4;
      this.btnClose.Text = "OK";
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // lblProgram
      // 
      this.lblProgram.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblProgram.Location = new System.Drawing.Point(176, 32);
      this.lblProgram.Name = "lblProgram";
      this.lblProgram.Size = new System.Drawing.Size(252, 64);
      this.lblProgram.TabIndex = 3;
      // 
      // lblLaw
      // 
      this.lblLaw.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblLaw.Location = new System.Drawing.Point(16, 144);
      this.lblLaw.Name = "lblLaw";
      this.lblLaw.Size = new System.Drawing.Size(400, 76);
      this.lblLaw.TabIndex = 2;
      // 
      // PictureBox1
      // 
      this.PictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox1.Image")));
      this.PictureBox1.Location = new System.Drawing.Point(16, 24);
      this.PictureBox1.Name = "PictureBox1";
      this.PictureBox1.Size = new System.Drawing.Size(154, 117);
      this.PictureBox1.TabIndex = 1;
      this.PictureBox1.TabStop = false;
      // 
      // About
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(444, 274);
      this.Controls.Add(this.GroupBox1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "About";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Load += new System.EventHandler(this.About_Load);
      this.GroupBox1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
      this.ResumeLayout(false);

    }
    private System.Windows.Forms.GroupBox GroupBox1;
    private System.Windows.Forms.Label lblProgram;
    private System.Windows.Forms.Label lblLaw;
    private System.Windows.Forms.PictureBox PictureBox1;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Label lblVersion;
    private System.Windows.Forms.Label lblCopyright;

    #endregion
  }
}