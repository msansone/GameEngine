namespace FiremelonEditor2
{
    partial class AnimationEditor
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
            this.components = new System.ComponentModel.Container();
            this.pgAnimation = new System.Windows.Forms.PropertyGrid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.scAnimationProperties = new System.Windows.Forms.SplitContainer();
            this.tvAnimation = new System.Windows.Forms.TreeView();
            this.pnAnimation = new System.Windows.Forms.Panel();
            this.vsAnimation = new System.Windows.Forms.VScrollBar();
            this.pbCurrentFrame = new System.Windows.Forms.PictureBox();
            this.hsAnimation = new System.Windows.Forms.HScrollBar();
            this.cmnuHitboxRoot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addHitboxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuHitbox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteHitboxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuFrame = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addHitboxToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.addFrameTriggerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.addActionPointToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteFrameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuFrameTriggerRoot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addFrameTriggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuFrameTrigger = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteFrameTriggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuActionPointRoot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addActionPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuActionPoint = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteActionPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuAnimationRoot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addFrameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scAnimationProperties)).BeginInit();
            this.scAnimationProperties.Panel1.SuspendLayout();
            this.scAnimationProperties.Panel2.SuspendLayout();
            this.scAnimationProperties.SuspendLayout();
            this.pnAnimation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCurrentFrame)).BeginInit();
            this.cmnuHitboxRoot.SuspendLayout();
            this.cmnuHitbox.SuspendLayout();
            this.cmnuFrame.SuspendLayout();
            this.cmnuFrameTriggerRoot.SuspendLayout();
            this.cmnuFrameTrigger.SuspendLayout();
            this.cmnuActionPointRoot.SuspendLayout();
            this.cmnuActionPoint.SuspendLayout();
            this.cmnuAnimationRoot.SuspendLayout();
            this.SuspendLayout();
            // 
            // pgAnimation
            // 
            this.pgAnimation.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pgAnimation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgAnimation.Location = new System.Drawing.Point(0, 0);
            this.pgAnimation.Name = "pgAnimation";
            this.pgAnimation.Size = new System.Drawing.Size(345, 261);
            this.pgAnimation.TabIndex = 0;
            this.pgAnimation.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgAnimation_PropertyValueChanged);
            this.pgAnimation.Click += new System.EventHandler(this.pgAnimation_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.scAnimationProperties);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pnAnimation);
            this.splitContainer1.Size = new System.Drawing.Size(942, 506);
            this.splitContainer1.SplitterDistance = 345;
            this.splitContainer1.TabIndex = 3;
            // 
            // scAnimationProperties
            // 
            this.scAnimationProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scAnimationProperties.Location = new System.Drawing.Point(0, 0);
            this.scAnimationProperties.Name = "scAnimationProperties";
            this.scAnimationProperties.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scAnimationProperties.Panel1
            // 
            this.scAnimationProperties.Panel1.Controls.Add(this.tvAnimation);
            // 
            // scAnimationProperties.Panel2
            // 
            this.scAnimationProperties.Panel2.Controls.Add(this.pgAnimation);
            this.scAnimationProperties.Size = new System.Drawing.Size(345, 506);
            this.scAnimationProperties.SplitterDistance = 241;
            this.scAnimationProperties.TabIndex = 1;
            // 
            // tvAnimation
            // 
            this.tvAnimation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tvAnimation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvAnimation.Location = new System.Drawing.Point(0, 0);
            this.tvAnimation.Name = "tvAnimation";
            this.tvAnimation.Size = new System.Drawing.Size(345, 241);
            this.tvAnimation.TabIndex = 0;
            this.tvAnimation.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvAnimation_AfterSelect);
            this.tvAnimation.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tvAnimation_MouseUp);
            // 
            // pnAnimation
            // 
            this.pnAnimation.BackColor = System.Drawing.Color.LightGray;
            this.pnAnimation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnAnimation.Controls.Add(this.vsAnimation);
            this.pnAnimation.Controls.Add(this.pbCurrentFrame);
            this.pnAnimation.Controls.Add(this.hsAnimation);
            this.pnAnimation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnAnimation.Location = new System.Drawing.Point(0, 0);
            this.pnAnimation.Name = "pnAnimation";
            this.pnAnimation.Size = new System.Drawing.Size(593, 506);
            this.pnAnimation.TabIndex = 13;
            // 
            // vsAnimation
            // 
            this.vsAnimation.Location = new System.Drawing.Point(153, -1);
            this.vsAnimation.Name = "vsAnimation";
            this.vsAnimation.Size = new System.Drawing.Size(16, 45);
            this.vsAnimation.TabIndex = 23;
            this.vsAnimation.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vsAnimation_Scroll);
            // 
            // pbCurrentFrame
            // 
            this.pbCurrentFrame.BackColor = System.Drawing.Color.DimGray;
            this.pbCurrentFrame.Location = new System.Drawing.Point(-1, -1);
            this.pbCurrentFrame.Name = "pbCurrentFrame";
            this.pbCurrentFrame.Size = new System.Drawing.Size(154, 45);
            this.pbCurrentFrame.TabIndex = 22;
            this.pbCurrentFrame.TabStop = false;
            this.pbCurrentFrame.Paint += new System.Windows.Forms.PaintEventHandler(this.pbCurrentFrame_Paint);
            this.pbCurrentFrame.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbCurrentFrame_MouseDown);
            this.pbCurrentFrame.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbCurrentFrame_MouseMove);
            this.pbCurrentFrame.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbCurrentFrame_MouseUp);
            // 
            // hsAnimation
            // 
            this.hsAnimation.Location = new System.Drawing.Point(-2, 44);
            this.hsAnimation.Name = "hsAnimation";
            this.hsAnimation.Size = new System.Drawing.Size(155, 16);
            this.hsAnimation.TabIndex = 21;
            this.hsAnimation.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsAnimation_Scroll);
            // 
            // cmnuHitboxRoot
            // 
            this.cmnuHitboxRoot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addHitboxToolStripMenuItem});
            this.cmnuHitboxRoot.Name = "cmnuHitboxRoot";
            this.cmnuHitboxRoot.Size = new System.Drawing.Size(135, 26);
            // 
            // addHitboxToolStripMenuItem
            // 
            this.addHitboxToolStripMenuItem.Name = "addHitboxToolStripMenuItem";
            this.addHitboxToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.addHitboxToolStripMenuItem.Text = "Add Hitbox";
            this.addHitboxToolStripMenuItem.Click += new System.EventHandler(this.addHitboxToolStripMenuItem_Click);
            // 
            // cmnuHitbox
            // 
            this.cmnuHitbox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteHitboxToolStripMenuItem});
            this.cmnuHitbox.Name = "cmnuHitboxRoot";
            this.cmnuHitbox.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteHitboxToolStripMenuItem
            // 
            this.deleteHitboxToolStripMenuItem.Name = "deleteHitboxToolStripMenuItem";
            this.deleteHitboxToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteHitboxToolStripMenuItem.Text = "Delete";
            this.deleteHitboxToolStripMenuItem.Click += new System.EventHandler(this.deleteHitboxToolStripMenuItem_Click);
            // 
            // cmnuFrame
            // 
            this.cmnuFrame.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addHitboxToolStripMenuItem1,
            this.addFrameTriggerToolStripMenuItem1,
            this.addActionPointToolStripMenuItem1,
            this.deleteFrameToolStripMenuItem});
            this.cmnuFrame.Name = "cmnuHitboxRoot";
            this.cmnuFrame.Size = new System.Drawing.Size(174, 92);
            // 
            // addHitboxToolStripMenuItem1
            // 
            this.addHitboxToolStripMenuItem1.Name = "addHitboxToolStripMenuItem1";
            this.addHitboxToolStripMenuItem1.Size = new System.Drawing.Size(173, 22);
            this.addHitboxToolStripMenuItem1.Text = "Add Hitbox";
            this.addHitboxToolStripMenuItem1.Click += new System.EventHandler(this.addHitboxToolStripMenuItem_Click);
            // 
            // addFrameTriggerToolStripMenuItem1
            // 
            this.addFrameTriggerToolStripMenuItem1.Name = "addFrameTriggerToolStripMenuItem1";
            this.addFrameTriggerToolStripMenuItem1.Size = new System.Drawing.Size(173, 22);
            this.addFrameTriggerToolStripMenuItem1.Text = "Add Frame Trigger";
            this.addFrameTriggerToolStripMenuItem1.Click += new System.EventHandler(this.addFrameTriggerToolStripMenuItem_Click);
            // 
            // addActionPointToolStripMenuItem1
            // 
            this.addActionPointToolStripMenuItem1.Name = "addActionPointToolStripMenuItem1";
            this.addActionPointToolStripMenuItem1.Size = new System.Drawing.Size(173, 22);
            this.addActionPointToolStripMenuItem1.Text = "Add Action Point";
            this.addActionPointToolStripMenuItem1.Click += new System.EventHandler(this.addActionPointToolStripMenuItem1_Click);
            // 
            // deleteFrameToolStripMenuItem
            // 
            this.deleteFrameToolStripMenuItem.Name = "deleteFrameToolStripMenuItem";
            this.deleteFrameToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.deleteFrameToolStripMenuItem.Text = "Delete Frame";
            this.deleteFrameToolStripMenuItem.Click += new System.EventHandler(this.deleteFrameToolStripMenuItem_Click);
            // 
            // cmnuFrameTriggerRoot
            // 
            this.cmnuFrameTriggerRoot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFrameTriggerToolStripMenuItem});
            this.cmnuFrameTriggerRoot.Name = "cmnuHitboxRoot";
            this.cmnuFrameTriggerRoot.Size = new System.Drawing.Size(174, 26);
            // 
            // addFrameTriggerToolStripMenuItem
            // 
            this.addFrameTriggerToolStripMenuItem.Name = "addFrameTriggerToolStripMenuItem";
            this.addFrameTriggerToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.addFrameTriggerToolStripMenuItem.Text = "Add Frame Trigger";
            this.addFrameTriggerToolStripMenuItem.Click += new System.EventHandler(this.addFrameTriggerToolStripMenuItem_Click);
            // 
            // cmnuFrameTrigger
            // 
            this.cmnuFrameTrigger.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteFrameTriggerToolStripMenuItem});
            this.cmnuFrameTrigger.Name = "cmnuHitboxRoot";
            this.cmnuFrameTrigger.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteFrameTriggerToolStripMenuItem
            // 
            this.deleteFrameTriggerToolStripMenuItem.Name = "deleteFrameTriggerToolStripMenuItem";
            this.deleteFrameTriggerToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteFrameTriggerToolStripMenuItem.Text = "Delete";
            this.deleteFrameTriggerToolStripMenuItem.Click += new System.EventHandler(this.deleteFrameTriggerToolStripMenuItem_Click);
            // 
            // cmnuActionPointRoot
            // 
            this.cmnuActionPointRoot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addActionPointToolStripMenuItem});
            this.cmnuActionPointRoot.Name = "cmnuHitboxRoot";
            this.cmnuActionPointRoot.Size = new System.Drawing.Size(166, 26);
            // 
            // addActionPointToolStripMenuItem
            // 
            this.addActionPointToolStripMenuItem.Name = "addActionPointToolStripMenuItem";
            this.addActionPointToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.addActionPointToolStripMenuItem.Text = "Add Action Point";
            this.addActionPointToolStripMenuItem.Click += new System.EventHandler(this.addActionPointToolStripMenuItem_Click);
            // 
            // cmnuActionPoint
            // 
            this.cmnuActionPoint.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteActionPointToolStripMenuItem});
            this.cmnuActionPoint.Name = "cmnuHitboxRoot";
            this.cmnuActionPoint.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteActionPointToolStripMenuItem
            // 
            this.deleteActionPointToolStripMenuItem.Name = "deleteActionPointToolStripMenuItem";
            this.deleteActionPointToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteActionPointToolStripMenuItem.Text = "Delete";
            this.deleteActionPointToolStripMenuItem.Click += new System.EventHandler(this.deleteActionPointToolStripMenuItem_Click);
            // 
            // cmnuAnimationRoot
            // 
            this.cmnuAnimationRoot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFrameToolStripMenuItem});
            this.cmnuAnimationRoot.Name = "cmnuHitboxRoot";
            this.cmnuAnimationRoot.Size = new System.Drawing.Size(133, 26);
            // 
            // addFrameToolStripMenuItem
            // 
            this.addFrameToolStripMenuItem.Name = "addFrameToolStripMenuItem";
            this.addFrameToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.addFrameToolStripMenuItem.Text = "Add Frame";
            this.addFrameToolStripMenuItem.Click += new System.EventHandler(this.addFrameToolStripMenuItem_Click);
            // 
            // AnimationEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(942, 506);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "AnimationEditor";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Animation Editor";
            this.Load += new System.EventHandler(this.AnimationEditor_Load);
            this.Resize += new System.EventHandler(this.AnimationEditor_Resize);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.scAnimationProperties.Panel1.ResumeLayout(false);
            this.scAnimationProperties.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scAnimationProperties)).EndInit();
            this.scAnimationProperties.ResumeLayout(false);
            this.pnAnimation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbCurrentFrame)).EndInit();
            this.cmnuHitboxRoot.ResumeLayout(false);
            this.cmnuHitbox.ResumeLayout(false);
            this.cmnuFrame.ResumeLayout(false);
            this.cmnuFrameTriggerRoot.ResumeLayout(false);
            this.cmnuFrameTrigger.ResumeLayout(false);
            this.cmnuActionPointRoot.ResumeLayout(false);
            this.cmnuActionPoint.ResumeLayout(false);
            this.cmnuAnimationRoot.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid pgAnimation;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer scAnimationProperties;
        private System.Windows.Forms.TreeView tvAnimation;
        private System.Windows.Forms.Panel pnAnimation;
        private System.Windows.Forms.ContextMenuStrip cmnuHitboxRoot;
        private System.Windows.Forms.ToolStripMenuItem addHitboxToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuHitbox;
        private System.Windows.Forms.ToolStripMenuItem deleteHitboxToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuFrame;
        private System.Windows.Forms.ToolStripMenuItem deleteFrameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addHitboxToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem addFrameTriggerToolStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip cmnuFrameTriggerRoot;
        private System.Windows.Forms.ToolStripMenuItem addFrameTriggerToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuFrameTrigger;
        private System.Windows.Forms.ToolStripMenuItem deleteFrameTriggerToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuActionPointRoot;
        private System.Windows.Forms.ToolStripMenuItem addActionPointToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuActionPoint;
        private System.Windows.Forms.ToolStripMenuItem deleteActionPointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addActionPointToolStripMenuItem1;
        private System.Windows.Forms.VScrollBar vsAnimation;
        private System.Windows.Forms.PictureBox pbCurrentFrame;
        private System.Windows.Forms.HScrollBar hsAnimation;
        private System.Windows.Forms.ContextMenuStrip cmnuAnimationRoot;
        private System.Windows.Forms.ToolStripMenuItem addFrameToolStripMenuItem;
    }
}