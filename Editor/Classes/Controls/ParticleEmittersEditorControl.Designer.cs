namespace FiremelonEditor2
{
    partial class ParticleEmittersEditorControl
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
            this.scParticleEmitters = new System.Windows.Forms.SplitContainer();
            this.scEntitiesList = new System.Windows.Forms.SplitContainer();
            this.tvParticleEmitters = new System.Windows.Forms.TreeView();
            this.pgProperties = new System.Windows.Forms.PropertyGrid();
            this.cmnuScript = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewEditScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuScriptRoot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuParticleEmitter = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteParticleEmitterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuParticle = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteParticleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.scParticleEmitters)).BeginInit();
            this.scParticleEmitters.Panel1.SuspendLayout();
            this.scParticleEmitters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scEntitiesList)).BeginInit();
            this.scEntitiesList.Panel1.SuspendLayout();
            this.scEntitiesList.Panel2.SuspendLayout();
            this.scEntitiesList.SuspendLayout();
            this.cmnuScript.SuspendLayout();
            this.cmnuScriptRoot.SuspendLayout();
            this.cmnuParticleEmitter.SuspendLayout();
            this.cmnuParticle.SuspendLayout();
            this.SuspendLayout();
            // 
            // scParticleEmitters
            // 
            this.scParticleEmitters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scParticleEmitters.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scParticleEmitters.Location = new System.Drawing.Point(0, 0);
            this.scParticleEmitters.Name = "scParticleEmitters";
            // 
            // scParticleEmitters.Panel1
            // 
            this.scParticleEmitters.Panel1.Controls.Add(this.scEntitiesList);
            this.scParticleEmitters.Size = new System.Drawing.Size(998, 544);
            this.scParticleEmitters.SplitterDistance = 280;
            this.scParticleEmitters.TabIndex = 6;
            // 
            // scEntitiesList
            // 
            this.scEntitiesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scEntitiesList.Location = new System.Drawing.Point(0, 0);
            this.scEntitiesList.Name = "scEntitiesList";
            this.scEntitiesList.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scEntitiesList.Panel1
            // 
            this.scEntitiesList.Panel1.Controls.Add(this.tvParticleEmitters);
            // 
            // scEntitiesList.Panel2
            // 
            this.scEntitiesList.Panel2.Controls.Add(this.pgProperties);
            this.scEntitiesList.Size = new System.Drawing.Size(280, 544);
            this.scEntitiesList.SplitterDistance = 257;
            this.scEntitiesList.TabIndex = 1;
            // 
            // tvParticleEmitters
            // 
            this.tvParticleEmitters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tvParticleEmitters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvParticleEmitters.Location = new System.Drawing.Point(0, 0);
            this.tvParticleEmitters.Name = "tvParticleEmitters";
            this.tvParticleEmitters.Size = new System.Drawing.Size(280, 257);
            this.tvParticleEmitters.TabIndex = 0;
            this.tvParticleEmitters.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvParticleEmitters_BeforeSelect);
            this.tvParticleEmitters.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvParticleEmitters_AfterSelect);
            // 
            // pgProperties
            // 
            this.pgProperties.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pgProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgProperties.Location = new System.Drawing.Point(0, 0);
            this.pgProperties.Name = "pgProperties";
            this.pgProperties.Size = new System.Drawing.Size(280, 283);
            this.pgProperties.TabIndex = 0;
            this.pgProperties.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgProperties_PropertyValueChanged);
            // 
            // cmnuScript
            // 
            this.cmnuScript.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewEditScriptToolStripMenuItem,
            this.generateScriptToolStripMenuItem,
            this.deleteScriptToolStripMenuItem});
            this.cmnuScript.Name = "cmnuTileSheetRoot";
            this.cmnuScript.Size = new System.Drawing.Size(182, 70);
            // 
            // viewEditScriptToolStripMenuItem
            // 
            this.viewEditScriptToolStripMenuItem.Name = "viewEditScriptToolStripMenuItem";
            this.viewEditScriptToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.viewEditScriptToolStripMenuItem.Text = "View/Edit";
            // 
            // generateScriptToolStripMenuItem
            // 
            this.generateScriptToolStripMenuItem.Name = "generateScriptToolStripMenuItem";
            this.generateScriptToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.generateScriptToolStripMenuItem.Text = "Generate New Script";
            // 
            // deleteScriptToolStripMenuItem
            // 
            this.deleteScriptToolStripMenuItem.Name = "deleteScriptToolStripMenuItem";
            this.deleteScriptToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.deleteScriptToolStripMenuItem.Text = "Delete Script";
            this.deleteScriptToolStripMenuItem.Visible = false;
            // 
            // cmnuScriptRoot
            // 
            this.cmnuScriptRoot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addScriptToolStripMenuItem});
            this.cmnuScriptRoot.Name = "contextMenuStrip3";
            this.cmnuScriptRoot.Size = new System.Drawing.Size(130, 26);
            // 
            // addScriptToolStripMenuItem
            // 
            this.addScriptToolStripMenuItem.Name = "addScriptToolStripMenuItem";
            this.addScriptToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.addScriptToolStripMenuItem.Text = "Add Script";
            // 
            // cmnuParticleEmitter
            // 
            this.cmnuParticleEmitter.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteParticleEmitterToolStripMenuItem});
            this.cmnuParticleEmitter.Name = "cmnuTileSheetRoot";
            this.cmnuParticleEmitter.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteParticleEmitterToolStripMenuItem
            // 
            this.deleteParticleEmitterToolStripMenuItem.Name = "deleteParticleEmitterToolStripMenuItem";
            this.deleteParticleEmitterToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteParticleEmitterToolStripMenuItem.Text = "Delete";
            // 
            // cmnuParticle
            // 
            this.cmnuParticle.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteParticleToolStripMenuItem});
            this.cmnuParticle.Name = "cmnuTileSheetRoot";
            this.cmnuParticle.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteParticleToolStripMenuItem
            // 
            this.deleteParticleToolStripMenuItem.Name = "deleteParticleToolStripMenuItem";
            this.deleteParticleToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteParticleToolStripMenuItem.Text = "Delete";
            // 
            // ParticleEmittersEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scParticleEmitters);
            this.Name = "ParticleEmittersEditorControl";
            this.Size = new System.Drawing.Size(998, 544);
            this.scParticleEmitters.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scParticleEmitters)).EndInit();
            this.scParticleEmitters.ResumeLayout(false);
            this.scEntitiesList.Panel1.ResumeLayout(false);
            this.scEntitiesList.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scEntitiesList)).EndInit();
            this.scEntitiesList.ResumeLayout(false);
            this.cmnuScript.ResumeLayout(false);
            this.cmnuScriptRoot.ResumeLayout(false);
            this.cmnuParticleEmitter.ResumeLayout(false);
            this.cmnuParticle.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer scParticleEmitters;
        private System.Windows.Forms.SplitContainer scEntitiesList;
        private System.Windows.Forms.TreeView tvParticleEmitters;
        private System.Windows.Forms.PropertyGrid pgProperties;
        private System.Windows.Forms.ContextMenuStrip cmnuScript;
        private System.Windows.Forms.ToolStripMenuItem viewEditScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteScriptToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuScriptRoot;
        private System.Windows.Forms.ToolStripMenuItem addScriptToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuParticleEmitter;
        private System.Windows.Forms.ToolStripMenuItem deleteParticleEmitterToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmnuParticle;
        private System.Windows.Forms.ToolStripMenuItem deleteParticleToolStripMenuItem;
    }
}
