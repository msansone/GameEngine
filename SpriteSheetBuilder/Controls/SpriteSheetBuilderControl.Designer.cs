namespace SpriteSheetBuilder
{
    partial class SpriteSheetBuilderControl
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
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.scImages = new System.Windows.Forms.SplitContainer();
            this.scFilesProperties = new System.Windows.Forms.SplitContainer();
            this.scExportImport = new System.Windows.Forms.SplitContainer();
            this.scExport = new System.Windows.Forms.SplitContainer();
            this.lbExport = new System.Windows.Forms.Label();
            this.pgSpriteSheet = new System.Windows.Forms.PropertyGrid();
            this.scImport = new System.Windows.Forms.SplitContainer();
            this.lbImportFrom = new System.Windows.Forms.Label();
            this.lstbxFiles = new System.Windows.Forms.ListBox();
            this.pgImageSource = new System.Windows.Forms.PropertyGrid();
            this.lblInitialized = new System.Windows.Forms.Label();
            this.vsSpriteSheet = new System.Windows.Forms.VScrollBar();
            this.hsSpriteSheet = new System.Windows.Forms.HScrollBar();
            this.pbSheetPreview = new System.Windows.Forms.PictureBox();
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
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.scImages, 0, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 1;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 622F));
            this.tlpMain.Size = new System.Drawing.Size(964, 519);
            this.tlpMain.TabIndex = 3;
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
            this.scImages.Panel2.Controls.Add(this.lblInitialized);
            this.scImages.Panel2.Controls.Add(this.vsSpriteSheet);
            this.scImages.Panel2.Controls.Add(this.hsSpriteSheet);
            this.scImages.Panel2.Controls.Add(this.pbSheetPreview);
            this.scImages.Size = new System.Drawing.Size(958, 513);
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
            this.scFilesProperties.Size = new System.Drawing.Size(333, 513);
            this.scFilesProperties.SplitterDistance = 418;
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
            this.scExportImport.Size = new System.Drawing.Size(333, 418);
            this.scExportImport.SplitterDistance = 192;
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
            this.scExport.Size = new System.Drawing.Size(333, 192);
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
            this.pgSpriteSheet.Size = new System.Drawing.Size(333, 166);
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
            this.scImport.Size = new System.Drawing.Size(333, 222);
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
            // lstbxFiles
            // 
            this.lstbxFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstbxFiles.FormattingEnabled = true;
            this.lstbxFiles.IntegralHeight = false;
            this.lstbxFiles.Location = new System.Drawing.Point(0, 0);
            this.lstbxFiles.Margin = new System.Windows.Forms.Padding(0);
            this.lstbxFiles.Name = "lstbxFiles";
            this.lstbxFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstbxFiles.Size = new System.Drawing.Size(333, 196);
            this.lstbxFiles.TabIndex = 1;
            this.lstbxFiles.SelectedIndexChanged += new System.EventHandler(this.lstbxFiles_SelectedIndexChanged);
            this.lstbxFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstbxFiles_KeyDown);
            // 
            // pgImageSource
            // 
            this.pgImageSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgImageSource.HelpVisible = false;
            this.pgImageSource.Location = new System.Drawing.Point(0, 0);
            this.pgImageSource.Name = "pgImageSource";
            this.pgImageSource.Size = new System.Drawing.Size(333, 91);
            this.pgImageSource.TabIndex = 1;
            this.pgImageSource.ToolbarVisible = false;
            // 
            // lblInitialized
            // 
            this.lblInitialized.BackColor = System.Drawing.Color.Black;
            this.lblInitialized.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInitialized.ForeColor = System.Drawing.Color.White;
            this.lblInitialized.Location = new System.Drawing.Point(0, 0);
            this.lblInitialized.Name = "lblInitialized";
            this.lblInitialized.Size = new System.Drawing.Size(621, 513);
            this.lblInitialized.TabIndex = 3;
            this.lblInitialized.Text = "Sheet Not Built";
            this.lblInitialized.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.pbSheetPreview.BackColor = System.Drawing.Color.Black;
            this.pbSheetPreview.Location = new System.Drawing.Point(0, 0);
            this.pbSheetPreview.Margin = new System.Windows.Forms.Padding(0);
            this.pbSheetPreview.Name = "pbSheetPreview";
            this.pbSheetPreview.Size = new System.Drawing.Size(318, 284);
            this.pbSheetPreview.TabIndex = 0;
            this.pbSheetPreview.TabStop = false;
            this.pbSheetPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.pbSheetPreview_Paint);
            // 
            // SpriteSheetBuilderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.tlpMain);
            this.Name = "SpriteSheetBuilderControl";
            this.Size = new System.Drawing.Size(964, 519);
            this.Load += new System.EventHandler(this.SpriteSheetBuilderControl_Load);
            this.Resize += new System.EventHandler(this.SpriteSheetBuilderControl_Resize);
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.SplitContainer scImages;
        private System.Windows.Forms.SplitContainer scFilesProperties;
        private System.Windows.Forms.SplitContainer scExportImport;
        private System.Windows.Forms.SplitContainer scExport;
        private System.Windows.Forms.Label lbExport;
        private System.Windows.Forms.PropertyGrid pgSpriteSheet;
        private System.Windows.Forms.SplitContainer scImport;
        private System.Windows.Forms.Label lbImportFrom;
        private System.Windows.Forms.ListBox lstbxFiles;
        private System.Windows.Forms.PropertyGrid pgImageSource;
        private System.Windows.Forms.VScrollBar vsSpriteSheet;
        private System.Windows.Forms.HScrollBar hsSpriteSheet;
        private System.Windows.Forms.PictureBox pbSheetPreview;
        private System.Windows.Forms.Label lblInitialized;
    }
}
