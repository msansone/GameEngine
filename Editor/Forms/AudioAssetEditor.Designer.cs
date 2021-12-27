namespace FiremelonEditor2
{
    partial class AudioAssetEditor
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
            this.pgAudioAsset = new System.Windows.Forms.PropertyGrid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pnAudioAsset = new System.Windows.Forms.Panel();
            this.pnPlayer = new System.Windows.Forms.Panel();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.pnAudioAsset.SuspendLayout();
            this.pnPlayer.SuspendLayout();
            this.SuspendLayout();
            // 
            // pgAudioAsset
            // 
            this.pgAudioAsset.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pgAudioAsset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgAudioAsset.Location = new System.Drawing.Point(0, 0);
            this.pgAudioAsset.Name = "pgAudioAsset";
            this.pgAudioAsset.Size = new System.Drawing.Size(239, 387);
            this.pgAudioAsset.TabIndex = 0;
            this.pgAudioAsset.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgAudioAsset_PropertyValueChanged);
            this.pgAudioAsset.Click += new System.EventHandler(this.pgAudioAsset_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pgAudioAsset);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pnAudioAsset);
            this.splitContainer1.Size = new System.Drawing.Size(650, 387);
            this.splitContainer1.SplitterDistance = 239;
            this.splitContainer1.TabIndex = 2;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // pnAudioAsset
            // 
            this.pnAudioAsset.BackColor = System.Drawing.Color.DimGray;
            this.pnAudioAsset.Controls.Add(this.pnPlayer);
            this.pnAudioAsset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnAudioAsset.Location = new System.Drawing.Point(0, 0);
            this.pnAudioAsset.Name = "pnAudioAsset";
            this.pnAudioAsset.Size = new System.Drawing.Size(407, 387);
            this.pnAudioAsset.TabIndex = 0;
            this.pnAudioAsset.Paint += new System.Windows.Forms.PaintEventHandler(this.pnAudioAsset_Paint);
            this.pnAudioAsset.Resize += new System.EventHandler(this.pnAudioAsset_Resize);
            // 
            // pnPlayer
            // 
            this.pnPlayer.Controls.Add(this.btnPlay);
            this.pnPlayer.Controls.Add(this.btnStop);
            this.pnPlayer.Controls.Add(this.btnPause);
            this.pnPlayer.Location = new System.Drawing.Point(167, 190);
            this.pnPlayer.Name = "pnPlayer";
            this.pnPlayer.Size = new System.Drawing.Size(94, 32);
            this.pnPlayer.TabIndex = 6;
            this.pnPlayer.Paint += new System.Windows.Forms.PaintEventHandler(this.pnPlayer_Paint);
            // 
            // btnPlay
            // 
            this.btnPlay.BackColor = System.Drawing.Color.White;
            this.btnPlay.FlatAppearance.BorderSize = 0;
            this.btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlay.Location = new System.Drawing.Point(0, 0);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(32, 32);
            this.btnPlay.TabIndex = 3;
            this.btnPlay.UseVisualStyleBackColor = false;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            this.btnPlay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPlay_MouseDown);
            this.btnPlay.MouseEnter += new System.EventHandler(this.btnPlay_MouseEnter);
            this.btnPlay.MouseLeave += new System.EventHandler(this.btnPlay_MouseLeave);
            this.btnPlay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPlay_MouseUp);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.White;
            this.btnStop.FlatAppearance.BorderSize = 0;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Location = new System.Drawing.Point(62, 0);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(32, 32);
            this.btnStop.TabIndex = 5;
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            this.btnStop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnStop_MouseDown);
            this.btnStop.MouseEnter += new System.EventHandler(this.btnStop_MouseEnter);
            this.btnStop.MouseLeave += new System.EventHandler(this.btnStop_MouseLeave);
            this.btnStop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnStop_MouseUp);
            // 
            // btnPause
            // 
            this.btnPause.BackColor = System.Drawing.Color.White;
            this.btnPause.FlatAppearance.BorderSize = 0;
            this.btnPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPause.Location = new System.Drawing.Point(31, 0);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(32, 32);
            this.btnPause.TabIndex = 4;
            this.btnPause.UseVisualStyleBackColor = false;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            this.btnPause.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPause_MouseDown);
            this.btnPause.MouseEnter += new System.EventHandler(this.btnPause_MouseEnter);
            this.btnPause.MouseLeave += new System.EventHandler(this.btnPause_MouseLeave);
            this.btnPause.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPause_MouseUp);
            // 
            // AudioAssetEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 387);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "AudioAssetEditor";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Audio Asset Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AudioAssetEditor_FormClosing);
            this.Load += new System.EventHandler(this.AudioAssetEditor_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.pnAudioAsset.ResumeLayout(false);
            this.pnPlayer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid pgAudioAsset;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel pnAudioAsset;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Panel pnPlayer;
    }
}