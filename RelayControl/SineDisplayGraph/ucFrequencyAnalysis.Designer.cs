namespace RelayControlLibrary
{
    partial class ucFrequencyAnalysis
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
            this.frequencyGraph1 = new FrequencyGraph();
            this.SuspendLayout();
            // 
            // frequencyGraph1
            // 
            this.frequencyGraph1.BackColor = System.Drawing.Color.White;
            this.frequencyGraph1.Location = new System.Drawing.Point(0, 0);
            this.frequencyGraph1.Name = "frequencyGraph1";
            this.frequencyGraph1.NumberOfHarmonics = 64;
            this.frequencyGraph1.Size = new System.Drawing.Size(318, 199);
            this.frequencyGraph1.TabIndex = 0;
            this.frequencyGraph1.Resize += new System.EventHandler(this.frequencyGraph1_Resize);
            // 
            // ucFrequencyAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.frequencyGraph1);
            this.Name = "ucFrequencyAnalysis";
            this.Size = new System.Drawing.Size(402, 202);
            this.ResumeLayout(false);

        }

        #endregion

        private FrequencyGraph frequencyGraph1;
    }
}
