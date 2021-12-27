namespace FiremelonEditor2
{
    partial class SheetCellSelectorDialog
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
            this.hsSpriteSheet = new System.Windows.Forms.HScrollBar();
            this.pbSpriteSheet = new System.Windows.Forms.PictureBox();
            this.vsSpriteSheet = new System.Windows.Forms.VScrollBar();
            this.ssCellInfo = new System.Windows.Forms.StatusStrip();
            this.tsslCellIndex = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslColumn = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslRow = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pbSpriteSheet)).BeginInit();
            this.ssCellInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // hsSpriteSheet
            // 
            this.hsSpriteSheet.Location = new System.Drawing.Point(1, 209);
            this.hsSpriteSheet.Name = "hsSpriteSheet";
            this.hsSpriteSheet.Size = new System.Drawing.Size(481, 16);
            this.hsSpriteSheet.TabIndex = 17;
            this.hsSpriteSheet.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsSpriteSheet_Scroll);
            // 
            // pbSpriteSheet
            // 
            this.pbSpriteSheet.BackColor = System.Drawing.Color.DimGray;
            this.pbSpriteSheet.Location = new System.Drawing.Point(0, 0);
            this.pbSpriteSheet.Name = "pbSpriteSheet";
            this.pbSpriteSheet.Size = new System.Drawing.Size(482, 209);
            this.pbSpriteSheet.TabIndex = 15;
            this.pbSpriteSheet.TabStop = false;
            this.pbSpriteSheet.Paint += new System.Windows.Forms.PaintEventHandler(this.pbSpriteSheet_Paint);
            this.pbSpriteSheet.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pbSpriteSheet_MouseDoubleClick);
            this.pbSpriteSheet.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbSpriteSheet_MouseDown);
            // 
            // vsSpriteSheet
            // 
            this.vsSpriteSheet.Location = new System.Drawing.Point(482, 1);
            this.vsSpriteSheet.Name = "vsSpriteSheet";
            this.vsSpriteSheet.Size = new System.Drawing.Size(16, 208);
            this.vsSpriteSheet.TabIndex = 16;
            this.vsSpriteSheet.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vsSpriteSheet_Scroll);
            // 
            // ssCellInfo
            // 
            this.ssCellInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslCellIndex,
            this.tsslColumn,
            this.tsslRow});
            this.ssCellInfo.Location = new System.Drawing.Point(0, 224);
            this.ssCellInfo.Name = "ssCellInfo";
            this.ssCellInfo.Size = new System.Drawing.Size(500, 22);
            this.ssCellInfo.TabIndex = 18;
            // 
            // tsslCellIndex
            // 
            this.tsslCellIndex.Name = "tsslCellIndex";
            this.tsslCellIndex.Size = new System.Drawing.Size(38, 17);
            this.tsslCellIndex.Text = "Index:";
            // 
            // tsslColumn
            // 
            this.tsslColumn.Name = "tsslColumn";
            this.tsslColumn.Size = new System.Drawing.Size(53, 17);
            this.tsslColumn.Text = "Column:";
            // 
            // tsslRow
            // 
            this.tsslRow.Name = "tsslRow";
            this.tsslRow.Size = new System.Drawing.Size(33, 17);
            this.tsslRow.Text = "Row:";
            // 
            // SheetCellSelectorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 246);
            this.Controls.Add(this.ssCellInfo);
            this.Controls.Add(this.hsSpriteSheet);
            this.Controls.Add(this.pbSpriteSheet);
            this.Controls.Add(this.vsSpriteSheet);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "SheetCellSelectorDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Sheet Cell";
            this.Load += new System.EventHandler(this.SheetCellSelectorDialog_Load);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.SheetCellSelectorDialog_MouseDoubleClick);
            this.Resize += new System.EventHandler(this.SheetCellSelectorDialog_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pbSpriteSheet)).EndInit();
            this.ssCellInfo.ResumeLayout(false);
            this.ssCellInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.HScrollBar hsSpriteSheet;
        private System.Windows.Forms.PictureBox pbSpriteSheet;
        private System.Windows.Forms.VScrollBar vsSpriteSheet;
        private System.Windows.Forms.StatusStrip ssCellInfo;
        private System.Windows.Forms.ToolStripStatusLabel tsslCellIndex;
        private System.Windows.Forms.ToolStripStatusLabel tsslColumn;
        private System.Windows.Forms.ToolStripStatusLabel tsslRow;
    }
}