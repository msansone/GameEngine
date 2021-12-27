namespace FiremelonEditor2
{
    partial class TileSheetEditor
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pgTileSheet = new System.Windows.Forms.PropertyGrid();
            this.pnTileSheet = new System.Windows.Forms.Panel();
            this.hsTileSheet = new System.Windows.Forms.HScrollBar();
            this.pbTileSheet = new System.Windows.Forms.PictureBox();
            this.vsTileSheet = new System.Windows.Forms.VScrollBar();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.pnTileSheet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTileSheet)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pgTileSheet);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pnTileSheet);
            this.splitContainer1.Size = new System.Drawing.Size(589, 320);
            this.splitContainer1.SplitterDistance = 218;
            this.splitContainer1.TabIndex = 0;
            // 
            // pgTileSheet
            // 
            this.pgTileSheet.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pgTileSheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgTileSheet.Location = new System.Drawing.Point(0, 0);
            this.pgTileSheet.Name = "pgTileSheet";
            this.pgTileSheet.Size = new System.Drawing.Size(218, 320);
            this.pgTileSheet.TabIndex = 0;
            this.pgTileSheet.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgTileSheet_PropertyValueChanged);
            // 
            // pnTileSheet
            // 
            this.pnTileSheet.BackColor = System.Drawing.Color.LightGray;
            this.pnTileSheet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnTileSheet.Controls.Add(this.hsTileSheet);
            this.pnTileSheet.Controls.Add(this.pbTileSheet);
            this.pnTileSheet.Controls.Add(this.vsTileSheet);
            this.pnTileSheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnTileSheet.Location = new System.Drawing.Point(0, 0);
            this.pnTileSheet.Name = "pnTileSheet";
            this.pnTileSheet.Size = new System.Drawing.Size(367, 320);
            this.pnTileSheet.TabIndex = 16;
            this.pnTileSheet.Resize += new System.EventHandler(this.pnTileSheet_Resize);
            // 
            // hsTileSheet
            // 
            this.hsTileSheet.Location = new System.Drawing.Point(0, 295);
            this.hsTileSheet.Name = "hsTileSheet";
            this.hsTileSheet.Size = new System.Drawing.Size(274, 16);
            this.hsTileSheet.TabIndex = 14;
            this.hsTileSheet.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsTileSheet_Scroll);
            // 
            // pbTileSheet
            // 
            this.pbTileSheet.BackColor = System.Drawing.Color.DimGray;
            this.pbTileSheet.Location = new System.Drawing.Point(-1, -1);
            this.pbTileSheet.Name = "pbTileSheet";
            this.pbTileSheet.Size = new System.Drawing.Size(275, 296);
            this.pbTileSheet.TabIndex = 12;
            this.pbTileSheet.TabStop = false;
            this.pbTileSheet.Paint += new System.Windows.Forms.PaintEventHandler(this.pbTileSheet_Paint);
            // 
            // vsTileSheet
            // 
            this.vsTileSheet.Location = new System.Drawing.Point(274, 0);
            this.vsTileSheet.Name = "vsTileSheet";
            this.vsTileSheet.Size = new System.Drawing.Size(16, 295);
            this.vsTileSheet.TabIndex = 13;
            this.vsTileSheet.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vsTileSheet_Scroll);
            // 
            // TileSheetEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 320);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "TileSheetEditor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tile Sheet Editor";
            this.Load += new System.EventHandler(this.TileSheetEditor_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.pnTileSheet.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbTileSheet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PropertyGrid pgTileSheet;
        private System.Windows.Forms.Panel pnTileSheet;
        private System.Windows.Forms.HScrollBar hsTileSheet;
        private System.Windows.Forms.PictureBox pbTileSheet;
        private System.Windows.Forms.VScrollBar vsTileSheet;
    }
}