using RelayControlLibrary;
namespace SineDisplayGraph
{
    partial class ucEventGraph
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
            this.frequencyGraphC = new RelayControlLibrary.FrequencyGraph();
            this.frequencyGraphB = new RelayControlLibrary.FrequencyGraph();
            this.frequencyGraphA = new RelayControlLibrary.FrequencyGraph();
            this.sineGraphIC = new RelayControlLibrary.SineGraph();
            this.sineGraphIB = new RelayControlLibrary.SineGraph();
            this.sineGraphIA = new RelayControlLibrary.SineGraph();
            this.sineGraphVnC = new RelayControlLibrary.SineGraph();
            this.sineGraphVnB = new RelayControlLibrary.SineGraph();
            this.sineGraphVnA = new RelayControlLibrary.SineGraph();
            this.sineGraphVtC = new RelayControlLibrary.SineGraph();
            this.sineGraphVtB = new RelayControlLibrary.SineGraph();
            this.sineGraphVtA = new RelayControlLibrary.SineGraph();
            this.labelEventLabel = new System.Windows.Forms.Label();
            this.labelEventLabel2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // frequencyGraphC
            // 
            this.frequencyGraphC.BackColor = System.Drawing.Color.White;
            this.frequencyGraphC.Location = new System.Drawing.Point(501, 548);
            this.frequencyGraphC.Name = "frequencyGraphC";
            this.frequencyGraphC.NumberOfHarmonics = 32;
            this.frequencyGraphC.Size = new System.Drawing.Size(495, 98);
            this.frequencyGraphC.TabIndex = 11;
            // 
            // frequencyGraphB
            // 
            this.frequencyGraphB.BackColor = System.Drawing.Color.White;
            this.frequencyGraphB.Location = new System.Drawing.Point(501, 348);
            this.frequencyGraphB.Name = "frequencyGraphB";
            this.frequencyGraphB.NumberOfHarmonics = 32;
            this.frequencyGraphB.Size = new System.Drawing.Size(495, 98);
            this.frequencyGraphB.TabIndex = 10;
            // 
            // frequencyGraphA
            // 
            this.frequencyGraphA.BackColor = System.Drawing.Color.White;
            this.frequencyGraphA.Location = new System.Drawing.Point(501, 148);
            this.frequencyGraphA.Name = "frequencyGraphA";
            this.frequencyGraphA.NumberOfHarmonics = 32;
            this.frequencyGraphA.Size = new System.Drawing.Size(495, 98);
            this.frequencyGraphA.TabIndex = 9;
            // 
            // sineGraphIC
            // 
            this.sineGraphIC.GraphName = "";
            this.sineGraphIC.Location = new System.Drawing.Point(4, 548);
            this.sineGraphIC.Name = "sineGraphIC";
            this.sineGraphIC.PointsToDraw = 0;
            this.sineGraphIC.ScrollEnabled = false;
            this.sineGraphIC.Size = new System.Drawing.Size(495, 98);
            this.sineGraphIC.TabIndex = 8;
            this.sineGraphIC.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.sineGraph_DoubleClick);
            // 
            // sineGraphIB
            // 
            this.sineGraphIB.GraphName = "";
            this.sineGraphIB.Location = new System.Drawing.Point(4, 348);
            this.sineGraphIB.Name = "sineGraphIB";
            this.sineGraphIB.PointsToDraw = 0;
            this.sineGraphIB.ScrollEnabled = false;
            this.sineGraphIB.Size = new System.Drawing.Size(495, 98);
            this.sineGraphIB.TabIndex = 7;
            this.sineGraphIB.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.sineGraph_DoubleClick);
            // 
            // sineGraphIA
            // 
            this.sineGraphIA.GraphName = "";
            this.sineGraphIA.Location = new System.Drawing.Point(4, 148);
            this.sineGraphIA.Name = "sineGraphIA";
            this.sineGraphIA.PointsToDraw = 0;
            this.sineGraphIA.ScrollEnabled = false;
            this.sineGraphIA.Size = new System.Drawing.Size(495, 98);
            this.sineGraphIA.TabIndex = 6;
            this.sineGraphIA.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.sineGraph_DoubleClick);
            // 
            // sineGraphVnC
            // 
            this.sineGraphVnC.GraphName = "";
            this.sineGraphVnC.Location = new System.Drawing.Point(501, 448);
            this.sineGraphVnC.Name = "sineGraphVnC";
            this.sineGraphVnC.PointsToDraw = 0;
            this.sineGraphVnC.ScrollEnabled = false;
            this.sineGraphVnC.Size = new System.Drawing.Size(495, 98);
            this.sineGraphVnC.TabIndex = 5;
            this.sineGraphVnC.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.sineGraph_DoubleClick);
            // 
            // sineGraphVnB
            // 
            this.sineGraphVnB.GraphName = "";
            this.sineGraphVnB.Location = new System.Drawing.Point(501, 248);
            this.sineGraphVnB.Name = "sineGraphVnB";
            this.sineGraphVnB.PointsToDraw = 0;
            this.sineGraphVnB.ScrollEnabled = false;
            this.sineGraphVnB.Size = new System.Drawing.Size(495, 98);
            this.sineGraphVnB.TabIndex = 4;
            this.sineGraphVnB.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.sineGraph_DoubleClick);
            // 
            // sineGraphVnA
            // 
            this.sineGraphVnA.GraphName = "";
            this.sineGraphVnA.Location = new System.Drawing.Point(501, 48);
            this.sineGraphVnA.Name = "sineGraphVnA";
            this.sineGraphVnA.PointsToDraw = 0;
            this.sineGraphVnA.ScrollEnabled = false;
            this.sineGraphVnA.Size = new System.Drawing.Size(495, 98);
            this.sineGraphVnA.TabIndex = 3;
            this.sineGraphVnA.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.sineGraph_DoubleClick);
            // 
            // sineGraphVtC
            // 
            this.sineGraphVtC.GraphName = "";
            this.sineGraphVtC.Location = new System.Drawing.Point(4, 448);
            this.sineGraphVtC.Name = "sineGraphVtC";
            this.sineGraphVtC.PointsToDraw = 0;
            this.sineGraphVtC.ScrollEnabled = false;
            this.sineGraphVtC.Size = new System.Drawing.Size(495, 98);
            this.sineGraphVtC.TabIndex = 2;
            this.sineGraphVtC.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.sineGraph_DoubleClick);
            // 
            // sineGraphVtB
            // 
            this.sineGraphVtB.GraphName = "";
            this.sineGraphVtB.Location = new System.Drawing.Point(4, 248);
            this.sineGraphVtB.Name = "sineGraphVtB";
            this.sineGraphVtB.PointsToDraw = 0;
            this.sineGraphVtB.ScrollEnabled = false;
            this.sineGraphVtB.Size = new System.Drawing.Size(495, 98);
            this.sineGraphVtB.TabIndex = 1;
            this.sineGraphVtB.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.sineGraph_DoubleClick);
            // 
            // sineGraphVtA
            // 
            this.sineGraphVtA.GraphName = "";
            this.sineGraphVtA.Location = new System.Drawing.Point(4, 48);
            this.sineGraphVtA.Name = "sineGraphVtA";
            this.sineGraphVtA.PointsToDraw = 0;
            this.sineGraphVtA.ScrollEnabled = false;
            this.sineGraphVtA.Size = new System.Drawing.Size(495, 98);
            this.sineGraphVtA.TabIndex = 0;
            this.sineGraphVtA.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.sineGraph_DoubleClick);
            // 
            // labelEventLabel
            // 
            this.labelEventLabel.AutoSize = true;
            this.labelEventLabel.Location = new System.Drawing.Point(421, 14);
            this.labelEventLabel.Name = "labelEventLabel";
            this.labelEventLabel.Size = new System.Drawing.Size(0, 13);
            this.labelEventLabel.TabIndex = 12;

            // 
            // labelEventLabel
            // 
            this.labelEventLabel2.AutoSize = true;
            this.labelEventLabel2.Location = new System.Drawing.Point(421, 34);
            this.labelEventLabel2.Name = "labelEventLabel2";
            this.labelEventLabel2.Size = new System.Drawing.Size(0, 13);
            //this.labelEventLabel2.TabIndex = 12;
            // 
            // ucEventGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelEventLabel);
            this.Controls.Add(this.labelEventLabel2);
            this.Controls.Add(this.frequencyGraphC);
            this.Controls.Add(this.frequencyGraphB);
            this.Controls.Add(this.frequencyGraphA);
            this.Controls.Add(this.sineGraphIC);
            this.Controls.Add(this.sineGraphIB);
            this.Controls.Add(this.sineGraphIA);
            this.Controls.Add(this.sineGraphVnC);
            this.Controls.Add(this.sineGraphVnB);
            this.Controls.Add(this.sineGraphVnA);
            this.Controls.Add(this.sineGraphVtC);
            this.Controls.Add(this.sineGraphVtB);
            this.Controls.Add(this.sineGraphVtA);
            this.Name = "ucEventGraph";
            this.Size = new System.Drawing.Size(1000, 650);
            this.SizeChanged += new System.EventHandler(this.ucEventGraph_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SineGraph sineGraphVtA;
        private SineGraph sineGraphVtB;
        private SineGraph sineGraphVtC;
        private SineGraph sineGraphVnA;
        private SineGraph sineGraphVnB;
        private SineGraph sineGraphVnC;
        private SineGraph sineGraphIC;
        private SineGraph sineGraphIB;
        private SineGraph sineGraphIA;
        private FrequencyGraph frequencyGraphA;
        private FrequencyGraph frequencyGraphB;
        private FrequencyGraph frequencyGraphC;
        private System.Windows.Forms.Label labelEventLabel;
        private System.Windows.Forms.Label labelEventLabel2;
    }
}
