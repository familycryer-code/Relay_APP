namespace RelayControlLibrary
{
    partial class SendAll_Message_PopUp
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
            this.progressBar1__SendAll = new System.Windows.Forms.ProgressBar();
            this.timer_SendAll = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar1__SendAll
            // 
            this.progressBar1__SendAll.Location = new System.Drawing.Point(57, 30);
            this.progressBar1__SendAll.MarqueeAnimationSpeed = 5;
            this.progressBar1__SendAll.Name = "progressBar1__SendAll";
            this.progressBar1__SendAll.Size = new System.Drawing.Size(626, 20);
            this.progressBar1__SendAll.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(47, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(654, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "Please have patience. The relay is updating its critical parameters !";
            // 
            // SendAll_Message_PopUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(2000, 972);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1__SendAll);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(7);
            this.Name = "SendAll_Message_PopUp";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.SendAll_Message_PopUp_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1__SendAll;
        private System.Windows.Forms.Timer timer_SendAll;
        private System.Windows.Forms.Label label1;
    }
}