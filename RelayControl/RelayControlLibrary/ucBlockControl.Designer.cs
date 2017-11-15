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
            this.labelBlockedState = new System.Windows.Forms.Label();
            this.groupBoxBlockOpen = new System.Windows.Forms.GroupBox();
            this.tsBlockOpen = new RelayControlLibrary.ucToggleSwitch();
            this.groupBoxBlockOpen.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelBlockedState
            // 
            this.labelBlockedState.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labelBlockedState.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelBlockedState.Location = new System.Drawing.Point(6, 38);
            this.labelBlockedState.Name = "labelBlockedState";
            this.labelBlockedState.Size = new System.Drawing.Size(89, 22);
            this.labelBlockedState.TabIndex = 1;
            this.labelBlockedState.Text = "Unknown";
            this.labelBlockedState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBoxBlockOpen
            // 
            this.groupBoxBlockOpen.Controls.Add(this.tsBlockOpen);
            this.groupBoxBlockOpen.Controls.Add(this.labelBlockedState);
            this.groupBoxBlockOpen.Location = new System.Drawing.Point(3, 3);
            this.groupBoxBlockOpen.Name = "groupBoxBlockOpen";
            this.groupBoxBlockOpen.Size = new System.Drawing.Size(99, 65);
            this.groupBoxBlockOpen.TabIndex = 2;
            this.groupBoxBlockOpen.TabStop = false;
            this.groupBoxBlockOpen.Text = "Block Open";
            // 
            // tsBlockOpen
            // 
            this.tsBlockOpen.Location = new System.Drawing.Point(6, 12);
            this.tsBlockOpen.Name = "tsBlockOpen";
            this.tsBlockOpen.Padding = new System.Windows.Forms.Padding(6);
            this.tsBlockOpen.Size = new System.Drawing.Size(87, 23);
            this.tsBlockOpen.TabIndex = 0;
            this.tsBlockOpen.Text = "tsBlockOpen";
            this.tsBlockOpen.UseVisualStyleBackColor = true;
            this.tsBlockOpen.CheckedChanged += new System.EventHandler(this.tsBlockOpen_CheckedChanged);
            // 
            // ucBlockControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxBlockOpen);
            this.Name = "ucBlockControl";
            this.Size = new System.Drawing.Size(104, 71);
            this.groupBoxBlockOpen.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label labelBlockedState;
        private System.Windows.Forms.GroupBox groupBoxBlockOpen;
        private ucToggleSwitch tsBlockOpen;
    }
}
