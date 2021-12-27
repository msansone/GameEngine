namespace FiremelonEditor2
{
    partial class PaletteMapControl
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
            this.flpnlColorMaps = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flpnlColorMaps
            // 
            this.flpnlColorMaps.AutoScroll = true;
            this.flpnlColorMaps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpnlColorMaps.Location = new System.Drawing.Point(0, 0);
            this.flpnlColorMaps.Name = "flpnlColorMaps";
            this.flpnlColorMaps.Size = new System.Drawing.Size(153, 126);
            this.flpnlColorMaps.TabIndex = 0;
            this.flpnlColorMaps.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanel1_Paint);
            // 
            // PaletteMapControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flpnlColorMaps);
            this.Name = "PaletteMapControl";
            this.Size = new System.Drawing.Size(153, 126);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpnlColorMaps;
    }
}
