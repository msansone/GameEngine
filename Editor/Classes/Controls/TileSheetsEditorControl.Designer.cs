namespace FiremelonEditor2
{
    partial class TileSheetsEditorControl
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
            this.scTileSheets = new System.Windows.Forms.SplitContainer();
            this.scTileSheetList = new System.Windows.Forms.SplitContainer();
            this.tvTileSheets = new System.Windows.Forms.TreeView();
            this.pgTileSheet = new System.Windows.Forms.PropertyGrid();
            this.cmnuTileSheet = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewEditTileSheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.duplicateTilesheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteTileSheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuTileSheetRoot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addNewTileSheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuTileObject = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuSceneryAnimation = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsDeleteSceneryAnimation = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.scTileSheets)).BeginInit();
            this.scTileSheets.Panel1.SuspendLayout();
            this.scTileSheets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scTileSheetList)).BeginInit();
            this.scTileSheetList.Panel1.SuspendLayout();
            this.scTileSheetList.Panel2.SuspendLayout();
            this.scTileSheetList.SuspendLayout();
            this.cmnuTileSheet.SuspendLayout();
            this.cmnuTileSheetRoot.SuspendLayout();
            this.cmnuTileObject.SuspendLayout();
            this.cmnuSceneryAnimation.SuspendLayout();
            this.SuspendLayout();
            // 
            // scTileSheets
            // 
            this.scTileSheets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scTileSheets.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scTileSheets.Location = new System.Drawing.Point(0, 0);
            this.scTileSheets.Name = "scTileSheets";
            // 
            // scTileSheets.Panel1
            // 
            this.scTileSheets.Panel1.Controls.Add(this.scTileSheetList);
            this.scTileSheets.Size = new System.Drawing.Size(764, 543);
            this.scTileSheets.SplitterDistance = 280;
            this.scTileSheets.TabIndex = 23;
            // 
            // scTileSheetList
            // 
            this.scTileSheetList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scTileSheetList.Location = new System.Drawing.Point(0, 0);
            this.scTileSheetList.Name = "scTileSheetList";
            this.scTileSheetList.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scTileSheetList.Panel1
            // 
            this.scTileSheetList.Panel1.Controls.Add(this.tvTileSheets);
            // 
            // scTileSheetList.Panel2
            // 
            this.scTileSheetList.Panel2.Controls.Add(this.pgTileSheet);
            this.scTileSheetList.Size = new System.Drawing.Size(280, 543);
            this.scTileSheetList.SplitterDistance = 249;
            this.scTileSheetList.TabIndex = 0;
            // 
            // tvTileSheets
            // 
            this.tvTileSheets.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tvTileSheets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvTileSheets.Location = new System.Drawing.Point(0, 0);
            this.tvTileSheets.Name = "tvTileSheets";
            this.tvTileSheets.Size = new System.Drawing.Size(280, 249);
            this.tvTileSheets.TabIndex = 1;
            this.tvTileSheets.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvTileSheets_AfterSelect);
            // 
            // pgTileSheet
            // 
            this.pgTileSheet.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pgTileSheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgTileSheet.Location = new System.Drawing.Point(0, 0);
            this.pgTileSheet.Name = "pgTileSheet";
            this.pgTileSheet.Size = new System.Drawing.Size(280, 290);
            this.pgTileSheet.TabIndex = 1;
            this.pgTileSheet.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgTileSheet_PropertyValueChanged);
            // 
            // cmnuTileSheet
            // 
            this.cmnuTileSheet.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewEditTileSheetToolStripMenuItem,
            this.duplicateTilesheetToolStripMenuItem,
            this.deleteTileSheetToolStripMenuItem});
            this.cmnuTileSheet.Name = "cmnuTileSheetRoot";
            this.cmnuTileSheet.Size = new System.Drawing.Size(125, 70);
            // 
            // viewEditTileSheetToolStripMenuItem
            // 
            this.viewEditTileSheetToolStripMenuItem.Name = "viewEditTileSheetToolStripMenuItem";
            this.viewEditTileSheetToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.viewEditTileSheetToolStripMenuItem.Text = "View/Edit";
            // 
            // duplicateTilesheetToolStripMenuItem
            // 
            this.duplicateTilesheetToolStripMenuItem.Name = "duplicateTilesheetToolStripMenuItem";
            this.duplicateTilesheetToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.duplicateTilesheetToolStripMenuItem.Text = "Duplicate";
            // 
            // deleteTileSheetToolStripMenuItem
            // 
            this.deleteTileSheetToolStripMenuItem.Name = "deleteTileSheetToolStripMenuItem";
            this.deleteTileSheetToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.deleteTileSheetToolStripMenuItem.Text = "Delete";
            // 
            // cmnuTileSheetRoot
            // 
            this.cmnuTileSheetRoot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewTileSheetToolStripMenuItem});
            this.cmnuTileSheetRoot.Name = "cmnuTileSheetRoot";
            this.cmnuTileSheetRoot.Size = new System.Drawing.Size(151, 26);
            // 
            // addNewTileSheetToolStripMenuItem
            // 
            this.addNewTileSheetToolStripMenuItem.Name = "addNewTileSheetToolStripMenuItem";
            this.addNewTileSheetToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.addNewTileSheetToolStripMenuItem.Text = "Add Tile Sheet";
            // 
            // cmnuTileObject
            // 
            this.cmnuTileObject.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem3});
            this.cmnuTileObject.Name = "cmnuTileSheetRoot";
            this.cmnuTileObject.Size = new System.Drawing.Size(125, 48);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem1.Text = "View/Edit";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem3.Text = "Delete";
            // 
            // cmnuSceneryAnimation
            // 
            this.cmnuSceneryAnimation.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.tsDeleteSceneryAnimation});
            this.cmnuSceneryAnimation.Name = "cmnuTileSheetRoot";
            this.cmnuSceneryAnimation.Size = new System.Drawing.Size(125, 48);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem2.Text = "View/Edit";
            // 
            // tsDeleteSceneryAnimation
            // 
            this.tsDeleteSceneryAnimation.Name = "tsDeleteSceneryAnimation";
            this.tsDeleteSceneryAnimation.Size = new System.Drawing.Size(124, 22);
            this.tsDeleteSceneryAnimation.Text = "Delete";
            // 
            // TileSheetsEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scTileSheets);
            this.Name = "TileSheetsEditorControl";
            this.Size = new System.Drawing.Size(764, 543);
            this.Load += new System.EventHandler(this.TileSheetsEditorControl_Load);
            this.scTileSheets.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scTileSheets)).EndInit();
            this.scTileSheets.ResumeLayout(false);
            this.scTileSheetList.Panel1.ResumeLayout(false);
            this.scTileSheetList.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scTileSheetList)).EndInit();
            this.scTileSheetList.ResumeLayout(false);
            this.cmnuTileSheet.ResumeLayout(false);
            this.cmnuTileSheetRoot.ResumeLayout(false);
            this.cmnuTileObject.ResumeLayout(false);
            this.cmnuSceneryAnimation.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer scTileSheets;
        private System.Windows.Forms.SplitContainer scTileSheetList;
        private System.Windows.Forms.PropertyGrid pgTileSheet;
        private System.Windows.Forms.TreeView tvTileSheets;
        private System.Windows.Forms.ContextMenuStrip cmnuTileSheet;
        private System.Windows.Forms.ToolStripMenuItem viewEditTileSheetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem duplicateTilesheetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteTileSheetToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuTileSheetRoot;
        private System.Windows.Forms.ToolStripMenuItem addNewTileSheetToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuTileObject;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ContextMenuStrip cmnuSceneryAnimation;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem tsDeleteSceneryAnimation;
    }
}
