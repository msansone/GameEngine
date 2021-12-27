namespace FiremelonEditor2
{
    partial class RoomPropertiesEditor
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
            this.pgRoom = new System.Windows.Forms.PropertyGrid();
            this.grpProperties = new System.Windows.Forms.GroupBox();
            this.grpProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // pgRoom
            // 
            this.pgRoom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgRoom.Location = new System.Drawing.Point(7, 20);
            this.pgRoom.Name = "pgRoom";
            this.pgRoom.Size = new System.Drawing.Size(315, 304);
            this.pgRoom.TabIndex = 0;
            // 
            // grpProperties
            // 
            this.grpProperties.Controls.Add(this.pgRoom);
            this.grpProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpProperties.Location = new System.Drawing.Point(10, 10);
            this.grpProperties.Name = "grpProperties";
            this.grpProperties.Padding = new System.Windows.Forms.Padding(7);
            this.grpProperties.Size = new System.Drawing.Size(329, 331);
            this.grpProperties.TabIndex = 1;
            this.grpProperties.TabStop = false;
            // 
            // RoomPropertiesEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 351);
            this.Controls.Add(this.grpProperties);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "RoomPropertiesEditor";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Room Properties";
            this.grpProperties.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid pgRoom;
        private System.Windows.Forms.GroupBox grpProperties;
    }
}