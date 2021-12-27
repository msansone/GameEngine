namespace FiremelonEditor2
{
    partial class SpriteSheetEditor
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
            this.pgSpriteSheet = new System.Windows.Forms.PropertyGrid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pnSpriteSheet = new System.Windows.Forms.Panel();
            this.hsSpriteSheet = new System.Windows.Forms.HScrollBar();
            this.pbSpriteSheet = new System.Windows.Forms.PictureBox();
            this.vsSpriteSheet = new System.Windows.Forms.VScrollBar();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.pnSpriteSheet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSpriteSheet)).BeginInit();
            this.SuspendLayout();
            // 
            // pgSpriteSheet
            // 
            this.pgSpriteSheet.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pgSpriteSheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgSpriteSheet.Location = new System.Drawing.Point(0, 0);
            this.pgSpriteSheet.Name = "pgSpriteSheet";
            this.pgSpriteSheet.Size = new System.Drawing.Size(239, 339);
            this.pgSpriteSheet.TabIndex = 0;
            this.pgSpriteSheet.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgSpriteSheet_PropertyValueChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pgSpriteSheet);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pnSpriteSheet);
            this.splitContainer1.Size = new System.Drawing.Size(648, 339);
            this.splitContainer1.SplitterDistance = 239;
            this.splitContainer1.TabIndex = 1;
            // 
            // pnSpriteSheet
            // 
            this.pnSpriteSheet.BackColor = System.Drawing.Color.LightGray;
            this.pnSpriteSheet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnSpriteSheet.Controls.Add(this.hsSpriteSheet);
            this.pnSpriteSheet.Controls.Add(this.pbSpriteSheet);
            this.pnSpriteSheet.Controls.Add(this.vsSpriteSheet);
            this.pnSpriteSheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnSpriteSheet.Location = new System.Drawing.Point(0, 0);
            this.pnSpriteSheet.Name = "pnSpriteSheet";
            this.pnSpriteSheet.Size = new System.Drawing.Size(405, 339);
            this.pnSpriteSheet.TabIndex = 16;
            this.pnSpriteSheet.Resize += new System.EventHandler(this.pnSpriteSheet_Resize);
            // 
            // hsSpriteSheet
            // 
            this.hsSpriteSheet.Location = new System.Drawing.Point(0, 295);
            this.hsSpriteSheet.Name = "hsSpriteSheet";
            this.hsSpriteSheet.Size = new System.Drawing.Size(274, 16);
            this.hsSpriteSheet.TabIndex = 14;
            this.hsSpriteSheet.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsSpriteSheet_Scroll);
            // 
            // pbSpriteSheet
            // 
            this.pbSpriteSheet.BackColor = System.Drawing.Color.DimGray;
            this.pbSpriteSheet.Location = new System.Drawing.Point(-1, -1);
            this.pbSpriteSheet.Name = "pbSpriteSheet";
            this.pbSpriteSheet.Size = new System.Drawing.Size(275, 296);
            this.pbSpriteSheet.TabIndex = 12;
            this.pbSpriteSheet.TabStop = false;
            this.pbSpriteSheet.Paint += new System.Windows.Forms.PaintEventHandler(this.pbSpriteSheet_Paint);
            // 
            // vsSpriteSheet
            // 
            this.vsSpriteSheet.Location = new System.Drawing.Point(274, 0);
            this.vsSpriteSheet.Name = "vsSpriteSheet";
            this.vsSpriteSheet.Size = new System.Drawing.Size(16, 295);
            this.vsSpriteSheet.TabIndex = 13;
            this.vsSpriteSheet.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vsSpriteSheet_Scroll);
            // 
            // SpriteSheetEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 339);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "SpriteSheetEditor";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sprite Sheet Editor";
            this.Load += new System.EventHandler(this.SpriteSheetEditor_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.pnSpriteSheet.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbSpriteSheet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid pgSpriteSheet;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel pnSpriteSheet;
        private System.Windows.Forms.HScrollBar hsSpriteSheet;
        private System.Windows.Forms.PictureBox pbSpriteSheet;
        private System.Windows.Forms.VScrollBar vsSpriteSheet;
    }
}