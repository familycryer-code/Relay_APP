using System;

namespace RelayControlLibrary
{
    partial class ucTimeControl
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
            this.buttonSendTime = new System.Windows.Forms.Button();
            this.buttonRequestTime = new System.Windows.Forms.Button();
            this.groupBoxTimeControl = new System.Windows.Forms.GroupBox();
            this.labelRelayTimeDisplay = new System.Windows.Forms.Label();
            this.labelMachineTimeDisplay = new System.Windows.Forms.Label();
            this.labelRelayTime = new System.Windows.Forms.Label();
            this.labelMachineTime = new System.Windows.Forms.Label();
            this.labelTimeDiff = new System.Windows.Forms.Label();
            this.buttonTable = new System.Windows.Forms.Button();
            this.groupBoxTimeControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSendTime
            // 
            this.buttonSendTime.Location = new System.Drawing.Point(85, 65);
            this.buttonSendTime.Name = "buttonSendTime";
            this.buttonSendTime.Size = new System.Drawing.Size(75, 23);
            this.buttonSendTime.TabIndex = 0;
            this.buttonSendTime.Text = "Send Time";
            this.buttonSendTime.UseVisualStyleBackColor = true;
            this.buttonSendTime.Click += new System.EventHandler(this.buttonSendTime_Click);
            // 
            // buttonRequestTime
            // 
            this.buttonRequestTime.Location = new System.Drawing.Point(5, 65);
            this.buttonRequestTime.Name = "buttonRequestTime";
            this.buttonRequestTime.Size = new System.Drawing.Size(75, 23);
            this.buttonRequestTime.TabIndex = 1;
            this.buttonRequestTime.Text = "Req Time";
            this.buttonRequestTime.UseVisualStyleBackColor = true;
            this.buttonRequestTime.Click += new System.EventHandler(this.buttonRequestTime_Click);
            // 
            // groupBoxTimeControl
            // 
            this.groupBoxTimeControl.Controls.Add(this.buttonTable);
            this.groupBoxTimeControl.Controls.Add(this.labelTimeDiff);
            this.groupBoxTimeControl.Controls.Add(this.labelRelayTimeDisplay);
            this.groupBoxTimeControl.Controls.Add(this.labelMachineTimeDisplay);
            this.groupBoxTimeControl.Controls.Add(this.labelRelayTime);
            this.groupBoxTimeControl.Controls.Add(this.labelMachineTime);
            this.groupBoxTimeControl.Controls.Add(this.buttonSendTime);
            this.groupBoxTimeControl.Controls.Add(this.buttonRequestTime);
            this.groupBoxTimeControl.Location = new System.Drawing.Point(3, 3);
            this.groupBoxTimeControl.Name = "groupBoxTimeControl";
            this.groupBoxTimeControl.Size = new System.Drawing.Size(236, 94);
            this.groupBoxTimeControl.TabIndex = 2;
            this.groupBoxTimeControl.TabStop = false;
            this.groupBoxTimeControl.Text = "Time Control";
            // 
            // labelRelayTimeDisplay
            // 
            this.labelRelayTimeDisplay.AutoSize = true;
            this.labelRelayTimeDisplay.Location = new System.Drawing.Point(86, 37);
            this.labelRelayTimeDisplay.Name = "labelRelayTimeDisplay";
            this.labelRelayTimeDisplay.Size = new System.Drawing.Size(0, 13);
            this.labelRelayTimeDisplay.TabIndex = 5;
            // 
            // labelMachineTimeDisplay
            // 
            this.labelMachineTimeDisplay.AutoSize = true;
            this.labelMachineTimeDisplay.Location = new System.Drawing.Point(86, 20);
            this.labelMachineTimeDisplay.Name = "labelMachineTimeDisplay";
            this.labelMachineTimeDisplay.Size = new System.Drawing.Size(0, 13);
            this.labelMachineTimeDisplay.TabIndex = 4;
            // 
            // labelRelayTime
            // 
            this.labelRelayTime.AutoSize = true;
            this.labelRelayTime.Location = new System.Drawing.Point(17, 37);
            this.labelRelayTime.Name = "labelRelayTime";
            this.labelRelayTime.Size = new System.Drawing.Size(63, 13);
            this.labelRelayTime.TabIndex = 3;
            this.labelRelayTime.Text = "Relay Time:";
            // 
            // labelMachineTime
            // 
            this.labelMachineTime.AutoSize = true;
            this.labelMachineTime.Location = new System.Drawing.Point(3, 20);
            this.labelMachineTime.Name = "labelMachineTime";
            this.labelMachineTime.Size = new System.Drawing.Size(77, 13);
            this.labelMachineTime.TabIndex = 2;
            this.labelMachineTime.Text = "Machine Time:";
            // 
            // labelTimeDiff
            // 
            this.labelTimeDiff.AutoSize = true;
            this.labelTimeDiff.Location = new System.Drawing.Point(162, 20);
            this.labelTimeDiff.Name = "labelTimeDiff";
            this.labelTimeDiff.Size = new System.Drawing.Size(0, 13);
            this.labelTimeDiff.TabIndex = 6;
            // 
            // buttonTable
            // 
            this.buttonTable.Location = new System.Drawing.Point(165, 65);
            this.buttonTable.Name = "buttonTable";
            this.buttonTable.Size = new System.Drawing.Size(65, 23);
            this.buttonTable.TabIndex = 7;
            this.buttonTable.Text = "Table";
            this.buttonTable.UseVisualStyleBackColor = true;
            this.buttonTable.Click += new System.EventHandler(this.buttonTable_Click);
            // 
            // ucTimeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxTimeControl);
            this.Name = "ucTimeControl";
            this.Size = new System.Drawing.Size(245, 101);
            this.groupBoxTimeControl.ResumeLayout(false);
            this.groupBoxTimeControl.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSendTime;
        private System.Windows.Forms.Button buttonRequestTime;
        private System.Windows.Forms.GroupBox groupBoxTimeControl;
        private System.Windows.Forms.Label labelRelayTimeDisplay;
        private System.Windows.Forms.Label labelMachineTimeDisplay;
        private System.Windows.Forms.Label labelRelayTime;
        private System.Windows.Forms.Label labelMachineTime;
        private System.Windows.Forms.Label labelTimeDiff;
        private System.Windows.Forms.Button buttonTable;
    }
}
