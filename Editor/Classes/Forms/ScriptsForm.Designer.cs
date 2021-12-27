namespace FiremelonEditor2
{
    partial class ScriptsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScriptsForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpScriptEditor = new System.Windows.Forms.TabPage();
            this.tpScriptManager = new System.Windows.Forms.TabPage();
            this.ilTabIcons = new System.Windows.Forms.ImageList(this.components);
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpScriptEditor);
            this.tabControl1.Controls.Add(this.tpScriptManager);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ImageList = this.ilTabIcons;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(836, 627);
            this.tabControl1.TabIndex = 2;
            // 
            // tpScriptEditor
            // 
            this.tpScriptEditor.ImageIndex = 0;
            this.tpScriptEditor.Location = new System.Drawing.Point(4, 23);
            this.tpScriptEditor.Name = "tpScriptEditor";
            this.tpScriptEditor.Padding = new System.Windows.Forms.Padding(3);
            this.tpScriptEditor.Size = new System.Drawing.Size(828, 600);
            this.tpScriptEditor.TabIndex = 0;
            this.tpScriptEditor.Text = "Edit";
            this.tpScriptEditor.UseVisualStyleBackColor = true;
            // 
            // tpScriptManager
            // 
            this.tpScriptManager.ImageIndex = 1;
            this.tpScriptManager.Location = new System.Drawing.Point(4, 23);
            this.tpScriptManager.Name = "tpScriptManager";
            this.tpScriptManager.Padding = new System.Windows.Forms.Padding(3);
            this.tpScriptManager.Size = new System.Drawing.Size(828, 600);
            this.tpScriptManager.TabIndex = 1;
            this.tpScriptManager.Text = "Manage";
            this.tpScriptManager.UseVisualStyleBackColor = true;
            // 
            // ilTabIcons
            // 
            this.ilTabIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilTabIcons.ImageStream")));
            this.ilTabIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.ilTabIcons.Images.SetKeyName(0, "script_code.png");
            this.ilTabIcons.Images.SetKeyName(1, "script_gear.png");
            // 
            // ScriptsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 627);
            this.Controls.Add(this.tabControl1);
            this.MinimizeBox = false;
            this.Name = "ScriptsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Scripts";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ScriptsForm_FormClosing);
            this.Shown += new System.EventHandler(this.ScriptsForm_Shown);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpScriptEditor;
        private System.Windows.Forms.TabPage tpScriptManager;
        private System.Windows.Forms.ImageList ilTabIcons;
    }
}