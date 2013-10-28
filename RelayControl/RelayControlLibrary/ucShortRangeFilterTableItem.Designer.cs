namespace RelayControlLibrary
{
    partial class ucShortRangeFilterTableItem
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
            this.textBoxID = new System.Windows.Forms.TextBox();
            this.labelSlotNumber = new System.Windows.Forms.Label();
            this.textBoxStrength133 = new System.Windows.Forms.TextBox();
            this.textBoxStrength153 = new System.Windows.Forms.TextBox();
            this.textBoxAge133 = new System.Windows.Forms.TextBox();
            this.textBoxAge153 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBoxID
            // 
            this.textBoxID.Location = new System.Drawing.Point(32, 3);
            this.textBoxID.Name = "textBoxID";
            this.textBoxID.Size = new System.Drawing.Size(67, 20);
            this.textBoxID.TabIndex = 1;
            this.textBoxID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelSlotNumber
            // 
            this.labelSlotNumber.AutoSize = true;
            this.labelSlotNumber.Location = new System.Drawing.Point(3, 6);
            this.labelSlotNumber.Name = "labelSlotNumber";
            this.labelSlotNumber.Size = new System.Drawing.Size(19, 13);
            this.labelSlotNumber.TabIndex = 0;
            this.labelSlotNumber.Text = "10";
            // 
            // textBoxStrength133
            // 
            this.textBoxStrength133.Location = new System.Drawing.Point(117, 3);
            this.textBoxStrength133.Name = "textBoxStrength133";
            this.textBoxStrength133.Size = new System.Drawing.Size(67, 20);
            this.textBoxStrength133.TabIndex = 2;
            this.textBoxStrength133.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxStrength153
            // 
            this.textBoxStrength153.Location = new System.Drawing.Point(202, 3);
            this.textBoxStrength153.Name = "textBoxStrength153";
            this.textBoxStrength153.Size = new System.Drawing.Size(67, 20);
            this.textBoxStrength153.TabIndex = 3;
            this.textBoxStrength153.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxAge133
            // 
            this.textBoxAge133.Location = new System.Drawing.Point(287, 3);
            this.textBoxAge133.Name = "textBoxAge133";
            this.textBoxAge133.Size = new System.Drawing.Size(67, 20);
            this.textBoxAge133.TabIndex = 4;
            this.textBoxAge133.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxAge153
            // 
            this.textBoxAge153.Location = new System.Drawing.Point(369, 3);
            this.textBoxAge153.Name = "textBoxAge153";
            this.textBoxAge153.Size = new System.Drawing.Size(67, 20);
            this.textBoxAge153.TabIndex = 5;
            this.textBoxAge153.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ucShortRangeFilterTableItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxAge153);
            this.Controls.Add(this.textBoxAge133);
            this.Controls.Add(this.textBoxStrength153);
            this.Controls.Add(this.textBoxStrength133);
            this.Controls.Add(this.labelSlotNumber);
            this.Controls.Add(this.textBoxID);
            this.Name = "ucShortRangeFilterTableItem";
            this.Size = new System.Drawing.Size(441, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxID;
        private System.Windows.Forms.Label labelSlotNumber;
        private System.Windows.Forms.TextBox textBoxStrength133;
        private System.Windows.Forms.TextBox textBoxStrength153;
        private System.Windows.Forms.TextBox textBoxAge133;
        private System.Windows.Forms.TextBox textBoxAge153;
    }
}
