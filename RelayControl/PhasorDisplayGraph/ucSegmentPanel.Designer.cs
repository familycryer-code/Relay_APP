namespace PhasorDisplayGraph
{
    partial class ucSegmentPanel
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
            this.checkBoxCurveName = new System.Windows.Forms.CheckBox();
            this.numericUpDownOffset = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownAngle = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAngle)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBoxCurveName
            // 
            this.checkBoxCurveName.AutoSize = true;
            this.checkBoxCurveName.Checked = true;
            this.checkBoxCurveName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCurveName.Location = new System.Drawing.Point(131, 3);
            this.checkBoxCurveName.Name = "checkBoxCurveName";
            this.checkBoxCurveName.Size = new System.Drawing.Size(63, 17);
            this.checkBoxCurveName.TabIndex = 27;
            this.checkBoxCurveName.Text = "Curve 1";
            this.checkBoxCurveName.UseVisualStyleBackColor = true;
            this.checkBoxCurveName.CheckedChanged += new System.EventHandler(this.checkBoxCurveName_CheckedChanged);
            // 
            // numericUpDownOffset
            // 
            this.numericUpDownOffset.DecimalPlaces = 1;
            this.numericUpDownOffset.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownOffset.Location = new System.Drawing.Point(3, 2);
            this.numericUpDownOffset.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDownOffset.Name = "numericUpDownOffset";
            this.numericUpDownOffset.Size = new System.Drawing.Size(59, 20);
            this.numericUpDownOffset.TabIndex = 30;
            this.numericUpDownOffset.ValueChanged += new System.EventHandler(this.valueChangedEvent);
            // 
            // numericUpDownAngle
            // 
            this.numericUpDownAngle.Location = new System.Drawing.Point(68, 2);
            this.numericUpDownAngle.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownAngle.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            -2147483648});
            this.numericUpDownAngle.Name = "numericUpDownAngle";
            this.numericUpDownAngle.Size = new System.Drawing.Size(57, 20);
            this.numericUpDownAngle.TabIndex = 31;
            this.numericUpDownAngle.Value = new decimal(new int[] {
            5,
            0,
            0,
            -2147483648});
            this.numericUpDownAngle.ValueChanged += new System.EventHandler(this.valueChangedEvent);
            // 
            // ucSegmentPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.numericUpDownAngle);
            this.Controls.Add(this.numericUpDownOffset);
            this.Controls.Add(this.checkBoxCurveName);
            this.Name = "ucSegmentPanel";
            this.Size = new System.Drawing.Size(199, 25);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAngle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxCurveName;
        private System.Windows.Forms.NumericUpDown numericUpDownOffset;
        private System.Windows.Forms.NumericUpDown numericUpDownAngle;
    }
}
