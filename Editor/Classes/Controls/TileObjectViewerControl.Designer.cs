namespace FiremelonEditor2
{
    partial class TileObjectViewerControl
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
            this.hsImage = new System.Windows.Forms.HScrollBar();
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.vsImage = new System.Windows.Forms.VScrollBar();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.SuspendLayout();
            // 
            // hsImage
            // 
            this.hsImage.Location = new System.Drawing.Point(1, 296);
            this.hsImage.Name = "hsImage";
            this.hsImage.Size = new System.Drawing.Size(274, 16);
            this.hsImage.TabIndex = 20;
            this.hsImage.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsImage_Scroll);
            // 
            // pbImage
            // 
            this.pbImage.BackColor = System.Drawing.Color.DimGray;
            this.pbImage.Location = new System.Drawing.Point(0, 0);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(275, 296);
            this.pbImage.TabIndex = 18;
            this.pbImage.TabStop = false;
            this.pbImage.Paint += new System.Windows.Forms.PaintEventHandler(this.pbImage_Paint);
            // 
            // vsImage
            // 
            this.vsImage.Location = new System.Drawing.Point(275, 1);
            this.vsImage.Name = "vsImage";
            this.vsImage.Size = new System.Drawing.Size(16, 295);
            this.vsImage.TabIndex = 19;
            this.vsImage.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vsImage_Scroll);
            // 
            // TileObjectViewerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.hsImage);
            this.Controls.Add(this.pbImage);
            this.Controls.Add(this.vsImage);
            this.Name = "TileObjectViewerControl";
            this.Size = new System.Drawing.Size(292, 314);
            this.Resize += new System.EventHandler(this.TileObjectViewerControl_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.HScrollBar hsImage;
        private System.Windows.Forms.PictureBox pbImage;
        private System.Windows.Forms.VScrollBar vsImage;
    }
}
