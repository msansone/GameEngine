namespace FiremelonEditor2
{
    partial class AnimationFrameViewerControl
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
            this.vsAnimation = new System.Windows.Forms.VScrollBar();
            this.pbCurrentFrame = new System.Windows.Forms.PictureBox();
            this.hsAnimation = new System.Windows.Forms.HScrollBar();
            this.cmnuConfig = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.darkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vividToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pbCurrentFrame)).BeginInit();
            this.cmnuConfig.SuspendLayout();
            this.SuspendLayout();
            // 
            // vsAnimation
            // 
            this.vsAnimation.Location = new System.Drawing.Point(154, 0);
            this.vsAnimation.Name = "vsAnimation";
            this.vsAnimation.Size = new System.Drawing.Size(16, 45);
            this.vsAnimation.TabIndex = 26;
            // 
            // pbCurrentFrame
            // 
            this.pbCurrentFrame.BackColor = System.Drawing.Color.DimGray;
            this.pbCurrentFrame.Location = new System.Drawing.Point(0, 0);
            this.pbCurrentFrame.Name = "pbCurrentFrame";
            this.pbCurrentFrame.Size = new System.Drawing.Size(154, 45);
            this.pbCurrentFrame.TabIndex = 25;
            this.pbCurrentFrame.TabStop = false;
            this.pbCurrentFrame.Paint += new System.Windows.Forms.PaintEventHandler(this.pbCurrentFrame_Paint);
            this.pbCurrentFrame.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbCurrentFrame_MouseDown);
            this.pbCurrentFrame.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbCurrentFrame_MouseMove);
            this.pbCurrentFrame.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbCurrentFrame_MouseUp);
            // 
            // hsAnimation
            // 
            this.hsAnimation.Location = new System.Drawing.Point(-1, 45);
            this.hsAnimation.Name = "hsAnimation";
            this.hsAnimation.Size = new System.Drawing.Size(155, 16);
            this.hsAnimation.TabIndex = 24;
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
            // AnimationFrameViewerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.vsAnimation);
            this.Controls.Add(this.pbCurrentFrame);
            this.Controls.Add(this.hsAnimation);
            this.Name = "AnimationFrameViewerControl";
            this.Size = new System.Drawing.Size(611, 487);
            this.Resize += new System.EventHandler(this.AnimationFrameViewerControl_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pbCurrentFrame)).EndInit();
            this.cmnuConfig.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.VScrollBar vsAnimation;
        private System.Windows.Forms.PictureBox pbCurrentFrame;
        private System.Windows.Forms.HScrollBar hsAnimation;
        private System.Windows.Forms.ContextMenuStrip cmnuConfig;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backgroundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lightToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem darkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vividToolStripMenuItem;
    }
}
