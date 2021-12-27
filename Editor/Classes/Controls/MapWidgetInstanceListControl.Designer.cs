namespace FiremelonEditor2
{
    partial class MapWidgetInstanceListControl
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
            this.lbxInstances = new FiremelonEditor2.RefreshableListBox();
            this.SuspendLayout();
            // 
            // lbxInstances
            // 
            this.lbxInstances.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxInstances.FormattingEnabled = true;
            this.lbxInstances.IntegralHeight = false;
            this.lbxInstances.Location = new System.Drawing.Point(0, 0);
            this.lbxInstances.Name = "lbxInstances";
            this.lbxInstances.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbxInstances.Size = new System.Drawing.Size(301, 278);
            this.lbxInstances.TabIndex = 0;
            this.lbxInstances.SelectedIndexChanged += new System.EventHandler(this.lbxInstances_SelectedIndexChanged);
            // 
            // MapWidgetInstanceListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbxInstances);
            this.Name = "MapWidgetInstanceListControl";
            this.Size = new System.Drawing.Size(301, 278);
            this.ResumeLayout(false);

        }

        #endregion

        private RefreshableListBox lbxInstances;
    }
}
