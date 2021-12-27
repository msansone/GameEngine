namespace FiremelonEditor2
{
    partial class TransitionsEditorControl
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
            this.scTransitions = new System.Windows.Forms.SplitContainer();
            this.scTransitionsList = new System.Windows.Forms.SplitContainer();
            this.lbxTransitions = new System.Windows.Forms.ListBox();
            this.pgTransitions = new System.Windows.Forms.PropertyGrid();
            this.scintilla1 = new ScintillaNET.Scintilla();
            this.pnSpriteSheets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scTransitions)).BeginInit();
            this.scTransitions.Panel1.SuspendLayout();
            this.scTransitions.Panel2.SuspendLayout();
            this.scTransitions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scTransitionsList)).BeginInit();
            this.scTransitionsList.Panel1.SuspendLayout();
            this.scTransitionsList.Panel2.SuspendLayout();
            this.scTransitionsList.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnSpriteSheets
            // 
            this.pnSpriteSheets.Controls.Add(this.scTransitions);
            this.pnSpriteSheets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnSpriteSheets.Location = new System.Drawing.Point(0, 0);
            this.pnSpriteSheets.Name = "pnSpriteSheets";
            this.pnSpriteSheets.Size = new System.Drawing.Size(559, 430);
            this.pnSpriteSheets.TabIndex = 27;
            // 
            // scTransitions
            // 
            this.scTransitions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scTransitions.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scTransitions.Location = new System.Drawing.Point(0, 0);
            this.scTransitions.Name = "scTransitions";
            // 
            // scTransitions.Panel1
            // 
            this.scTransitions.Panel1.Controls.Add(this.scTransitionsList);
            // 
            // scTransitions.Panel2
            // 
            this.scTransitions.Panel2.Controls.Add(this.scintilla1);
            this.scTransitions.Size = new System.Drawing.Size(559, 430);
            this.scTransitions.SplitterDistance = 280;
            this.scTransitions.TabIndex = 22;
            // 
            // scTransitionsList
            // 
            this.scTransitionsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scTransitionsList.Location = new System.Drawing.Point(0, 0);
            this.scTransitionsList.Name = "scTransitionsList";
            this.scTransitionsList.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scTransitionsList.Panel1
            // 
            this.scTransitionsList.Panel1.Controls.Add(this.lbxTransitions);
            // 
            // scTransitionsList.Panel2
            // 
            this.scTransitionsList.Panel2.Controls.Add(this.pgTransitions);
            this.scTransitionsList.Size = new System.Drawing.Size(280, 430);
            this.scTransitionsList.SplitterDistance = 196;
            this.scTransitionsList.TabIndex = 0;
            // 
            // lbxTransitions
            // 
            this.lbxTransitions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxTransitions.FormattingEnabled = true;
            this.lbxTransitions.IntegralHeight = false;
            this.lbxTransitions.Location = new System.Drawing.Point(0, 0);
            this.lbxTransitions.Name = "lbxTransitions";
            this.lbxTransitions.Size = new System.Drawing.Size(280, 196);
            this.lbxTransitions.Sorted = true;
            this.lbxTransitions.TabIndex = 0;
            this.lbxTransitions.SelectedIndexChanged += new System.EventHandler(this.lbxTransitions_SelectedIndexChanged);
            // 
            // pgTransitions
            // 
            this.pgTransitions.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pgTransitions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgTransitions.Location = new System.Drawing.Point(0, 0);
            this.pgTransitions.Name = "pgTransitions";
            this.pgTransitions.Size = new System.Drawing.Size(280, 230);
            this.pgTransitions.TabIndex = 1;
            // 
            // scintilla1
            // 
            this.scintilla1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla1.Location = new System.Drawing.Point(0, 0);
            this.scintilla1.Name = "scintilla1";
            this.scintilla1.Size = new System.Drawing.Size(275, 430);
            this.scintilla1.TabIndex = 1;
            // 
            // TransitionsEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnSpriteSheets);
            this.Name = "TransitionsEditorControl";
            this.Size = new System.Drawing.Size(559, 430);
            this.pnSpriteSheets.ResumeLayout(false);
            this.scTransitions.Panel1.ResumeLayout(false);
            this.scTransitions.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scTransitions)).EndInit();
            this.scTransitions.ResumeLayout(false);
            this.scTransitionsList.Panel1.ResumeLayout(false);
            this.scTransitionsList.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scTransitionsList)).EndInit();
            this.scTransitionsList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnSpriteSheets;
        private System.Windows.Forms.SplitContainer scTransitions;
        private System.Windows.Forms.SplitContainer scTransitionsList;
        private System.Windows.Forms.ListBox lbxTransitions;
        private System.Windows.Forms.PropertyGrid pgTransitions;
        private ScintillaNET.Scintilla scintilla1;
    }
}
