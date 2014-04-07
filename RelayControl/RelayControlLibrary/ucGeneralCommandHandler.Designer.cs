namespace RelayControlLibrary
{
    partial class ucGeneralCommandHandler
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
            this.groupBoxGeneralCommand = new System.Windows.Forms.GroupBox();
            this.textBoxReturnValue = new System.Windows.Forms.TextBox();
            this.labelIncomingCommandName = new System.Windows.Forms.Label();
            this.buttonRepeatedSend = new System.Windows.Forms.Button();
            this.buttonSendOnce = new System.Windows.Forms.Button();
            this.comboBoxOutgoingCommands = new System.Windows.Forms.ComboBox();
            this.groupBoxGeneralCommand.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxGeneralCommand
            // 
            this.groupBoxGeneralCommand.Controls.Add(this.textBoxReturnValue);
            this.groupBoxGeneralCommand.Controls.Add(this.labelIncomingCommandName);
            this.groupBoxGeneralCommand.Controls.Add(this.buttonRepeatedSend);
            this.groupBoxGeneralCommand.Controls.Add(this.buttonSendOnce);
            this.groupBoxGeneralCommand.Controls.Add(this.comboBoxOutgoingCommands);
            this.groupBoxGeneralCommand.Location = new System.Drawing.Point(3, 3);
            this.groupBoxGeneralCommand.Name = "groupBoxGeneralCommand";
            this.groupBoxGeneralCommand.Size = new System.Drawing.Size(420, 208);
            this.groupBoxGeneralCommand.TabIndex = 0;
            this.groupBoxGeneralCommand.TabStop = false;
            this.groupBoxGeneralCommand.Text = "General Command";
            // 
            // textBoxReturnValue
            // 
            this.textBoxReturnValue.Location = new System.Drawing.Point(49, 48);
            this.textBoxReturnValue.Name = "textBoxReturnValue";
            this.textBoxReturnValue.ReadOnly = true;
            this.textBoxReturnValue.Size = new System.Drawing.Size(365, 20);
            this.textBoxReturnValue.TabIndex = 4;
            // 
            // labelIncomingCommandName
            // 
            this.labelIncomingCommandName.AutoSize = true;
            this.labelIncomingCommandName.Location = new System.Drawing.Point(7, 51);
            this.labelIncomingCommandName.Name = "labelIncomingCommandName";
            this.labelIncomingCommandName.Size = new System.Drawing.Size(36, 13);
            this.labelIncomingCommandName.TabIndex = 3;
            this.labelIncomingCommandName.Text = "None:";
            this.labelIncomingCommandName.Resize += new System.EventHandler(this.labelIncomingCommandName_Resize);
            // 
            // buttonRepeatedSend
            // 
            this.buttonRepeatedSend.Location = new System.Drawing.Point(321, 19);
            this.buttonRepeatedSend.Name = "buttonRepeatedSend";
            this.buttonRepeatedSend.Size = new System.Drawing.Size(93, 23);
            this.buttonRepeatedSend.TabIndex = 2;
            this.buttonRepeatedSend.Text = "Repeated";
            this.buttonRepeatedSend.UseVisualStyleBackColor = true;
            this.buttonRepeatedSend.Click += new System.EventHandler(this.buttonRepeatedSend_Click);
            // 
            // buttonSendOnce
            // 
            this.buttonSendOnce.Location = new System.Drawing.Point(256, 19);
            this.buttonSendOnce.Name = "buttonSendOnce";
            this.buttonSendOnce.Size = new System.Drawing.Size(59, 23);
            this.buttonSendOnce.TabIndex = 1;
            this.buttonSendOnce.Text = "Once";
            this.buttonSendOnce.UseVisualStyleBackColor = true;
            this.buttonSendOnce.Click += new System.EventHandler(this.buttonSendOnce_Click);
            // 
            // comboBoxOutgoingCommands
            // 
            this.comboBoxOutgoingCommands.FormattingEnabled = true;
            this.comboBoxOutgoingCommands.Location = new System.Drawing.Point(6, 19);
            this.comboBoxOutgoingCommands.Name = "comboBoxOutgoingCommands";
            this.comboBoxOutgoingCommands.Size = new System.Drawing.Size(244, 21);
            this.comboBoxOutgoingCommands.TabIndex = 0;
            // 
            // ucGeneralCommandHandler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxGeneralCommand);
            this.Name = "ucGeneralCommandHandler";
            this.Size = new System.Drawing.Size(426, 215);
            this.groupBoxGeneralCommand.ResumeLayout(false);
            this.groupBoxGeneralCommand.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxGeneralCommand;
        private System.Windows.Forms.Label labelIncomingCommandName;
        private System.Windows.Forms.Button buttonRepeatedSend;
        private System.Windows.Forms.Button buttonSendOnce;
        private System.Windows.Forms.ComboBox comboBoxOutgoingCommands;
        private System.Windows.Forms.TextBox textBoxReturnValue;

    }
}
