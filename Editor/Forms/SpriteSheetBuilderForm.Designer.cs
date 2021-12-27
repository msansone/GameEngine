namespace FiremelonEditor2
{
    partial class SpriteSheetBuilderForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpriteSheetBuilderForm));
            this.lstbxFiles = new System.Windows.Forms.ListBox();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.scImages = new System.Windows.Forms.SplitContainer();
            this.scFilesProperties = new System.Windows.Forms.SplitContainer();
            this.scExportImport = new System.Windows.Forms.SplitContainer();
            this.scExport = new System.Windows.Forms.SplitContainer();
            this.lbExport = new System.Windows.Forms.Label();
            this.pgSpriteSheet = new System.Windows.Forms.PropertyGrid();
            this.scImport = new System.Windows.Forms.SplitContainer();
            this.lbImportFrom = new System.Windows.Forms.Label();
            this.pgImageSource = new System.Windows.Forms.PropertyGrid();
            this.vsSpriteSheet = new System.Windows.Forms.VScrollBar();
            this.hsSpriteSheet = new System.Windows.Forms.HScrollBar();
            this.pbSheetPreview = new System.Windows.Forms.PictureBox();
            this.tsToolbar = new System.Windows.Forms.ToolStrip();
            this.tsbAddImages = new System.Windows.Forms.ToolStripButton();
            this.tsbExportSpriteSheet = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbMoveImageUp = new System.Windows.Forms.ToolStripButton();
            this.tsbMoveImageDown = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ofdAddImages = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
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
            this.fromSheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveSelectionUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveSelectionDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.previewSpriteSheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tlpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scImages)).BeginInit();
            this.scImages.Panel1.SuspendLayout();
            this.scImages.Panel2.SuspendLayout();
            this.scImages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scFilesProperties)).BeginInit();
            this.scFilesProperties.Panel1.SuspendLayout();
            this.scFilesProperties.Panel2.SuspendLayout();
            this.scFilesProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scExportImport)).BeginInit();
            this.scExportImport.Panel1.SuspendLayout();
            this.scExportImport.Panel2.SuspendLayout();
            this.scExportImport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scExport)).BeginInit();
            this.scExport.Panel1.SuspendLayout();
            this.scExport.Panel2.SuspendLayout();
            this.scExport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scImport)).BeginInit();
            this.scImport.Panel1.SuspendLayout();
            this.scImport.Panel2.SuspendLayout();
            this.scImport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSheetPreview)).BeginInit();
            this.tsToolbar.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstbxFiles
            // 
            this.lstbxFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstbxFiles.FormattingEnabled = true;
            this.lstbxFiles.IntegralHeight = false;
            this.lstbxFiles.Location = new System.Drawing.Point(0, 0);
            this.lstbxFiles.Margin = new System.Windows.Forms.Padding(0);
            this.lstbxFiles.Name = "lstbxFiles";
            this.lstbxFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstbxFiles.Size = new System.Drawing.Size(333, 326);
            this.lstbxFiles.TabIndex = 1;
            this.lstbxFiles.SelectedIndexChanged += new System.EventHandler(this.lstbxFiles_SelectedIndexChanged);
            this.lstbxFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstbxFiles_KeyDown);
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.scImages, 0, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 24);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 1;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 619F));
            this.tlpMain.Size = new System.Drawing.Size(1057, 619);
            this.tlpMain.TabIndex = 2;
            // 
            // scImages
            // 
            this.scImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scImages.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scImages.Location = new System.Drawing.Point(3, 3);
            this.scImages.Name = "scImages";
            // 
            // scImages.Panel1
            // 
            this.scImages.Panel1.Controls.Add(this.scFilesProperties);
            // 
            // scImages.Panel2
            // 
            this.scImages.Panel2.Controls.Add(this.vsSpriteSheet);
            this.scImages.Panel2.Controls.Add(this.hsSpriteSheet);
            this.scImages.Panel2.Controls.Add(this.pbSheetPreview);
            this.scImages.Size = new System.Drawing.Size(1051, 613);
            this.scImages.SplitterDistance = 333;
            this.scImages.TabIndex = 3;
            this.scImages.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.scImages_SplitterMoved);
            // 
            // scFilesProperties
            // 
            this.scFilesProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scFilesProperties.Location = new System.Drawing.Point(0, 0);
            this.scFilesProperties.Name = "scFilesProperties";
            this.scFilesProperties.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scFilesProperties.Panel1
            // 
            this.scFilesProperties.Panel1.Controls.Add(this.scExportImport);
            // 
            // scFilesProperties.Panel2
            // 
            this.scFilesProperties.Panel2.Controls.Add(this.pgImageSource);
            this.scFilesProperties.Size = new System.Drawing.Size(333, 613);
            this.scFilesProperties.SplitterDistance = 500;
            this.scFilesProperties.TabIndex = 0;
            // 
            // scExportImport
            // 
            this.scExportImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scExportImport.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scExportImport.IsSplitterFixed = true;
            this.scExportImport.Location = new System.Drawing.Point(0, 0);
            this.scExportImport.Name = "scExportImport";
            this.scExportImport.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scExportImport.Panel1
            // 
            this.scExportImport.Panel1.Controls.Add(this.scExport);
            // 
            // scExportImport.Panel2
            // 
            this.scExportImport.Panel2.Controls.Add(this.scImport);
            this.scExportImport.Size = new System.Drawing.Size(333, 500);
            this.scExportImport.SplitterDistance = 144;
            this.scExportImport.TabIndex = 2;
            // 
            // scExport
            // 
            this.scExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scExport.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scExport.IsSplitterFixed = true;
            this.scExport.Location = new System.Drawing.Point(0, 0);
            this.scExport.Name = "scExport";
            this.scExport.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scExport.Panel1
            // 
            this.scExport.Panel1.Controls.Add(this.lbExport);
            // 
            // scExport.Panel2
            // 
            this.scExport.Panel2.Controls.Add(this.pgSpriteSheet);
            this.scExport.Size = new System.Drawing.Size(333, 144);
            this.scExport.SplitterDistance = 25;
            this.scExport.SplitterWidth = 1;
            this.scExport.TabIndex = 0;
            // 
            // lbExport
            // 
            this.lbExport.BackColor = System.Drawing.Color.Transparent;
            this.lbExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbExport.Location = new System.Drawing.Point(0, 0);
            this.lbExport.Name = "lbExport";
            this.lbExport.Size = new System.Drawing.Size(333, 25);
            this.lbExport.TabIndex = 0;
            this.lbExport.Text = "Export Parameters";
            this.lbExport.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // pgSpriteSheet
            // 
            this.pgSpriteSheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgSpriteSheet.Enabled = false;
            this.pgSpriteSheet.HelpVisible = false;
            this.pgSpriteSheet.Location = new System.Drawing.Point(0, 0);
            this.pgSpriteSheet.Name = "pgSpriteSheet";
            this.pgSpriteSheet.Size = new System.Drawing.Size(333, 118);
            this.pgSpriteSheet.TabIndex = 0;
            this.pgSpriteSheet.ToolbarVisible = false;
            // 
            // scImport
            // 
            this.scImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scImport.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scImport.IsSplitterFixed = true;
            this.scImport.Location = new System.Drawing.Point(0, 0);
            this.scImport.Name = "scImport";
            this.scImport.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scImport.Panel1
            // 
            this.scImport.Panel1.Controls.Add(this.lbImportFrom);
            // 
            // scImport.Panel2
            // 
            this.scImport.Panel2.Controls.Add(this.lstbxFiles);
            this.scImport.Size = new System.Drawing.Size(333, 352);
            this.scImport.SplitterDistance = 25;
            this.scImport.SplitterWidth = 1;
            this.scImport.TabIndex = 2;
            // 
            // lbImportFrom
            // 
            this.lbImportFrom.BackColor = System.Drawing.Color.Transparent;
            this.lbImportFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbImportFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbImportFrom.Location = new System.Drawing.Point(0, 0);
            this.lbImportFrom.Name = "lbImportFrom";
            this.lbImportFrom.Size = new System.Drawing.Size(333, 25);
            this.lbImportFrom.TabIndex = 0;
            this.lbImportFrom.Text = "Import From";
            this.lbImportFrom.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // pgImageSource
            // 
            this.pgImageSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgImageSource.HelpVisible = false;
            this.pgImageSource.Location = new System.Drawing.Point(0, 0);
            this.pgImageSource.Name = "pgImageSource";
            this.pgImageSource.Size = new System.Drawing.Size(333, 109);
            this.pgImageSource.TabIndex = 1;
            this.pgImageSource.ToolbarVisible = false;
            // 
            // vsSpriteSheet
            // 
            this.vsSpriteSheet.Location = new System.Drawing.Point(318, 0);
            this.vsSpriteSheet.Name = "vsSpriteSheet";
            this.vsSpriteSheet.Size = new System.Drawing.Size(16, 284);
            this.vsSpriteSheet.TabIndex = 2;
            this.vsSpriteSheet.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vsSpriteSheet_Scroll);
            // 
            // hsSpriteSheet
            // 
            this.hsSpriteSheet.Location = new System.Drawing.Point(0, 284);
            this.hsSpriteSheet.Name = "hsSpriteSheet";
            this.hsSpriteSheet.Size = new System.Drawing.Size(318, 16);
            this.hsSpriteSheet.TabIndex = 1;
            this.hsSpriteSheet.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsSpriteSheet_Scroll);
            // 
            // pbSheetPreview
            // 
            this.pbSheetPreview.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pbSheetPreview.Location = new System.Drawing.Point(0, 0);
            this.pbSheetPreview.Margin = new System.Windows.Forms.Padding(0);
            this.pbSheetPreview.Name = "pbSheetPreview";
            this.pbSheetPreview.Size = new System.Drawing.Size(318, 284);
            this.pbSheetPreview.TabIndex = 0;
            this.pbSheetPreview.TabStop = false;
            this.pbSheetPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.pbSheetPreview_Paint);
            // 
            // tsToolbar
            // 
            this.tsToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddImages,
            this.tsbExportSpriteSheet,
            this.toolStripSeparator1,
            this.tsbMoveImageUp,
            this.tsbMoveImageDown,
            this.toolStripSeparator2});
            this.tsToolbar.Location = new System.Drawing.Point(0, 24);
            this.tsToolbar.Name = "tsToolbar";
            this.tsToolbar.Size = new System.Drawing.Size(1057, 25);
            this.tsToolbar.TabIndex = 4;
            this.tsToolbar.Text = "toolStrip1";
            this.tsToolbar.Visible = false;
            // 
            // tsbAddImages
            // 
            this.tsbAddImages.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddImages.Enabled = false;
            this.tsbAddImages.Image = ((System.Drawing.Image)(resources.GetObject("tsbAddImages.Image")));
            this.tsbAddImages.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddImages.Name = "tsbAddImages";
            this.tsbAddImages.Size = new System.Drawing.Size(23, 22);
            this.tsbAddImages.Text = "Add Images";
            // 
            // tsbExportSpriteSheet
            // 
            this.tsbExportSpriteSheet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbExportSpriteSheet.Enabled = false;
            this.tsbExportSpriteSheet.Image = ((System.Drawing.Image)(resources.GetObject("tsbExportSpriteSheet.Image")));
            this.tsbExportSpriteSheet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExportSpriteSheet.Name = "tsbExportSpriteSheet";
            this.tsbExportSpriteSheet.Size = new System.Drawing.Size(23, 22);
            this.tsbExportSpriteSheet.Text = "Export";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbMoveImageUp
            // 
            this.tsbMoveImageUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbMoveImageUp.Enabled = false;
            this.tsbMoveImageUp.Image = ((System.Drawing.Image)(resources.GetObject("tsbMoveImageUp.Image")));
            this.tsbMoveImageUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMoveImageUp.Name = "tsbMoveImageUp";
            this.tsbMoveImageUp.Size = new System.Drawing.Size(23, 22);
            this.tsbMoveImageUp.Text = "toolStripButton1";
            // 
            // tsbMoveImageDown
            // 
            this.tsbMoveImageDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbMoveImageDown.Enabled = false;
            this.tsbMoveImageDown.Image = ((System.Drawing.Image)(resources.GetObject("tsbMoveImageDown.Image")));
            this.tsbMoveImageDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMoveImageDown.Name = "tsbMoveImageDown";
            this.tsbMoveImageDown.Size = new System.Drawing.Size(23, 22);
            this.tsbMoveImageDown.Text = "toolStripButton2";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1057, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newSpriteSheetToolStripMenuItem,
            this.saveSpriteSheetBuildFileToolStripMenuItem,
            this.openSpriteSheetBuildFileToolStripMenuItem,
            this.buildSpriteSheetToolStripMenuItem,
            this.buildAlphaMaskSheetToolStripMenuItem,
            this.exportSheetToolStripMenuItem,
            this.addImagesToolStripMenuItem});
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
            this.fromSheetToolStripMenuItem});
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
            // fromSheetToolStripMenuItem
            // 
            this.fromSheetToolStripMenuItem.Enabled = false;
            this.fromSheetToolStripMenuItem.Name = "fromSheetToolStripMenuItem";
            this.fromSheetToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.fromSheetToolStripMenuItem.Text = "From Sheet";
            this.fromSheetToolStripMenuItem.Click += new System.EventHandler(this.fromSheetToolStripMenuItem_Click);
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
            // SpriteSheetBuilderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 643);
            this.Controls.Add(this.tlpMain);
            this.Controls.Add(this.tsToolbar);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SpriteSheetBuilderForm";
            this.Text = "SpriteSheetBuilder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpriteSheetBuilderForm_FormClosing);
            this.Load += new System.EventHandler(this.SpriteSheetBuilderForm_Load);
            this.Resize += new System.EventHandler(this.SpriteSheetBuilderForm_Resize);
            this.tlpMain.ResumeLayout(false);
            this.scImages.Panel1.ResumeLayout(false);
            this.scImages.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scImages)).EndInit();
            this.scImages.ResumeLayout(false);
            this.scFilesProperties.Panel1.ResumeLayout(false);
            this.scFilesProperties.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scFilesProperties)).EndInit();
            this.scFilesProperties.ResumeLayout(false);
            this.scExportImport.Panel1.ResumeLayout(false);
            this.scExportImport.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scExportImport)).EndInit();
            this.scExportImport.ResumeLayout(false);
            this.scExport.Panel1.ResumeLayout(false);
            this.scExport.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scExport)).EndInit();
            this.scExport.ResumeLayout(false);
            this.scImport.Panel1.ResumeLayout(false);
            this.scImport.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scImport)).EndInit();
            this.scImport.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbSheetPreview)).EndInit();
            this.tsToolbar.ResumeLayout(false);
            this.tsToolbar.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox lstbxFiles;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.SplitContainer scImages;
        private System.Windows.Forms.PictureBox pbSheetPreview;
        private System.Windows.Forms.ToolStrip tsToolbar;
        private System.Windows.Forms.ToolStripButton tsbAddImages;
        private System.Windows.Forms.ToolStripButton tsbExportSpriteSheet;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbMoveImageUp;
        private System.Windows.Forms.ToolStripButton tsbMoveImageDown;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.OpenFileDialog ofdAddImages;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newSpriteSheetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addImagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buildSpriteSheetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveSelectionUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveSelectionDownToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem previewSpriteSheetToolStripMenuItem;
        private System.Windows.Forms.SplitContainer scFilesProperties;
        private System.Windows.Forms.PropertyGrid pgSpriteSheet;
        private System.Windows.Forms.ToolStripMenuItem saveSpriteSheetBuildFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSpriteSheetBuildFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportSheetToolStripMenuItem;
        private System.Windows.Forms.HScrollBar hsSpriteSheet;
        private System.Windows.Forms.VScrollBar vsSpriteSheet;
        private System.Windows.Forms.ToolStripMenuItem buildAlphaMaskSheetToolStripMenuItem;
        private System.Windows.Forms.SplitContainer scExportImport;
        private System.Windows.Forms.SplitContainer scExport;
        private System.Windows.Forms.Label lbExport;
        private System.Windows.Forms.PropertyGrid pgImageSource;
        private System.Windows.Forms.SplitContainer scImport;
        private System.Windows.Forms.Label lbImportFrom;
        private System.Windows.Forms.ToolStripMenuItem singleImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stripToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromSheetToolStripMenuItem;
    }
}