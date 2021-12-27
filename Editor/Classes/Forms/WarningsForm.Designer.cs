namespace FiremelonEditor2
{
    partial class WarningsForm
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
            this.lvWarnings = new System.Windows.Forms.ListView();
            this.colWarnings = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPrimarySource = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSecondarySource = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lvWarnings
            // 
            this.lvWarnings.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colWarnings,
            this.colPrimarySource,
            this.colSecondarySource});
            this.lvWarnings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvWarnings.FullRowSelect = true;
            this.lvWarnings.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvWarnings.Location = new System.Drawing.Point(0, 0);
            this.lvWarnings.MultiSelect = false;
            this.lvWarnings.Name = "lvWarnings";
            this.lvWarnings.Size = new System.Drawing.Size(804, 158);
            this.lvWarnings.TabIndex = 0;
            this.lvWarnings.UseCompatibleStateImageBehavior = false;
            this.lvWarnings.View = System.Windows.Forms.View.Details;
            // 
            // colWarnings
            // 
            this.colWarnings.Text = "Warning";
            this.colWarnings.Width = 500;
            // 
            // colPrimarySource
            // 
            this.colPrimarySource.Text = "Primary Source";
            this.colPrimarySource.Width = 150;
            // 
            // colSecondarySource
            // 
            this.colSecondarySource.Text = "Secondary Source";
            this.colSecondarySource.Width = 150;
            // 
            // WarningsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 158);
            this.Controls.Add(this.lvWarnings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimizeBox = false;
            this.Name = "WarningsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvWarnings;
        private System.Windows.Forms.ColumnHeader colWarnings;
        private System.Windows.Forms.ColumnHeader colPrimarySource;
        private System.Windows.Forms.ColumnHeader colSecondarySource;
    }
}