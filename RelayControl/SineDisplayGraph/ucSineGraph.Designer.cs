using RelayControlLibrary;
namespace SineDisplayGraph
{
    partial class ucSineGraph
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
            this.checkBoxPhAVn = new System.Windows.Forms.CheckBox();
            this.checkBoxPhAI = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxPhBI = new System.Windows.Forms.CheckBox();
            this.checkBoxPhBVn = new System.Windows.Forms.CheckBox();
            this.checkBoxPhCI = new System.Windows.Forms.CheckBox();
            this.checkBoxPhCVn = new System.Windows.Forms.CheckBox();
            this.checkBox10xCurrent = new System.Windows.Forms.CheckBox();
            this.labelVoltageMaxValue = new System.Windows.Forms.Label();
            this.labelCurrentMaxValue = new System.Windows.Forms.Label();
            this.sineGraph1 = new SineGraph();
            this.SuspendLayout();
            // 
            // checkBoxPhAVn
            // 
            this.checkBoxPhAVn.AutoSize = true;
            this.checkBoxPhAVn.Location = new System.Drawing.Point(518, 43);
            this.checkBoxPhAVn.Name = "checkBoxPhAVn";
            this.checkBoxPhAVn.Size = new System.Drawing.Size(39, 17);
            this.checkBoxPhAVn.TabIndex = 2;
            this.checkBoxPhAVn.Text = "Vn";
            this.checkBoxPhAVn.UseVisualStyleBackColor = true;
            this.checkBoxPhAVn.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged1);
            // 
            // checkBoxPhAI
            // 
            this.checkBoxPhAI.AutoSize = true;
            this.checkBoxPhAI.Location = new System.Drawing.Point(518, 20);
            this.checkBoxPhAI.Name = "checkBoxPhAI";
            this.checkBoxPhAI.Size = new System.Drawing.Size(29, 17);
            this.checkBoxPhAI.TabIndex = 3;
            this.checkBoxPhAI.Text = "I";
            this.checkBoxPhAI.UseVisualStyleBackColor = true;
            this.checkBoxPhAI.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(515, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "PhA";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(561, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "PhB";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(606, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "PhC";
            // 
            // checkBoxPhBI
            // 
            this.checkBoxPhBI.AutoSize = true;
            this.checkBoxPhBI.Location = new System.Drawing.Point(564, 20);
            this.checkBoxPhBI.Name = "checkBoxPhBI";
            this.checkBoxPhBI.Size = new System.Drawing.Size(29, 17);
            this.checkBoxPhBI.TabIndex = 15;
            this.checkBoxPhBI.Text = "I";
            this.checkBoxPhBI.UseVisualStyleBackColor = true;
            this.checkBoxPhBI.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged1);
            // 
            // checkBoxPhBVn
            // 
            this.checkBoxPhBVn.AutoSize = true;
            this.checkBoxPhBVn.Location = new System.Drawing.Point(564, 43);
            this.checkBoxPhBVn.Name = "checkBoxPhBVn";
            this.checkBoxPhBVn.Size = new System.Drawing.Size(39, 17);
            this.checkBoxPhBVn.TabIndex = 14;
            this.checkBoxPhBVn.Text = "Vn";
            this.checkBoxPhBVn.UseVisualStyleBackColor = true;
            this.checkBoxPhBVn.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged1);
            // 
            // checkBoxPhCI
            // 
            this.checkBoxPhCI.AutoSize = true;
            this.checkBoxPhCI.Location = new System.Drawing.Point(609, 20);
            this.checkBoxPhCI.Name = "checkBoxPhCI";
            this.checkBoxPhCI.Size = new System.Drawing.Size(29, 17);
            this.checkBoxPhCI.TabIndex = 18;
            this.checkBoxPhCI.Text = "I";
            this.checkBoxPhCI.UseVisualStyleBackColor = true;
            this.checkBoxPhCI.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged1);
            // 
            // checkBoxPhCVn
            // 
            this.checkBoxPhCVn.AutoSize = true;
            this.checkBoxPhCVn.Location = new System.Drawing.Point(609, 43);
            this.checkBoxPhCVn.Name = "checkBoxPhCVn";
            this.checkBoxPhCVn.Size = new System.Drawing.Size(39, 17);
            this.checkBoxPhCVn.TabIndex = 17;
            this.checkBoxPhCVn.Text = "Vn";
            this.checkBoxPhCVn.UseVisualStyleBackColor = true;
            this.checkBoxPhCVn.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged1);
            // 
            // checkBox10xCurrent
            // 
            this.checkBox10xCurrent.AutoSize = true;
            this.checkBox10xCurrent.Location = new System.Drawing.Point(518, 66);
            this.checkBox10xCurrent.Name = "checkBox10xCurrent";
            this.checkBox10xCurrent.Size = new System.Drawing.Size(80, 17);
            this.checkBox10xCurrent.TabIndex = 20;
            this.checkBox10xCurrent.Text = "10x Current";
            this.checkBox10xCurrent.UseVisualStyleBackColor = true;
            this.checkBox10xCurrent.CheckedChanged += new System.EventHandler(this.checkBox10xCurrent_CheckedChanged);
            // 
            // labelVoltageMaxValue
            // 
            this.labelVoltageMaxValue.AutoSize = true;
            this.labelVoltageMaxValue.Location = new System.Drawing.Point(465, 4);
            this.labelVoltageMaxValue.Name = "labelVoltageMaxValue";
            this.labelVoltageMaxValue.Size = new System.Drawing.Size(35, 13);
            this.labelVoltageMaxValue.TabIndex = 21;
            this.labelVoltageMaxValue.Text = "254 V";
            // 
            // labelCurrentMaxValue
            // 
            this.labelCurrentMaxValue.AutoSize = true;
            this.labelCurrentMaxValue.Location = new System.Drawing.Point(465, 261);
            this.labelCurrentMaxValue.Name = "labelCurrentMaxValue";
            this.labelCurrentMaxValue.Size = new System.Drawing.Size(32, 13);
            this.labelCurrentMaxValue.TabIndex = 22;
            this.labelCurrentMaxValue.Text = "-15 A";
            // 
            // sineGraph1
            // 
            this.sineGraph1.BackColor = System.Drawing.Color.White;
            this.sineGraph1.GraphName = "";
            this.sineGraph1.Location = new System.Drawing.Point(3, 3);
            this.sineGraph1.Name = "sineGraph1";
            this.sineGraph1.PointsToDraw = 0;
            this.sineGraph1.ScrollEnabled = false;
            this.sineGraph1.Size = new System.Drawing.Size(455, 271);
            this.sineGraph1.TabIndex = 0;
            // 
            // ucSineGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelCurrentMaxValue);
            this.Controls.Add(this.labelVoltageMaxValue);
            this.Controls.Add(this.checkBox10xCurrent);
            this.Controls.Add(this.checkBoxPhCI);
            this.Controls.Add(this.checkBoxPhCVn);
            this.Controls.Add(this.checkBoxPhBI);
            this.Controls.Add(this.checkBoxPhBVn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBoxPhAI);
            this.Controls.Add(this.checkBoxPhAVn);
            this.Controls.Add(this.sineGraph1);
            this.Name = "ucSineGraph";
            this.Size = new System.Drawing.Size(657, 279);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SineGraph sineGraph1;
        private System.Windows.Forms.CheckBox checkBoxPhAVn;
        private System.Windows.Forms.CheckBox checkBoxPhAI;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBoxPhBI;
        private System.Windows.Forms.CheckBox checkBoxPhBVn;
        private System.Windows.Forms.CheckBox checkBoxPhCI;
        private System.Windows.Forms.CheckBox checkBoxPhCVn;
        private System.Windows.Forms.CheckBox checkBox10xCurrent;
        private System.Windows.Forms.Label labelVoltageMaxValue;
        private System.Windows.Forms.Label labelCurrentMaxValue;
    }
}
