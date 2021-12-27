namespace FiremelonEditor2
{
    partial class GameButtonsEditorControl
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
            this.tvGameButtons = new System.Windows.Forms.TreeView();
            this.pgGameButtons = new System.Windows.Forms.PropertyGrid();
            this.scGameButtons = new System.Windows.Forms.SplitContainer();
            this.cmnuGameButtonGroup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteGameButtonGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuGameButtonGroupRoot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addGameButtonGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuGameButton = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteGameButtonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuGameButtonRoot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addGameButtonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.scGameButtonsList)).BeginInit();
            this.scGameButtonsList.Panel1.SuspendLayout();
            this.scGameButtonsList.Panel2.SuspendLayout();
            this.scGameButtonsList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scGameButtons)).BeginInit();
            this.scGameButtons.Panel1.SuspendLayout();
            this.scGameButtons.SuspendLayout();
            this.cmnuGameButtonGroup.SuspendLayout();
            this.cmnuGameButtonGroupRoot.SuspendLayout();
            this.cmnuGameButton.SuspendLayout();
            this.cmnuGameButtonRoot.SuspendLayout();
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
            this.scGameButtonsList.Panel1.Controls.Add(this.tvGameButtons);
            // 
            // scGameButtonsList.Panel2
            // 
            this.scGameButtonsList.Panel2.Controls.Add(this.pgGameButtons);
            this.scGameButtonsList.Size = new System.Drawing.Size(280, 520);
            this.scGameButtonsList.SplitterDistance = 246;
            this.scGameButtonsList.TabIndex = 1;
            // 
            // tvGameButtons
            // 
            this.tvGameButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tvGameButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvGameButtons.Location = new System.Drawing.Point(0, 0);
            this.tvGameButtons.Name = "tvGameButtons";
            this.tvGameButtons.Size = new System.Drawing.Size(280, 246);
            this.tvGameButtons.TabIndex = 0;
            this.tvGameButtons.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvGameButtons_AfterSelect);
            // 
            // pgGameButtons
            // 
            this.pgGameButtons.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pgGameButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgGameButtons.Location = new System.Drawing.Point(0, 0);
            this.pgGameButtons.Name = "pgGameButtons";
            this.pgGameButtons.Size = new System.Drawing.Size(280, 270);
            this.pgGameButtons.TabIndex = 0;
            this.pgGameButtons.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgGameButtons_PropertyValueChanged);
            // 
            // scGameButtons
            // 
            this.scGameButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scGameButtons.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scGameButtons.Location = new System.Drawing.Point(0, 0);
            this.scGameButtons.Name = "scGameButtons";
            // 
            // scGameButtons.Panel1
            // 
            this.scGameButtons.Panel1.Controls.Add(this.scGameButtonsList);
            this.scGameButtons.Size = new System.Drawing.Size(771, 520);
            this.scGameButtons.SplitterDistance = 280;
            this.scGameButtons.TabIndex = 5;
            // 
            // cmnuGameButtonGroup
            // 
            this.cmnuGameButtonGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteGameButtonGroupToolStripMenuItem});
            this.cmnuGameButtonGroup.Name = "cmnuTileSheetRoot";
            this.cmnuGameButtonGroup.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteGameButtonGroupToolStripMenuItem
            // 
            this.deleteGameButtonGroupToolStripMenuItem.Name = "deleteGameButtonGroupToolStripMenuItem";
            this.deleteGameButtonGroupToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteGameButtonGroupToolStripMenuItem.Text = "Delete";
            // 
            // cmnuGameButtonGroupRoot
            // 
            this.cmnuGameButtonGroupRoot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addGameButtonGroupToolStripMenuItem});
            this.cmnuGameButtonGroupRoot.Name = "contextMenuStrip3";
            this.cmnuGameButtonGroupRoot.Size = new System.Drawing.Size(172, 26);
            // 
            // addGameButtonGroupToolStripMenuItem
            // 
            this.addGameButtonGroupToolStripMenuItem.Name = "addGameButtonGroupToolStripMenuItem";
            this.addGameButtonGroupToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.addGameButtonGroupToolStripMenuItem.Text = "Add Button Group";
            // 
            // cmnuGameButton
            // 
            this.cmnuGameButton.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteGameButtonToolStripMenuItem});
            this.cmnuGameButton.Name = "cmnuTileSheetRoot";
            this.cmnuGameButton.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteGameButtonToolStripMenuItem
            // 
            this.deleteGameButtonToolStripMenuItem.Name = "deleteGameButtonToolStripMenuItem";
            this.deleteGameButtonToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteGameButtonToolStripMenuItem.Text = "Delete";
            // 
            // cmnuGameButtonRoot
            // 
            this.cmnuGameButtonRoot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addGameButtonToolStripMenuItem});
            this.cmnuGameButtonRoot.Name = "contextMenuStrip3";
            this.cmnuGameButtonRoot.Size = new System.Drawing.Size(170, 26);
            // 
            // addGameButtonToolStripMenuItem
            // 
            this.addGameButtonToolStripMenuItem.Name = "addGameButtonToolStripMenuItem";
            this.addGameButtonToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.addGameButtonToolStripMenuItem.Text = "Add Game Button";
            // 
            // GameButtonsEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scGameButtons);
            this.Name = "GameButtonsEditorControl";
            this.Size = new System.Drawing.Size(771, 520);
            this.scGameButtonsList.Panel1.ResumeLayout(false);
            this.scGameButtonsList.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scGameButtonsList)).EndInit();
            this.scGameButtonsList.ResumeLayout(false);
            this.scGameButtons.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scGameButtons)).EndInit();
            this.scGameButtons.ResumeLayout(false);
            this.cmnuGameButtonGroup.ResumeLayout(false);
            this.cmnuGameButtonGroupRoot.ResumeLayout(false);
            this.cmnuGameButton.ResumeLayout(false);
            this.cmnuGameButtonRoot.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer scGameButtonsList;
        private System.Windows.Forms.TreeView tvGameButtons;
        private System.Windows.Forms.PropertyGrid pgGameButtons;
        private System.Windows.Forms.SplitContainer scGameButtons;
        private System.Windows.Forms.ContextMenuStrip cmnuGameButtonGroup;
        private System.Windows.Forms.ToolStripMenuItem deleteGameButtonGroupToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuGameButtonGroupRoot;
        private System.Windows.Forms.ToolStripMenuItem addGameButtonGroupToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuGameButton;
        private System.Windows.Forms.ToolStripMenuItem deleteGameButtonToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuGameButtonRoot;
        private System.Windows.Forms.ToolStripMenuItem addGameButtonToolStripMenuItem;
    }
}
