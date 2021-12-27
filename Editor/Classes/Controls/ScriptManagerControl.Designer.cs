namespace FiremelonEditor2
{
    partial class ScriptManagerControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScriptManagerControl));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbFolderReplacement = new System.Windows.Forms.ToolStripButton();
            this.lvScripts = new System.Windows.Forms.ListView();
            this.colScriptName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colScriptType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colScriptRelativePath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colScriptFullPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbFolderReplacement});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(825, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbFolderReplacement
            // 
            this.tsbFolderReplacement.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbFolderReplacement.Image = ((System.Drawing.Image)(resources.GetObject("tsbFolderReplacement.Image")));
            this.tsbFolderReplacement.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFolderReplacement.Name = "tsbFolderReplacement";
            this.tsbFolderReplacement.Size = new System.Drawing.Size(23, 22);
            this.tsbFolderReplacement.Text = "Folder Replacement";
            this.tsbFolderReplacement.Click += new System.EventHandler(this.tsbFolderReplacement_Click);
            // 
            // lvScripts
            // 
            this.lvScripts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colScriptName,
            this.colScriptType,
            this.colScriptRelativePath,
            this.colScriptFullPath});
            this.lvScripts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvScripts.Location = new System.Drawing.Point(0, 25);
            this.lvScripts.Name = "lvScripts";
            this.lvScripts.Size = new System.Drawing.Size(825, 528);
            this.lvScripts.TabIndex = 1;
            this.lvScripts.UseCompatibleStateImageBehavior = false;
            this.lvScripts.View = System.Windows.Forms.View.Details;
            // 
            // colScriptName
            // 
            this.colScriptName.Text = "Name";
            this.colScriptName.Width = 130;
            // 
            // colScriptType
            // 
            this.colScriptType.Text = "Type";
            this.colScriptType.Width = 120;
            // 
            // colScriptRelativePath
            // 
            this.colScriptRelativePath.Text = "Relative Path";
            this.colScriptRelativePath.Width = 288;
            // 
            // colScriptFullPath
            // 
            this.colScriptFullPath.Text = "Full Path";
            this.colScriptFullPath.Width = 474;
            // 
            // ScriptManagerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvScripts);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ScriptManagerControl";
            this.Size = new System.Drawing.Size(825, 553);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbFolderReplacement;
        private System.Windows.Forms.ListView lvScripts;
        private System.Windows.Forms.ColumnHeader colScriptName;
        private System.Windows.Forms.ColumnHeader colScriptType;
        private System.Windows.Forms.ColumnHeader colScriptRelativePath;
        private System.Windows.Forms.ColumnHeader colScriptFullPath;
    }
}
