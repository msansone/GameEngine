namespace FiremelonEditor2
{
    partial class FindReplaceDialog
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
            this.txtFind = new System.Windows.Forms.TextBox();
            this.txtReplace = new System.Windows.Forms.TextBox();
            this.lbFind = new System.Windows.Forms.Label();
            this.lbReplace = new System.Windows.Forms.Label();
            this.btnFindNext = new System.Windows.Forms.Button();
            this.btnReplace = new System.Windows.Forms.Button();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnReplaceAll = new System.Windows.Forms.Button();
            this.btnReplaceNext = new System.Windows.Forms.Button();
            this.chkMatchCase = new System.Windows.Forms.CheckBox();
            this.chkFindWholeWordOnly = new System.Windows.Forms.CheckBox();
            this.grpConditions = new System.Windows.Forms.GroupBox();
            this.mnuCommands = new System.Windows.Forms.MenuStrip();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlButtons.SuspendLayout();
            this.grpConditions.SuspendLayout();
            this.mnuCommands.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtFind
            // 
            this.txtFind.Location = new System.Drawing.Point(68, 7);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(195, 20);
            this.txtFind.TabIndex = 0;
            // 
            // txtReplace
            // 
            this.txtReplace.Location = new System.Drawing.Point(68, 33);
            this.txtReplace.Name = "txtReplace";
            this.txtReplace.Size = new System.Drawing.Size(195, 20);
            this.txtReplace.TabIndex = 1;
            // 
            // lbFind
            // 
            this.lbFind.AutoSize = true;
            this.lbFind.Location = new System.Drawing.Point(12, 14);
            this.lbFind.Name = "lbFind";
            this.lbFind.Size = new System.Drawing.Size(30, 13);
            this.lbFind.TabIndex = 2;
            this.lbFind.Text = "Find:";
            // 
            // lbReplace
            // 
            this.lbReplace.AutoSize = true;
            this.lbReplace.Location = new System.Drawing.Point(12, 36);
            this.lbReplace.Name = "lbReplace";
            this.lbReplace.Size = new System.Drawing.Size(50, 13);
            this.lbReplace.TabIndex = 3;
            this.lbReplace.Text = "Replace:";
            // 
            // btnFindNext
            // 
            this.btnFindNext.Location = new System.Drawing.Point(3, 9);
            this.btnFindNext.Name = "btnFindNext";
            this.btnFindNext.Size = new System.Drawing.Size(80, 21);
            this.btnFindNext.TabIndex = 4;
            this.btnFindNext.Text = "Find Next";
            this.btnFindNext.UseVisualStyleBackColor = true;
            this.btnFindNext.Click += new System.EventHandler(this.btnFindNext_Click);
            // 
            // btnReplace
            // 
            this.btnReplace.Location = new System.Drawing.Point(3, 38);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(80, 21);
            this.btnReplace.TabIndex = 5;
            this.btnReplace.Text = "Replace";
            this.btnReplace.UseVisualStyleBackColor = true;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnReplaceAll);
            this.pnlButtons.Controls.Add(this.btnReplaceNext);
            this.pnlButtons.Controls.Add(this.btnFindNext);
            this.pnlButtons.Controls.Add(this.btnReplace);
            this.pnlButtons.Controls.Add(this.mnuCommands);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlButtons.Location = new System.Drawing.Point(269, 0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(90, 131);
            this.pnlButtons.TabIndex = 6;
            // 
            // btnReplaceAll
            // 
            this.btnReplaceAll.Location = new System.Drawing.Point(3, 102);
            this.btnReplaceAll.Name = "btnReplaceAll";
            this.btnReplaceAll.Size = new System.Drawing.Size(80, 21);
            this.btnReplaceAll.TabIndex = 7;
            this.btnReplaceAll.Text = "Replace All";
            this.btnReplaceAll.UseVisualStyleBackColor = true;
            // 
            // btnReplaceNext
            // 
            this.btnReplaceNext.Location = new System.Drawing.Point(3, 70);
            this.btnReplaceNext.Name = "btnReplaceNext";
            this.btnReplaceNext.Size = new System.Drawing.Size(80, 21);
            this.btnReplaceNext.TabIndex = 6;
            this.btnReplaceNext.Text = "Replace Next";
            this.btnReplaceNext.UseVisualStyleBackColor = true;
            // 
            // chkMatchCase
            // 
            this.chkMatchCase.AutoSize = true;
            this.chkMatchCase.Location = new System.Drawing.Point(6, 39);
            this.chkMatchCase.Name = "chkMatchCase";
            this.chkMatchCase.Size = new System.Drawing.Size(82, 17);
            this.chkMatchCase.TabIndex = 7;
            this.chkMatchCase.Text = "Match case";
            this.chkMatchCase.UseVisualStyleBackColor = true;
            // 
            // chkFindWholeWordOnly
            // 
            this.chkFindWholeWordOnly.AutoSize = true;
            this.chkFindWholeWordOnly.Location = new System.Drawing.Point(6, 19);
            this.chkFindWholeWordOnly.Name = "chkFindWholeWordOnly";
            this.chkFindWholeWordOnly.Size = new System.Drawing.Size(113, 17);
            this.chkFindWholeWordOnly.TabIndex = 8;
            this.chkFindWholeWordOnly.Text = "Match whole word";
            this.chkFindWholeWordOnly.UseVisualStyleBackColor = true;
            // 
            // grpConditions
            // 
            this.grpConditions.Controls.Add(this.chkFindWholeWordOnly);
            this.grpConditions.Controls.Add(this.chkMatchCase);
            this.grpConditions.Location = new System.Drawing.Point(12, 60);
            this.grpConditions.Name = "grpConditions";
            this.grpConditions.Size = new System.Drawing.Size(251, 65);
            this.grpConditions.TabIndex = 9;
            this.grpConditions.TabStop = false;
            this.grpConditions.Text = "Conditions:";
            // 
            // mnuCommands
            // 
            this.mnuCommands.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem});
            this.mnuCommands.Location = new System.Drawing.Point(0, 0);
            this.mnuCommands.Name = "mnuCommands";
            this.mnuCommands.Size = new System.Drawing.Size(90, 24);
            this.mnuCommands.TabIndex = 8;
            this.mnuCommands.Text = "menuStrip1";
            this.mnuCommands.Visible = false;
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.findToolStripMenuItem.Text = "Find";
            this.findToolStripMenuItem.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
            // 
            // FindReplaceDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 131);
            this.Controls.Add(this.grpConditions);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.lbReplace);
            this.Controls.Add(this.lbFind);
            this.Controls.Add(this.txtReplace);
            this.Controls.Add(this.txtFind);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MainMenuStrip = this.mnuCommands;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(5000, 165);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(375, 165);
            this.Name = "FindReplaceDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Find And Replace";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FindReplaceDialog_FormClosing);
            this.Shown += new System.EventHandler(this.FindReplaceDialog_Shown);
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.grpConditions.ResumeLayout(false);
            this.grpConditions.PerformLayout();
            this.mnuCommands.ResumeLayout(false);
            this.mnuCommands.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFind;
        private System.Windows.Forms.TextBox txtReplace;
        private System.Windows.Forms.Label lbFind;
        private System.Windows.Forms.Label lbReplace;
        private System.Windows.Forms.Button btnFindNext;
        private System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnReplaceAll;
        private System.Windows.Forms.Button btnReplaceNext;
        private System.Windows.Forms.CheckBox chkMatchCase;
        private System.Windows.Forms.CheckBox chkFindWholeWordOnly;
        private System.Windows.Forms.GroupBox grpConditions;
        private System.Windows.Forms.MenuStrip mnuCommands;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
    }
}