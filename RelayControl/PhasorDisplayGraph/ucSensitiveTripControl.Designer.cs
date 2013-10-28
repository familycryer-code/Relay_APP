namespace PhasorDisplayGraph
{
    partial class ucSensitiveTripControl
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
            this.ucSegmentPanel1 = new PhasorDisplayGraph.ucSegmentPanel();
            this.ucSegmentPanel2 = new PhasorDisplayGraph.ucSegmentPanel();
            this.ucSegmentPanel3 = new PhasorDisplayGraph.ucSegmentPanel();
            this.ucSegmentPanel4 = new PhasorDisplayGraph.ucSegmentPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ucSegmentPanel1
            // 
            this.ucSegmentPanel1.Location = new System.Drawing.Point(3, 52);
            this.ucSegmentPanel1.Name = "ucSegmentPanel1";
            this.ucSegmentPanel1.SegmentName = "Curve 1";
            this.ucSegmentPanel1.Size = new System.Drawing.Size(199, 25);
            this.ucSegmentPanel1.TabIndex = 0;
            // 
            // ucSegmentPanel2
            // 
            this.ucSegmentPanel2.Location = new System.Drawing.Point(3, 74);
            this.ucSegmentPanel2.Name = "ucSegmentPanel2";
            this.ucSegmentPanel2.SegmentName = "Curve 1";
            this.ucSegmentPanel2.Size = new System.Drawing.Size(199, 25);
            this.ucSegmentPanel2.TabIndex = 1;
            // 
            // ucSegmentPanel3
            // 
            this.ucSegmentPanel3.Location = new System.Drawing.Point(3, 96);
            this.ucSegmentPanel3.Name = "ucSegmentPanel3";
            this.ucSegmentPanel3.SegmentName = "Curve 1";
            this.ucSegmentPanel3.Size = new System.Drawing.Size(199, 25);
            this.ucSegmentPanel3.TabIndex = 2;
            // 
            // ucSegmentPanel4
            // 
            this.ucSegmentPanel4.Location = new System.Drawing.Point(3, 118);
            this.ucSegmentPanel4.Name = "ucSegmentPanel4";
            this.ucSegmentPanel4.SegmentName = "Curve 1";
            this.ucSegmentPanel4.Size = new System.Drawing.Size(199, 25);
            this.ucSegmentPanel4.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(142, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(77, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "label3";
            // 
            // ucSensitiveTripControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ucSegmentPanel4);
            this.Controls.Add(this.ucSegmentPanel3);
            this.Controls.Add(this.ucSegmentPanel2);
            this.Controls.Add(this.ucSegmentPanel1);
            this.Name = "ucSensitiveTripControl";
            this.Size = new System.Drawing.Size(313, 150);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ucSegmentPanel ucSegmentPanel1;
        private ucSegmentPanel ucSegmentPanel2;
        private ucSegmentPanel ucSegmentPanel3;
        private ucSegmentPanel ucSegmentPanel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;

    }
}
