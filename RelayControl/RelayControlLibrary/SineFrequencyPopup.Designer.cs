
namespace RelayControlLibrary
{
    partial class SineFrequencyPopup
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonClose = new System.Windows.Forms.Button();
            this.sineGraph1 = new RelayControlLibrary.SineGraph();
            this.frequencyGraph1 = new RelayControlLibrary.FrequencyGraph();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(841, 432);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 2;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // sineGraph1
            // 
            this.sineGraph1.GraphName = "";
            this.sineGraph1.Location = new System.Drawing.Point(12, 12);
            this.sineGraph1.Name = "sineGraph1";
            this.sineGraph1.PointsToDraw = 0;
            this.sineGraph1.ScrollEnabled = true;
            this.sineGraph1.Size = new System.Drawing.Size(923, 209);
            this.sineGraph1.TabIndex = 1;
            this.sineGraph1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.sineGraph1_MouseClick);
            // 
            // frequencyGraph1
            // 
            this.frequencyGraph1.BackColor = System.Drawing.Color.White;
            this.frequencyGraph1.Location = new System.Drawing.Point(12, 227);
            this.frequencyGraph1.Name = "frequencyGraph1";
            this.frequencyGraph1.NumberOfHarmonics = 32;
            this.frequencyGraph1.Size = new System.Drawing.Size(923, 187);
            this.frequencyGraph1.TabIndex = 0;
            // 
            // SineFrequencyPopup
            // 
            this.ClientSize = new System.Drawing.Size(947, 471);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.sineGraph1);
            this.Controls.Add(this.frequencyGraph1);
            this.Name = "SineFrequencyPopup";
            this.Resize += new System.EventHandler(this.SineFrequencyPopup_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private FrequencyGraph frequencyGraph1;
        private SineGraph sineGraph1;
        private System.Windows.Forms.Button buttonClose;

    }
}