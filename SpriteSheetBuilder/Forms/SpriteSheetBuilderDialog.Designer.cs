namespace SpriteSheetBuilder
{
    partial class SpriteSheetBuilderDialog
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
            this.ofdAddImages = new System.Windows.Forms.OpenFileDialog();
            this.msMainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newSpriteSheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSpriteSheetBuildFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openSpriteSheetBuildFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buildSpriteSheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buildAlphaMaskSheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportSheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addImagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.singleImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stripToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveSelectionUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveSelectionDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.previewSpriteSheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.msMainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // msMainMenu
            // 
            this.msMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.msMainMenu.Location = new System.Drawing.Point(0, 0);
            this.msMainMenu.Name = "msMainMenu";
            this.msMainMenu.Size = new System.Drawing.Size(944, 24);
            this.msMainMenu.TabIndex = 4;
            this.msMainMenu.Text = "Main Menu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newSpriteSheetToolStripMenuItem,
            this.saveSpriteSheetBuildFileToolStripMenuItem,
            this.openSpriteSheetBuildFileToolStripMenuItem,
            this.addImagesToolStripMenuItem,
            this.buildSpriteSheetToolStripMenuItem,
            this.buildAlphaMaskSheetToolStripMenuItem,
            this.exportSheetToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newSpriteSheetToolStripMenuItem
            // 
            this.newSpriteSheetToolStripMenuItem.Name = "newSpriteSheetToolStripMenuItem";
            this.newSpriteSheetToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.newSpriteSheetToolStripMenuItem.Text = "New Sprite Sheet";
            this.newSpriteSheetToolStripMenuItem.Click += new System.EventHandler(this.newSpriteSheetToolStripMenuItem_Click);
            // 
            // saveSpriteSheetBuildFileToolStripMenuItem
            // 
            this.saveSpriteSheetBuildFileToolStripMenuItem.Enabled = false;
            this.saveSpriteSheetBuildFileToolStripMenuItem.Name = "saveSpriteSheetBuildFileToolStripMenuItem";
            this.saveSpriteSheetBuildFileToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.saveSpriteSheetBuildFileToolStripMenuItem.Text = "Save Sprite Sheet Build File";
            this.saveSpriteSheetBuildFileToolStripMenuItem.Click += new System.EventHandler(this.saveSpriteSheetBuildFileToolStripMenuItem_Click);
            // 
            // openSpriteSheetBuildFileToolStripMenuItem
            // 
            this.openSpriteSheetBuildFileToolStripMenuItem.Name = "openSpriteSheetBuildFileToolStripMenuItem";
            this.openSpriteSheetBuildFileToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.openSpriteSheetBuildFileToolStripMenuItem.Text = "Open Sprite Sheet Build File";
            this.openSpriteSheetBuildFileToolStripMenuItem.Click += new System.EventHandler(this.openSpriteSheetBuildFileToolStripMenuItem_Click);
            // 
            // buildSpriteSheetToolStripMenuItem
            // 
            this.buildSpriteSheetToolStripMenuItem.Enabled = false;
            this.buildSpriteSheetToolStripMenuItem.Name = "buildSpriteSheetToolStripMenuItem";
            this.buildSpriteSheetToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.buildSpriteSheetToolStripMenuItem.Text = "Build Sprite Sheet";
            this.buildSpriteSheetToolStripMenuItem.Click += new System.EventHandler(this.buildSpriteSheetToolStripMenuItem_Click);
            // 
            // buildAlphaMaskSheetToolStripMenuItem
            // 
            this.buildAlphaMaskSheetToolStripMenuItem.Enabled = false;
            this.buildAlphaMaskSheetToolStripMenuItem.Name = "buildAlphaMaskSheetToolStripMenuItem";
            this.buildAlphaMaskSheetToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.buildAlphaMaskSheetToolStripMenuItem.Text = "Build Alpha Mask Sheet";
            this.buildAlphaMaskSheetToolStripMenuItem.Click += new System.EventHandler(this.buildAlphaMaskSheetToolStripMenuItem_Click);
            // 
            // exportSheetToolStripMenuItem
            // 
            this.exportSheetToolStripMenuItem.Enabled = false;
            this.exportSheetToolStripMenuItem.Name = "exportSheetToolStripMenuItem";
            this.exportSheetToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.exportSheetToolStripMenuItem.Text = "Export Sheet";
            this.exportSheetToolStripMenuItem.Click += new System.EventHandler(this.exportSheetToolStripMenuItem_Click);
            // 
            // addImagesToolStripMenuItem
            // 
            this.addImagesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.singleImageToolStripMenuItem,
            this.stripToolStripMenuItem,
            this.sheetToolStripMenuItem});
            this.addImagesToolStripMenuItem.Enabled = false;
            this.addImagesToolStripMenuItem.Name = "addImagesToolStripMenuItem";
            this.addImagesToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.addImagesToolStripMenuItem.Text = "Add Images";
            // 
            // singleImageToolStripMenuItem
            // 
            this.singleImageToolStripMenuItem.Enabled = false;
            this.singleImageToolStripMenuItem.Name = "singleImageToolStripMenuItem";
            this.singleImageToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.singleImageToolStripMenuItem.Text = "From Single Image";
            this.singleImageToolStripMenuItem.Click += new System.EventHandler(this.singleImageToolStripMenuItem_Click);
            // 
            // stripToolStripMenuItem
            // 
            this.stripToolStripMenuItem.Enabled = false;
            this.stripToolStripMenuItem.Name = "stripToolStripMenuItem";
            this.stripToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.stripToolStripMenuItem.Text = "From Strip";
            this.stripToolStripMenuItem.Click += new System.EventHandler(this.stripToolStripMenuItem_Click);
            // 
            // sheetToolStripMenuItem
            // 
            this.sheetToolStripMenuItem.Enabled = false;
            this.sheetToolStripMenuItem.Name = "sheetToolStripMenuItem";
            this.sheetToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.sheetToolStripMenuItem.Text = "From Sheet";
            this.sheetToolStripMenuItem.Click += new System.EventHandler(this.sheetToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveSelectionUpToolStripMenuItem,
            this.moveSelectionDownToolStripMenuItem,
            this.setToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Visible = false;
            // 
            // moveSelectionUpToolStripMenuItem
            // 
            this.moveSelectionUpToolStripMenuItem.Name = "moveSelectionUpToolStripMenuItem";
            this.moveSelectionUpToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.moveSelectionUpToolStripMenuItem.Text = "Move Selection Up";
            // 
            // moveSelectionDownToolStripMenuItem
            // 
            this.moveSelectionDownToolStripMenuItem.Name = "moveSelectionDownToolStripMenuItem";
            this.moveSelectionDownToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.moveSelectionDownToolStripMenuItem.Text = "Move Selection Down";
            // 
            // setToolStripMenuItem
            // 
            this.setToolStripMenuItem.Name = "setToolStripMenuItem";
            this.setToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.setToolStripMenuItem.Text = "Set";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.previewSpriteSheetToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            this.viewToolStripMenuItem.Visible = false;
            // 
            // previewSpriteSheetToolStripMenuItem
            // 
            this.previewSpriteSheetToolStripMenuItem.Name = "previewSpriteSheetToolStripMenuItem";
            this.previewSpriteSheetToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.previewSpriteSheetToolStripMenuItem.Text = "Preview Sprite Sheet";
            // 
            // SpriteSheetBuilderDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 604);
            this.Controls.Add(this.msMainMenu);
            this.MinimizeBox = false;
            this.Name = "SpriteSheetBuilderDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sprite Sheet Builder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpriteSheetBuilderDialog_FormClosing);
            this.Load += new System.EventHandler(this.SpriteSheetBuilderDialog_Load);
            this.msMainMenu.ResumeLayout(false);
            this.msMainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog ofdAddImages;
        private System.Windows.Forms.MenuStrip msMainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newSpriteSheetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSpriteSheetBuildFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSpriteSheetBuildFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buildSpriteSheetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buildAlphaMaskSheetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportSheetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addImagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem singleImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stripToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sheetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveSelectionUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveSelectionDownToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem previewSpriteSheetToolStripMenuItem;
    }
}