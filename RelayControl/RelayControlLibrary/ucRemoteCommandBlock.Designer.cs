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
            this.labelRemoteCommandState = new System.Windows.Forms.Label();
            this.tsCommandBlock = new RelayControlLibrary.ucToggleSwitch();
            this.groupBoxBlockCommands = new System.Windows.Forms.GroupBox();
            this.groupBoxBlockCommands.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelRemoteCommandState
            // 
            this.labelRemoteCommandState.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labelRemoteCommandState.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelRemoteCommandState.Location = new System.Drawing.Point(15, 43);
            this.labelRemoteCommandState.Name = "labelRemoteCommandState";
            this.labelRemoteCommandState.Size = new System.Drawing.Size(89, 19);
            this.labelRemoteCommandState.TabIndex = 1;
            this.labelRemoteCommandState.Text = "Unknown";
            this.labelRemoteCommandState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tsCommandBlock
            // 
            this.tsCommandBlock.Location = new System.Drawing.Point(6, 19);
            this.tsCommandBlock.Name = "tsCommandBlock";
            this.tsCommandBlock.Padding = new System.Windows.Forms.Padding(6);
            this.tsCommandBlock.Size = new System.Drawing.Size(98, 21);
            this.tsCommandBlock.TabIndex = 2;
            this.tsCommandBlock.Text = "ucToggleSwitch1";
            this.tsCommandBlock.UseVisualStyleBackColor = true;
            this.tsCommandBlock.CheckedChanged += new System.EventHandler(this.ucToggleSwitch1_CheckedChanged);
            // 
            // groupBoxBlockCommands
            // 
            this.groupBoxBlockCommands.Controls.Add(this.tsCommandBlock);
            this.groupBoxBlockCommands.Controls.Add(this.labelRemoteCommandState);
            this.groupBoxBlockCommands.Location = new System.Drawing.Point(3, 3);
            this.groupBoxBlockCommands.Name = "groupBoxBlockCommands";
            this.groupBoxBlockCommands.Size = new System.Drawing.Size(113, 67);
            this.groupBoxBlockCommands.TabIndex = 3;
            this.groupBoxBlockCommands.TabStop = false;
            this.groupBoxBlockCommands.Text = "PLC Control";
            // 
            // ucRemoteCommandBlock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.groupBoxBlockCommands);
            this.Name = "ucRemoteCommandBlock";
            this.Size = new System.Drawing.Size(120, 72);
            this.groupBoxBlockCommands.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label labelRemoteCommandState;
        private ucToggleSwitch tsCommandBlock;
        private System.Windows.Forms.GroupBox groupBoxBlockCommands;
    }
}
