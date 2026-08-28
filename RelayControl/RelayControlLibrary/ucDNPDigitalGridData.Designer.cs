namespace RelayControlLibrary
{
    partial class ucDNPDIGITALGRIDData
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
            this.buttonSendBinaryEventEnables = new System.Windows.Forms.Button();
            this.tabControlMemphisDNP = new System.Windows.Forms.TabControl();
            this.buttonDisableAllBinaryEvents = new System.Windows.Forms.Button();
            this.buttonEnableAllBinaryEvents = new System.Windows.Forms.Button();
            this.tabPageBinaryInputs = new System.Windows.Forms.TabPage();
            this.tabPageBinaryInputs2 = new System.Windows.Forms.TabPage();
            this.tabPageBinaryOuputs = new System.Windows.Forms.TabPage();
            this.tabPageAnalogInputs1 = new System.Windows.Forms.TabPage();
            this.tabPageAnalogInputs2 = new System.Windows.Forms.TabPage();
            this.tabPageAnalogInputs3 = new System.Windows.Forms.TabPage();
            this.tabPageAnalogInputs4 = new System.Windows.Forms.TabPage();
            this.buttonDisableAllAnalogEvents = new System.Windows.Forms.Button();
            this.buttonEnableAllAnalogEvents = new System.Windows.Forms.Button();
            this.buttonSendAnalogEnables = new System.Windows.Forms.Button();
            this.tabPageAnalogOutputs = new System.Windows.Forms.TabPage();
            this.tabControlMemphisDNP.SuspendLayout();
            this.tabPageBinaryInputs.SuspendLayout();
            this.tabPageBinaryInputs2.SuspendLayout();
            this.tabPageAnalogInputs1.SuspendLayout();
            this.tabPageAnalogInputs2.SuspendLayout();
            this.tabPageAnalogInputs3.SuspendLayout();
            this.tabPageAnalogInputs4.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSendBinaryEventEnables
            // 
            this.buttonSendBinaryEventEnables.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSendBinaryEventEnables.Location = new System.Drawing.Point(743, 509);
            this.buttonSendBinaryEventEnables.Name = "buttonSendBinaryEventEnables";
            this.buttonSendBinaryEventEnables.Size = new System.Drawing.Size(204, 30);
            this.buttonSendBinaryEventEnables.TabIndex = 2;
            this.buttonSendBinaryEventEnables.Text = "Send Binary Event Enables";
            this.buttonSendBinaryEventEnables.UseVisualStyleBackColor = true;
            this.buttonSendBinaryEventEnables.Click += new System.EventHandler(this.buttonSendBinaryEventEnables_Click);
            // 
            // tabControlMemphisDNP
            // 
            this.tabControlMemphisDNP.Controls.Add(this.tabPageBinaryInputs);
            this.tabControlMemphisDNP.Controls.Add(this.tabPageBinaryOuputs);
            this.tabControlMemphisDNP.Controls.Add(this.tabPageAnalogInputs1);
            this.tabControlMemphisDNP.Controls.Add(this.tabPageAnalogInputs2);
            this.tabControlMemphisDNP.Controls.Add(this.tabPageAnalogOutputs);
            this.tabControlMemphisDNP.Dock = System.Windows.Forms.DockStyle.Fill; // added
            this.tabControlMemphisDNP.Location = new System.Drawing.Point(0, 0);
            this.tabControlMemphisDNP.Name = "tabControlMemphisDNP";
            this.tabControlMemphisDNP.SelectedIndex = 0;
            // this.tabControlMemphisDNP.Size = new System.Drawing.Size(989, 570); // removed fixed size
            this.tabControlMemphisDNP.TabIndex = 0;
            //this.tabControlMemphisDNP.Resize += new System.EventHandler(this.tabControlMemphisDNP_Resize);
            this.tabControlMemphisDNP.SelectedIndexChanged += new System.EventHandler(this.tabControlMemphisDNP_SelectedIndexChanged);
            // 
            // tabPageBinaryInputs
            // 
            this.tabPageBinaryInputs.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageBinaryInputs.Controls.Add(this.buttonDisableAllBinaryEvents);
            this.tabPageBinaryInputs.Controls.Add(this.buttonEnableAllBinaryEvents);
            this.tabPageBinaryInputs.Controls.Add(this.buttonSendBinaryEventEnables);
            this.tabPageBinaryInputs.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageBinaryInputs.Location = new System.Drawing.Point(4, 25);
            this.tabPageBinaryInputs.Name = "tabPageBinaryInputs";
            this.tabPageBinaryInputs.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBinaryInputs.Size = new System.Drawing.Size(981, 541);
            this.tabPageBinaryInputs.TabIndex = 0;
            this.tabPageBinaryInputs.Text = "Binary Inputs";
            // 
            // tabPageBinaryInputs2
            // 
            this.tabPageBinaryInputs2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageBinaryInputs2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageBinaryInputs2.Location = new System.Drawing.Point(4, 25);
            this.tabPageBinaryInputs2.Name = "tabPageBinaryInputs2";
            this.tabPageBinaryInputs2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBinaryInputs2.Size = new System.Drawing.Size(981, 541);
            this.tabPageBinaryInputs2.TabIndex = 1;
            this.tabPageBinaryInputs2.Text = "Binary Inputs";
            // 
            // buttonDisableAllBinaryEvents
            // 
            this.buttonDisableAllBinaryEvents.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDisableAllBinaryEvents.Location = new System.Drawing.Point(743, 437);
            this.buttonDisableAllBinaryEvents.Name = "buttonDisableAllBinaryEvents";
            this.buttonDisableAllBinaryEvents.Size = new System.Drawing.Size(204, 30);
            this.buttonDisableAllBinaryEvents.TabIndex = 4;
            this.buttonDisableAllBinaryEvents.Text = "Disable All Binary Events";
            this.buttonDisableAllBinaryEvents.UseVisualStyleBackColor = true;
            this.buttonDisableAllBinaryEvents.Click += new System.EventHandler(this.buttonDisableAllBinaryEvents_Click);
            // 
            // buttonEnableAllBinaryEvents
            // 
            this.buttonEnableAllBinaryEvents.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonEnableAllBinaryEvents.Location = new System.Drawing.Point(743, 473);
            this.buttonEnableAllBinaryEvents.Name = "buttonEnableAllBinaryEvents";
            this.buttonEnableAllBinaryEvents.Size = new System.Drawing.Size(204, 30);
            this.buttonEnableAllBinaryEvents.TabIndex = 3;
            this.buttonEnableAllBinaryEvents.Text = "Enable All Binary Events";
            this.buttonEnableAllBinaryEvents.UseVisualStyleBackColor = true;
            this.buttonEnableAllBinaryEvents.Click += new System.EventHandler(this.buttonEnableAllBinaryEvents_Click);
            // 
            // tabPageBinaryOuputs
            // 
            this.tabPageBinaryOuputs.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageBinaryOuputs.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageBinaryOuputs.Location = new System.Drawing.Point(4, 25);
            this.tabPageBinaryOuputs.Name = "tabPageBinaryOuputs";
            this.tabPageBinaryOuputs.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBinaryOuputs.Size = new System.Drawing.Size(981, 541);
            this.tabPageBinaryOuputs.TabIndex = 1;
            this.tabPageBinaryOuputs.Text = "Binary Outputs";
            // 
            // tabPageAnalogInputs1
            // 
            this.tabPageAnalogInputs1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageAnalogInputs1.Controls.Add(this.buttonDisableAllAnalogEvents);
            this.tabPageAnalogInputs1.Controls.Add(this.buttonEnableAllAnalogEvents);
            this.tabPageAnalogInputs1.Controls.Add(this.buttonSendAnalogEnables);
            this.tabPageAnalogInputs1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageAnalogInputs1.Location = new System.Drawing.Point(4, 25);
            this.tabPageAnalogInputs1.Name = "tabPageAnalogInputs1";
            this.tabPageAnalogInputs1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAnalogInputs1.Size = new System.Drawing.Size(981, 541);
            this.tabPageAnalogInputs1.TabIndex = 2;
            this.tabPageAnalogInputs1.Text = "Analog Inputs";
            // 
            // tabPageAnalogInputs2
            // 
            this.tabPageAnalogInputs2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageAnalogInputs2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageAnalogInputs2.Location = new System.Drawing.Point(4, 25);
            this.tabPageAnalogInputs2.Name = "tabPageAnalogInputs2";
            this.tabPageAnalogInputs2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAnalogInputs2.Size = new System.Drawing.Size(981, 541);
            this.tabPageAnalogInputs2.TabIndex = 2;
            this.tabPageAnalogInputs2.Text = "Analog Inputs";
            // 
            // tabPageAnalogInputs3
            // 
            this.tabPageAnalogInputs3.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageAnalogInputs3.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageAnalogInputs3.Location = new System.Drawing.Point(4, 25);
            this.tabPageAnalogInputs3.Name = "tabPageAnalogInputs3";
            this.tabPageAnalogInputs3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAnalogInputs3.Size = new System.Drawing.Size(981, 541);
            this.tabPageAnalogInputs3.TabIndex = 2;
            this.tabPageAnalogInputs3.Text = "Analog Inputs";

            // 
            // tabPageAnalogInputs4
            // 
            this.tabPageAnalogInputs4.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageAnalogInputs4.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageAnalogInputs4.Location = new System.Drawing.Point(4, 25);
            this.tabPageAnalogInputs4.Name = "tabPageAnalogInputs4";
            this.tabPageAnalogInputs4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAnalogInputs4.Size = new System.Drawing.Size(981, 541);
            this.tabPageAnalogInputs4.TabIndex = 2;
            this.tabPageAnalogInputs4.Text = "Analog Inputs";
            // 
            // buttonDisableAllAnalogEvents
            // 
            this.buttonDisableAllAnalogEvents.Location = new System.Drawing.Point(443, 515);
            this.buttonDisableAllAnalogEvents.Name = "buttonDisableAllAnalogEvents";
            this.buttonDisableAllAnalogEvents.Size = new System.Drawing.Size(214, 34); //(183, 23);
            this.buttonDisableAllAnalogEvents.TabIndex = 6;
            this.buttonDisableAllAnalogEvents.Text = "Disable All Analog Events (this tab)";
            this.buttonDisableAllAnalogEvents.UseVisualStyleBackColor = true;
            this.buttonDisableAllAnalogEvents.Click += new System.EventHandler(this.buttonDisableAllAnalogEvents_Click);
            // 
            // buttonEnableAllAnalogEvents
            // 
            this.buttonEnableAllAnalogEvents.Location = new System.Drawing.Point(632, 515);
            this.buttonEnableAllAnalogEvents.Name = "buttonEnableAllAnalogEvents";
            this.buttonEnableAllAnalogEvents.Size = new System.Drawing.Size(214, 34); //(183, 23);
            this.buttonEnableAllAnalogEvents.TabIndex = 5;
            this.buttonEnableAllAnalogEvents.Text = "Enable All Analog Events (this tab)";
            this.buttonEnableAllAnalogEvents.UseVisualStyleBackColor = true;
            this.buttonEnableAllAnalogEvents.Click += new System.EventHandler(this.buttonEnableAllAnalogEvents_Click);
            // 
            // buttonSendAnalogEnables
            // 
            this.buttonSendAnalogEnables.Location = new System.Drawing.Point(821, 515);
            this.buttonSendAnalogEnables.Name = "buttonSendAnalogEnables";
            this.buttonSendAnalogEnables.Size = new System.Drawing.Size(214, 34); //(154, 23);
            this.buttonSendAnalogEnables.TabIndex = 4;
            this.buttonSendAnalogEnables.Text = "Send Analog Event Enables";
            this.buttonSendAnalogEnables.UseVisualStyleBackColor = true;
            this.buttonSendAnalogEnables.Click += new System.EventHandler(this.buttonSendAnalogEnables_Click);
            // 
            // tabPageAnalogOutputs
            // 
            this.tabPageAnalogOutputs.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageAnalogOutputs.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageAnalogOutputs.Location = new System.Drawing.Point(4, 25);
            this.tabPageAnalogOutputs.Name = "tabPageAnalogOutputs";
            this.tabPageAnalogOutputs.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAnalogOutputs.Size = new System.Drawing.Size(981, 541);
            this.tabPageAnalogOutputs.TabIndex = 4;
            this.tabPageAnalogOutputs.Text = "Analog Outputs";
            // 
            // tabPageAnalogInputs2
            // 
            this.tabPageAnalogInputs2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageAnalogInputs2.Location = new System.Drawing.Point(4, 22);
            this.tabPageAnalogInputs2.Name = "tabPageAnalogInputs2";
            this.tabPageAnalogInputs2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAnalogInputs2.Size = new System.Drawing.Size(981, 544);
            this.tabPageAnalogInputs2.TabIndex = 3;
            this.tabPageAnalogInputs2.Text = "Analog Inputs";
            // 
            // tabPageAnalogInputs3
            // 
            this.tabPageAnalogInputs3.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageAnalogInputs3.Location = new System.Drawing.Point(4, 22);
            this.tabPageAnalogInputs3.Name = "tabPageAnalogInputs3";
            this.tabPageAnalogInputs3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAnalogInputs3.Size = new System.Drawing.Size(981, 544);
            this.tabPageAnalogInputs3.TabIndex = 3;
            this.tabPageAnalogInputs3.Text = "Analog Inputs";

            // 
            // tabPageAnalogInputs4
            // 
            this.tabPageAnalogInputs4.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageAnalogInputs4.Location = new System.Drawing.Point(4, 22);
            this.tabPageAnalogInputs4.Name = "tabPageAnalogInputs4";
            this.tabPageAnalogInputs4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAnalogInputs4.Size = new System.Drawing.Size(981, 544);
            this.tabPageAnalogInputs4.TabIndex = 3;
            this.tabPageAnalogInputs4.Text = "Analog Inputs";
            // 
            // ucDNPDIGITALGRIDData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlMemphisDNP);
            this.Name = "ucDNPDIGITALGRIDData";
            this.Size = new System.Drawing.Size(1310, 682);
            this.tabControlMemphisDNP.ResumeLayout(false);
            this.tabPageBinaryInputs.ResumeLayout(false);
            this.tabPageBinaryInputs2.ResumeLayout(false);
            this.tabPageAnalogInputs1.ResumeLayout(false);
            this.tabPageAnalogInputs2.ResumeLayout(false);
            this.tabPageAnalogInputs3.ResumeLayout(false);
            this.tabPageAnalogInputs4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

#endregion

        private System.Windows.Forms.Button buttonSendBinaryEventEnables;
        private System.Windows.Forms.TabControl tabControlMemphisDNP;
        public System.Windows.Forms.TabPage tabPageBinaryInputs;
        private System.Windows.Forms.TabPage tabPageBinaryInputs2;
        private System.Windows.Forms.TabPage tabPageBinaryOuputs;
        private System.Windows.Forms.TabPage tabPageAnalogInputs1;
        private System.Windows.Forms.TabPage tabPageAnalogInputs2;
        private System.Windows.Forms.TabPage tabPageAnalogInputs3;
        private System.Windows.Forms.TabPage tabPageAnalogInputs4;
        private System.Windows.Forms.TabPage tabPageAnalogOutputs;
        private System.Windows.Forms.Button buttonSendAnalogEnables;
        private System.Windows.Forms.Button buttonEnableAllBinaryEvents;
        private System.Windows.Forms.Button buttonEnableAllAnalogEvents;
        private System.Windows.Forms.Button buttonDisableAllBinaryEvents;
        private System.Windows.Forms.Button buttonDisableAllAnalogEvents;
    }
}
