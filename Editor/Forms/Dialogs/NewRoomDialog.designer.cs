namespace FiremelonEditor2
{
    partial class NewRoomDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewRoomDialog));
            this.pnMap = new System.Windows.Forms.Panel();
            this.pbRoomNameError = new System.Windows.Forms.PictureBox();
            this.txtRoomName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.txtLayerName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRows = new System.Windows.Forms.TextBox();
            this.txtCols = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.ttRoomNameError = new System.Windows.Forms.ToolTip(this.components);
            this.pnMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRoomNameError)).BeginInit();
            this.SuspendLayout();
            // 
            // pnMap
            // 
            this.pnMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnMap.Controls.Add(this.pbRoomNameError);
            this.pnMap.Controls.Add(this.txtRoomName);
            this.pnMap.Controls.Add(this.label8);
            this.pnMap.Controls.Add(this.panel4);
            this.pnMap.Controls.Add(this.label10);
            this.pnMap.Controls.Add(this.txtLayerName);
            this.pnMap.Controls.Add(this.label1);
            this.pnMap.Controls.Add(this.label2);
            this.pnMap.Controls.Add(this.label5);
            this.pnMap.Controls.Add(this.txtRows);
            this.pnMap.Controls.Add(this.txtCols);
            this.pnMap.Controls.Add(this.textBox1);
            this.pnMap.Controls.Add(this.textBox2);
            this.pnMap.Controls.Add(this.textBox3);
            this.pnMap.Controls.Add(this.textBox4);
            this.pnMap.Location = new System.Drawing.Point(12, 12);
            this.pnMap.Name = "pnMap";
            this.pnMap.Size = new System.Drawing.Size(263, 145);
            this.pnMap.TabIndex = 28;
            // 
            // pbRoomNameError
            // 
            this.pbRoomNameError.Image = ((System.Drawing.Image)(resources.GetObject("pbRoomNameError.Image")));
            this.pbRoomNameError.Location = new System.Drawing.Point(243, 34);
            this.pbRoomNameError.Name = "pbRoomNameError";
            this.pbRoomNameError.Size = new System.Drawing.Size(16, 16);
            this.pbRoomNameError.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbRoomNameError.TabIndex = 29;
            this.pbRoomNameError.TabStop = false;
            this.pbRoomNameError.Visible = false;
            // 
            // txtRoomName
            // 
            this.txtRoomName.BackColor = System.Drawing.Color.White;
            this.txtRoomName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRoomName.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRoomName.ForeColor = System.Drawing.Color.Black;
            this.txtRoomName.Location = new System.Drawing.Point(88, 31);
            this.txtRoomName.MaxLength = 0;
            this.txtRoomName.Name = "txtRoomName";
            this.txtRoomName.Size = new System.Drawing.Size(154, 23);
            this.txtRoomName.TabIndex = 0;
            this.txtRoomName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtRoomName.Enter += new System.EventHandler(this.txtRoomName_Enter);
            this.txtRoomName.Leave += new System.EventHandler(this.txtRoomName_Leave);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.label8.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Navy;
            this.label8.Location = new System.Drawing.Point(3, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 16);
            this.label8.TabIndex = 25;
            this.label8.Text = "Room Data";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.panel4.Location = new System.Drawing.Point(-1, -1);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(264, 25);
            this.panel4.TabIndex = 26;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.White;
            this.label10.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Navy;
            this.label10.Location = new System.Drawing.Point(4, 112);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(82, 21);
            this.label10.TabIndex = 19;
            this.label10.Text = "Layer Name";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLayerName
            // 
            this.txtLayerName.BackColor = System.Drawing.Color.White;
            this.txtLayerName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLayerName.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLayerName.ForeColor = System.Drawing.Color.Black;
            this.txtLayerName.Location = new System.Drawing.Point(88, 112);
            this.txtLayerName.MaxLength = 0;
            this.txtLayerName.Name = "txtLayerName";
            this.txtLayerName.Size = new System.Drawing.Size(154, 23);
            this.txtLayerName.TabIndex = 15;
            this.txtLayerName.Text = "Layer 1";
            this.txtLayerName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtLayerName.Enter += new System.EventHandler(this.txtLayerName_Enter);
            this.txtLayerName.Leave += new System.EventHandler(this.txtLayerName_Leave);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Navy;
            this.label1.Location = new System.Drawing.Point(4, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "Rows";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Navy;
            this.label2.Location = new System.Drawing.Point(4, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 21);
            this.label2.TabIndex = 3;
            this.label2.Text = "Columns";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.White;
            this.label5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Navy;
            this.label5.Location = new System.Drawing.Point(4, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 21);
            this.label5.TabIndex = 16;
            this.label5.Text = "Room Name";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRows
            // 
            this.txtRows.BackColor = System.Drawing.Color.White;
            this.txtRows.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRows.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRows.ForeColor = System.Drawing.Color.Black;
            this.txtRows.Location = new System.Drawing.Point(88, 58);
            this.txtRows.MaxLength = 5;
            this.txtRows.Name = "txtRows";
            this.txtRows.Size = new System.Drawing.Size(154, 23);
            this.txtRows.TabIndex = 5;
            this.txtRows.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtRows.Enter += new System.EventHandler(this.txtRows_Enter);
            this.txtRows.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRows_KeyPress);
            this.txtRows.Leave += new System.EventHandler(this.txtRows_Leave);
            // 
            // txtCols
            // 
            this.txtCols.BackColor = System.Drawing.Color.White;
            this.txtCols.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCols.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCols.ForeColor = System.Drawing.Color.Black;
            this.txtCols.Location = new System.Drawing.Point(88, 85);
            this.txtCols.MaxLength = 5;
            this.txtCols.Name = "txtCols";
            this.txtCols.Size = new System.Drawing.Size(154, 23);
            this.txtCols.TabIndex = 10;
            this.txtCols.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCols.Enter += new System.EventHandler(this.txtCols_Enter);
            this.txtCols.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCols_KeyPress);
            this.txtCols.Leave += new System.EventHandler(this.txtCols_Leave);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.Black;
            this.textBox1.Location = new System.Drawing.Point(87, 113);
            this.textBox1.MaxLength = 0;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(154, 23);
            this.textBox1.TabIndex = 35;
            this.textBox1.Text = "Layer 1";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.White;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox2.Enabled = false;
            this.textBox2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.ForeColor = System.Drawing.Color.Black;
            this.textBox2.Location = new System.Drawing.Point(87, 86);
            this.textBox2.MaxLength = 0;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(154, 23);
            this.textBox2.TabIndex = 36;
            this.textBox2.Text = "Layer 1";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.White;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox3.Enabled = false;
            this.textBox3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.ForeColor = System.Drawing.Color.Black;
            this.textBox3.Location = new System.Drawing.Point(87, 59);
            this.textBox3.MaxLength = 0;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(154, 23);
            this.textBox3.TabIndex = 37;
            this.textBox3.Text = "Layer 1";
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.Color.White;
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox4.Enabled = false;
            this.textBox4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox4.ForeColor = System.Drawing.Color.Black;
            this.textBox4.Location = new System.Drawing.Point(87, 32);
            this.textBox4.MaxLength = 0;
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(154, 23);
            this.textBox4.TabIndex = 38;
            this.textBox4.Text = "Layer 1";
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cmdOK
            // 
            this.cmdOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.cmdOK.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.cmdOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdOK.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdOK.ForeColor = System.Drawing.Color.Navy;
            this.cmdOK.Location = new System.Drawing.Point(12, 163);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(126, 23);
            this.cmdOK.TabIndex = 25;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = false;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.cmdCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdCancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdCancel.ForeColor = System.Drawing.Color.Navy;
            this.cmdCancel.Location = new System.Drawing.Point(149, 163);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(126, 23);
            this.cmdCancel.TabIndex = 20;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // NewRoomDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(291, 198);
            this.Controls.Add(this.pnMap);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "NewRoomDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New Room";
            this.Load += new System.EventHandler(this.NewRoomDialog_Load);
            this.pnMap.ResumeLayout(false);
            this.pnMap.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRoomNameError)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnMap;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtLayerName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtRows;
        private System.Windows.Forms.TextBox txtCols;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.TextBox txtRoomName;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.ToolTip ttRoomNameError;
        private System.Windows.Forms.PictureBox pbRoomNameError;

    }
}