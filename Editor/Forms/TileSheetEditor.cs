using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public partial class TileSheetEditor : Form, ITileSheetEditor
    {
        private ITileSheetDtoProxy tileSheetProxy_;

        public TileSheetEditor()
        {
            InitializeComponent();

            vsTileSheet.SmallChange = 1;
            vsTileSheet.LargeChange = 1;
            hsTileSheet.SmallChange = 1;
            hsTileSheet.LargeChange = 1;
        }

        public void ShowDialog(IWin32Window owner, ITileSheetDtoProxy tileSheetProxy)
        {
            tileSheetProxy_ = tileSheetProxy;

            pgTileSheet.SelectedObject = tileSheetProxy_;

            base.ShowDialog(owner);
        }

        private void pnTileSheet_Resize(object sender, EventArgs e)
        {
            resize();
        }

        private void resize()
        {
            hsTileSheet.Top = pnTileSheet.ClientSize.Height - hsTileSheet.Height;
            hsTileSheet.Width = pnTileSheet.ClientSize.Width - vsTileSheet.Width;

            vsTileSheet.Left = pnTileSheet.ClientSize.Width - vsTileSheet.Width;
            vsTileSheet.Height = pnTileSheet.ClientSize.Height - hsTileSheet.Height;

            pbTileSheet.Width = pnTileSheet.Width - vsTileSheet.Width - 1;
            pbTileSheet.Height = pnTileSheet.Height - hsTileSheet.Height - 1;

            int vScrollMax = ((int)(tileSheetProxy_.Image.Height * tileSheetProxy_.ScaleFactor)) - pbTileSheet.Height;
            
            if (vScrollMax > 0)
            {
                vsTileSheet.Maximum = vScrollMax;
            }
            else
            {
                vsTileSheet.Maximum = vsTileSheet.Minimum;
            }

            int hScrollMax = ((int)(tileSheetProxy_.Image.Width * tileSheetProxy_.ScaleFactor)) - pbTileSheet.Width;

            if (hScrollMax > 0)
            {
                hsTileSheet.Maximum = hScrollMax;
            }
            else
            {
                hsTileSheet.Maximum = hsTileSheet.Minimum;
            }

            pbTileSheet.Refresh();
        }

        private void TileSheetEditor_Load(object sender, EventArgs e)
        {
            resize();
        }

        private void pbTileSheet_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            float scaleFactor = tileSheetProxy_.ScaleFactor;

            int hscrollOffset = -1 * hsTileSheet.Value;

            int vscrollOffset = -1 * vsTileSheet.Value;

            Bitmap bmpSheet = tileSheetProxy_.Image;
            
            int sourceWidth = bmpSheet.Width;

            int sourceHeight = bmpSheet.Height;

            // Scale the destination by the scaling factor.
            int destinationWidth = (int)(sourceWidth * scaleFactor);

            int destinationHeight = (int)(sourceHeight * scaleFactor);

            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(hscrollOffset, vscrollOffset, destinationWidth, destinationHeight);

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            g.DrawImage(bmpSheet, destRect, 0, 0, bmpSheet.Width, bmpSheet.Height, GraphicsUnit.Pixel, null);
            
            //g.DrawImage(bmpSheet, new System.Drawing.Rectangle(hscrollOffset, vscrollOffset, bmpSheet.Width, bmpSheet.Height), 0, 0, bmpSheet.Width, bmpSheet.Height, GraphicsUnit.Pixel, null);

            int cols = tileSheetProxy_.Columns;
            int rows = tileSheetProxy_.Rows;
            int height = tileSheetProxy_.CellHeight;
            int width = tileSheetProxy_.CellWidth;

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

        private void vsTileSheet_Scroll(object sender, ScrollEventArgs e)
        {
            pbTileSheet.Refresh();
        }

        private void hsTileSheet_Scroll(object sender, ScrollEventArgs e)
        {
            pbTileSheet.Refresh();
        }

        private void pgTileSheet_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            switch (e.ChangedItem.Label.ToUpper())
            {
                case "IMAGEPATH":

                    // Scroll bars likely need update.
                    resize();

                    break;

                case "SCALEFACTOR":

                    // Scroll bars likely need update and the image needs refreshed.
                    resize();

                    break;
            }
        }
    }
}
