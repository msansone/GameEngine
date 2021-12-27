namespace FiremelonEditor2
{
    partial class ScriptsEditorControl
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
            this.scScripts = new System.Windows.Forms.SplitContainer();
            this.scScriptsList = new System.Windows.Forms.SplitContainer();
            this.lbxScripts = new System.Windows.Forms.ListBox();
            this.pgScripts = new System.Windows.Forms.PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.scScripts)).BeginInit();
            this.scScripts.Panel1.SuspendLayout();
            this.scScripts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scScriptsList)).BeginInit();
            this.scScriptsList.Panel1.SuspendLayout();
            this.scScriptsList.Panel2.SuspendLayout();
            this.scScriptsList.SuspendLayout();
            this.SuspendLayout();
            // 
            // scScripts
            // 
            this.scScripts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scScripts.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scScripts.Location = new System.Drawing.Point(0, 0);
            this.scScripts.Name = "scScripts";
            // 
            // scScripts.Panel1
            // 
            this.scScripts.Panel1.Controls.Add(this.scScriptsList);
            this.scScripts.Size = new System.Drawing.Size(704, 493);
            this.scScripts.SplitterDistance = 280;
            this.scScripts.TabIndex = 6;
            // 
            // scScriptsList
            // 
            this.scScriptsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scScriptsList.Location = new System.Drawing.Point(0, 0);
            this.scScriptsList.Name = "scScriptsList";
            this.scScriptsList.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scScriptsList.Panel1
            // 
            this.scScriptsList.Panel1.Controls.Add(this.lbxScripts);
            // 
            // scScriptsList.Panel2
            // 
            this.scScriptsList.Panel2.Controls.Add(this.pgScripts);
            this.scScriptsList.Size = new System.Drawing.Size(280, 493);
            this.scScriptsList.SplitterDistance = 233;
            this.scScriptsList.TabIndex = 1;
            // 
            // lbxScripts
            // 
            this.lbxScripts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxScripts.FormattingEnabled = true;
            this.lbxScripts.IntegralHeight = false;
            this.lbxScripts.Location = new System.Drawing.Point(0, 0);
            this.lbxScripts.Name = "lbxScripts";
            this.lbxScripts.Size = new System.Drawing.Size(280, 233);
            this.lbxScripts.Sorted = true;
            this.lbxScripts.TabIndex = 1;
            this.lbxScripts.SelectedIndexChanged += new System.EventHandler(this.lbxScripts_SelectedIndexChanged);
            // 
            // pgScripts
            // 
            this.pgScripts.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pgScripts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgScripts.Location = new System.Drawing.Point(0, 0);
            this.pgScripts.Name = "pgScripts";
            this.pgScripts.Size = new System.Drawing.Size(280, 256);
            this.pgScripts.TabIndex = 0;
            this.pgScripts.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgScripts_PropertyValueChanged);
            // 
            // ScriptsEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scScripts);
            this.Name = "ScriptsEditorControl";
            this.Size = new System.Drawing.Size(704, 493);
            this.scScripts.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scScripts)).EndInit();
            this.scScripts.ResumeLayout(false);
            this.scScriptsList.Panel1.ResumeLayout(false);
            this.scScriptsList.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scScriptsList)).EndInit();
            this.scScriptsList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer scScripts;
        private System.Windows.Forms.SplitContainer scScriptsList;
        private System.Windows.Forms.PropertyGrid pgScripts;
        private System.Windows.Forms.ListBox lbxScripts;
    }
}
