namespace FiremelonEditor2
{
    partial class UiEditorControl
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
            this.tvUi = new System.Windows.Forms.TreeView();
            this.pgUi = new System.Windows.Forms.PropertyGrid();
            this.scintilla1 = new ScintillaNET.Scintilla();
            this.cmnuUiWidgetRoot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addMenuItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuScript = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewEditScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuDataFile = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewDataFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteDataFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuUiWidget = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteMenuItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.scTileSheets)).BeginInit();
            this.scTileSheets.Panel1.SuspendLayout();
            this.scTileSheets.Panel2.SuspendLayout();
            this.scTileSheets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scTileSheetList)).BeginInit();
            this.scTileSheetList.Panel1.SuspendLayout();
            this.scTileSheetList.Panel2.SuspendLayout();
            this.scTileSheetList.SuspendLayout();
            this.cmnuUiWidgetRoot.SuspendLayout();
            this.cmnuScript.SuspendLayout();
            this.cmnuDataFile.SuspendLayout();
            this.cmnuUiWidget.SuspendLayout();
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
            // 
            // scTileSheets.Panel2
            // 
            this.scTileSheets.Panel2.Controls.Add(this.scintilla1);
            this.scTileSheets.Size = new System.Drawing.Size(784, 510);
            this.scTileSheets.SplitterDistance = 280;
            this.scTileSheets.TabIndex = 24;
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
            this.scTileSheetList.Panel1.Controls.Add(this.tvUi);
            // 
            // scTileSheetList.Panel2
            // 
            this.scTileSheetList.Panel2.Controls.Add(this.pgUi);
            this.scTileSheetList.Size = new System.Drawing.Size(280, 510);
            this.scTileSheetList.SplitterDistance = 233;
            this.scTileSheetList.TabIndex = 0;
            // 
            // tvUi
            // 
            this.tvUi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tvUi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvUi.Location = new System.Drawing.Point(0, 0);
            this.tvUi.Name = "tvUi";
            this.tvUi.Size = new System.Drawing.Size(280, 233);
            this.tvUi.TabIndex = 1;
            this.tvUi.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvUi_AfterSelect);
            // 
            // pgUi
            // 
            this.pgUi.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pgUi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgUi.Location = new System.Drawing.Point(0, 0);
            this.pgUi.Name = "pgUi";
            this.pgUi.Size = new System.Drawing.Size(280, 273);
            this.pgUi.TabIndex = 1;
            // 
            // scintilla1
            // 
            this.scintilla1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla1.Location = new System.Drawing.Point(0, 0);
            this.scintilla1.Name = "scintilla1";
            this.scintilla1.Size = new System.Drawing.Size(500, 510);
            this.scintilla1.TabIndex = 2;
            // 
            // cmnuUiWidgetRoot
            // 
            this.cmnuUiWidgetRoot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addMenuItemToolStripMenuItem});
            this.cmnuUiWidgetRoot.Name = "cmnuUiWidgetRoot";
            this.cmnuUiWidgetRoot.Size = new System.Drawing.Size(138, 26);
            // 
            // addMenuItemToolStripMenuItem
            // 
            this.addMenuItemToolStripMenuItem.Name = "addMenuItemToolStripMenuItem";
            this.addMenuItemToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.addMenuItemToolStripMenuItem.Text = "Add Widget";
            // 
            // cmnuScript
            // 
            this.cmnuScript.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewEditScriptToolStripMenuItem,
            this.generateScriptToolStripMenuItem,
            this.deleteScriptToolStripMenuItem});
            this.cmnuScript.Name = "cmnuScript";
            this.cmnuScript.Size = new System.Drawing.Size(182, 70);
            // 
            // viewEditScriptToolStripMenuItem
            // 
            this.viewEditScriptToolStripMenuItem.Name = "viewEditScriptToolStripMenuItem";
            this.viewEditScriptToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.viewEditScriptToolStripMenuItem.Text = "View/Edit";
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
            // cmnuDataFile
            // 
            this.cmnuDataFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewDataFileToolStripMenuItem,
            this.deleteDataFileToolStripMenuItem});
            this.cmnuDataFile.Name = "cmnuDataFile";
            this.cmnuDataFile.Size = new System.Drawing.Size(129, 48);
            // 
            // viewDataFileToolStripMenuItem
            // 
            this.viewDataFileToolStripMenuItem.Name = "viewDataFileToolStripMenuItem";
            this.viewDataFileToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.viewDataFileToolStripMenuItem.Text = "View/Edit";
            // 
            // deleteDataFileToolStripMenuItem
            // 
            this.deleteDataFileToolStripMenuItem.Name = "deleteDataFileToolStripMenuItem";
            this.deleteDataFileToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.deleteDataFileToolStripMenuItem.Text = "Delete File";
            this.deleteDataFileToolStripMenuItem.Visible = false;
            // 
            // cmnuUiWidget
            // 
            this.cmnuUiWidget.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteMenuItemToolStripMenuItem});
            this.cmnuUiWidget.Name = "cmnuUiWidget";
            this.cmnuUiWidget.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteMenuItemToolStripMenuItem
            // 
            this.deleteMenuItemToolStripMenuItem.Name = "deleteMenuItemToolStripMenuItem";
            this.deleteMenuItemToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteMenuItemToolStripMenuItem.Text = "Delete";
            // 
            // UiEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scTileSheets);
            this.Name = "UiEditorControl";
            this.Size = new System.Drawing.Size(784, 510);
            this.scTileSheets.Panel1.ResumeLayout(false);
            this.scTileSheets.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scTileSheets)).EndInit();
            this.scTileSheets.ResumeLayout(false);
            this.scTileSheetList.Panel1.ResumeLayout(false);
            this.scTileSheetList.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scTileSheetList)).EndInit();
            this.scTileSheetList.ResumeLayout(false);
            this.cmnuUiWidgetRoot.ResumeLayout(false);
            this.cmnuScript.ResumeLayout(false);
            this.cmnuDataFile.ResumeLayout(false);
            this.cmnuUiWidget.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer scTileSheets;
        private System.Windows.Forms.SplitContainer scTileSheetList;
        private System.Windows.Forms.TreeView tvUi;
        private System.Windows.Forms.PropertyGrid pgUi;
        private ScintillaNET.Scintilla scintilla1;
        private System.Windows.Forms.ContextMenuStrip cmnuUiWidgetRoot;
        private System.Windows.Forms.ToolStripMenuItem addMenuItemToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuScript;
        private System.Windows.Forms.ToolStripMenuItem viewEditScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteScriptToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuDataFile;
        private System.Windows.Forms.ToolStripMenuItem viewDataFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteDataFileToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuUiWidget;
        private System.Windows.Forms.ToolStripMenuItem deleteMenuItemToolStripMenuItem;
    }
}
