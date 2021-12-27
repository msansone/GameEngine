namespace FiremelonEditor2
{
    partial class EntitiesEditorControl
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
            this.scEntities = new System.Windows.Forms.SplitContainer();
            this.scEntitiesList = new System.Windows.Forms.SplitContainer();
            this.tvEntities = new System.Windows.Forms.TreeView();
            this.pgProperties = new System.Windows.Forms.PropertyGrid();
            this.cmnuActor = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteAactorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuEvent = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuHudElement = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteHudElementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuState = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuScript = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewEditScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuProperty = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deletePropertyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuScriptRoot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuStateRoot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuPropertyRoot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addPropertyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuAnimationSlot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteAnimationSlotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuAnimationSlotRoot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addAnimationSlotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuHitbox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteHitboxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuHitboxRoot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addHitboxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyHitboxesFromStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.scEntities)).BeginInit();
            this.scEntities.Panel1.SuspendLayout();
            this.scEntities.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scEntitiesList)).BeginInit();
            this.scEntitiesList.Panel1.SuspendLayout();
            this.scEntitiesList.Panel2.SuspendLayout();
            this.scEntitiesList.SuspendLayout();
            this.cmnuActor.SuspendLayout();
            this.cmnuEvent.SuspendLayout();
            this.cmnuHudElement.SuspendLayout();
            this.cmnuState.SuspendLayout();
            this.cmnuScript.SuspendLayout();
            this.cmnuProperty.SuspendLayout();
            this.cmnuScriptRoot.SuspendLayout();
            this.cmnuStateRoot.SuspendLayout();
            this.cmnuPropertyRoot.SuspendLayout();
            this.cmnuAnimationSlot.SuspendLayout();
            this.cmnuAnimationSlotRoot.SuspendLayout();
            this.cmnuHitbox.SuspendLayout();
            this.cmnuHitboxRoot.SuspendLayout();
            this.SuspendLayout();
            // 
            // scEntities
            // 
            this.scEntities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scEntities.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scEntities.Location = new System.Drawing.Point(0, 0);
            this.scEntities.Name = "scEntities";
            // 
            // scEntities.Panel1
            // 
            this.scEntities.Panel1.Controls.Add(this.scEntitiesList);
            this.scEntities.Size = new System.Drawing.Size(1012, 598);
            this.scEntities.SplitterDistance = 280;
            this.scEntities.TabIndex = 5;
            // 
            // scEntitiesList
            // 
            this.scEntitiesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scEntitiesList.Location = new System.Drawing.Point(0, 0);
            this.scEntitiesList.Name = "scEntitiesList";
            this.scEntitiesList.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scEntitiesList.Panel1
            // 
            this.scEntitiesList.Panel1.Controls.Add(this.tvEntities);
            // 
            // scEntitiesList.Panel2
            // 
            this.scEntitiesList.Panel2.Controls.Add(this.pgProperties);
            this.scEntitiesList.Size = new System.Drawing.Size(280, 598);
            this.scEntitiesList.SplitterDistance = 283;
            this.scEntitiesList.TabIndex = 1;
            // 
            // tvEntities
            // 
            this.tvEntities.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tvEntities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvEntities.Location = new System.Drawing.Point(0, 0);
            this.tvEntities.Name = "tvEntities";
            this.tvEntities.Size = new System.Drawing.Size(280, 283);
            this.tvEntities.TabIndex = 0;
            this.tvEntities.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvEntities_BeforeSelect);
            this.tvEntities.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvEntities_AfterSelect);
            this.tvEntities.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tvEntities_MouseUp);
            // 
            // pgProperties
            // 
            this.pgProperties.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pgProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgProperties.Location = new System.Drawing.Point(0, 0);
            this.pgProperties.Name = "pgProperties";
            this.pgProperties.Size = new System.Drawing.Size(280, 311);
            this.pgProperties.TabIndex = 0;
            this.pgProperties.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgProperties_PropertyValueChanged);
            // 
            // cmnuActor
            // 
            this.cmnuActor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteAactorToolStripMenuItem});
            this.cmnuActor.Name = "cmnuTileSheetRoot";
            this.cmnuActor.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteAactorToolStripMenuItem
            // 
            this.deleteAactorToolStripMenuItem.Name = "deleteAactorToolStripMenuItem";
            this.deleteAactorToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteAactorToolStripMenuItem.Text = "Delete";
            // 
            // cmnuEvent
            // 
            this.cmnuEvent.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteEventToolStripMenuItem});
            this.cmnuEvent.Name = "cmnuTileSheetRoot";
            this.cmnuEvent.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteEventToolStripMenuItem
            // 
            this.deleteEventToolStripMenuItem.Name = "deleteEventToolStripMenuItem";
            this.deleteEventToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteEventToolStripMenuItem.Text = "Delete";
            // 
            // cmnuHudElement
            // 
            this.cmnuHudElement.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteHudElementToolStripMenuItem});
            this.cmnuHudElement.Name = "cmnuTileSheetRoot";
            this.cmnuHudElement.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteHudElementToolStripMenuItem
            // 
            this.deleteHudElementToolStripMenuItem.Name = "deleteHudElementToolStripMenuItem";
            this.deleteHudElementToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteHudElementToolStripMenuItem.Text = "Delete";
            // 
            // cmnuState
            // 
            this.cmnuState.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteStateToolStripMenuItem});
            this.cmnuState.Name = "cmnuTileSheetRoot";
            this.cmnuState.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteStateToolStripMenuItem
            // 
            this.deleteStateToolStripMenuItem.Name = "deleteStateToolStripMenuItem";
            this.deleteStateToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteStateToolStripMenuItem.Text = "Delete";
            // 
            // cmnuScript
            // 
            this.cmnuScript.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewEditScriptToolStripMenuItem,
            this.generateScriptToolStripMenuItem,
            this.deleteScriptToolStripMenuItem});
            this.cmnuScript.Name = "cmnuTileSheetRoot";
            this.cmnuScript.Size = new System.Drawing.Size(182, 70);
            // 
            // viewEditScriptToolStripMenuItem
            // 
            this.viewEditScriptToolStripMenuItem.Name = "viewEditScriptToolStripMenuItem";
            this.viewEditScriptToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.viewEditScriptToolStripMenuItem.Text = "View/Edit";
            this.viewEditScriptToolStripMenuItem.Click += new System.EventHandler(this.viewEditScriptToolStripMenuItem_Click);
            // 
            // generateScriptToolStripMenuItem
            // 
            this.generateScriptToolStripMenuItem.Name = "generateScriptToolStripMenuItem";
            this.generateScriptToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.generateScriptToolStripMenuItem.Text = "Generate New Script";
            // 
            // deleteScriptToolStripMenuItem
            // 
            this.deleteScriptToolStripMenuItem.Name = "deleteScriptToolStripMenuItem";
            this.deleteScriptToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.deleteScriptToolStripMenuItem.Text = "Delete Script";
            this.deleteScriptToolStripMenuItem.Visible = false;
            // 
            // cmnuProperty
            // 
            this.cmnuProperty.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deletePropertyToolStripMenuItem});
            this.cmnuProperty.Name = "cmnuTileSheetRoot";
            this.cmnuProperty.Size = new System.Drawing.Size(108, 26);
            // 
            // deletePropertyToolStripMenuItem
            // 
            this.deletePropertyToolStripMenuItem.Name = "deletePropertyToolStripMenuItem";
            this.deletePropertyToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deletePropertyToolStripMenuItem.Text = "Delete";
            // 
            // cmnuScriptRoot
            // 
            this.cmnuScriptRoot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addScriptToolStripMenuItem});
            this.cmnuScriptRoot.Name = "contextMenuStrip3";
            this.cmnuScriptRoot.Size = new System.Drawing.Size(130, 26);
            // 
            // addScriptToolStripMenuItem
            // 
            this.addScriptToolStripMenuItem.Name = "addScriptToolStripMenuItem";
            this.addScriptToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.addScriptToolStripMenuItem.Text = "Add Script";
            // 
            // cmnuStateRoot
            // 
            this.cmnuStateRoot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addStateToolStripMenuItem});
            this.cmnuStateRoot.Name = "contextMenuStrip3";
            this.cmnuStateRoot.Size = new System.Drawing.Size(126, 26);
            // 
            // addStateToolStripMenuItem
            // 
            this.addStateToolStripMenuItem.Name = "addStateToolStripMenuItem";
            this.addStateToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.addStateToolStripMenuItem.Text = "Add State";
            // 
            // cmnuPropertyRoot
            // 
            this.cmnuPropertyRoot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPropertyToolStripMenuItem});
            this.cmnuPropertyRoot.Name = "contextMenuStrip3";
            this.cmnuPropertyRoot.Size = new System.Drawing.Size(145, 26);
            // 
            // addPropertyToolStripMenuItem
            // 
            this.addPropertyToolStripMenuItem.Name = "addPropertyToolStripMenuItem";
            this.addPropertyToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.addPropertyToolStripMenuItem.Text = "Add Property";
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
            // EntitiesEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scEntities);
            this.Name = "EntitiesEditorControl";
            this.Size = new System.Drawing.Size(1012, 598);
            this.scEntities.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scEntities)).EndInit();
            this.scEntities.ResumeLayout(false);
            this.scEntitiesList.Panel1.ResumeLayout(false);
            this.scEntitiesList.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scEntitiesList)).EndInit();
            this.scEntitiesList.ResumeLayout(false);
            this.cmnuActor.ResumeLayout(false);
            this.cmnuEvent.ResumeLayout(false);
            this.cmnuHudElement.ResumeLayout(false);
            this.cmnuState.ResumeLayout(false);
            this.cmnuScript.ResumeLayout(false);
            this.cmnuProperty.ResumeLayout(false);
            this.cmnuScriptRoot.ResumeLayout(false);
            this.cmnuStateRoot.ResumeLayout(false);
            this.cmnuPropertyRoot.ResumeLayout(false);
            this.cmnuAnimationSlot.ResumeLayout(false);
            this.cmnuAnimationSlotRoot.ResumeLayout(false);
            this.cmnuHitbox.ResumeLayout(false);
            this.cmnuHitboxRoot.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer scEntities;
        private System.Windows.Forms.SplitContainer scEntitiesList;
        private System.Windows.Forms.TreeView tvEntities;
        private System.Windows.Forms.PropertyGrid pgProperties;
        private System.Windows.Forms.ContextMenuStrip cmnuActor;
        private System.Windows.Forms.ToolStripMenuItem deleteAactorToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuEvent;
        private System.Windows.Forms.ToolStripMenuItem deleteEventToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuHudElement;
        private System.Windows.Forms.ToolStripMenuItem deleteHudElementToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuState;
        private System.Windows.Forms.ToolStripMenuItem deleteStateToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuScript;
        private System.Windows.Forms.ToolStripMenuItem viewEditScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteScriptToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuProperty;
        private System.Windows.Forms.ToolStripMenuItem deletePropertyToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuScriptRoot;
        private System.Windows.Forms.ToolStripMenuItem addScriptToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuStateRoot;
        private System.Windows.Forms.ToolStripMenuItem addStateToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuPropertyRoot;
        private System.Windows.Forms.ToolStripMenuItem addPropertyToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuAnimationSlot;
        private System.Windows.Forms.ToolStripMenuItem deleteAnimationSlotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveDownToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuAnimationSlotRoot;
        private System.Windows.Forms.ToolStripMenuItem addAnimationSlotToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuHitbox;
        private System.Windows.Forms.ToolStripMenuItem deleteHitboxToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuHitboxRoot;
        private System.Windows.Forms.ToolStripMenuItem addHitboxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyHitboxesFromStateToolStripMenuItem;
    }
}
