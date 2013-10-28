namespace RelayControlLibrary
{
    partial class ucMemphisDNPData
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
            this.tabControlMemphisDNP = new System.Windows.Forms.TabControl();
            this.tabPageBinaryInputs = new System.Windows.Forms.TabPage();
            this.tabPageBinaryOuputs = new System.Windows.Forms.TabPage();
            this.tabPageAnalogInputs1 = new System.Windows.Forms.TabPage();
            this.tabPageAnalogInputs2 = new System.Windows.Forms.TabPage();
            this.tabPageAnalogOutputs = new System.Windows.Forms.TabPage();
            this.buttonSendBinaryEventEnables = new System.Windows.Forms.Button();
            this.tabControlMemphisDNP.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMemphisDNP
            // 
            this.tabControlMemphisDNP.Controls.Add(this.tabPageBinaryInputs);
            this.tabControlMemphisDNP.Controls.Add(this.tabPageBinaryOuputs);
            this.tabControlMemphisDNP.Controls.Add(this.tabPageAnalogInputs1);
            this.tabControlMemphisDNP.Controls.Add(this.tabPageAnalogInputs2);
            this.tabControlMemphisDNP.Controls.Add(this.tabPageAnalogOutputs);
            this.tabControlMemphisDNP.Location = new System.Drawing.Point(0, 0);
            this.tabControlMemphisDNP.Name = "tabControlMemphisDNP";
            this.tabControlMemphisDNP.SelectedIndex = 0;
            this.tabControlMemphisDNP.Size = new System.Drawing.Size(989, 570);
            this.tabControlMemphisDNP.TabIndex = 0;
            this.tabControlMemphisDNP.Resize += new System.EventHandler(this.tabControlMemphisDNP_Resize);
            this.tabControlMemphisDNP.SelectedIndexChanged += new System.EventHandler(this.tabControlMemphisDNP_SelectedIndexChanged);
            // 
            // tabPageBinaryInputs
            // 
            this.tabPageBinaryInputs.Location = new System.Drawing.Point(4, 22);
            this.tabPageBinaryInputs.Name = "tabPageBinaryInputs";
            this.tabPageBinaryInputs.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBinaryInputs.Size = new System.Drawing.Size(981, 544);
            this.tabPageBinaryInputs.TabIndex = 0;
            this.tabPageBinaryInputs.Text = "Binary Inputs";
            this.tabPageBinaryInputs.UseVisualStyleBackColor = true;
            // 
            // tabPageBinaryOuputs
            // 
            this.tabPageBinaryOuputs.Location = new System.Drawing.Point(4, 22);
            this.tabPageBinaryOuputs.Name = "tabPageBinaryOuputs";
            this.tabPageBinaryOuputs.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBinaryOuputs.Size = new System.Drawing.Size(981, 544);
            this.tabPageBinaryOuputs.TabIndex = 1;
            this.tabPageBinaryOuputs.Text = "Binary Outputs";
            this.tabPageBinaryOuputs.UseVisualStyleBackColor = true;
            // 
            // tabPageAnalogInputs1
            // 
            this.tabPageAnalogInputs1.Location = new System.Drawing.Point(4, 22);
            this.tabPageAnalogInputs1.Name = "tabPageAnalogInputs1";
            this.tabPageAnalogInputs1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAnalogInputs1.Size = new System.Drawing.Size(981, 544);
            this.tabPageAnalogInputs1.TabIndex = 2;
            this.tabPageAnalogInputs1.Text = "Analog Inputs";
            this.tabPageAnalogInputs1.UseVisualStyleBackColor = true;
            // 
            // tabPageAnalogInputs2
            // 
            this.tabPageAnalogInputs2.Location = new System.Drawing.Point(4, 22);
            this.tabPageAnalogInputs2.Name = "tabPageAnalogInputs2";
            this.tabPageAnalogInputs2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAnalogInputs2.Size = new System.Drawing.Size(981, 544);
            this.tabPageAnalogInputs2.TabIndex = 3;
            this.tabPageAnalogInputs2.Text = "Analog Inputs";
            this.tabPageAnalogInputs2.UseVisualStyleBackColor = true;
            // 
            // tabPageAnalogOutputs
            // 
            this.tabPageAnalogOutputs.Location = new System.Drawing.Point(4, 22);
            this.tabPageAnalogOutputs.Name = "tabPageAnalogOutputs";
            this.tabPageAnalogOutputs.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAnalogOutputs.Size = new System.Drawing.Size(981, 544);
            this.tabPageAnalogOutputs.TabIndex = 4;
            this.tabPageAnalogOutputs.Text = "Analog Outputs";
            this.tabPageAnalogOutputs.UseVisualStyleBackColor = true;
            // 
            // buttonSendBinaryEventEnables
            // 
            this.buttonSendBinaryEventEnables.Location = new System.Drawing.Point(831, 572);
            this.buttonSendBinaryEventEnables.Name = "buttonSendBinaryEventEnables";
            this.buttonSendBinaryEventEnables.Size = new System.Drawing.Size(154, 23);
            this.buttonSendBinaryEventEnables.TabIndex = 1;
            this.buttonSendBinaryEventEnables.Text = "Send Binary Event Enables";
            this.buttonSendBinaryEventEnables.UseVisualStyleBackColor = true;
            this.buttonSendBinaryEventEnables.Click += new System.EventHandler(this.buttonSendBinaryInputEventEnables_Click);
            // 
            // ucMemphisDNPData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonSendBinaryEventEnables);
            this.Controls.Add(this.tabControlMemphisDNP);
            this.Name = "ucMemphisDNPData";
            this.Size = new System.Drawing.Size(989, 598);
            this.tabControlMemphisDNP.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMemphisDNP;
        private System.Windows.Forms.TabPage tabPageBinaryInputs;
        private System.Windows.Forms.TabPage tabPageBinaryOuputs;
        private System.Windows.Forms.TabPage tabPageAnalogInputs1;
        private System.Windows.Forms.TabPage tabPageAnalogInputs2;
        private System.Windows.Forms.TabPage tabPageAnalogOutputs;
        private System.Windows.Forms.Button buttonSendBinaryEventEnables;
    }
}
