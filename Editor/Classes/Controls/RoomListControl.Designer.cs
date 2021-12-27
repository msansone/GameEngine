namespace FiremelonEditor2
{
    partial class RoomListControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RoomListControl));
            this.pnAddNew = new System.Windows.Forms.Panel();
            this.lbTitle = new System.Windows.Forms.Label();
            this.pbAddButton = new System.Windows.Forms.PictureBox();
            this.btnPopOut = new System.Windows.Forms.Button();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.pbRoomList = new System.Windows.Forms.PictureBox();
            this.pbRoomSelectedBG = new System.Windows.Forms.PictureBox();
            this.pbRoomBG = new System.Windows.Forms.PictureBox();
            this.ilIcons = new System.Windows.Forms.ImageList(this.components);
            this.pbAddNormal = new System.Windows.Forms.PictureBox();
            this.pbAddOver = new System.Windows.Forms.PictureBox();
            this.pbAddDown = new System.Windows.Forms.PictureBox();
            this.mnuRoomProperties = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.roomPropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnAddNew.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAddButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRoomList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRoomSelectedBG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRoomBG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAddNormal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAddOver)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAddDown)).BeginInit();
            this.mnuRoomProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnAddNew
            // 
            this.pnAddNew.BackColor = System.Drawing.Color.Transparent;
            this.pnAddNew.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnAddNew.BackgroundImage")));
            this.pnAddNew.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnAddNew.Controls.Add(this.lbTitle);
            this.pnAddNew.Controls.Add(this.pbAddButton);
            this.pnAddNew.Controls.Add(this.btnPopOut);
            this.pnAddNew.Location = new System.Drawing.Point(0, 0);
            this.pnAddNew.Name = "pnAddNew";
            this.pnAddNew.Size = new System.Drawing.Size(359, 22);
            this.pnAddNew.TabIndex = 9;
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.BackColor = System.Drawing.Color.Transparent;
            this.lbTitle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitle.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbTitle.Location = new System.Drawing.Point(32, 3);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(48, 13);
            this.lbTitle.TabIndex = 13;
            this.lbTitle.Text = "ROOMS";
            this.lbTitle.Visible = false;
            // 
            // pbAddButton
            // 
            this.pbAddButton.Location = new System.Drawing.Point(0, 0);
            this.pbAddButton.Name = "pbAddButton";
            this.pbAddButton.Size = new System.Drawing.Size(21, 20);
            this.pbAddButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbAddButton.TabIndex = 24;
            this.pbAddButton.TabStop = false;
            this.pbAddButton.Paint += new System.Windows.Forms.PaintEventHandler(this.pbAddButton_Paint);
            this.pbAddButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbAddButton_MouseDown);
            this.pbAddButton.MouseLeave += new System.EventHandler(this.pbAddButton_MouseLeave);
            this.pbAddButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbAddButton_MouseMove);
            this.pbAddButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbAddButton_MouseUp);
            // 
            // btnPopOut
            // 
            this.btnPopOut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnPopOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPopOut.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnPopOut.Image = ((System.Drawing.Image)(resources.GetObject("btnPopOut.Image")));
            this.btnPopOut.Location = new System.Drawing.Point(160, -1);
            this.btnPopOut.Name = "btnPopOut";
            this.btnPopOut.Size = new System.Drawing.Size(20, 20);
            this.btnPopOut.TabIndex = 12;
            this.btnPopOut.UseVisualStyleBackColor = false;
            this.btnPopOut.Visible = false;
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Location = new System.Drawing.Point(343, 22);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(16, 452);
            this.vScrollBar1.TabIndex = 8;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll_1);
            // 
            // pbRoomList
            // 
            this.pbRoomList.BackColor = System.Drawing.Color.White;
            this.pbRoomList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbRoomList.Location = new System.Drawing.Point(0, 22);
            this.pbRoomList.Name = "pbRoomList";
            this.pbRoomList.Size = new System.Drawing.Size(343, 454);
            this.pbRoomList.TabIndex = 7;
            this.pbRoomList.TabStop = false;
            this.pbRoomList.Paint += new System.Windows.Forms.PaintEventHandler(this.pbRoomList_Paint);
            this.pbRoomList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbRoomList_MouseDown);
            this.pbRoomList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbRoomList_MouseMove);
            this.pbRoomList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbRoomList_MouseUp);
            // 
            // pbRoomSelectedBG
            // 
            this.pbRoomSelectedBG.Image = ((System.Drawing.Image)(resources.GetObject("pbRoomSelectedBG.Image")));
            this.pbRoomSelectedBG.Location = new System.Drawing.Point(0, 46);
            this.pbRoomSelectedBG.Name = "pbRoomSelectedBG";
            this.pbRoomSelectedBG.Size = new System.Drawing.Size(250, 25);
            this.pbRoomSelectedBG.TabIndex = 11;
            this.pbRoomSelectedBG.TabStop = false;
            this.pbRoomSelectedBG.Visible = false;
            // 
            // pbRoomBG
            // 
            this.pbRoomBG.Image = ((System.Drawing.Image)(resources.GetObject("pbRoomBG.Image")));
            this.pbRoomBG.Location = new System.Drawing.Point(0, 22);
            this.pbRoomBG.Name = "pbRoomBG";
            this.pbRoomBG.Size = new System.Drawing.Size(250, 25);
            this.pbRoomBG.TabIndex = 10;
            this.pbRoomBG.TabStop = false;
            this.pbRoomBG.Visible = false;
            // 
            // ilIcons
            // 
            this.ilIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilIcons.ImageStream")));
            this.ilIcons.TransparentColor = System.Drawing.Color.Magenta;
            this.ilIcons.Images.SetKeyName(0, "delete.bmp");
            this.ilIcons.Images.SetKeyName(1, "deleteover.bmp");
            this.ilIcons.Images.SetKeyName(2, "startingroom.bmp");
            this.ilIcons.Images.SetKeyName(3, "startingroomover.bmp");
            this.ilIcons.Images.SetKeyName(4, "startingroomselected.bmp");
            // 
            // pbAddNormal
            // 
            this.pbAddNormal.Image = ((System.Drawing.Image)(resources.GetObject("pbAddNormal.Image")));
            this.pbAddNormal.Location = new System.Drawing.Point(362, 202);
            this.pbAddNormal.Name = "pbAddNormal";
            this.pbAddNormal.Size = new System.Drawing.Size(21, 20);
            this.pbAddNormal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbAddNormal.TabIndex = 27;
            this.pbAddNormal.TabStop = false;
            this.pbAddNormal.Visible = false;
            // 
            // pbAddOver
            // 
            this.pbAddOver.Image = ((System.Drawing.Image)(resources.GetObject("pbAddOver.Image")));
            this.pbAddOver.Location = new System.Drawing.Point(362, 228);
            this.pbAddOver.Name = "pbAddOver";
            this.pbAddOver.Size = new System.Drawing.Size(21, 20);
            this.pbAddOver.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbAddOver.TabIndex = 25;
            this.pbAddOver.TabStop = false;
            this.pbAddOver.Visible = false;
            // 
            // pbAddDown
            // 
            this.pbAddDown.Image = ((System.Drawing.Image)(resources.GetObject("pbAddDown.Image")));
            this.pbAddDown.Location = new System.Drawing.Point(362, 254);
            this.pbAddDown.Name = "pbAddDown";
            this.pbAddDown.Size = new System.Drawing.Size(21, 20);
            this.pbAddDown.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbAddDown.TabIndex = 26;
            this.pbAddDown.TabStop = false;
            this.pbAddDown.Visible = false;
            // 
            // mnuRoomProperties
            // 
            this.mnuRoomProperties.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.roomPropertiesToolStripMenuItem});
            this.mnuRoomProperties.Name = "mnuTileProperties";
            this.mnuRoomProperties.Size = new System.Drawing.Size(163, 26);
            // 
            // roomPropertiesToolStripMenuItem
            // 
            this.roomPropertiesToolStripMenuItem.Name = "roomPropertiesToolStripMenuItem";
            this.roomPropertiesToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.roomPropertiesToolStripMenuItem.Text = "Room Properties";
            this.roomPropertiesToolStripMenuItem.Click += new System.EventHandler(this.roomPropertiesToolStripMenuItem_Click);
            // 
            // RoomListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbAddNormal);
            this.Controls.Add(this.pbAddOver);
            this.Controls.Add(this.pbAddDown);
            this.Controls.Add(this.pnAddNew);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.pbRoomSelectedBG);
            this.Controls.Add(this.pbRoomBG);
            this.Controls.Add(this.pbRoomList);
            this.DoubleBuffered = true;
            this.Name = "RoomListControl";
            this.Size = new System.Drawing.Size(389, 503);
            this.Load += new System.EventHandler(this.RoomListControl_Load);
            this.Resize += new System.EventHandler(this.RoomListControl_Resize);
            this.pnAddNew.ResumeLayout(false);
            this.pnAddNew.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAddButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRoomList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRoomSelectedBG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRoomBG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAddNormal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAddOver)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAddDown)).EndInit();
            this.mnuRoomProperties.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnAddNew;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Button btnPopOut;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.PictureBox pbRoomList;
        private System.Windows.Forms.PictureBox pbRoomSelectedBG;
        private System.Windows.Forms.PictureBox pbRoomBG;
        private System.Windows.Forms.ImageList ilIcons;
        private System.Windows.Forms.PictureBox pbAddButton;
        private System.Windows.Forms.PictureBox pbAddNormal;
        private System.Windows.Forms.PictureBox pbAddOver;
        private System.Windows.Forms.PictureBox pbAddDown;
        private System.Windows.Forms.ContextMenuStrip mnuRoomProperties;
        private System.Windows.Forms.ToolStripMenuItem roomPropertiesToolStripMenuItem;
    }
}
