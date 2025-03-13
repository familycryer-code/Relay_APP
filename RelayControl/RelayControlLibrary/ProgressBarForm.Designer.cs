namespace RelayControlLibrary
{
    partial class ProgressBarForm
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
            this.components = new System.ComponentModel.Container();
            this.progressBarMain = new System.Windows.Forms.ProgressBar();
            this.labelDownloadingText = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.timerCountDown = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // progressBarMain
            // 
            this.progressBarMain.Location = new System.Drawing.Point(12, 33);
            this.progressBarMain.Name = "progressBarMain";
            this.progressBarMain.Size = new System.Drawing.Size(282, 23);
            this.progressBarMain.TabIndex = 0;
            // 
            // labelDownloadingText
            // 
            this.labelDownloadingText.AutoSize = true;
            this.labelDownloadingText.Location = new System.Drawing.Point(12, 13);
            this.labelDownloadingText.Name = "labelDownloadingText";
            this.labelDownloadingText.Size = new System.Drawing.Size(0, 13);
            this.labelDownloadingText.TabIndex = 1;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(219, 62);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // timerCountDown
            // 
            this.timerCountDown.Interval = 500;
            this.timerCountDown.Tick += new System.EventHandler(this.timerCountDown_Tick);
            // 
            // ProgressBarForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(4F, 13F);// (6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(306, 93);
            this.ControlBox = false;
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.labelDownloadingText);
            this.Controls.Add(this.progressBarMain);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressBarForm";
            this.Text = "ProgressBarForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ProgressBarForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBarMain;
        private System.Windows.Forms.Label labelDownloadingText;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Timer timerCountDown;
    }
}