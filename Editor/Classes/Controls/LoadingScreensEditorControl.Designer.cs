namespace FiremelonEditor2
{
    partial class LoadingScreensEditorControl
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
            this.scLoadingScreens = new System.Windows.Forms.SplitContainer();
            this.scLoadingScreensList = new System.Windows.Forms.SplitContainer();
            this.lbxLoadingScreens = new System.Windows.Forms.ListBox();
            this.pgLoadingScreens = new System.Windows.Forms.PropertyGrid();
            this.scintilla1 = new ScintillaNET.Scintilla();
            this.pnSpriteSheets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scLoadingScreens)).BeginInit();
            this.scLoadingScreens.Panel1.SuspendLayout();
            this.scLoadingScreens.Panel2.SuspendLayout();
            this.scLoadingScreens.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scLoadingScreensList)).BeginInit();
            this.scLoadingScreensList.Panel1.SuspendLayout();
            this.scLoadingScreensList.Panel2.SuspendLayout();
            this.scLoadingScreensList.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnSpriteSheets
            // 
            this.pnSpriteSheets.Controls.Add(this.scLoadingScreens);
            this.pnSpriteSheets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnSpriteSheets.Location = new System.Drawing.Point(0, 0);
            this.pnSpriteSheets.Name = "pnSpriteSheets";
            this.pnSpriteSheets.Size = new System.Drawing.Size(504, 320);
            this.pnSpriteSheets.TabIndex = 26;
            // 
            // scLoadingScreens
            // 
            this.scLoadingScreens.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scLoadingScreens.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scLoadingScreens.Location = new System.Drawing.Point(0, 0);
            this.scLoadingScreens.Name = "scLoadingScreens";
            // 
            // scLoadingScreens.Panel1
            // 
            this.scLoadingScreens.Panel1.Controls.Add(this.scLoadingScreensList);
            // 
            // scLoadingScreens.Panel2
            // 
            this.scLoadingScreens.Panel2.Controls.Add(this.scintilla1);
            this.scLoadingScreens.Size = new System.Drawing.Size(504, 320);
            this.scLoadingScreens.SplitterDistance = 280;
            this.scLoadingScreens.TabIndex = 22;
            // 
            // scLoadingScreensList
            // 
            this.scLoadingScreensList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scLoadingScreensList.Location = new System.Drawing.Point(0, 0);
            this.scLoadingScreensList.Name = "scLoadingScreensList";
            this.scLoadingScreensList.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scLoadingScreensList.Panel1
            // 
            this.scLoadingScreensList.Panel1.Controls.Add(this.lbxLoadingScreens);
            // 
            // scLoadingScreensList.Panel2
            // 
            this.scLoadingScreensList.Panel2.Controls.Add(this.pgLoadingScreens);
            this.scLoadingScreensList.Size = new System.Drawing.Size(280, 320);
            this.scLoadingScreensList.SplitterDistance = 146;
            this.scLoadingScreensList.TabIndex = 0;
            // 
            // lbxLoadingScreens
            // 
            this.lbxLoadingScreens.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxLoadingScreens.FormattingEnabled = true;
            this.lbxLoadingScreens.IntegralHeight = false;
            this.lbxLoadingScreens.Location = new System.Drawing.Point(0, 0);
            this.lbxLoadingScreens.Name = "lbxLoadingScreens";
            this.lbxLoadingScreens.Size = new System.Drawing.Size(280, 146);
            this.lbxLoadingScreens.Sorted = true;
            this.lbxLoadingScreens.TabIndex = 0;
            this.lbxLoadingScreens.SelectedIndexChanged += new System.EventHandler(this.lbxLoadingScreens_SelectedIndexChanged);
            // 
            // pgLoadingScreens
            // 
            this.pgLoadingScreens.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pgLoadingScreens.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgLoadingScreens.Location = new System.Drawing.Point(0, 0);
            this.pgLoadingScreens.Name = "pgLoadingScreens";
            this.pgLoadingScreens.Size = new System.Drawing.Size(280, 170);
            this.pgLoadingScreens.TabIndex = 1;
            // 
            // scintilla1
            // 
            this.scintilla1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla1.Location = new System.Drawing.Point(0, 0);
            this.scintilla1.Name = "scintilla1";
            this.scintilla1.Size = new System.Drawing.Size(220, 320);
            this.scintilla1.TabIndex = 0;
            // 
            // LoadingScreensEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnSpriteSheets);
            this.Name = "LoadingScreensEditorControl";
            this.Size = new System.Drawing.Size(504, 320);
            this.pnSpriteSheets.ResumeLayout(false);
            this.scLoadingScreens.Panel1.ResumeLayout(false);
            this.scLoadingScreens.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scLoadingScreens)).EndInit();
            this.scLoadingScreens.ResumeLayout(false);
            this.scLoadingScreensList.Panel1.ResumeLayout(false);
            this.scLoadingScreensList.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scLoadingScreensList)).EndInit();
            this.scLoadingScreensList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnSpriteSheets;
        private System.Windows.Forms.SplitContainer scLoadingScreens;
        private System.Windows.Forms.SplitContainer scLoadingScreensList;
        private System.Windows.Forms.ListBox lbxLoadingScreens;
        private System.Windows.Forms.PropertyGrid pgLoadingScreens;
        private ScintillaNET.Scintilla scintilla1;
    }
}
