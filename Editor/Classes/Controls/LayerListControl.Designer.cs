namespace FiremelonEditor2
{
    partial class LayerListControl
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
            if (g != null)
            {
                g.Dispose();
            }

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayerListControl));
            this.pnAddNew = new System.Windows.Forms.Panel();
            this.btnPopOut = new System.Windows.Forms.Button();
            this.pbAddButton = new System.Windows.Forms.PictureBox();
            this.pbAddNormal = new System.Windows.Forms.PictureBox();
            this.pbAddOver = new System.Windows.Forms.PictureBox();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.pbLayerBG = new System.Windows.Forms.PictureBox();
            this.pbLayerSelectedBG = new System.Windows.Forms.PictureBox();
            this.ilIcons = new System.Windows.Forms.ImageList(this.components);
            this.tmrScroll = new System.Windows.Forms.Timer(this.components);
            this.pbLayers = new System.Windows.Forms.PictureBox();
            this.pbAddDown = new System.Windows.Forms.PictureBox();
            this.pnAddNew.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAddButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAddNormal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAddOver)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLayerBG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLayerSelectedBG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLayers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAddDown)).BeginInit();
            this.SuspendLayout();
            // 
            // pnAddNew
            // 
            this.pnAddNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.pnAddNew.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnAddNew.BackgroundImage")));
            this.pnAddNew.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnAddNew.Controls.Add(this.btnPopOut);
            this.pnAddNew.Controls.Add(this.pbAddButton);
            this.pnAddNew.Location = new System.Drawing.Point(0, 0);
            this.pnAddNew.Name = "pnAddNew";
            this.pnAddNew.Size = new System.Drawing.Size(193, 22);
            this.pnAddNew.TabIndex = 0;
            // 
            // btnPopOut
            // 
            this.btnPopOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPopOut.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnPopOut.Image = ((System.Drawing.Image)(resources.GetObject("btnPopOut.Image")));
            this.btnPopOut.Location = new System.Drawing.Point(167, -2);
            this.btnPopOut.Name = "btnPopOut";
            this.btnPopOut.Size = new System.Drawing.Size(20, 20);
            this.btnPopOut.TabIndex = 4;
            this.btnPopOut.UseVisualStyleBackColor = true;
            this.btnPopOut.Visible = false;
            // 
            // pbAddButton
            // 
            this.pbAddButton.Location = new System.Drawing.Point(0, 0);
            this.pbAddButton.Name = "pbAddButton";
            this.pbAddButton.Size = new System.Drawing.Size(21, 20);
            this.pbAddButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbAddButton.TabIndex = 20;
            this.pbAddButton.TabStop = false;
            this.pbAddButton.Paint += new System.Windows.Forms.PaintEventHandler(this.pbAddButton_Paint);
            this.pbAddButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbAddButton_MouseDown);
            this.pbAddButton.MouseLeave += new System.EventHandler(this.pbAddButton_MouseLeave);
            this.pbAddButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbAddButton_MouseMove);
            this.pbAddButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbAddButton_MouseUp);
            // 
            // pbAddNormal
            // 
            this.pbAddNormal.Image = ((System.Drawing.Image)(resources.GetObject("pbAddNormal.Image")));
            this.pbAddNormal.Location = new System.Drawing.Point(196, 102);
            this.pbAddNormal.Name = "pbAddNormal";
            this.pbAddNormal.Size = new System.Drawing.Size(21, 20);
            this.pbAddNormal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbAddNormal.TabIndex = 23;
            this.pbAddNormal.TabStop = false;
            this.pbAddNormal.Visible = false;
            // 
            // pbAddOver
            // 
            this.pbAddOver.Image = ((System.Drawing.Image)(resources.GetObject("pbAddOver.Image")));
            this.pbAddOver.Location = new System.Drawing.Point(196, 128);
            this.pbAddOver.Name = "pbAddOver";
            this.pbAddOver.Size = new System.Drawing.Size(21, 20);
            this.pbAddOver.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbAddOver.TabIndex = 21;
            this.pbAddOver.TabStop = false;
            this.pbAddOver.Visible = false;
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.LargeChange = 2;
            this.vScrollBar1.Location = new System.Drawing.Point(177, 22);
            this.vScrollBar1.Maximum = 1;
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(16, 263);
            this.vScrollBar1.TabIndex = 1;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // pbLayerBG
            // 
            this.pbLayerBG.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbLayerBG.BackgroundImage")));
            this.pbLayerBG.Image = ((System.Drawing.Image)(resources.GetObject("pbLayerBG.Image")));
            this.pbLayerBG.Location = new System.Drawing.Point(0, 72);
            this.pbLayerBG.Name = "pbLayerBG";
            this.pbLayerBG.Size = new System.Drawing.Size(177, 50);
            this.pbLayerBG.TabIndex = 5;
            this.pbLayerBG.TabStop = false;
            this.pbLayerBG.Visible = false;
            // 
            // pbLayerSelectedBG
            // 
            this.pbLayerSelectedBG.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbLayerSelectedBG.BackgroundImage")));
            this.pbLayerSelectedBG.Image = ((System.Drawing.Image)(resources.GetObject("pbLayerSelectedBG.Image")));
            this.pbLayerSelectedBG.Location = new System.Drawing.Point(0, 22);
            this.pbLayerSelectedBG.Name = "pbLayerSelectedBG";
            this.pbLayerSelectedBG.Size = new System.Drawing.Size(188, 50);
            this.pbLayerSelectedBG.TabIndex = 6;
            this.pbLayerSelectedBG.TabStop = false;
            this.pbLayerSelectedBG.Visible = false;
            // 
            // ilIcons
            // 
            this.ilIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilIcons.ImageStream")));
            this.ilIcons.TransparentColor = System.Drawing.Color.Magenta;
            this.ilIcons.Images.SetKeyName(0, "eyeopened.bmp");
            this.ilIcons.Images.SetKeyName(1, "eyeclosed.bmp");
            this.ilIcons.Images.SetKeyName(2, "eyeopenedover.bmp");
            this.ilIcons.Images.SetKeyName(3, "eyeclosedover.bmp");
            this.ilIcons.Images.SetKeyName(4, "delete.bmp");
            this.ilIcons.Images.SetKeyName(5, "deleteover.bmp");
            this.ilIcons.Images.SetKeyName(6, "interactive.bmp");
            this.ilIcons.Images.SetKeyName(7, "interactiveover.bmp");
            this.ilIcons.Images.SetKeyName(8, "interactiveselected.bmp");
            this.ilIcons.Images.SetKeyName(9, "edit.bmp");
            this.ilIcons.Images.SetKeyName(10, "editover.bmp");
            // 
            // tmrScroll
            // 
            this.tmrScroll.Interval = 250;
            this.tmrScroll.Tick += new System.EventHandler(this.tmrScroll_Tick);
            // 
            // pbLayers
            // 
            this.pbLayers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbLayers.Location = new System.Drawing.Point(0, 22);
            this.pbLayers.Name = "pbLayers";
            this.pbLayers.Size = new System.Drawing.Size(177, 263);
            this.pbLayers.TabIndex = 7;
            this.pbLayers.TabStop = false;
            this.pbLayers.Click += new System.EventHandler(this.pbLayers_Click);
            this.pbLayers.Paint += new System.Windows.Forms.PaintEventHandler(this.pbLayers_Paint);
            this.pbLayers.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbLayers_MouseDown);
            this.pbLayers.MouseLeave += new System.EventHandler(this.pbLayers_MouseLeave);
            this.pbLayers.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbLayers_MouseMove);
            this.pbLayers.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbLayers_MouseUp);
            // 
            // pbAddDown
            // 
            this.pbAddDown.Image = ((System.Drawing.Image)(resources.GetObject("pbAddDown.Image")));
            this.pbAddDown.Location = new System.Drawing.Point(196, 154);
            this.pbAddDown.Name = "pbAddDown";
            this.pbAddDown.Size = new System.Drawing.Size(21, 20);
            this.pbAddDown.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbAddDown.TabIndex = 22;
            this.pbAddDown.TabStop = false;
            this.pbAddDown.Visible = false;
            // 
            // LayerListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pbAddNormal);
            this.Controls.Add(this.pbAddOver);
            this.Controls.Add(this.pbAddDown);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.pbLayerSelectedBG);
            this.Controls.Add(this.pbLayerBG);
            this.Controls.Add(this.pbLayers);
            this.Controls.Add(this.pnAddNew);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "LayerListControl";
            this.Size = new System.Drawing.Size(264, 285);
            this.Resize += new System.EventHandler(this.LayerListControl_Resize);
            this.pnAddNew.ResumeLayout(false);
            this.pnAddNew.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAddButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAddNormal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAddOver)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLayerBG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLayerSelectedBG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLayers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAddDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnAddNew;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.PictureBox pbLayerBG;
        private System.Windows.Forms.PictureBox pbLayerSelectedBG;
        private System.Windows.Forms.ImageList ilIcons;
        private System.Windows.Forms.Button btnPopOut;
        private System.Windows.Forms.Timer tmrScroll;
        private System.Windows.Forms.PictureBox pbLayers;
        private System.Windows.Forms.PictureBox pbAddNormal;
        private System.Windows.Forms.PictureBox pbAddButton;
        private System.Windows.Forms.PictureBox pbAddOver;
        private System.Windows.Forms.PictureBox pbAddDown;
    }
}
