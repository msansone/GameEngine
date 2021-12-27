namespace FiremelonEditor2
{
    partial class StateEditorControl
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
            this.hsState = new System.Windows.Forms.HScrollBar();
            this.vsState = new System.Windows.Forms.VScrollBar();
            this.pbState = new System.Windows.Forms.PictureBox();
            this.cmnuConfig = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.darkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vividToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pbState)).BeginInit();
            this.cmnuConfig.SuspendLayout();
            this.SuspendLayout();
            // 
            // hsState
            // 
            this.hsState.Location = new System.Drawing.Point(0, 113);
            this.hsState.Name = "hsState";
            this.hsState.Size = new System.Drawing.Size(133, 16);
            this.hsState.TabIndex = 10;
            this.hsState.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsState_Scroll);
            // 
            // vsState
            // 
            this.vsState.Location = new System.Drawing.Point(133, 0);
            this.vsState.Name = "vsState";
            this.vsState.Size = new System.Drawing.Size(16, 113);
            this.vsState.TabIndex = 9;
            this.vsState.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vsState_Scroll);
            // 
            // pbState
            // 
            this.pbState.BackColor = System.Drawing.Color.DimGray;
            this.pbState.Location = new System.Drawing.Point(0, 0);
            this.pbState.Name = "pbState";
            this.pbState.Size = new System.Drawing.Size(133, 113);
            this.pbState.TabIndex = 11;
            this.pbState.TabStop = false;
            this.pbState.Paint += new System.Windows.Forms.PaintEventHandler(this.pbState_Paint);
            this.pbState.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbState_MouseUp);
            // 
            // cmnuConfig
            // 
            this.cmnuConfig.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem});
            this.cmnuConfig.Name = "cmnuConfig";
            this.cmnuConfig.Size = new System.Drawing.Size(153, 48);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backgroundToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // backgroundToolStripMenuItem
            // 
            this.backgroundToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lightToolStripMenuItem,
            this.darkToolStripMenuItem,
            this.vividToolStripMenuItem});
            this.backgroundToolStripMenuItem.Name = "backgroundToolStripMenuItem";
            this.backgroundToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.backgroundToolStripMenuItem.Text = "Background";
            // 
            // lightToolStripMenuItem
            // 
            this.lightToolStripMenuItem.Checked = true;
            this.lightToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.lightToolStripMenuItem.Name = "lightToolStripMenuItem";
            this.lightToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.lightToolStripMenuItem.Text = "Light";
            this.lightToolStripMenuItem.Click += new System.EventHandler(this.lightToolStripMenuItem_Click);
            // 
            // darkToolStripMenuItem
            // 
            this.darkToolStripMenuItem.Name = "darkToolStripMenuItem";
            this.darkToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.darkToolStripMenuItem.Text = "Dark";
            this.darkToolStripMenuItem.Click += new System.EventHandler(this.darkToolStripMenuItem_Click);
            // 
            // vividToolStripMenuItem
            // 
            this.vividToolStripMenuItem.Name = "vividToolStripMenuItem";
            this.vividToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.vividToolStripMenuItem.Text = "Vivid";
            this.vividToolStripMenuItem.Click += new System.EventHandler(this.vividToolStripMenuItem_Click);
            // 
            // StateEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.hsState);
            this.Controls.Add(this.vsState);
            this.Controls.Add(this.pbState);
            this.Name = "StateEditorControl";
            this.Size = new System.Drawing.Size(231, 192);
            this.Resize += new System.EventHandler(this.StateEditorControl_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pbState)).EndInit();
            this.cmnuConfig.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.HScrollBar hsState;
        private System.Windows.Forms.VScrollBar vsState;
        private System.Windows.Forms.PictureBox pbState;
        private System.Windows.Forms.ContextMenuStrip cmnuConfig;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backgroundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lightToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem darkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vividToolStripMenuItem;
    }
}
