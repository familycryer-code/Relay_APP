namespace RelayControlLibrary
{
    partial class ucCalibration
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
            this.groupBoxCalibration = new System.Windows.Forms.GroupBox();
            this.buttonRestConstants = new System.Windows.Forms.Button();
            this.buttonStartCal = new System.Windows.Forms.Button();
            this.buttonCalHigh = new System.Windows.Forms.Button();
            this.buttonSaveCalibration = new System.Windows.Forms.Button();
            this.buttonReqCalConstants = new System.Windows.Forms.Button();
            this.textBoxDisplayCalConstants = new System.Windows.Forms.TextBox();
            this.panelCommFlags = new System.Windows.Forms.Panel();
            this.groupBoxCalibration.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxCalibration
            // 
            this.groupBoxCalibration.Controls.Add(this.buttonRestConstants);
            this.groupBoxCalibration.Controls.Add(this.buttonStartCal);
            this.groupBoxCalibration.Controls.Add(this.buttonCalHigh);
            this.groupBoxCalibration.Controls.Add(this.buttonSaveCalibration);
            this.groupBoxCalibration.Controls.Add(this.buttonReqCalConstants);
            this.groupBoxCalibration.Controls.Add(this.textBoxDisplayCalConstants);
            this.groupBoxCalibration.Controls.Add(this.panelCommFlags);
            this.groupBoxCalibration.Location = new System.Drawing.Point(3, 3);
            this.groupBoxCalibration.Name = "groupBoxCalibration";
            this.groupBoxCalibration.Size = new System.Drawing.Size(275, 218);
            this.groupBoxCalibration.TabIndex = 100;
            this.groupBoxCalibration.TabStop = false;
            this.groupBoxCalibration.Text = "Calibration";
            // 
            // buttonRestConstants
            // 
            this.buttonRestConstants.Location = new System.Drawing.Point(6, 127);
            this.buttonRestConstants.Name = "buttonRestConstants";
            this.buttonRestConstants.Size = new System.Drawing.Size(75, 40);
            this.buttonRestConstants.TabIndex = 91;
            this.buttonRestConstants.Text = "Reset Constants";
            this.buttonRestConstants.UseVisualStyleBackColor = true;
            this.buttonRestConstants.Click += new System.EventHandler(this.buttonRestConstants_Click);
            // 
            // buttonStartCal
            // 
            this.buttonStartCal.Location = new System.Drawing.Point(6, 19);
            this.buttonStartCal.Name = "buttonStartCal";
            this.buttonStartCal.Size = new System.Drawing.Size(75, 23);
            this.buttonStartCal.TabIndex = 72;
            this.buttonStartCal.Text = "Low";
            this.buttonStartCal.UseVisualStyleBackColor = true;
            this.buttonStartCal.Click += new System.EventHandler(this.buttonStartCal_Click);
            // 
            // buttonCalHigh
            // 
            this.buttonCalHigh.Location = new System.Drawing.Point(6, 48);
            this.buttonCalHigh.Name = "buttonCalHigh";
            this.buttonCalHigh.Size = new System.Drawing.Size(75, 23);
            this.buttonCalHigh.TabIndex = 73;
            this.buttonCalHigh.Text = "High";
            this.buttonCalHigh.UseVisualStyleBackColor = true;
            this.buttonCalHigh.Click += new System.EventHandler(this.buttonCalHigh_Click);
            // 
            // buttonSaveCalibration
            // 
            this.buttonSaveCalibration.Location = new System.Drawing.Point(6, 77);
            this.buttonSaveCalibration.Name = "buttonSaveCalibration";
            this.buttonSaveCalibration.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveCalibration.TabIndex = 83;
            this.buttonSaveCalibration.Text = "Save";
            this.buttonSaveCalibration.UseVisualStyleBackColor = true;
            this.buttonSaveCalibration.Click += new System.EventHandler(this.buttonSaveCalibration_Click);
            // 
            // buttonReqCalConstants
            // 
            this.buttonReqCalConstants.Location = new System.Drawing.Point(6, 184);
            this.buttonReqCalConstants.Name = "buttonReqCalConstants";
            this.buttonReqCalConstants.Size = new System.Drawing.Size(75, 23);
            this.buttonReqCalConstants.TabIndex = 0;
            this.buttonReqCalConstants.Text = "Req Consts";
            this.buttonReqCalConstants.UseVisualStyleBackColor = true;
            this.buttonReqCalConstants.Click += new System.EventHandler(this.buttonReqCalConstants_Click);
            // 
            // textBoxDisplayCalConstants
            // 
            this.textBoxDisplayCalConstants.Location = new System.Drawing.Point(83, 19);
            this.textBoxDisplayCalConstants.Multiline = true;
            this.textBoxDisplayCalConstants.Name = "textBoxDisplayCalConstants";
            this.textBoxDisplayCalConstants.ReadOnly = true;
            this.textBoxDisplayCalConstants.Size = new System.Drawing.Size(186, 193);
            this.textBoxDisplayCalConstants.TabIndex = 1;
            // 
            // panelCommFlags
            // 
            this.panelCommFlags.Location = new System.Drawing.Point(157, 218);
            this.panelCommFlags.Name = "panelCommFlags";
            this.panelCommFlags.Size = new System.Drawing.Size(495, 376);
            this.panelCommFlags.TabIndex = 90;
            // 
            // ucCalibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxCalibration);
            this.Name = "ucCalibration";
            this.Size = new System.Drawing.Size(283, 225);
            this.groupBoxCalibration.ResumeLayout(false);
            this.groupBoxCalibration.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxCalibration;
        private System.Windows.Forms.Button buttonRestConstants;
        private System.Windows.Forms.Button buttonStartCal;
        private System.Windows.Forms.Button buttonCalHigh;
        private System.Windows.Forms.Button buttonSaveCalibration;
        private System.Windows.Forms.Button buttonReqCalConstants;
        private System.Windows.Forms.TextBox textBoxDisplayCalConstants;
        private System.Windows.Forms.Panel panelCommFlags;
    }
}
