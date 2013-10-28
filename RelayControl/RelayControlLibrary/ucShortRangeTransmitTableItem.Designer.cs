namespace RelayControlLibrary
{
    partial class ucShortRangeTransmitTableItem
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
            this.textBoxChangeCount = new System.Windows.Forms.TextBox();
            this.textBoxAvgStrength = new System.Windows.Forms.TextBox();
            this.labelSlotNumber = new System.Windows.Forms.Label();
            this.textBoxID = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBoxChangeCount
            // 
            this.textBoxChangeCount.Location = new System.Drawing.Point(202, 3);
            this.textBoxChangeCount.Name = "textBoxChangeCount";
            this.textBoxChangeCount.Size = new System.Drawing.Size(67, 20);
            this.textBoxChangeCount.TabIndex = 7;
            this.textBoxChangeCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxAvgStrength
            // 
            this.textBoxAvgStrength.Location = new System.Drawing.Point(117, 3);
            this.textBoxAvgStrength.Name = "textBoxAvgStrength";
            this.textBoxAvgStrength.Size = new System.Drawing.Size(67, 20);
            this.textBoxAvgStrength.TabIndex = 6;
            this.textBoxAvgStrength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelSlotNumber
            // 
            this.labelSlotNumber.AutoSize = true;
            this.labelSlotNumber.Location = new System.Drawing.Point(3, 6);
            this.labelSlotNumber.Name = "labelSlotNumber";
            this.labelSlotNumber.Size = new System.Drawing.Size(19, 13);
            this.labelSlotNumber.TabIndex = 4;
            this.labelSlotNumber.Text = "10";
            // 
            // textBoxID
            // 
            this.textBoxID.Location = new System.Drawing.Point(32, 3);
            this.textBoxID.Name = "textBoxID";
            this.textBoxID.Size = new System.Drawing.Size(67, 20);
            this.textBoxID.TabIndex = 5;
            this.textBoxID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ucShortRangeTransmitTableItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxChangeCount);
            this.Controls.Add(this.textBoxAvgStrength);
            this.Controls.Add(this.labelSlotNumber);
            this.Controls.Add(this.textBoxID);
            this.Name = "ucShortRangeTransmitTableItem";
            this.Size = new System.Drawing.Size(273, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxChangeCount;
        private System.Windows.Forms.TextBox textBoxAvgStrength;
        private System.Windows.Forms.Label labelSlotNumber;
        private System.Windows.Forms.TextBox textBoxID;
    }
}
