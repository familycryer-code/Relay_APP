using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;



public class YesNoMessageBoxResized : Form
{
    private Button buttonYes;
    private Label labelMessage;
    private Panel panelBackgroundGrey;
    private PictureBox pictureBoxWarning;
    private Button buttonNo;

    //no default button specified
    public YesNoMessageBoxResized(string title, string message)
    {
        InitializeComponent();
        this.Text = title;
        this.labelMessage.Text = message;
        this.Deactivate += MyDeactivateHandler;
        this.buttonYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
        this.buttonNo.DialogResult = System.Windows.Forms.DialogResult.No;
    }

    //no default button specified
    public YesNoMessageBoxResized(string title, string message, string buttonYes, string buttonNo)
    {
        InitializeComponent();
        this.Text = title;
        this.labelMessage.Text = message;
        this.Deactivate += MyDeactivateHandler;
        this.buttonYes.Text = buttonYes;
        this.buttonNo.Text = buttonNo;
        this.buttonYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
        this.buttonNo.DialogResult = System.Windows.Forms.DialogResult.No;
    }

    public YesNoMessageBoxResized()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(YesNoMessageBoxResized));
            this.buttonYes = new System.Windows.Forms.Button();
            this.buttonNo = new System.Windows.Forms.Button();
            this.labelMessage = new System.Windows.Forms.Label();
            this.panelBackgroundGrey = new System.Windows.Forms.Panel();
            this.pictureBoxWarning = new System.Windows.Forms.PictureBox();
            this.panelBackgroundGrey.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWarning)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonYes
            // 
            this.buttonYes.BackColor = System.Drawing.SystemColors.Control;
            this.buttonYes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonYes.Location = new System.Drawing.Point(182, 9);
            this.buttonYes.Name = "buttonYes";
            this.buttonYes.Size = new System.Drawing.Size(91, 31);
            this.buttonYes.TabIndex = 1;
            this.buttonYes.Text = "Yes";
            this.buttonYes.UseVisualStyleBackColor = false;
            this.buttonYes.Click += new System.EventHandler(this.buttonYes_Click);
            // 
            // buttonNo
            // 
            this.buttonNo.BackColor = System.Drawing.SystemColors.Control;
            this.buttonNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonNo.Location = new System.Drawing.Point(290, 9);
            this.buttonNo.Name = "buttonNo";
            this.buttonNo.Size = new System.Drawing.Size(91, 31);
            this.buttonNo.TabIndex = 2;
            this.buttonNo.Text = "No";
            this.buttonNo.UseVisualStyleBackColor = false;
            this.buttonNo.Click += new System.EventHandler(this.buttonNo_Click);
            // 
            // labelMessage
            // 
            this.labelMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMessage.Location = new System.Drawing.Point(64, 9);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(312, 76);
            this.labelMessage.TabIndex = 3;
            this.labelMessage.Text = "labelMessage";
            // 
            // panelBackgroundGrey
            // 
            this.panelBackgroundGrey.BackColor = System.Drawing.SystemColors.Control;
            this.panelBackgroundGrey.Controls.Add(this.buttonNo);
            this.panelBackgroundGrey.Controls.Add(this.buttonYes);
            this.panelBackgroundGrey.Location = new System.Drawing.Point(-5, 90);
            this.panelBackgroundGrey.Name = "panelBackgroundGrey";
            this.panelBackgroundGrey.Size = new System.Drawing.Size(400, 100);
            this.panelBackgroundGrey.TabIndex = 5;
            // 
            // pictureBoxWarning
            // 
            this.pictureBoxWarning.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxWarning.Image")));
            this.pictureBoxWarning.Location = new System.Drawing.Point(11, 12);
            this.pictureBoxWarning.Name = "pictureBoxWarning";
            this.pictureBoxWarning.Size = new System.Drawing.Size(46, 50);
            this.pictureBoxWarning.TabIndex = 6;
            this.pictureBoxWarning.TabStop = false;
            // 
            // YesNoMessageBoxResized
            // 
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(388, 142);
            this.Controls.Add(this.pictureBoxWarning);
            this.Controls.Add(this.labelMessage);
            this.Controls.Add(this.panelBackgroundGrey);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "YesNoMessageBoxResized";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TopMost = true;
            this.panelBackgroundGrey.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWarning)).EndInit();
            this.ResumeLayout(false);

    }


    public void buttonYes_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    public void buttonNo_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    protected void MyDeactivateHandler(object sender, EventArgs e)
    {
        if (this.TopLevel == false || this.TopMost == false)
        {
            this.TopLevel = true;
            this.TopMost = true;
        }
    }

}

public class buttonYes_ClickResultEvent : EventArgs
{
    public buttonYes_ClickResultEvent(bool choice)
    {
        this.buttonResult = choice;
    }

    public bool buttonResult;
}