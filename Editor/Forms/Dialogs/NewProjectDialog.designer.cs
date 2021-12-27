namespace FiremelonEditor2
{
    partial class NewProjectDialog
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
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRows = new System.Windows.Forms.TextBox();
            this.txtCols = new System.Windows.Forms.TextBox();
            this.cboIngameSize = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pnBorder = new System.Windows.Forms.Panel();
            this.pnMap = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtImportAssets = new System.Windows.Forms.TextBox();
            this.btnBrowseAssets = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtProjectName = new System.Windows.Forms.TextBox();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtProjectFolder = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRoomName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboTileSize = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.txtLayerName = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pnMap.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.cmdCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdCancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdCancel.ForeColor = System.Drawing.Color.Navy;
            this.cmdCancel.Location = new System.Drawing.Point(145, 304);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(126, 23);
            this.cmdCancel.TabIndex = 0;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.cmdOK.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.cmdOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdOK.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdOK.ForeColor = System.Drawing.Color.Navy;
            this.cmdOK.Location = new System.Drawing.Point(12, 304);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(126, 23);
            this.cmdOK.TabIndex = 1;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = false;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Navy;
            this.label1.Location = new System.Drawing.Point(9, 113);
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
            this.label2.Location = new System.Drawing.Point(9, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 21);
            this.label2.TabIndex = 3;
            this.label2.Text = "Columns";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRows
            // 
            this.txtRows.BackColor = System.Drawing.Color.White;
            this.txtRows.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRows.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRows.ForeColor = System.Drawing.Color.Black;
            this.txtRows.Location = new System.Drawing.Point(93, 113);
            this.txtRows.MaxLength = 5;
            this.txtRows.Name = "txtRows";
            this.txtRows.Size = new System.Drawing.Size(154, 23);
            this.txtRows.TabIndex = 4;
            this.txtRows.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtRows.Enter += new System.EventHandler(this.textBox_Enter);
            this.txtRows.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRows_KeyPress);
            this.txtRows.Leave += new System.EventHandler(this.txtRows_Leave);
            // 
            // txtCols
            // 
            this.txtCols.BackColor = System.Drawing.Color.White;
            this.txtCols.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCols.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCols.ForeColor = System.Drawing.Color.Black;
            this.txtCols.Location = new System.Drawing.Point(93, 140);
            this.txtCols.MaxLength = 5;
            this.txtCols.Name = "txtCols";
            this.txtCols.Size = new System.Drawing.Size(154, 23);
            this.txtCols.TabIndex = 5;
            this.txtCols.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCols.Enter += new System.EventHandler(this.textBox_Enter);
            this.txtCols.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCols_KeyPress);
            this.txtCols.Leave += new System.EventHandler(this.txtCols_Leave);
            // 
            // cboIngameSize
            // 
            this.cboIngameSize.BackColor = System.Drawing.Color.White;
            this.cboIngameSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIngameSize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboIngameSize.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboIngameSize.FormattingEnabled = true;
            this.cboIngameSize.Items.AddRange(new object[] {
            "           1280x720",
            "           1024x768",
            "             800x600",
            "             640x480"});
            this.cboIngameSize.Location = new System.Drawing.Point(94, 59);
            this.cboIngameSize.Name = "cboIngameSize";
            this.cboIngameSize.Size = new System.Drawing.Size(152, 21);
            this.cboIngameSize.TabIndex = 0;
            this.cboIngameSize.SelectedIndexChanged += new System.EventHandler(this.cboIngameSize_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.White;
            this.label5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Navy;
            this.label5.Location = new System.Drawing.Point(9, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 21);
            this.label5.TabIndex = 16;
            this.label5.Text = "Resolution";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnBorder
            // 
            this.pnBorder.BackColor = System.Drawing.Color.Transparent;
            this.pnBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnBorder.Location = new System.Drawing.Point(93, 58);
            this.pnBorder.Name = "pnBorder";
            this.pnBorder.Size = new System.Drawing.Size(154, 23);
            this.pnBorder.TabIndex = 1;
            // 
            // pnMap
            // 
            this.pnMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnMap.Controls.Add(this.panel3);
            this.pnMap.Controls.Add(this.label12);
            this.pnMap.Controls.Add(this.textBox12);
            this.pnMap.Controls.Add(this.label11);
            this.pnMap.Controls.Add(this.txtProjectName);
            this.pnMap.Controls.Add(this.textBox11);
            this.pnMap.Controls.Add(this.panel2);
            this.pnMap.Controls.Add(this.label9);
            this.pnMap.Controls.Add(this.label3);
            this.pnMap.Controls.Add(this.txtRoomName);
            this.pnMap.Controls.Add(this.label4);
            this.pnMap.Controls.Add(this.cboTileSize);
            this.pnMap.Controls.Add(this.panel1);
            this.pnMap.Controls.Add(this.label6);
            this.pnMap.Controls.Add(this.txtHeight);
            this.pnMap.Controls.Add(this.txtWidth);
            this.pnMap.Controls.Add(this.label8);
            this.pnMap.Controls.Add(this.panel4);
            this.pnMap.Controls.Add(this.label10);
            this.pnMap.Controls.Add(this.txtLayerName);
            this.pnMap.Controls.Add(this.cboIngameSize);
            this.pnMap.Controls.Add(this.label1);
            this.pnMap.Controls.Add(this.label2);
            this.pnMap.Controls.Add(this.label5);
            this.pnMap.Controls.Add(this.pnBorder);
            this.pnMap.Controls.Add(this.txtRows);
            this.pnMap.Controls.Add(this.txtCols);
            this.pnMap.Controls.Add(this.textBox4);
            this.pnMap.Controls.Add(this.textBox9);
            this.pnMap.Controls.Add(this.textBox8);
            this.pnMap.Controls.Add(this.textBox7);
            this.pnMap.Controls.Add(this.textBox6);
            this.pnMap.Controls.Add(this.textBox5);
            this.pnMap.Controls.Add(this.textBox3);
            this.pnMap.Controls.Add(this.textBox2);
            this.pnMap.Controls.Add(this.textBox1);
            this.pnMap.Controls.Add(this.label7);
            this.pnMap.Location = new System.Drawing.Point(12, 12);
            this.pnMap.Name = "pnMap";
            this.pnMap.Size = new System.Drawing.Size(259, 286);
            this.pnMap.TabIndex = 24;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.txtImportAssets);
            this.panel3.Controls.Add(this.btnBrowseAssets);
            this.panel3.Location = new System.Drawing.Point(93, 252);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(154, 23);
            this.panel3.TabIndex = 52;
            // 
            // txtImportAssets
            // 
            this.txtImportAssets.BackColor = System.Drawing.Color.White;
            this.txtImportAssets.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtImportAssets.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtImportAssets.ForeColor = System.Drawing.Color.Black;
            this.txtImportAssets.Location = new System.Drawing.Point(-1, -1);
            this.txtImportAssets.MaxLength = 5;
            this.txtImportAssets.Name = "txtImportAssets";
            this.txtImportAssets.Size = new System.Drawing.Size(127, 23);
            this.txtImportAssets.TabIndex = 0;
            this.txtImportAssets.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtImportAssets.Enter += new System.EventHandler(this.textBox_Enter);
            this.txtImportAssets.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textboxBrowseFolders_KeyPress);
            this.txtImportAssets.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // btnBrowseAssets
            // 
            this.btnBrowseAssets.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnBrowseAssets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseAssets.Location = new System.Drawing.Point(125, -1);
            this.btnBrowseAssets.Name = "btnBrowseAssets";
            this.btnBrowseAssets.Size = new System.Drawing.Size(28, 23);
            this.btnBrowseAssets.TabIndex = 85;
            this.btnBrowseAssets.Text = "...";
            this.btnBrowseAssets.UseVisualStyleBackColor = false;
            this.btnBrowseAssets.Click += new System.EventHandler(this.btnBrowseAssets_Click);
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.White;
            this.label12.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Navy;
            this.label12.Location = new System.Drawing.Point(-1, 252);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(92, 21);
            this.label12.TabIndex = 51;
            this.label12.Text = "Import Assets";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox12
            // 
            this.textBox12.BackColor = System.Drawing.Color.White;
            this.textBox12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox12.Enabled = false;
            this.textBox12.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox12.ForeColor = System.Drawing.Color.Black;
            this.textBox12.Location = new System.Drawing.Point(92, 253);
            this.textBox12.MaxLength = 0;
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(154, 23);
            this.textBox12.TabIndex = 53;
            this.textBox12.Text = "Layer 1";
            this.textBox12.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.White;
            this.label11.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Navy;
            this.label11.Location = new System.Drawing.Point(-1, 31);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(92, 21);
            this.label11.TabIndex = 49;
            this.label11.Text = "Project Name";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProjectName
            // 
            this.txtProjectName.BackColor = System.Drawing.Color.White;
            this.txtProjectName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjectName.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProjectName.ForeColor = System.Drawing.Color.Black;
            this.txtProjectName.Location = new System.Drawing.Point(93, 31);
            this.txtProjectName.MaxLength = 25;
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.Size = new System.Drawing.Size(154, 23);
            this.txtProjectName.TabIndex = 0;
            this.txtProjectName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtProjectName.Enter += new System.EventHandler(this.textBox_Enter);
            this.txtProjectName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtProjectName_KeyPress);
            this.txtProjectName.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // textBox11
            // 
            this.textBox11.BackColor = System.Drawing.Color.White;
            this.textBox11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox11.Enabled = false;
            this.textBox11.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox11.ForeColor = System.Drawing.Color.Black;
            this.textBox11.Location = new System.Drawing.Point(92, 32);
            this.textBox11.MaxLength = 0;
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(154, 23);
            this.textBox11.TabIndex = 50;
            this.textBox11.Text = "Layer 1";
            this.textBox11.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.txtProjectFolder);
            this.panel2.Controls.Add(this.btnBrowse);
            this.panel2.Location = new System.Drawing.Point(93, 223);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(154, 23);
            this.panel2.TabIndex = 38;
            // 
            // txtProjectFolder
            // 
            this.txtProjectFolder.BackColor = System.Drawing.Color.White;
            this.txtProjectFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjectFolder.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProjectFolder.ForeColor = System.Drawing.Color.Black;
            this.txtProjectFolder.Location = new System.Drawing.Point(-1, -1);
            this.txtProjectFolder.MaxLength = 5;
            this.txtProjectFolder.Name = "txtProjectFolder";
            this.txtProjectFolder.Size = new System.Drawing.Size(127, 23);
            this.txtProjectFolder.TabIndex = 0;
            this.txtProjectFolder.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtProjectFolder.Enter += new System.EventHandler(this.textBox_Enter);
            this.txtProjectFolder.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textboxBrowseFolders_KeyPress);
            this.txtProjectFolder.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // btnBrowse
            // 
            this.btnBrowse.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowse.Location = new System.Drawing.Point(125, -1);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(28, 23);
            this.btnBrowse.TabIndex = 85;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = false;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.White;
            this.label9.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Navy;
            this.label9.Location = new System.Drawing.Point(4, 223);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(87, 21);
            this.label9.TabIndex = 36;
            this.label9.Text = "Project Folder";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Navy;
            this.label3.Location = new System.Drawing.Point(9, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 21);
            this.label3.TabIndex = 34;
            this.label3.Text = "Room Name";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRoomName
            // 
            this.txtRoomName.BackColor = System.Drawing.Color.White;
            this.txtRoomName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRoomName.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRoomName.ForeColor = System.Drawing.Color.Black;
            this.txtRoomName.Location = new System.Drawing.Point(93, 85);
            this.txtRoomName.MaxLength = 0;
            this.txtRoomName.Name = "txtRoomName";
            this.txtRoomName.Size = new System.Drawing.Size(154, 23);
            this.txtRoomName.TabIndex = 3;
            this.txtRoomName.Text = "Room 1";
            this.txtRoomName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtRoomName.Enter += new System.EventHandler(this.textBox_Enter);
            this.txtRoomName.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Navy;
            this.label4.Location = new System.Drawing.Point(16, 195);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 21);
            this.label4.TabIndex = 31;
            this.label4.Text = "Tile Size";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboTileSize
            // 
            this.cboTileSize.BackColor = System.Drawing.Color.White;
            this.cboTileSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTileSize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboTileSize.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTileSize.FormattingEnabled = true;
            this.cboTileSize.Items.AddRange(new object[] {
            "                 48x48",
            "                 32x32",
            "                 16x16"});
            this.cboTileSize.Location = new System.Drawing.Point(94, 196);
            this.cboTileSize.Name = "cboTileSize";
            this.cboTileSize.Size = new System.Drawing.Size(152, 21);
            this.cboTileSize.TabIndex = 7;
            this.cboTileSize.SelectedIndexChanged += new System.EventHandler(this.cboTileSize_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(93, 195);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(154, 23);
            this.panel1.TabIndex = 33;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.White;
            this.label6.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Navy;
            this.label6.Location = new System.Drawing.Point(9, 85);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 21);
            this.label6.TabIndex = 27;
            this.label6.Text = "Height";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label6.Visible = false;
            // 
            // txtHeight
            // 
            this.txtHeight.BackColor = System.Drawing.Color.White;
            this.txtHeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHeight.Enabled = false;
            this.txtHeight.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHeight.ForeColor = System.Drawing.Color.Black;
            this.txtHeight.Location = new System.Drawing.Point(93, 85);
            this.txtHeight.MaxLength = 5;
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(154, 23);
            this.txtHeight.TabIndex = 1;
            this.txtHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtHeight.Visible = false;
            this.txtHeight.Enter += new System.EventHandler(this.txtHeight_Enter);
            this.txtHeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtHeight_KeyPress);
            this.txtHeight.Leave += new System.EventHandler(this.txtHeight_Leave);
            // 
            // txtWidth
            // 
            this.txtWidth.BackColor = System.Drawing.Color.White;
            this.txtWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtWidth.Enabled = false;
            this.txtWidth.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWidth.ForeColor = System.Drawing.Color.Black;
            this.txtWidth.Location = new System.Drawing.Point(93, 112);
            this.txtWidth.MaxLength = 5;
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(154, 23);
            this.txtWidth.TabIndex = 2;
            this.txtWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtWidth.Visible = false;
            this.txtWidth.Enter += new System.EventHandler(this.txtWidth_Enter);
            this.txtWidth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtWidth_KeyPress);
            this.txtWidth.Leave += new System.EventHandler(this.txtWidth_Leave);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.label8.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Navy;
            this.label8.Location = new System.Drawing.Point(3, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 16);
            this.label8.TabIndex = 25;
            this.label8.Text = "Project Data";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.panel4.Location = new System.Drawing.Point(-1, -1);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(260, 25);
            this.panel4.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.White;
            this.label10.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Navy;
            this.label10.Location = new System.Drawing.Point(9, 167);
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
            this.txtLayerName.Location = new System.Drawing.Point(93, 167);
            this.txtLayerName.MaxLength = 0;
            this.txtLayerName.Name = "txtLayerName";
            this.txtLayerName.Size = new System.Drawing.Size(154, 23);
            this.txtLayerName.TabIndex = 6;
            this.txtLayerName.Text = "Layer 1";
            this.txtLayerName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtLayerName.Enter += new System.EventHandler(this.textBox_Enter);
            this.txtLayerName.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.Color.White;
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox4.Enabled = false;
            this.textBox4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox4.ForeColor = System.Drawing.Color.Black;
            this.textBox4.Location = new System.Drawing.Point(92, 59);
            this.textBox4.MaxLength = 0;
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(154, 23);
            this.textBox4.TabIndex = 39;
            this.textBox4.Text = "Layer 1";
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox9
            // 
            this.textBox9.BackColor = System.Drawing.Color.White;
            this.textBox9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox9.Enabled = false;
            this.textBox9.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox9.ForeColor = System.Drawing.Color.Black;
            this.textBox9.Location = new System.Drawing.Point(92, 224);
            this.textBox9.MaxLength = 0;
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(154, 23);
            this.textBox9.TabIndex = 47;
            this.textBox9.Text = "Layer 1";
            this.textBox9.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox8
            // 
            this.textBox8.BackColor = System.Drawing.Color.White;
            this.textBox8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox8.Enabled = false;
            this.textBox8.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox8.ForeColor = System.Drawing.Color.Black;
            this.textBox8.Location = new System.Drawing.Point(92, 196);
            this.textBox8.MaxLength = 0;
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(154, 23);
            this.textBox8.TabIndex = 46;
            this.textBox8.Text = "Layer 1";
            this.textBox8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox7
            // 
            this.textBox7.BackColor = System.Drawing.Color.White;
            this.textBox7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox7.Enabled = false;
            this.textBox7.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox7.ForeColor = System.Drawing.Color.Black;
            this.textBox7.Location = new System.Drawing.Point(92, 168);
            this.textBox7.MaxLength = 0;
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(154, 23);
            this.textBox7.TabIndex = 45;
            this.textBox7.Text = "Layer 1";
            this.textBox7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox6
            // 
            this.textBox6.BackColor = System.Drawing.Color.White;
            this.textBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox6.Enabled = false;
            this.textBox6.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox6.ForeColor = System.Drawing.Color.Black;
            this.textBox6.Location = new System.Drawing.Point(92, 141);
            this.textBox6.MaxLength = 0;
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(154, 23);
            this.textBox6.TabIndex = 44;
            this.textBox6.Text = "Layer 1";
            this.textBox6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox5
            // 
            this.textBox5.BackColor = System.Drawing.Color.White;
            this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox5.Enabled = false;
            this.textBox5.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox5.ForeColor = System.Drawing.Color.Black;
            this.textBox5.Location = new System.Drawing.Point(94, 113);
            this.textBox5.MaxLength = 0;
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(152, 23);
            this.textBox5.TabIndex = 43;
            this.textBox5.Text = "Layer 1";
            this.textBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.White;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox3.Enabled = false;
            this.textBox3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.ForeColor = System.Drawing.Color.Black;
            this.textBox3.Location = new System.Drawing.Point(92, 86);
            this.textBox3.MaxLength = 0;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(154, 23);
            this.textBox3.TabIndex = 42;
            this.textBox3.Text = "Layer 1";
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.White;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox2.Enabled = false;
            this.textBox2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.ForeColor = System.Drawing.Color.Black;
            this.textBox2.Location = new System.Drawing.Point(92, 114);
            this.textBox2.MaxLength = 0;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(154, 23);
            this.textBox2.TabIndex = 41;
            this.textBox2.Text = "Layer 1";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.Black;
            this.textBox1.Location = new System.Drawing.Point(92, 86);
            this.textBox1.MaxLength = 0;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(154, 23);
            this.textBox1.TabIndex = 40;
            this.textBox1.Text = "Layer 1";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.White;
            this.label7.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Navy;
            this.label7.Location = new System.Drawing.Point(9, 112);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 21);
            this.label7.TabIndex = 28;
            this.label7.Text = "Width";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label7.Visible = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // NewProjectDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(284, 338);
            this.Controls.Add(this.pnMap);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewProjectDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Project";
            this.Shown += new System.EventHandler(this.NewProjectDialog_Shown);
            this.pnMap.ResumeLayout(false);
            this.pnMap.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRows;
        private System.Windows.Forms.TextBox txtCols;
        private System.Windows.Forms.ComboBox cboIngameSize;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel pnBorder;
        private System.Windows.Forms.Panel pnMap;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtLayerName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.TextBox txtWidth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboTileSize;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRoomName;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtProjectFolder;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtProjectName;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtImportAssets;
        private System.Windows.Forms.Button btnBrowseAssets;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}