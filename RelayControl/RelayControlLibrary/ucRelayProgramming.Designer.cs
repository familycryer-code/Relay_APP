namespace RelayControlLibrary
{
    partial class ucRelayProgramming
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
            this.components = new System.ComponentModel.Container();
            this.buttonSelectMasterSFile = new System.Windows.Forms.Button();
            this.buttonSelectRelaySFile = new System.Windows.Forms.Button();
            this.buttonProgramMaster = new System.Windows.Forms.Button();
            this.buttonProgramRelay = new System.Windows.Forms.Button();
            this.textBoxMasterFileName = new System.Windows.Forms.TextBox();
            this.textBoxRelayFileName = new System.Windows.Forms.TextBox();
            this.labelCode = new System.Windows.Forms.Label();
            this.labelData = new System.Windows.Forms.Label();
            this.labelCodeTotal = new System.Windows.Forms.Label();
            this.labelDataTotal = new System.Windows.Forms.Label();
            this.labelCodeCount = new System.Windows.Forms.Label();
            this.labelDataCount = new System.Windows.Forms.Label();
            this.timerTimeout = new System.Windows.Forms.Timer(this.components);
            this.labelState = new System.Windows.Forms.Label();
            this.textBoxFPGAFile = new System.Windows.Forms.TextBox();
            this.buttonSelectFPGAFile = new System.Windows.Forms.Button();
            this.buttonProgramFPGA = new System.Windows.Forms.Button();
            this.buttonStartAutoLoad = new System.Windows.Forms.Button();
            this.buttonFirstLoad = new System.Windows.Forms.Button();
            this.buttonLoadNewest = new System.Windows.Forms.Button();
            this.buttonFixBootLoader = new System.Windows.Forms.Button();
            this.labelCustomer = new System.Windows.Forms.Label();
            this.comboBoxCustomer = new System.Windows.Forms.ComboBox();
            this.buttonClearAllProgrammingFields = new System.Windows.Forms.Button();
            this.buttonLoadDefaultResourceSFiles = new System.Windows.Forms.Button();
            this.buttonProgramMasterBootCode = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonSelectMasterSFile
            // 
            this.buttonSelectMasterSFile.Location = new System.Drawing.Point(349, 5);
            this.buttonSelectMasterSFile.Name = "buttonSelectMasterSFile";
            this.buttonSelectMasterSFile.Size = new System.Drawing.Size(101, 23);
            this.buttonSelectMasterSFile.TabIndex = 0;
            this.buttonSelectMasterSFile.Text = "Select Master File";
            this.buttonSelectMasterSFile.UseVisualStyleBackColor = true;
            this.buttonSelectMasterSFile.Click += new System.EventHandler(this.buttonSelectMasterSFile_Click);
            // 
            // buttonSelectRelaySFile
            // 
            this.buttonSelectRelaySFile.Location = new System.Drawing.Point(349, 31);
            this.buttonSelectRelaySFile.Name = "buttonSelectRelaySFile";
            this.buttonSelectRelaySFile.Size = new System.Drawing.Size(101, 23);
            this.buttonSelectRelaySFile.TabIndex = 1;
            this.buttonSelectRelaySFile.Text = "Select Relay File";
            this.buttonSelectRelaySFile.UseVisualStyleBackColor = true;
            this.buttonSelectRelaySFile.Click += new System.EventHandler(this.buttonSelectRelaySFile_Click);
            // 
            // buttonProgramMaster
            // 
            this.buttonProgramMaster.Location = new System.Drawing.Point(349, 86);
            this.buttonProgramMaster.Name = "buttonProgramMaster";
            this.buttonProgramMaster.Size = new System.Drawing.Size(101, 23);
            this.buttonProgramMaster.TabIndex = 2;
            this.buttonProgramMaster.Text = "Program Master";
            this.buttonProgramMaster.UseVisualStyleBackColor = true;
            this.buttonProgramMaster.Click += new System.EventHandler(this.buttonProgramMaster_Click);
            // 
            // buttonProgramRelay
            // 
            this.buttonProgramRelay.Location = new System.Drawing.Point(349, 110);
            this.buttonProgramRelay.Name = "buttonProgramRelay";
            this.buttonProgramRelay.Size = new System.Drawing.Size(101, 23);
            this.buttonProgramRelay.TabIndex = 3;
            this.buttonProgramRelay.Text = "Program Relay";
            this.buttonProgramRelay.UseVisualStyleBackColor = true;
            this.buttonProgramRelay.Click += new System.EventHandler(this.buttonProgramRelay_Click);
            // 
            // textBoxMasterFileName
            // 
            this.textBoxMasterFileName.Location = new System.Drawing.Point(6, 5);
            this.textBoxMasterFileName.Name = "textBoxMasterFileName";
            this.textBoxMasterFileName.ReadOnly = true;
            this.textBoxMasterFileName.Size = new System.Drawing.Size(337, 20);
            this.textBoxMasterFileName.TabIndex = 4;
            // 
            // textBoxRelayFileName
            // 
            this.textBoxRelayFileName.Location = new System.Drawing.Point(6, 31);
            this.textBoxRelayFileName.Name = "textBoxRelayFileName";
            this.textBoxRelayFileName.ReadOnly = true;
            this.textBoxRelayFileName.Size = new System.Drawing.Size(337, 20);
            this.textBoxRelayFileName.TabIndex = 5;
            // 
            // labelCode
            // 
            this.labelCode.AutoSize = true;
            this.labelCode.Location = new System.Drawing.Point(12, 85);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(35, 13);
            this.labelCode.TabIndex = 6;
            this.labelCode.Text = "Code:";
            // 
            // labelData
            // 
            this.labelData.AutoSize = true;
            this.labelData.Location = new System.Drawing.Point(14, 102);
            this.labelData.Name = "labelData";
            this.labelData.Size = new System.Drawing.Size(33, 13);
            this.labelData.TabIndex = 7;
            this.labelData.Text = "Data:";
            // 
            // labelCodeTotal
            // 
            this.labelCodeTotal.AutoSize = true;
            this.labelCodeTotal.Location = new System.Drawing.Point(53, 85);
            this.labelCodeTotal.Name = "labelCodeTotal";
            this.labelCodeTotal.Size = new System.Drawing.Size(13, 13);
            this.labelCodeTotal.TabIndex = 8;
            this.labelCodeTotal.Text = "0";
            // 
            // labelDataTotal
            // 
            this.labelDataTotal.AutoSize = true;
            this.labelDataTotal.Location = new System.Drawing.Point(53, 102);
            this.labelDataTotal.Name = "labelDataTotal";
            this.labelDataTotal.Size = new System.Drawing.Size(13, 13);
            this.labelDataTotal.TabIndex = 9;
            this.labelDataTotal.Text = "0";
            // 
            // labelCodeCount
            // 
            this.labelCodeCount.AutoSize = true;
            this.labelCodeCount.Location = new System.Drawing.Point(84, 85);
            this.labelCodeCount.Name = "labelCodeCount";
            this.labelCodeCount.Size = new System.Drawing.Size(13, 13);
            this.labelCodeCount.TabIndex = 10;
            this.labelCodeCount.Text = "0";
            // 
            // labelDataCount
            // 
            this.labelDataCount.AutoSize = true;
            this.labelDataCount.Location = new System.Drawing.Point(84, 102);
            this.labelDataCount.Name = "labelDataCount";
            this.labelDataCount.Size = new System.Drawing.Size(13, 13);
            this.labelDataCount.TabIndex = 11;
            this.labelDataCount.Text = "0";
            // 
            // timerTimeout
            // 
            this.timerTimeout.Interval = 1000;
            this.timerTimeout.Tick += new System.EventHandler(this.timerTimeout_Tick);
            // 
            // labelState
            // 
            this.labelState.AutoSize = true;
            this.labelState.Location = new System.Drawing.Point(103, 82);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(0, 13);
            this.labelState.TabIndex = 13;
            // 
            // textBoxFPGAFile
            // 
            this.textBoxFPGAFile.Location = new System.Drawing.Point(6, 60);
            this.textBoxFPGAFile.Name = "textBoxFPGAFile";
            this.textBoxFPGAFile.ReadOnly = true;
            this.textBoxFPGAFile.Size = new System.Drawing.Size(337, 20);
            this.textBoxFPGAFile.TabIndex = 16;
            // 
            // buttonSelectFPGAFile
            // 
            this.buttonSelectFPGAFile.Location = new System.Drawing.Point(349, 58);
            this.buttonSelectFPGAFile.Name = "buttonSelectFPGAFile";
            this.buttonSelectFPGAFile.Size = new System.Drawing.Size(101, 23);
            this.buttonSelectFPGAFile.TabIndex = 15;
            this.buttonSelectFPGAFile.Text = "Select FPGA File";
            this.buttonSelectFPGAFile.UseVisualStyleBackColor = true;
            this.buttonSelectFPGAFile.Click += new System.EventHandler(this.buttonSelectFPGAFile_Click);
            // 
            // buttonProgramFPGA
            // 
            this.buttonProgramFPGA.Location = new System.Drawing.Point(349, 135);
            this.buttonProgramFPGA.Name = "buttonProgramFPGA";
            this.buttonProgramFPGA.Size = new System.Drawing.Size(101, 23);
            this.buttonProgramFPGA.TabIndex = 17;
            this.buttonProgramFPGA.Text = "Program FPGA";
            this.buttonProgramFPGA.UseVisualStyleBackColor = true;
            this.buttonProgramFPGA.Click += new System.EventHandler(this.buttonProgramFPGA_Click);
            // 
            // buttonStartAutoLoad
            // 
            this.buttonStartAutoLoad.Location = new System.Drawing.Point(139, 167);
            this.buttonStartAutoLoad.Name = "buttonStartAutoLoad";
            this.buttonStartAutoLoad.Size = new System.Drawing.Size(96, 39);
            this.buttonStartAutoLoad.TabIndex = 18;
            this.buttonStartAutoLoad.Text = "Program Using Selected Files";
            this.buttonStartAutoLoad.UseVisualStyleBackColor = true;
            this.buttonStartAutoLoad.Click += new System.EventHandler(this.buttonStartAutoLoad_Click);
            // 
            // buttonFirstLoad
            // 
            this.buttonFirstLoad.Location = new System.Drawing.Point(6, 167);
            this.buttonFirstLoad.Name = "buttonFirstLoad";
            this.buttonFirstLoad.Size = new System.Drawing.Size(117, 39);
            this.buttonFirstLoad.TabIndex = 19;
            this.buttonFirstLoad.Text = "Program with Customer Firmware";
            this.buttonFirstLoad.UseVisualStyleBackColor = true;
            //this.buttonFirstLoad.Click += new System.EventHandler(this.buttonFirstLoad_Click);
            // 
            // buttonLoadNewest
            // 
            this.buttonLoadNewest.Location = new System.Drawing.Point(246, 167);
            this.buttonLoadNewest.Name = "buttonLoadNewest";
            this.buttonLoadNewest.Size = new System.Drawing.Size(98, 39);
            this.buttonLoadNewest.TabIndex = 20;
            this.buttonLoadNewest.Text = "Program Using Newest Files";
            this.buttonLoadNewest.UseVisualStyleBackColor = true;
            //this.buttonLoadNewest.Click += new System.EventHandler(this.buttonLoadNewest_Click);
            // 
            // buttonFixBootLoader
            // 
            this.buttonFixBootLoader.Location = new System.Drawing.Point(246, 127);
            this.buttonFixBootLoader.Name = "buttonFixBootLoader";
            this.buttonFixBootLoader.Size = new System.Drawing.Size(98, 35);
            this.buttonFixBootLoader.TabIndex = 21;
            this.buttonFixBootLoader.Text = "Fix Relay BootLoader";
            this.buttonFixBootLoader.UseVisualStyleBackColor = true;
            this.buttonFixBootLoader.Click += new System.EventHandler(this.buttonFixBootLoader_Click);
            // 
            // labelCustomer
            // 
            this.labelCustomer.AutoSize = true;
            this.labelCustomer.Location = new System.Drawing.Point(12, 127);
            this.labelCustomer.Name = "labelCustomer";
            this.labelCustomer.Size = new System.Drawing.Size(51, 13);
            this.labelCustomer.TabIndex = 22;
            this.labelCustomer.Text = "Customer";
            // 
            // comboBoxCustomer
            // 
            this.comboBoxCustomer.FormattingEnabled = true;
            this.comboBoxCustomer.Location = new System.Drawing.Point(69, 124);
            this.comboBoxCustomer.Name = "comboBoxCustomer";
            this.comboBoxCustomer.Size = new System.Drawing.Size(121, 21);
            this.comboBoxCustomer.TabIndex = 23;
            this.comboBoxCustomer.SelectedIndexChanged += new System.EventHandler(this.comboBoxCustomer_SelectedIndexChanged);
            // 
            // buttonClearAllProgrammingFields
            // 
            this.buttonClearAllProgrammingFields.Location = new System.Drawing.Point(349, 183);
            this.buttonClearAllProgrammingFields.Name = "buttonClearAllProgrammingFields";
            this.buttonClearAllProgrammingFields.Size = new System.Drawing.Size(100, 23);
            this.buttonClearAllProgrammingFields.TabIndex = 24;
            this.buttonClearAllProgrammingFields.Text = "Clear All";
            this.buttonClearAllProgrammingFields.UseVisualStyleBackColor = true;
            this.buttonClearAllProgrammingFields.Click += new System.EventHandler(this.buttonClearAllProgrammingFields_Click);
            // 
            // buttonLoadDefaultResourceSFiles
            // 
            this.buttonLoadDefaultResourceSFiles.Location = new System.Drawing.Point(349, 159);
            this.buttonLoadDefaultResourceSFiles.Name = "buttonLoadDefaultResourceSFiles";
            this.buttonLoadDefaultResourceSFiles.Size = new System.Drawing.Size(100, 23);
            this.buttonLoadDefaultResourceSFiles.TabIndex = 25;
            this.buttonLoadDefaultResourceSFiles.Text = "Load Defaults";
            this.buttonLoadDefaultResourceSFiles.UseVisualStyleBackColor = true;
            this.buttonLoadDefaultResourceSFiles.Click += new System.EventHandler(this.buttonLoadDefaultResourceSFiles_Click);
            // 
            // buttonProgramMasterBootCode
            // 
            this.buttonProgramMasterBootCode.Location = new System.Drawing.Point(246, 85);
            this.buttonProgramMasterBootCode.Name = "buttonProgramMasterBootCode";
            this.buttonProgramMasterBootCode.Size = new System.Drawing.Size(98, 36);
            this.buttonProgramMasterBootCode.TabIndex = 26;
            this.buttonProgramMasterBootCode.Text = "Program Master Boot Code";
            this.buttonProgramMasterBootCode.UseVisualStyleBackColor = true;
            this.buttonProgramMasterBootCode.Click += new System.EventHandler(this.buttonProgramMasterBootCode_Click);
            // 
            // ucRelayProgramming
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonProgramMasterBootCode);
            this.Controls.Add(this.buttonLoadDefaultResourceSFiles);
            this.Controls.Add(this.buttonClearAllProgrammingFields);
            this.Controls.Add(this.comboBoxCustomer);
            this.Controls.Add(this.labelCustomer);
            this.Controls.Add(this.buttonFixBootLoader);
            this.Controls.Add(this.buttonLoadNewest);
            this.Controls.Add(this.buttonFirstLoad);
            this.Controls.Add(this.buttonStartAutoLoad);
            this.Controls.Add(this.buttonProgramFPGA);
            this.Controls.Add(this.textBoxFPGAFile);
            this.Controls.Add(this.buttonSelectFPGAFile);
            this.Controls.Add(this.labelState);
            this.Controls.Add(this.labelDataCount);
            this.Controls.Add(this.labelCodeCount);
            this.Controls.Add(this.labelDataTotal);
            this.Controls.Add(this.labelCodeTotal);
            this.Controls.Add(this.labelData);
            this.Controls.Add(this.labelCode);
            this.Controls.Add(this.textBoxRelayFileName);
            this.Controls.Add(this.textBoxMasterFileName);
            this.Controls.Add(this.buttonProgramRelay);
            this.Controls.Add(this.buttonProgramMaster);
            this.Controls.Add(this.buttonSelectRelaySFile);
            this.Controls.Add(this.buttonSelectMasterSFile);
            this.Name = "ucRelayProgramming";
            this.Size = new System.Drawing.Size(459, 209);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSelectMasterSFile;
        private System.Windows.Forms.Button buttonSelectRelaySFile;
        private System.Windows.Forms.Button buttonProgramMaster;
        private System.Windows.Forms.Button buttonProgramRelay;
        private System.Windows.Forms.TextBox textBoxMasterFileName;
        private System.Windows.Forms.TextBox textBoxRelayFileName;
        private System.Windows.Forms.Label labelCode;
        private System.Windows.Forms.Label labelData;
        private System.Windows.Forms.Label labelCodeTotal;
        private System.Windows.Forms.Label labelDataTotal;
        private System.Windows.Forms.Label labelCodeCount;
        private System.Windows.Forms.Label labelDataCount;
        private System.Windows.Forms.Timer timerTimeout;
        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.TextBox textBoxFPGAFile;
        private System.Windows.Forms.Button buttonSelectFPGAFile;
        private System.Windows.Forms.Button buttonProgramFPGA;
        private System.Windows.Forms.Button buttonStartAutoLoad;
        private System.Windows.Forms.Button buttonFirstLoad;
        private System.Windows.Forms.Button buttonLoadNewest;
        private System.Windows.Forms.Button buttonFixBootLoader;
        private System.Windows.Forms.Label labelCustomer;
        private System.Windows.Forms.ComboBox comboBoxCustomer;
        private System.Windows.Forms.Button buttonClearAllProgrammingFields;
        private System.Windows.Forms.Button buttonLoadDefaultResourceSFiles;
        private System.Windows.Forms.Button buttonProgramMasterBootCode;
    }
}
