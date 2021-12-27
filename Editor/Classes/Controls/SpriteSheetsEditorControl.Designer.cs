namespace FiremelonEditor2
{
    partial class SpriteSheetsEditorControl
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
            this.pnSpriteSheets = new System.Windows.Forms.Panel();
            this.scSpriteSheets = new System.Windows.Forms.SplitContainer();
            this.scSpriteSheetList = new System.Windows.Forms.SplitContainer();
            this.lbxSpriteSheets = new System.Windows.Forms.ListBox();
            this.pgSpriteSheet = new System.Windows.Forms.PropertyGrid();
            this.pnSpriteSheets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scSpriteSheets)).BeginInit();
            this.scSpriteSheets.Panel1.SuspendLayout();
            this.scSpriteSheets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scSpriteSheetList)).BeginInit();
            this.scSpriteSheetList.Panel1.SuspendLayout();
            this.scSpriteSheetList.Panel2.SuspendLayout();
            this.scSpriteSheetList.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnSpriteSheets
            // 
            this.pnSpriteSheets.Controls.Add(this.scSpriteSheets);
            this.pnSpriteSheets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnSpriteSheets.Location = new System.Drawing.Point(0, 0);
            this.pnSpriteSheets.Name = "pnSpriteSheets";
            this.pnSpriteSheets.Size = new System.Drawing.Size(760, 358);
            this.pnSpriteSheets.TabIndex = 25;
            // 
            // scSpriteSheets
            // 
            this.scSpriteSheets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scSpriteSheets.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scSpriteSheets.Location = new System.Drawing.Point(0, 0);
            this.scSpriteSheets.Name = "scSpriteSheets";
            // 
            // scSpriteSheets.Panel1
            // 
            this.scSpriteSheets.Panel1.Controls.Add(this.scSpriteSheetList);
            this.scSpriteSheets.Size = new System.Drawing.Size(760, 358);
            this.scSpriteSheets.SplitterDistance = 280;
            this.scSpriteSheets.TabIndex = 22;
            // 
            // scSpriteSheetList
            // 
            this.scSpriteSheetList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scSpriteSheetList.Location = new System.Drawing.Point(0, 0);
            this.scSpriteSheetList.Name = "scSpriteSheetList";
            this.scSpriteSheetList.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scSpriteSheetList.Panel1
            // 
            this.scSpriteSheetList.Panel1.Controls.Add(this.lbxSpriteSheets);
            // 
            // scSpriteSheetList.Panel2
            // 
            this.scSpriteSheetList.Panel2.Controls.Add(this.pgSpriteSheet);
            this.scSpriteSheetList.Size = new System.Drawing.Size(280, 358);
            this.scSpriteSheetList.SplitterDistance = 164;
            this.scSpriteSheetList.TabIndex = 0;
            // 
            // lbxSpriteSheets
            // 
            this.lbxSpriteSheets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxSpriteSheets.FormattingEnabled = true;
            this.lbxSpriteSheets.IntegralHeight = false;
            this.lbxSpriteSheets.Location = new System.Drawing.Point(0, 0);
            this.lbxSpriteSheets.Name = "lbxSpriteSheets";
            this.lbxSpriteSheets.Size = new System.Drawing.Size(280, 164);
            this.lbxSpriteSheets.Sorted = true;
            this.lbxSpriteSheets.TabIndex = 0;
            this.lbxSpriteSheets.SelectedIndexChanged += new System.EventHandler(this.lbxSpriteSheets_SelectedIndexChanged);
            // 
            // pgSpriteSheet
            // 
            this.pgSpriteSheet.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pgSpriteSheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgSpriteSheet.Location = new System.Drawing.Point(0, 0);
            this.pgSpriteSheet.Name = "pgSpriteSheet";
            this.pgSpriteSheet.Size = new System.Drawing.Size(280, 190);
            this.pgSpriteSheet.TabIndex = 1;
            this.pgSpriteSheet.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgSpriteSheet_PropertyValueChanged);
            // 
            // SpriteSheetsEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnSpriteSheets);
            this.Name = "SpriteSheetsEditorControl";
            this.Size = new System.Drawing.Size(760, 358);
            this.pnSpriteSheets.ResumeLayout(false);
            this.scSpriteSheets.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scSpriteSheets)).EndInit();
            this.scSpriteSheets.ResumeLayout(false);
            this.scSpriteSheetList.Panel1.ResumeLayout(false);
            this.scSpriteSheetList.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scSpriteSheetList)).EndInit();
            this.scSpriteSheetList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnSpriteSheets;
        private System.Windows.Forms.SplitContainer scSpriteSheets;
        private System.Windows.Forms.SplitContainer scSpriteSheetList;
        private System.Windows.Forms.ListBox lbxSpriteSheets;
        private System.Windows.Forms.PropertyGrid pgSpriteSheet;
    }
}
