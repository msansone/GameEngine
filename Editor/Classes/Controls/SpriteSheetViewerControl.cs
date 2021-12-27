using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public partial class SpriteSheetViewerControl : UserControl, ISheetViewerControl
    {

        public SpriteSheetViewerControl(IProjectController projectController)
        {
            InitializeComponent();
            
            vsImage.SmallChange = 1;
            vsImage.LargeChange = 1;

            hsImage.SmallChange = 1;
            hsImage.LargeChange = 1;

            projectController_ = projectController;
        }

        #region Private Variables

        IProjectController projectController_;

        #endregion

        #region Properties

        public ISheetDtoProxy Sheet
        {
            get
            {
                return sheetDtoProxy_;
            }
            set
            {
                sheetDtoProxy_ = value;

                RefreshImage();
            }
        }
        private ISheetDtoProxy sheetDtoProxy_ = null;

        #endregion

        #region Public Functions

        public void RefreshImage()
        {
            resize();

            pbImage.Refresh();
        }

        #endregion

        #region Private Functions

        private void resize()
        {
            hsImage.Top = this.ClientSize.Height - hsImage.Height;
            hsImage.Width = this.ClientSize.Width - vsImage.Width;

            vsImage.Left = this.ClientSize.Width - vsImage.Width;
            vsImage.Height = this.ClientSize.Height - hsImage.Height;

            pbImage.Width = this.Width - vsImage.Width - 1;
            pbImage.Height = this.Height - hsImage.Height - 1;

            if (sheetDtoProxy_ != null)
            {
                BitmapResourceDto bitmapResource = projectController_.GetBitmapResource(sheetDtoProxy_.BitmapResourceId, true);

                bitmapResource.LoadedModules ^= (byte)EditorModule.SpriteSheetViewer;

                int vScrollMax = ((int)(sheetDtoProxy_.Image.Height * sheetDtoProxy_.ScaleFactor)) - pbImage.Height;

                if (vScrollMax > 0)
                {
                    vsImage.Maximum = vScrollMax;
                }
                else
                {
                    vsImage.Maximum = vsImage.Minimum;
                }

                int hScrollMax = ((int)(sheetDtoProxy_.Image.Width * sheetDtoProxy_.ScaleFactor)) - pbImage.Width;

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
            if (sheetDtoProxy_ != null)
            {
                float scaleFactor = sheetDtoProxy_.ScaleFactor;

                int hscrollOffset = -1 * hsImage.Value;

                int vscrollOffset = -1 * vsImage.Value;
                
                BitmapResourceDto bitmapResource = projectController_.GetBitmapResource(sheetDtoProxy_.BitmapResourceId, true);

                bitmapResource.LoadedModules ^= (byte)EditorModule.SpriteSheetViewer;
                
                int sourceWidth = bitmapResource.BitmapImage.Width;

                int sourceHeight = bitmapResource.BitmapImage.Height;

                // Scale the destination by the scaling factor.
                int destinationWidth = (int)(sourceWidth * scaleFactor);

                int destinationHeight = (int)(sourceHeight * scaleFactor);

                System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(hscrollOffset, vscrollOffset, destinationWidth, destinationHeight);

                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                g.DrawImage(bitmapResource.BitmapImage, destRect, 0, 0, bitmapResource.BitmapImage.Width, bitmapResource.BitmapImage.Height, GraphicsUnit.Pixel, null);

                //g.DrawImage(bmpSheet, new System.Drawing.Rectangle(hscrollOffset, vscrollOffset, bmpSheet.Width, bmpSheet.Height), 0, 0, bmpSheet.Width, bmpSheet.Height, GraphicsUnit.Pixel, null);

                int cols = sheetDtoProxy_.Columns;
                int rows = sheetDtoProxy_.Rows;
                
                int height = (int)((sheetDtoProxy_.CellHeight + sheetDtoProxy_.Padding) * scaleFactor);
                int width = (int)((sheetDtoProxy_.CellWidth + sheetDtoProxy_.Padding) * scaleFactor);

                Pen p = new Pen(Color.Black, 1);
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                int lineHeight = height * rows;

                int lineWidth = width * cols;

                int padding = (int)(sheetDtoProxy_.Padding * scaleFactor);

                for (int i = 0; i <= rows; i++)
                {
                    // Draw two lines, for the start and end of the padding size.
                    g.DrawLine(p, hscrollOffset, (i * height) + vscrollOffset, lineWidth + hscrollOffset, (i * height) + vscrollOffset);
                    g.DrawLine(p, hscrollOffset, ((i * height) - padding) + vscrollOffset, lineWidth + hscrollOffset, ((i * height) - padding) + vscrollOffset);
                }

                for (int i = 0; i <= cols; i++)
                {
                    g.DrawLine(p, (i * width) + hscrollOffset, vscrollOffset, (i * width) + hscrollOffset, lineHeight + vscrollOffset);
                    g.DrawLine(p, ((i * width) - padding) + hscrollOffset, vscrollOffset, ((i * width) - padding) + hscrollOffset, lineHeight + vscrollOffset);
                }
            }
        }

        #endregion

        #region Event Handlers

        private void vsImage_Scroll(object sender, ScrollEventArgs e)
        {
            pbImage.Refresh();
        }

        private void hsImage_Scroll(object sender, ScrollEventArgs e)
        {
            pbImage.Refresh();
        }

        private void SheetViewerControl_Resize(object sender, EventArgs e)
        {
            resize();
        }

        private void pbImage_Paint(object sender, PaintEventArgs e)
        {
            paint(e.Graphics);
        }

        #endregion

    }
}
