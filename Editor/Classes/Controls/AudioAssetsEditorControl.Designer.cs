namespace FiremelonEditor2
{
    partial class AudioAssetsEditorControl
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
            this.pnAudioAssets = new System.Windows.Forms.Panel();
            this.scAudioAssets = new System.Windows.Forms.SplitContainer();
            this.scAudioAssetsList = new System.Windows.Forms.SplitContainer();
            this.lbxAudioAssets = new System.Windows.Forms.ListBox();
            this.pgAudioAsset = new System.Windows.Forms.PropertyGrid();
            this.pnAudioAssets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scAudioAssets)).BeginInit();
            this.scAudioAssets.Panel1.SuspendLayout();
            this.scAudioAssets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scAudioAssetsList)).BeginInit();
            this.scAudioAssetsList.Panel1.SuspendLayout();
            this.scAudioAssetsList.Panel2.SuspendLayout();
            this.scAudioAssetsList.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnAudioAssets
            // 
            this.pnAudioAssets.Controls.Add(this.scAudioAssets);
            this.pnAudioAssets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnAudioAssets.Location = new System.Drawing.Point(0, 0);
            this.pnAudioAssets.Name = "pnAudioAssets";
            this.pnAudioAssets.Size = new System.Drawing.Size(729, 394);
            this.pnAudioAssets.TabIndex = 26;
            // 
            // scAudioAssets
            // 
            this.scAudioAssets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scAudioAssets.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scAudioAssets.Location = new System.Drawing.Point(0, 0);
            this.scAudioAssets.Name = "scAudioAssets";
            // 
            // scAudioAssets.Panel1
            // 
            this.scAudioAssets.Panel1.Controls.Add(this.scAudioAssetsList);
            this.scAudioAssets.Size = new System.Drawing.Size(729, 394);
            this.scAudioAssets.SplitterDistance = 280;
            this.scAudioAssets.TabIndex = 22;
            // 
            // scAudioAssetsList
            // 
            this.scAudioAssetsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scAudioAssetsList.Location = new System.Drawing.Point(0, 0);
            this.scAudioAssetsList.Name = "scAudioAssetsList";
            this.scAudioAssetsList.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scAudioAssetsList.Panel1
            // 
            this.scAudioAssetsList.Panel1.Controls.Add(this.lbxAudioAssets);
            // 
            // scAudioAssetsList.Panel2
            // 
            this.scAudioAssetsList.Panel2.Controls.Add(this.pgAudioAsset);
            this.scAudioAssetsList.Size = new System.Drawing.Size(280, 394);
            this.scAudioAssetsList.SplitterDistance = 179;
            this.scAudioAssetsList.TabIndex = 0;
            // 
            // lbxAudioAssets
            // 
            this.lbxAudioAssets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxAudioAssets.FormattingEnabled = true;
            this.lbxAudioAssets.IntegralHeight = false;
            this.lbxAudioAssets.Location = new System.Drawing.Point(0, 0);
            this.lbxAudioAssets.Name = "lbxAudioAssets";
            this.lbxAudioAssets.Size = new System.Drawing.Size(280, 179);
            this.lbxAudioAssets.Sorted = true;
            this.lbxAudioAssets.TabIndex = 0;
            this.lbxAudioAssets.SelectedIndexChanged += new System.EventHandler(this.lbxAudioAssets_SelectedIndexChanged);
            // 
            // pgAudioAsset
            // 
            this.pgAudioAsset.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pgAudioAsset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgAudioAsset.Location = new System.Drawing.Point(0, 0);
            this.pgAudioAsset.Name = "pgAudioAsset";
            this.pgAudioAsset.Size = new System.Drawing.Size(280, 211);
            this.pgAudioAsset.TabIndex = 1;
            this.pgAudioAsset.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgAudioAsset_PropertyValueChanged);
            // 
            // AudioAssetsEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnAudioAssets);
            this.Name = "AudioAssetsEditorControl";
            this.Size = new System.Drawing.Size(729, 394);
            this.VisibleChanged += new System.EventHandler(this.AudioAssetsEditorControl_VisibleChanged);
            this.pnAudioAssets.ResumeLayout(false);
            this.scAudioAssets.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scAudioAssets)).EndInit();
            this.scAudioAssets.ResumeLayout(false);
            this.scAudioAssetsList.Panel1.ResumeLayout(false);
            this.scAudioAssetsList.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scAudioAssetsList)).EndInit();
            this.scAudioAssetsList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnAudioAssets;
        private System.Windows.Forms.SplitContainer scAudioAssets;
        private System.Windows.Forms.SplitContainer scAudioAssetsList;
        private System.Windows.Forms.ListBox lbxAudioAssets;
        private System.Windows.Forms.PropertyGrid pgAudioAsset;
    }
}
