namespace RelayControlLibrary
{
    partial class ucRemoteCommandBlock
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
            this.buttonSendCommand = new System.Windows.Forms.Button();
            this.labelRemoteCommandState = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonSendCommand
            // 
            this.buttonSendCommand.Location = new System.Drawing.Point(3, 3);
            this.buttonSendCommand.Name = "buttonSendCommand";
            this.buttonSendCommand.Size = new System.Drawing.Size(117, 23);
            this.buttonSendCommand.TabIndex = 0;
            this.buttonSendCommand.Text = "Block Commands";
            this.buttonSendCommand.UseVisualStyleBackColor = true;
            this.buttonSendCommand.Click += new System.EventHandler(this.buttonSendCommand_Click);
            // 
            // labelRemoteCommandState
            // 
            this.labelRemoteCommandState.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labelRemoteCommandState.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelRemoteCommandState.Location = new System.Drawing.Point(3, 29);
            this.labelRemoteCommandState.Name = "labelRemoteCommandState";
            this.labelRemoteCommandState.Size = new System.Drawing.Size(117, 19);
            this.labelRemoteCommandState.TabIndex = 1;
            this.labelRemoteCommandState.Text = "Commands Allowed";
            this.labelRemoteCommandState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ucRemoteCommandBlock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelRemoteCommandState);
            this.Controls.Add(this.buttonSendCommand);
            this.Name = "ucRemoteCommandBlock";
            this.Size = new System.Drawing.Size(125, 51);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSendCommand;
        private System.Windows.Forms.Label labelRemoteCommandState;
    }
}
