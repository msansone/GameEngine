namespace FiremelonEditor2
{
    partial class StateEditor
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
            this.pnState = new System.Windows.Forms.Panel();
            this.hsState = new System.Windows.Forms.HScrollBar();
            this.vsState = new System.Windows.Forms.VScrollBar();
            this.pbState = new System.Windows.Forms.PictureBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.scAnimationProperties = new System.Windows.Forms.SplitContainer();
            this.tvState = new System.Windows.Forms.TreeView();
            this.pgState = new System.Windows.Forms.PropertyGrid();
            this.cmnuAnimationSlotRoot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addAnimationSlotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuAnimationSlot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteAnimationSlotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuHitboxRoot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addHitboxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyHitboxesFromStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuHitbox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteHitboxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnState.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scAnimationProperties)).BeginInit();
            this.scAnimationProperties.Panel1.SuspendLayout();
            this.scAnimationProperties.Panel2.SuspendLayout();
            this.scAnimationProperties.SuspendLayout();
            this.cmnuAnimationSlotRoot.SuspendLayout();
            this.cmnuAnimationSlot.SuspendLayout();
            this.cmnuHitboxRoot.SuspendLayout();
            this.cmnuHitbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnState
            // 
            this.pnState.BackColor = System.Drawing.Color.LightGray;
            this.pnState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnState.Controls.Add(this.hsState);
            this.pnState.Controls.Add(this.vsState);
            this.pnState.Controls.Add(this.pbState);
            this.pnState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnState.Location = new System.Drawing.Point(0, 0);
            this.pnState.Name = "pnState";
            this.pnState.Size = new System.Drawing.Size(527, 512);
            this.pnState.TabIndex = 10;
            this.pnState.Resize += new System.EventHandler(this.pnState_Resize);
            // 
            // hsState
            // 
            this.hsState.Location = new System.Drawing.Point(-1, 112);
            this.hsState.Name = "hsState";
            this.hsState.Size = new System.Drawing.Size(133, 16);
            this.hsState.TabIndex = 7;
            this.hsState.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsState_Scroll);
            // 
            // vsState
            // 
            this.vsState.Location = new System.Drawing.Point(132, -1);
            this.vsState.Name = "vsState";
            this.vsState.Size = new System.Drawing.Size(16, 113);
            this.vsState.TabIndex = 6;
            this.vsState.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vsState_Scroll);
            // 
            // pbState
            // 
            this.pbState.BackColor = System.Drawing.Color.DimGray;
            this.pbState.Location = new System.Drawing.Point(-1, -1);
            this.pbState.Name = "pbState";
            this.pbState.Size = new System.Drawing.Size(133, 113);
            this.pbState.TabIndex = 8;
            this.pbState.TabStop = false;
            this.pbState.Paint += new System.Windows.Forms.PaintEventHandler(this.pbState_Paint);
            this.pbState.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbState_MouseDown);
            this.pbState.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbState_MouseMove);
            this.pbState.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbState_MouseUp);
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
            this.splitContainer1.Panel2.Controls.Add(this.pnState);
            this.splitContainer1.Size = new System.Drawing.Size(833, 512);
            this.splitContainer1.SplitterDistance = 302;
            this.splitContainer1.TabIndex = 11;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
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
            this.scAnimationProperties.Panel1.Controls.Add(this.tvState);
            // 
            // scAnimationProperties.Panel2
            // 
            this.scAnimationProperties.Panel2.Controls.Add(this.pgState);
            this.scAnimationProperties.Size = new System.Drawing.Size(302, 512);
            this.scAnimationProperties.SplitterDistance = 242;
            this.scAnimationProperties.TabIndex = 1;
            // 
            // tvState
            // 
            this.tvState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tvState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvState.Location = new System.Drawing.Point(0, 0);
            this.tvState.Name = "tvState";
            this.tvState.Size = new System.Drawing.Size(302, 242);
            this.tvState.TabIndex = 0;
            this.tvState.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvState_AfterSelect);
            this.tvState.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tvState_MouseUp);
            // 
            // pgState
            // 
            this.pgState.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pgState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgState.Location = new System.Drawing.Point(0, 0);
            this.pgState.Name = "pgState";
            this.pgState.Size = new System.Drawing.Size(302, 266);
            this.pgState.TabIndex = 0;
            this.pgState.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgState_PropertyValueChanged);
            this.pgState.Click += new System.EventHandler(this.pgState_Click);
            // 
            // cmnuAnimationSlotRoot
            // 
            this.cmnuAnimationSlotRoot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addAnimationSlotToolStripMenuItem});
            this.cmnuAnimationSlotRoot.Name = "cmnuAnimationSlotRoot";
            this.cmnuAnimationSlotRoot.Size = new System.Drawing.Size(179, 26);
            // 
            // addAnimationSlotToolStripMenuItem
            // 
            this.addAnimationSlotToolStripMenuItem.Name = "addAnimationSlotToolStripMenuItem";
            this.addAnimationSlotToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.addAnimationSlotToolStripMenuItem.Text = "Add Animation Slot";
            this.addAnimationSlotToolStripMenuItem.Click += new System.EventHandler(this.addAnimationSlotToolStripMenuItem_Click);
            // 
            // cmnuAnimationSlot
            // 
            this.cmnuAnimationSlot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteAnimationSlotToolStripMenuItem,
            this.moveUpToolStripMenuItem,
            this.moveDownToolStripMenuItem});
            this.cmnuAnimationSlot.Name = "cmnuAnimationSlot";
            this.cmnuAnimationSlot.Size = new System.Drawing.Size(139, 70);
            // 
            // deleteAnimationSlotToolStripMenuItem
            // 
            this.deleteAnimationSlotToolStripMenuItem.Name = "deleteAnimationSlotToolStripMenuItem";
            this.deleteAnimationSlotToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.deleteAnimationSlotToolStripMenuItem.Text = "Delete";
            this.deleteAnimationSlotToolStripMenuItem.Click += new System.EventHandler(this.deleteAnimationSlotToolStripMenuItem_Click);
            // 
            // moveUpToolStripMenuItem
            // 
            this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.moveUpToolStripMenuItem.Text = "Move Up";
            this.moveUpToolStripMenuItem.Click += new System.EventHandler(this.moveUpToolStripMenuItem_Click);
            // 
            // moveDownToolStripMenuItem
            // 
            this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            this.moveDownToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.moveDownToolStripMenuItem.Text = "Move Down";
            this.moveDownToolStripMenuItem.Click += new System.EventHandler(this.moveDownToolStripMenuItem_Click);
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
            this.addHitboxToolStripMenuItem.Click += new System.EventHandler(this.addHitboxToolStripMenuItem_Click);
            // 
            // copyHitboxesFromStateToolStripMenuItem
            // 
            this.copyHitboxesFromStateToolStripMenuItem.Name = "copyHitboxesFromStateToolStripMenuItem";
            this.copyHitboxesFromStateToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.copyHitboxesFromStateToolStripMenuItem.Text = "Copy Hitboxes From State";
            this.copyHitboxesFromStateToolStripMenuItem.DropDownOpening += new System.EventHandler(this.copyHitboxesFromStateToolStripMenuItem_DropDownOpening);
            this.copyHitboxesFromStateToolStripMenuItem.Click += new System.EventHandler(this.copyHitboxesFromStateToolStripMenuItem_Click);
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
            this.deleteHitboxToolStripMenuItem.Click += new System.EventHandler(this.deleteHitboxToolStripMenuItem_Click);
            // 
            // StateEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 512);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "StateEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "State Editor";
            this.Load += new System.EventHandler(this.StateEditor_Load);
            this.pnState.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbState)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.scAnimationProperties.Panel1.ResumeLayout(false);
            this.scAnimationProperties.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scAnimationProperties)).EndInit();
            this.scAnimationProperties.ResumeLayout(false);
            this.cmnuAnimationSlotRoot.ResumeLayout(false);
            this.cmnuAnimationSlot.ResumeLayout(false);
            this.cmnuHitboxRoot.ResumeLayout(false);
            this.cmnuHitbox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnState;
        private System.Windows.Forms.HScrollBar hsState;
        private System.Windows.Forms.VScrollBar vsState;
        private System.Windows.Forms.PictureBox pbState;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer scAnimationProperties;
        private System.Windows.Forms.TreeView tvState;
        private System.Windows.Forms.PropertyGrid pgState;
        private System.Windows.Forms.ContextMenuStrip cmnuAnimationSlotRoot;
        private System.Windows.Forms.ToolStripMenuItem addAnimationSlotToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuAnimationSlot;
        private System.Windows.Forms.ToolStripMenuItem deleteAnimationSlotToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuHitboxRoot;
        private System.Windows.Forms.ToolStripMenuItem addHitboxToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuHitbox;
        private System.Windows.Forms.ToolStripMenuItem deleteHitboxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveDownToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyHitboxesFromStateToolStripMenuItem;
    }
}