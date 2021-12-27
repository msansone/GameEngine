using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpriteSheetBuilder
{
    public partial class SpriteSheetBuilderDialog : Form, ISpriteSheetBuilderDialog
    {
        #region Constructors

        public SpriteSheetBuilderDialog()
        {
            InitializeComponent();

            spriteSheetBuilder = new SpriteSheetBuilderControl();

            SpriteSheetBuilderControl spriteSheetBuilderControl = (SpriteSheetBuilderControl)spriteSheetBuilder;

            spriteSheetBuilderControl.Dock = DockStyle.Fill;
            
            this.Controls.Add(spriteSheetBuilderControl);

            spriteSheetBuilderControl.BringToFront();
        }

        #endregion

        #region Private Variables

        ISpriteSheetBuilderControl spriteSheetBuilder;

        #endregion

        #region Public Functions

        new public void ShowDialog(IWin32Window owner)
        {
            base.ShowDialog(owner);
        }

        #endregion

        #region Private Functions
        
        private void enableMenuItems()
        {
            //tsbAddImages.Enabled = true;
            //tsbExportSpriteSheet.Enabled = true;
            sheetToolStripMenuItem.Enabled = true;
            singleImageToolStripMenuItem.Enabled = true;
            stripToolStripMenuItem.Enabled = true;

            addImagesToolStripMenuItem.Enabled = true;
            buildSpriteSheetToolStripMenuItem.Enabled = true;
            saveSpriteSheetBuildFileToolStripMenuItem.Enabled = true;
            buildAlphaMaskSheetToolStripMenuItem.Enabled = true;
        }

        #endregion

        #region Event Handlers

        private void openSpriteSheetBuildFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (spriteSheetBuilder.ChangesMade == true)
            {
                DialogResult res = MessageBox.Show("Changes have been made to the current sprite sheet build file. Do you want to save?", "Save Changes?", MessageBoxButtons.YesNoCancel);

                if (res == DialogResult.Yes)
                {
                    spriteSheetBuilder.SaveBuildFile();
                }
                else if (res == DialogResult.Cancel)
                {
                    return;
                }
            }

            OpenFileDialog openDialog = new OpenFileDialog();

            openDialog.CheckFileExists = true;
            openDialog.CheckPathExists = true;
            openDialog.DefaultExt = "build";
            openDialog.Filter = "Sprite Sheet Build Files (*.build)|*.build";
            openDialog.Multiselect = false;
            openDialog.RestoreDirectory = true;

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                spriteSheetBuilder.OpenBuildFile(openDialog.FileName);

                enableMenuItems();
            }
        }

        private void saveSpriteSheetBuildFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spriteSheetBuilder.SaveBuildFile();
        }

        private void singleImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ofdAddImages.CheckFileExists = true;
            ofdAddImages.CheckPathExists = true;
            ofdAddImages.DefaultExt = "png";
            ofdAddImages.Filter = "PNG Files|*.png";
            ofdAddImages.FileName = string.Empty;
            ofdAddImages.Multiselect = true;
            ofdAddImages.RestoreDirectory = true;

            if (ofdAddImages.ShowDialog() == DialogResult.OK)
            {
                spriteSheetBuilder.AddImages(ImageSourceType.Single, ofdAddImages.FileNames);
            }
        }

        private void stripToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ofdAddImages.CheckFileExists = true;
            ofdAddImages.CheckPathExists = true;
            ofdAddImages.DefaultExt = "png";
            ofdAddImages.Filter = "PNG Files|*.png";
            ofdAddImages.FileName = string.Empty;
            ofdAddImages.Multiselect = true;
            ofdAddImages.RestoreDirectory = true;

            if (ofdAddImages.ShowDialog() == DialogResult.OK)
            {
                spriteSheetBuilder.AddImages(ImageSourceType.Strip, ofdAddImages.FileNames);
            }
        }

        private void sheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ofdAddImages.CheckFileExists = true;
            ofdAddImages.CheckPathExists = true;
            ofdAddImages.DefaultExt = "png";
            ofdAddImages.Filter = "PNG Files|*.png";
            ofdAddImages.FileName = string.Empty;
            ofdAddImages.Multiselect = true;
            ofdAddImages.RestoreDirectory = true;

            if (ofdAddImages.ShowDialog() == DialogResult.OK)
            {
                spriteSheetBuilder.AddImages(ImageSourceType.Sheet, ofdAddImages.FileNames);
            }
        }

        private void SpriteSheetBuilderDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ShowInTaskbar == false)
            {
                e.Cancel = true;

                this.Hide();
            }
        }

        private void exportSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spriteSheetBuilder.ExportSheet();
        }

        private void newSpriteSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            enableMenuItems();

            spriteSheetBuilder.NewSpriteSheet();
        }

        private void buildSpriteSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spriteSheetBuilder.BuildSheet();

            exportSheetToolStripMenuItem.Enabled = true;
        }

        private void buildAlphaMaskSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spriteSheetBuilder.BuildAlphaMask();

            exportSheetToolStripMenuItem.Enabled = true;
        }

        private void SpriteSheetBuilderDialog_Load(object sender, EventArgs e)
        {

        }

        #endregion
    }

    public interface ISpriteSheetBuilderDialog
    {
        #region Properties

        // Derived from base.
        int Width { get; set; }
        int Height { get; set; }
        int Bottom { get; }
        int Left { get; set; }
        int Top { get; set; }
        bool TopLevel { get; set; }

        #endregion

        #region Public Functions

        void Show(IWin32Window owner);
        void ShowDialog(IWin32Window owner);

        void Hide();
        void Close();

        #endregion
    }
}
