using System.Drawing;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public partial class TileObjectViewerControl : UserControl, ITileObjectViewerControl
    {
        public TileObjectViewerControl(IProjectController projectController)
        {
            InitializeComponent();

            firemelonEditorFactory_ = new FiremelonEditorFactory();

            projectController_ = projectController;

            utilityFactor_ = new UtilityFactory();

            bitmapUtility_ = utilityFactor_.NewBitmapUtility();

            // This should be moved into the utility factory.
            backgroundGenerator_ = firemelonEditorFactory_.NewBackgroundGenerator();
        }

        private IFiremelonEditorFactory firemelonEditorFactory_;

        private IProjectController projectController_;

        private IBackgroundGenerator backgroundGenerator_;

        private IBitmapUtility bitmapUtility_;

        private IUtilityFactory utilityFactor_;

        #region Properties

        public ITileSheetDtoProxy TileSheet
        {
            get
            {
                return tileSheetDtoProxy_;
            }
            set
            {
                tileSheetDtoProxy_ = value;

                resize();             
            }
        }
        private ITileSheetDtoProxy tileSheetDtoProxy_ = null;

        public ITileObjectDtoProxy TileObject
        {
            get
            {
                return tileObject_;
            }
            set
            {
                tileObject_ = value;

                resize();
            }
        }
        private ITileObjectDtoProxy tileObject_;

        #endregion

        #region Private Functions

        private void resize()
        {
            ProjectDto project = projectController_.GetProjectDto();

            hsImage.Top = this.ClientSize.Height - hsImage.Height;
            hsImage.Width = this.ClientSize.Width - vsImage.Width;

            vsImage.Left = this.ClientSize.Width - vsImage.Width;
            vsImage.Height = this.ClientSize.Height - hsImage.Height;

            pbImage.Width = this.Width - vsImage.Width - 1;
            pbImage.Height = this.Height - hsImage.Height - 1;

            backgroundGenerator_.Regenerate();

            if (tileObject_ != null && tileSheetDtoProxy_ != null)
            {
                int vScrollMax = ((int)(tileObject_.Rows * project.TileSize)) - pbImage.Height;
                
                if (vScrollMax > 0)
                {
                    vsImage.Maximum = vScrollMax;
                }
                else
                {
                    vsImage.Maximum = vsImage.Minimum;
                }

                int hScrollMax = ((int)(tileObject_.Columns * project.TileSize)) - pbImage.Width;

                if (hScrollMax > 0)
                {
                    hsImage.Maximum = hScrollMax;
                }
                else
                {
                    hsImage.Maximum = hsImage.Minimum;
                }
            }

            pbImage.Refresh();
        }

        private void paint(Graphics g)
        {
            // Render only the portion of the tilesheet that contains the tile object.
            if (tileObject_ != null && tileSheetDtoProxy_ != null)
            {
                ProjectDto project = projectController_.GetProjectDto();

                float scaleFactor = tileSheetDtoProxy_.ScaleFactor;

                int hscrollOffset = -1 * hsImage.Value;

                int vscrollOffset = -1 * vsImage.Value;

                TileSheetDto tileSheet = projectController_.GetTileSheet(tileSheetDtoProxy_.Id);

                BitmapResourceDto bitmapResource = projectController_.GetBitmapResource(tileSheet.BitmapResourceId, true);

                bitmapResource.LoadedModules ^= (byte)EditorModule.TileSheetViewer;
                
                int sourceX = (int)((tileObject_.TopLeftCornerColumn * project.TileSize) / scaleFactor);

                int sourceY = (int)((tileObject_.TopLeftCornerRow * project.TileSize) / scaleFactor);

                int sourceWidth = (int)((tileObject_.Columns * project.TileSize) / scaleFactor);

                int sourceHeight = (int)((tileObject_.Rows * project.TileSize) / scaleFactor);

                System.Drawing.Rectangle sourceRect = new System.Drawing.Rectangle(sourceX, sourceY, sourceWidth, sourceHeight);

                int destinationWidth = (int)(sourceWidth * scaleFactor);

                int destinationHeight = (int)(sourceHeight * scaleFactor);

                System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(hscrollOffset, vscrollOffset, destinationWidth, destinationHeight);

                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                
                // Get the background.
                backgroundGenerator_.GenerateBackground(destRect.Width, destRect.Height);

                g.DrawImageUnscaled(backgroundGenerator_.BackgroundImage, new Point(0, 0));

                g.DrawImage(bitmapResource.BitmapImageWithTransparency, destRect, sourceX, sourceY, sourceWidth, sourceHeight, GraphicsUnit.Pixel, null);
                
                int cols = tileObject_.Columns;
                int rows = tileObject_.Rows;

                int height = tileSheetDtoProxy_.CellHeight;
                int width = tileSheetDtoProxy_.CellWidth;

                Pen p = new Pen(Color.Black, 1);
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                int lineHeight = height * rows;
                int lineWidth = width * cols;

                for (int i = 0; i <= rows; i++)
                {
                    g.DrawLine(p, hscrollOffset, (i * height) + vscrollOffset, lineWidth + hscrollOffset, (i * height) + vscrollOffset);
                }

                for (int i = 0; i <= cols; i++)
                {
                    g.DrawLine(p, (i * width) + hscrollOffset, vscrollOffset, (i * width) + hscrollOffset, lineHeight + vscrollOffset);
                }
            }
        }

        #endregion

        #region Event Handlers

        private void hsImage_Scroll(object sender, ScrollEventArgs e)
        {
            pbImage.Refresh();
        }

        private void pbImage_Paint(object sender, PaintEventArgs e)
        {
            paint(e.Graphics);
        }

        private void TileObjectViewerControl_Resize(object sender, System.EventArgs e)
        {
            resize();
        }

        private void vsImage_Scroll(object sender, ScrollEventArgs e)
        {
            pbImage.Refresh();
        }

        #endregion
    }
}
