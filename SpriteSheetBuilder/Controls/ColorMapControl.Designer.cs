namespace SpriteSheetBuilder
{
    partial class ColorMapControl
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
            this.btnFromColor = new System.Windows.Forms.Button();
            this.btnToColor = new System.Windows.Forms.Button();
            this.lbArrow = new System.Windows.Forms.Label();
            this.dlgColorChooser = new System.Windows.Forms.ColorDialog();
            this.SuspendLayout();
            // 
            // btnFromColor
            // 
            this.btnFromColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFromColor.Location = new System.Drawing.Point(3, 3);
            this.btnFromColor.Name = "btnFromColor";
            this.btnFromColor.Size = new System.Drawing.Size(15, 15);
            this.btnFromColor.TabIndex = 0;
            this.btnFromColor.UseVisualStyleBackColor = true;
            // 
            // btnToColor
            // 
            this.btnToColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToColor.Location = new System.Drawing.Point(42, 3);
            this.btnToColor.Name = "btnToColor";
            this.btnToColor.Size = new System.Drawing.Size(15, 15);
            this.btnToColor.TabIndex = 1;
            this.btnToColor.UseVisualStyleBackColor = true;
            this.btnToColor.Click += new System.EventHandler(this.btnToColor_Click);
            // 
            // lbArrow
            // 
            this.lbArrow.AutoSize = true;
            this.lbArrow.Location = new System.Drawing.Point(21, 4);
            this.lbArrow.Name = "lbArrow";
            this.lbArrow.Size = new System.Drawing.Size(19, 13);
            this.lbArrow.TabIndex = 2;
            this.lbArrow.Text = "→";
            // 
            // dlgColorChooser
            // 
            this.dlgColorChooser.FullOpen = true;
            // 
            // ColorMapControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnToColor);
            this.Controls.Add(this.btnFromColor);
            this.Controls.Add(this.lbArrow);
            this.Name = "ColorMapControl";
            this.Size = new System.Drawing.Size(60, 20);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFromColor;
        private System.Windows.Forms.Button btnToColor;
        private System.Windows.Forms.Label lbArrow;
        private System.Windows.Forms.ColorDialog dlgColorChooser;
    }
}
