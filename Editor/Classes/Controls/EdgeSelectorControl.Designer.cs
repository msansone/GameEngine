namespace FiremelonEditor2
{
    partial class EdgeSelectorControl
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
            this.clbEdges = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // clbEdges
            // 
            this.clbEdges.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbEdges.FormattingEnabled = true;
            this.clbEdges.IntegralHeight = false;
            this.clbEdges.Items.AddRange(new object[] {
            "Top",
            "Right",
            "Bottom",
            "Left"});
            this.clbEdges.Location = new System.Drawing.Point(0, 0);
            this.clbEdges.Name = "clbEdges";
            this.clbEdges.Size = new System.Drawing.Size(62, 66);
            this.clbEdges.TabIndex = 0;
            this.clbEdges.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbEdges_ItemCheck);
            // 
            // EdgeSelectorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.clbEdges);
            this.Name = "EdgeSelectorControl";
            this.Size = new System.Drawing.Size(62, 66);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox clbEdges;
    }
}
