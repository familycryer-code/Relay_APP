namespace RelayControlLibrary
{
    partial class SineGraph
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
            this.components = new System.ComponentModel.Container();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.labelType = new System.Windows.Forms.Label();
            this.contextMenuStripRightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewSingleCycleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stepThroughEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripRightClick.SuspendLayout();
            this.SuspendLayout();
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.LargeChange = 100;
            this.hScrollBar1.Location = new System.Drawing.Point(0, 138);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(270, 12);
            this.hScrollBar1.TabIndex = 0;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.Location = new System.Drawing.Point(4, 4);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(0, 13);
            this.labelType.TabIndex = 5;
            // 
            // contextMenuStripRightClick
            // 
            this.contextMenuStripRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewSingleCycleToolStripMenuItem,
            this.stepThroughEventToolStripMenuItem});
            this.contextMenuStripRightClick.Name = "contextMenuStripRightClick";
            this.contextMenuStripRightClick.Size = new System.Drawing.Size(182, 48);
            // 
            // viewSingleCycleToolStripMenuItem
            // 
            this.viewSingleCycleToolStripMenuItem.Name = "viewSingleCycleToolStripMenuItem";
            this.viewSingleCycleToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.viewSingleCycleToolStripMenuItem.Text = "View Single Cycle";
            // 
            // stepThroughEventToolStripMenuItem
            // 
            this.stepThroughEventToolStripMenuItem.Name = "stepThroughEventToolStripMenuItem";
            this.stepThroughEventToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.stepThroughEventToolStripMenuItem.Text = "Step Through Event";
            // 
            // SineGraph
            // 
            this.Controls.Add(this.labelType);
            this.Controls.Add(this.hScrollBar1);
            this.Name = "SineGraph";
            this.Size = new System.Drawing.Size(270, 150);
            this.MouseLeave += new System.EventHandler(this.SineGraph_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SineGraph_MouseMove);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SineGraph_MouseClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SineGraph_KeyDown);
            this.contextMenuStripRightClick.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripRightClick;
        private System.Windows.Forms.ToolStripMenuItem viewSingleCycleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stepThroughEventToolStripMenuItem;

    }
}
