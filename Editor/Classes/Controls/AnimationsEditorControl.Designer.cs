namespace FiremelonEditor2
{
    partial class AnimationsEditorControl
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
            this.scAnimations = new System.Windows.Forms.SplitContainer();
            this.scAnimationsList = new System.Windows.Forms.SplitContainer();
            this.tvAnimations = new System.Windows.Forms.TreeView();
            this.pgAnimation = new System.Windows.Forms.PropertyGrid();
            this.cmnuAnimationRoot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addFrameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuAnimation = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteAnimationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuAnimationGroup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteAnimationGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuFrame = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteAnimationFrameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuHitbox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteHitboxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuHitboxRoot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addHitboxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyHitboxesFromStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuFrameTriggerRoot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addFrameTriggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuFrameTrigger = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteFrameTriggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuActionPointRoot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addActionPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuActionPoint = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteActionPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.scAnimations)).BeginInit();
            this.scAnimations.Panel1.SuspendLayout();
            this.scAnimations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scAnimationsList)).BeginInit();
            this.scAnimationsList.Panel1.SuspendLayout();
            this.scAnimationsList.Panel2.SuspendLayout();
            this.scAnimationsList.SuspendLayout();
            this.cmnuAnimationRoot.SuspendLayout();
            this.cmnuAnimation.SuspendLayout();
            this.cmnuAnimationGroup.SuspendLayout();
            this.cmnuFrame.SuspendLayout();
            this.cmnuHitbox.SuspendLayout();
            this.cmnuHitboxRoot.SuspendLayout();
            this.cmnuFrameTriggerRoot.SuspendLayout();
            this.cmnuFrameTrigger.SuspendLayout();
            this.cmnuActionPointRoot.SuspendLayout();
            this.cmnuActionPoint.SuspendLayout();
            this.SuspendLayout();
            // 
            // scAnimations
            // 
            this.scAnimations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scAnimations.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scAnimations.Location = new System.Drawing.Point(0, 0);
            this.scAnimations.Name = "scAnimations";
            // 
            // scAnimations.Panel1
            // 
            this.scAnimations.Panel1.Controls.Add(this.scAnimationsList);
            this.scAnimations.Size = new System.Drawing.Size(761, 536);
            this.scAnimations.SplitterDistance = 280;
            this.scAnimations.TabIndex = 4;
            // 
            // scAnimationsList
            // 
            this.scAnimationsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scAnimationsList.Location = new System.Drawing.Point(0, 0);
            this.scAnimationsList.Name = "scAnimationsList";
            this.scAnimationsList.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scAnimationsList.Panel1
            // 
            this.scAnimationsList.Panel1.Controls.Add(this.tvAnimations);
            // 
            // scAnimationsList.Panel2
            // 
            this.scAnimationsList.Panel2.Controls.Add(this.pgAnimation);
            this.scAnimationsList.Size = new System.Drawing.Size(280, 536);
            this.scAnimationsList.SplitterDistance = 254;
            this.scAnimationsList.TabIndex = 1;
            // 
            // tvAnimations
            // 
            this.tvAnimations.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tvAnimations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvAnimations.Location = new System.Drawing.Point(0, 0);
            this.tvAnimations.Name = "tvAnimations";
            this.tvAnimations.Size = new System.Drawing.Size(280, 254);
            this.tvAnimations.TabIndex = 0;
            this.tvAnimations.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvAnimations_AfterSelect);
            // 
            // pgAnimation
            // 
            this.pgAnimation.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pgAnimation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgAnimation.Location = new System.Drawing.Point(0, 0);
            this.pgAnimation.Name = "pgAnimation";
            this.pgAnimation.Size = new System.Drawing.Size(280, 278);
            this.pgAnimation.TabIndex = 0;
            this.pgAnimation.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgAnimation_PropertyValueChanged);
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
            // 
            // cmnuAnimation
            // 
            this.cmnuAnimation.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteAnimationToolStripMenuItem});
            this.cmnuAnimation.Name = "cmnuTileSheetRoot";
            this.cmnuAnimation.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteAnimationToolStripMenuItem
            // 
            this.deleteAnimationToolStripMenuItem.Name = "deleteAnimationToolStripMenuItem";
            this.deleteAnimationToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteAnimationToolStripMenuItem.Text = "Delete";
            // 
            // cmnuAnimationGroup
            // 
            this.cmnuAnimationGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteAnimationGroupToolStripMenuItem});
            this.cmnuAnimationGroup.Name = "cmnuTileSheetRoot";
            this.cmnuAnimationGroup.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteAnimationGroupToolStripMenuItem
            // 
            this.deleteAnimationGroupToolStripMenuItem.Name = "deleteAnimationGroupToolStripMenuItem";
            this.deleteAnimationGroupToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteAnimationGroupToolStripMenuItem.Text = "Delete";
            // 
            // cmnuFrame
            // 
            this.cmnuFrame.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteAnimationFrameToolStripMenuItem});
            this.cmnuFrame.Name = "cmnuTileSheetRoot";
            this.cmnuFrame.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteAnimationFrameToolStripMenuItem
            // 
            this.deleteAnimationFrameToolStripMenuItem.Name = "deleteAnimationFrameToolStripMenuItem";
            this.deleteAnimationFrameToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteAnimationFrameToolStripMenuItem.Text = "Delete";
            // 
            // cmnuHitbox
            // 
            this.cmnuHitbox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteHitboxToolStripMenuItem});
            this.cmnuHitbox.Name = "cmnuHitbox";
            this.cmnuHitbox.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteHitboxToolStripMenuItem
            // 
            this.deleteHitboxToolStripMenuItem.Name = "deleteHitboxToolStripMenuItem";
            this.deleteHitboxToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteHitboxToolStripMenuItem.Text = "Delete";
            // 
            // cmnuHitboxRoot
            // 
            this.cmnuHitboxRoot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addHitboxToolStripMenuItem,
            this.copyHitboxesFromStateToolStripMenuItem});
            this.cmnuHitboxRoot.Name = "cmnuHitboxRoot";
            this.cmnuHitboxRoot.Size = new System.Drawing.Size(212, 48);
            // 
            // addHitboxToolStripMenuItem
            // 
            this.addHitboxToolStripMenuItem.Name = "addHitboxToolStripMenuItem";
            this.addHitboxToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.addHitboxToolStripMenuItem.Text = "Add Hitbox";
            // 
            // copyHitboxesFromStateToolStripMenuItem
            // 
            this.copyHitboxesFromStateToolStripMenuItem.Name = "copyHitboxesFromStateToolStripMenuItem";
            this.copyHitboxesFromStateToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.copyHitboxesFromStateToolStripMenuItem.Text = "Copy Hitboxes From State";
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
            // 
            // AnimationsEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scAnimations);
            this.Name = "AnimationsEditorControl";
            this.Size = new System.Drawing.Size(761, 536);
            this.scAnimations.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scAnimations)).EndInit();
            this.scAnimations.ResumeLayout(false);
            this.scAnimationsList.Panel1.ResumeLayout(false);
            this.scAnimationsList.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scAnimationsList)).EndInit();
            this.scAnimationsList.ResumeLayout(false);
            this.cmnuAnimationRoot.ResumeLayout(false);
            this.cmnuAnimation.ResumeLayout(false);
            this.cmnuAnimationGroup.ResumeLayout(false);
            this.cmnuFrame.ResumeLayout(false);
            this.cmnuHitbox.ResumeLayout(false);
            this.cmnuHitboxRoot.ResumeLayout(false);
            this.cmnuFrameTriggerRoot.ResumeLayout(false);
            this.cmnuFrameTrigger.ResumeLayout(false);
            this.cmnuActionPointRoot.ResumeLayout(false);
            this.cmnuActionPoint.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer scAnimations;
        private System.Windows.Forms.SplitContainer scAnimationsList;
        private System.Windows.Forms.TreeView tvAnimations;
        private System.Windows.Forms.PropertyGrid pgAnimation;
        private System.Windows.Forms.ContextMenuStrip cmnuAnimationRoot;
        private System.Windows.Forms.ToolStripMenuItem addFrameToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuAnimation;
        private System.Windows.Forms.ToolStripMenuItem deleteAnimationToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuAnimationGroup;
        private System.Windows.Forms.ToolStripMenuItem deleteAnimationGroupToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuFrame;
        private System.Windows.Forms.ToolStripMenuItem deleteAnimationFrameToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuHitbox;
        private System.Windows.Forms.ToolStripMenuItem deleteHitboxToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuHitboxRoot;
        private System.Windows.Forms.ToolStripMenuItem addHitboxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyHitboxesFromStateToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuFrameTriggerRoot;
        private System.Windows.Forms.ToolStripMenuItem addFrameTriggerToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuFrameTrigger;
        private System.Windows.Forms.ToolStripMenuItem deleteFrameTriggerToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuActionPointRoot;
        private System.Windows.Forms.ToolStripMenuItem addActionPointToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuActionPoint;
        private System.Windows.Forms.ToolStripMenuItem deleteActionPointToolStripMenuItem;
    }
}
