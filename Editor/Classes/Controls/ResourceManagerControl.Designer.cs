namespace FiremelonEditor2
{
    partial class ResourceManagerControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResourceManagerControl));
            this.lvResources = new System.Windows.Forms.ListView();
            this.colResourceType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colResourceRelativePath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colResourceFullPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbFolderReplacement = new System.Windows.Forms.ToolStripButton();
            this.tslbProjectFolder = new System.Windows.Forms.ToolStripLabel();
            this.colExists = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvResources
            // 
            this.lvResources.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colResourceType,
            this.colResourceRelativePath,
            this.colResourceFullPath,
            this.colExists});
            this.lvResources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvResources.FullRowSelect = true;
            this.lvResources.Location = new System.Drawing.Point(0, 25);
            this.lvResources.Name = "lvResources";
            this.lvResources.Size = new System.Drawing.Size(802, 402);
            this.lvResources.TabIndex = 3;
            this.lvResources.UseCompatibleStateImageBehavior = false;
            this.lvResources.View = System.Windows.Forms.View.Details;
            this.lvResources.DoubleClick += new System.EventHandler(this.lvResources_DoubleClick);
            // 
            // colResourceType
            // 
            this.colResourceType.Text = "Type";
            this.colResourceType.Width = 120;
            // 
            // colResourceRelativePath
            // 
            this.colResourceRelativePath.Text = "Relative Path";
            this.colResourceRelativePath.Width = 300;
            // 
            // colResourceFullPath
            // 
            this.colResourceFullPath.Text = "Full Path";
            this.colResourceFullPath.Width = 300;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbFolderReplacement,
            this.tslbProjectFolder});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(802, 25);
            this.toolStrip1.TabIndex = 2;
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
            // tslbProjectFolder
            // 
            this.tslbProjectFolder.Name = "tslbProjectFolder";
            this.tslbProjectFolder.Size = new System.Drawing.Size(83, 22);
            this.tslbProjectFolder.Text = "Project Folder:";
            // 
            // colExists
            // 
            this.colExists.Text = "Exist On Disk";
            this.colExists.Width = 78;
            // 
            // ResourceManagerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvResources);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ResourceManagerControl";
            this.Size = new System.Drawing.Size(802, 427);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvResources;
        private System.Windows.Forms.ColumnHeader colResourceType;
        private System.Windows.Forms.ColumnHeader colResourceRelativePath;
        private System.Windows.Forms.ColumnHeader colResourceFullPath;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbFolderReplacement;
        private System.Windows.Forms.ToolStripLabel tslbProjectFolder;
        private System.Windows.Forms.ColumnHeader colExists;
    }
}
