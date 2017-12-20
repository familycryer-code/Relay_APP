namespace RelayControlLibrary
{
    partial class ucPhasorRequest
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
            this.groupBoxSinglePhasorRequest = new System.Windows.Forms.GroupBox();
            this.buttonRequest = new System.Windows.Forms.Button();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.labelType = new System.Windows.Forms.Label();
            this.labelPhase = new System.Windows.Forms.Label();
            this.comboBoxPhase = new System.Windows.Forms.ComboBox();
            this.labelIncPhase = new System.Windows.Forms.Label();
            this.labelIncType = new System.Windows.Forms.Label();
            this.labelIncReal = new System.Windows.Forms.Label();
            this.labelIncImaginary = new System.Windows.Forms.Label();
            this.labelIncRMS = new System.Windows.Forms.Label();
            this.textBoxIncPhase = new System.Windows.Forms.TextBox();
            this.textBoxIncType = new System.Windows.Forms.TextBox();
            this.textBoxIncReal = new System.Windows.Forms.TextBox();
            this.textBoxIncImaginary = new System.Windows.Forms.TextBox();
            this.textBoxIncRMS = new System.Windows.Forms.TextBox();
            this.groupBoxSinglePhasorRequest.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxSinglePhasorRequest
            // 
            this.groupBoxSinglePhasorRequest.Controls.Add(this.textBoxIncRMS);
            this.groupBoxSinglePhasorRequest.Controls.Add(this.textBoxIncImaginary);
            this.groupBoxSinglePhasorRequest.Controls.Add(this.textBoxIncReal);
            this.groupBoxSinglePhasorRequest.Controls.Add(this.textBoxIncType);
            this.groupBoxSinglePhasorRequest.Controls.Add(this.textBoxIncPhase);
            this.groupBoxSinglePhasorRequest.Controls.Add(this.labelIncRMS);
            this.groupBoxSinglePhasorRequest.Controls.Add(this.labelIncImaginary);
            this.groupBoxSinglePhasorRequest.Controls.Add(this.labelIncReal);
            this.groupBoxSinglePhasorRequest.Controls.Add(this.labelIncType);
            this.groupBoxSinglePhasorRequest.Controls.Add(this.labelIncPhase);
            this.groupBoxSinglePhasorRequest.Controls.Add(this.buttonRequest);
            this.groupBoxSinglePhasorRequest.Controls.Add(this.comboBoxType);
            this.groupBoxSinglePhasorRequest.Controls.Add(this.labelType);
            this.groupBoxSinglePhasorRequest.Controls.Add(this.labelPhase);
            this.groupBoxSinglePhasorRequest.Controls.Add(this.comboBoxPhase);
            this.groupBoxSinglePhasorRequest.Location = new System.Drawing.Point(3, 3);
            this.groupBoxSinglePhasorRequest.Name = "groupBoxSinglePhasorRequest";
            this.groupBoxSinglePhasorRequest.Size = new System.Drawing.Size(265, 162);
            this.groupBoxSinglePhasorRequest.TabIndex = 1;
            this.groupBoxSinglePhasorRequest.TabStop = false;
            this.groupBoxSinglePhasorRequest.Text = "Single Phasor Request";
            // 
            // buttonRequest
            // 
            this.buttonRequest.Location = new System.Drawing.Point(189, 19);
            this.buttonRequest.Name = "buttonRequest";
            this.buttonRequest.Size = new System.Drawing.Size(75, 23);
            this.buttonRequest.TabIndex = 4;
            this.buttonRequest.Text = "Request";
            this.buttonRequest.UseVisualStyleBackColor = true;
            this.buttonRequest.Click += new System.EventHandler(this.buttonRequest_Click);
            // 
            // comboBoxType
            // 
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Location = new System.Drawing.Point(62, 43);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(121, 21);
            this.comboBoxType.TabIndex = 3;
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.Location = new System.Drawing.Point(22, 46);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(34, 13);
            this.labelType.TabIndex = 2;
            this.labelType.Text = "Type:";
            // 
            // labelPhase
            // 
            this.labelPhase.AutoSize = true;
            this.labelPhase.Location = new System.Drawing.Point(16, 22);
            this.labelPhase.Name = "labelPhase";
            this.labelPhase.Size = new System.Drawing.Size(40, 13);
            this.labelPhase.TabIndex = 1;
            this.labelPhase.Text = "Phase:";
            // 
            // comboBoxPhase
            // 
            this.comboBoxPhase.FormattingEnabled = true;
            this.comboBoxPhase.Location = new System.Drawing.Point(62, 19);
            this.comboBoxPhase.Name = "comboBoxPhase";
            this.comboBoxPhase.Size = new System.Drawing.Size(121, 21);
            this.comboBoxPhase.TabIndex = 0;
            // 
            // labelIncPhase
            // 
            this.labelIncPhase.AutoSize = true;
            this.labelIncPhase.Location = new System.Drawing.Point(19, 72);
            this.labelIncPhase.Name = "labelIncPhase";
            this.labelIncPhase.Size = new System.Drawing.Size(40, 13);
            this.labelIncPhase.TabIndex = 5;
            this.labelIncPhase.Text = "Phase:";
            // 
            // labelIncType
            // 
            this.labelIncType.AutoSize = true;
            this.labelIncType.Location = new System.Drawing.Point(122, 72);
            this.labelIncType.Name = "labelIncType";
            this.labelIncType.Size = new System.Drawing.Size(34, 13);
            this.labelIncType.TabIndex = 6;
            this.labelIncType.Text = "Type:";
            // 
            // labelIncReal
            // 
            this.labelIncReal.AutoSize = true;
            this.labelIncReal.Location = new System.Drawing.Point(19, 95);
            this.labelIncReal.Name = "labelIncReal";
            this.labelIncReal.Size = new System.Drawing.Size(32, 13);
            this.labelIncReal.TabIndex = 7;
            this.labelIncReal.Text = "Real:";
            // 
            // labelIncImaginary
            // 
            this.labelIncImaginary.AutoSize = true;
            this.labelIncImaginary.Location = new System.Drawing.Point(19, 117);
            this.labelIncImaginary.Name = "labelIncImaginary";
            this.labelIncImaginary.Size = new System.Drawing.Size(27, 13);
            this.labelIncImaginary.TabIndex = 8;
            this.labelIncImaginary.Text = "Img:";
            // 
            // labelIncRMS
            // 
            this.labelIncRMS.AutoSize = true;
            this.labelIncRMS.Location = new System.Drawing.Point(19, 140);
            this.labelIncRMS.Name = "labelIncRMS";
            this.labelIncRMS.Size = new System.Drawing.Size(34, 13);
            this.labelIncRMS.TabIndex = 9;
            this.labelIncRMS.Text = "RMS:";
            // 
            // textBoxIncPhase
            // 
            this.textBoxIncPhase.Location = new System.Drawing.Point(62, 69);
            this.textBoxIncPhase.Name = "textBoxIncPhase";
            this.textBoxIncPhase.ReadOnly = true;
            this.textBoxIncPhase.Size = new System.Drawing.Size(43, 20);
            this.textBoxIncPhase.TabIndex = 10;
            // 
            // textBoxIncType
            // 
            this.textBoxIncType.Location = new System.Drawing.Point(162, 69);
            this.textBoxIncType.Name = "textBoxIncType";
            this.textBoxIncType.ReadOnly = true;
            this.textBoxIncType.Size = new System.Drawing.Size(43, 20);
            this.textBoxIncType.TabIndex = 11;
            // 
            // textBoxIncReal
            // 
            this.textBoxIncReal.Location = new System.Drawing.Point(62, 92);
            this.textBoxIncReal.Name = "textBoxIncReal";
            this.textBoxIncReal.ReadOnly = true;
            this.textBoxIncReal.Size = new System.Drawing.Size(94, 20);
            this.textBoxIncReal.TabIndex = 12;
            // 
            // textBoxIncImaginary
            // 
            this.textBoxIncImaginary.Location = new System.Drawing.Point(62, 114);
            this.textBoxIncImaginary.Name = "textBoxIncImaginary";
            this.textBoxIncImaginary.ReadOnly = true;
            this.textBoxIncImaginary.Size = new System.Drawing.Size(94, 20);
            this.textBoxIncImaginary.TabIndex = 13;
            // 
            // textBoxIncRMS
            // 
            this.textBoxIncRMS.Location = new System.Drawing.Point(62, 136);
            this.textBoxIncRMS.Name = "textBoxIncRMS";
            this.textBoxIncRMS.ReadOnly = true;
            this.textBoxIncRMS.Size = new System.Drawing.Size(94, 20);
            this.textBoxIncRMS.TabIndex = 14;
            // 
            // ucPhasorRequest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxSinglePhasorRequest);
            this.Name = "ucPhasorRequest";
            this.Size = new System.Drawing.Size(273, 169);
            this.groupBoxSinglePhasorRequest.ResumeLayout(false);
            this.groupBoxSinglePhasorRequest.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxPhase;
        private System.Windows.Forms.GroupBox groupBoxSinglePhasorRequest;
        private System.Windows.Forms.Label labelPhase;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.Button buttonRequest;
        private System.Windows.Forms.Label labelIncReal;
        private System.Windows.Forms.Label labelIncType;
        private System.Windows.Forms.Label labelIncPhase;
        private System.Windows.Forms.Label labelIncRMS;
        private System.Windows.Forms.Label labelIncImaginary;
        private System.Windows.Forms.TextBox textBoxIncRMS;
        private System.Windows.Forms.TextBox textBoxIncImaginary;
        private System.Windows.Forms.TextBox textBoxIncReal;
        private System.Windows.Forms.TextBox textBoxIncType;
        private System.Windows.Forms.TextBox textBoxIncPhase;
    }
}
