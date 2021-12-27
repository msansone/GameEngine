namespace FiremelonEditor2
{
    partial class ConstantsEditorControl
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
            this.scGameButtonsList = new System.Windows.Forms.SplitContainer();
            this.tvConstants = new System.Windows.Forms.TreeView();
            this.pgConstants = new System.Windows.Forms.PropertyGrid();
            this.scConstants = new System.Windows.Forms.SplitContainer();
            this.cmnuHitboxIdentity = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteHitboxIdentityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuHitboxIdentityRoot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addHitboxIdentityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuTriggerSignal = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteTriggerSignalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuTriggerSignalRoot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addTriggerSignalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.scGameButtonsList)).BeginInit();
            this.scGameButtonsList.Panel1.SuspendLayout();
            this.scGameButtonsList.Panel2.SuspendLayout();
            this.scGameButtonsList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scConstants)).BeginInit();
            this.scConstants.Panel1.SuspendLayout();
            this.scConstants.SuspendLayout();
            this.cmnuHitboxIdentity.SuspendLayout();
            this.cmnuHitboxIdentityRoot.SuspendLayout();
            this.cmnuTriggerSignal.SuspendLayout();
            this.cmnuTriggerSignalRoot.SuspendLayout();
            this.SuspendLayout();
            // 
            // scGameButtonsList
            // 
            this.scGameButtonsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scGameButtonsList.Location = new System.Drawing.Point(0, 0);
            this.scGameButtonsList.Name = "scGameButtonsList";
            this.scGameButtonsList.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scGameButtonsList.Panel1
            // 
            this.scGameButtonsList.Panel1.Controls.Add(this.tvConstants);
            // 
            // scGameButtonsList.Panel2
            // 
            this.scGameButtonsList.Panel2.Controls.Add(this.pgConstants);
            this.scGameButtonsList.Size = new System.Drawing.Size(280, 436);
            this.scGameButtonsList.SplitterDistance = 206;
            this.scGameButtonsList.TabIndex = 1;
            // 
            // tvConstants
            // 
            this.tvConstants.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tvConstants.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvConstants.Location = new System.Drawing.Point(0, 0);
            this.tvConstants.Name = "tvConstants";
            this.tvConstants.Size = new System.Drawing.Size(280, 206);
            this.tvConstants.TabIndex = 0;
            this.tvConstants.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvConstants_AfterSelect);
            // 
            // pgConstants
            // 
            this.pgConstants.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pgConstants.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgConstants.Location = new System.Drawing.Point(0, 0);
            this.pgConstants.Name = "pgConstants";
            this.pgConstants.Size = new System.Drawing.Size(280, 226);
            this.pgConstants.TabIndex = 0;
            this.pgConstants.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgConstants_PropertyValueChanged);
            // 
            // scConstants
            // 
            this.scConstants.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scConstants.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scConstants.Location = new System.Drawing.Point(0, 0);
            this.scConstants.Name = "scConstants";
            // 
            // scConstants.Panel1
            // 
            this.scConstants.Panel1.Controls.Add(this.scGameButtonsList);
            this.scConstants.Size = new System.Drawing.Size(881, 436);
            this.scConstants.SplitterDistance = 280;
            this.scConstants.TabIndex = 6;
            // 
            // cmnuHitboxIdentity
            // 
            this.cmnuHitboxIdentity.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteHitboxIdentityToolStripMenuItem});
            this.cmnuHitboxIdentity.Name = "cmnuTileSheetRoot";
            this.cmnuHitboxIdentity.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteHitboxIdentityToolStripMenuItem
            // 
            this.deleteHitboxIdentityToolStripMenuItem.Name = "deleteHitboxIdentityToolStripMenuItem";
            this.deleteHitboxIdentityToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteHitboxIdentityToolStripMenuItem.Text = "Delete";
            // 
            // cmnuHitboxIdentityRoot
            // 
            this.cmnuHitboxIdentityRoot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addHitboxIdentityToolStripMenuItem});
            this.cmnuHitboxIdentityRoot.Name = "contextMenuStrip3";
            this.cmnuHitboxIdentityRoot.Size = new System.Drawing.Size(178, 26);
            // 
            // addHitboxIdentityToolStripMenuItem
            // 
            this.addHitboxIdentityToolStripMenuItem.Name = "addHitboxIdentityToolStripMenuItem";
            this.addHitboxIdentityToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.addHitboxIdentityToolStripMenuItem.Text = "Add Hitbox Identity";
            // 
            // cmnuTriggerSignal
            // 
            this.cmnuTriggerSignal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteTriggerSignalToolStripMenuItem});
            this.cmnuTriggerSignal.Name = "cmnuTileSheetRoot";
            this.cmnuTriggerSignal.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteTriggerSignalToolStripMenuItem
            // 
            this.deleteTriggerSignalToolStripMenuItem.Name = "deleteTriggerSignalToolStripMenuItem";
            this.deleteTriggerSignalToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteTriggerSignalToolStripMenuItem.Text = "Delete";
            // 
            // cmnuTriggerSignalRoot
            // 
            this.cmnuTriggerSignalRoot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addTriggerSignalToolStripMenuItem});
            this.cmnuTriggerSignalRoot.Name = "contextMenuStrip3";
            this.cmnuTriggerSignalRoot.Size = new System.Drawing.Size(173, 26);
            // 
            // addTriggerSignalToolStripMenuItem
            // 
            this.addTriggerSignalToolStripMenuItem.Name = "addTriggerSignalToolStripMenuItem";
            this.addTriggerSignalToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.addTriggerSignalToolStripMenuItem.Text = "Add Trigger Signal";
            // 
            // ConstantsEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scConstants);
            this.Name = "ConstantsEditorControl";
            this.Size = new System.Drawing.Size(881, 436);
            this.scGameButtonsList.Panel1.ResumeLayout(false);
            this.scGameButtonsList.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scGameButtonsList)).EndInit();
            this.scGameButtonsList.ResumeLayout(false);
            this.scConstants.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scConstants)).EndInit();
            this.scConstants.ResumeLayout(false);
            this.cmnuHitboxIdentity.ResumeLayout(false);
            this.cmnuHitboxIdentityRoot.ResumeLayout(false);
            this.cmnuTriggerSignal.ResumeLayout(false);
            this.cmnuTriggerSignalRoot.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer scGameButtonsList;
        private System.Windows.Forms.TreeView tvConstants;
        private System.Windows.Forms.PropertyGrid pgConstants;
        private System.Windows.Forms.SplitContainer scConstants;
        private System.Windows.Forms.ContextMenuStrip cmnuHitboxIdentity;
        private System.Windows.Forms.ToolStripMenuItem deleteHitboxIdentityToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuHitboxIdentityRoot;
        private System.Windows.Forms.ToolStripMenuItem addHitboxIdentityToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuTriggerSignal;
        private System.Windows.Forms.ToolStripMenuItem deleteTriggerSignalToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuTriggerSignalRoot;
        private System.Windows.Forms.ToolStripMenuItem addTriggerSignalToolStripMenuItem;
    }
}
