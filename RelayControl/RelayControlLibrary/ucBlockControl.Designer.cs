namespace RelayControlLibrary
{
    partial class ucBlockControl
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
            this.buttonSendBlockState = new System.Windows.Forms.Button();
            this.labelBlockedState = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonSendBlockState
            // 
            this.buttonSendBlockState.Location = new System.Drawing.Point(3, 3);
            this.buttonSendBlockState.Name = "buttonSendBlockState";
            this.buttonSendBlockState.Size = new System.Drawing.Size(89, 23);
            this.buttonSendBlockState.TabIndex = 0;
            this.buttonSendBlockState.Text = "Block Relay";
            this.buttonSendBlockState.UseVisualStyleBackColor = true;
            this.buttonSendBlockState.Click += new System.EventHandler(this.buttonSendBlockState_Click);
            // 
            // labelBlockedState
            // 
            this.labelBlockedState.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labelBlockedState.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelBlockedState.Location = new System.Drawing.Point(3, 29);
            this.labelBlockedState.Name = "labelBlockedState";
            this.labelBlockedState.Size = new System.Drawing.Size(89, 22);
            this.labelBlockedState.TabIndex = 1;
            this.labelBlockedState.Text = "Unknown";
            this.labelBlockedState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ucBlockControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelBlockedState);
            this.Controls.Add(this.buttonSendBlockState);
            this.Name = "ucBlockControl";
            this.Size = new System.Drawing.Size(97, 55);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSendBlockState;
        private System.Windows.Forms.Label labelBlockedState;
    }
}
