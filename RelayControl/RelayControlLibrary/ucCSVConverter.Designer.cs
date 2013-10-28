namespace RelayControlLibrary
{
    partial class ucCSVConverterCSVFile
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
            this.groupBoxCSVConverterMain = new System.Windows.Forms.GroupBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.openFileDialogCSVFile = new System.Windows.Forms.OpenFileDialog();
            this.groupBoxCSVConverterMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxCSVConverterMain
            // 
            this.groupBoxCSVConverterMain.Controls.Add(this.buttonSave);
            this.groupBoxCSVConverterMain.Controls.Add(this.buttonOpen);
            this.groupBoxCSVConverterMain.Location = new System.Drawing.Point(3, 3);
            this.groupBoxCSVConverterMain.Name = "groupBoxCSVConverterMain";
            this.groupBoxCSVConverterMain.Size = new System.Drawing.Size(88, 76);
            this.groupBoxCSVConverterMain.TabIndex = 0;
            this.groupBoxCSVConverterMain.TabStop = false;
            this.groupBoxCSVConverterMain.Text = "CSV Convert";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(6, 48);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 1;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(6, 19);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(75, 23);
            this.buttonOpen.TabIndex = 0;
            this.buttonOpen.Text = "Open";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // openFileDialogCSVFile
            // 
            this.openFileDialogCSVFile.FileName = "Open CSV File";
            this.openFileDialogCSVFile.Filter = "CSV Files | *.csv";
            this.openFileDialogCSVFile.InitialDirectory = "C:\\Documents and Settings\\charlesbarnes\\My Documents\\C\\Files\\Relay Project\\Safe S" +
                "ervice Data\\";
            this.openFileDialogCSVFile.RestoreDirectory = true;
            // 
            // ucCSVConverterCSVFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxCSVConverterMain);
            this.Name = "ucCSVConverterCSVFile";
            this.Size = new System.Drawing.Size(94, 84);
            this.groupBoxCSVConverterMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxCSVConverterMain;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.OpenFileDialog openFileDialogCSVFile;
    }
}
