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
    public partial class SpriteSheetEditor : Form, ISpriteSheetEditor
    {
        private ISpriteSheetDtoProxy spriteSheetProxy_;

        public SpriteSheetEditor()
        {
            InitializeComponent();

            vsSpriteSheet.SmallChange = 1;
            vsSpriteSheet.LargeChange = 1;
            hsSpriteSheet.SmallChange = 1;
            hsSpriteSheet.LargeChange = 1;
        }

        public void ShowDialog(IWin32Window owner, ISpriteSheetDtoProxy spriteSheetProxy)
        {
            spriteSheetProxy_ = spriteSheetProxy;

            pgSpriteSheet.SelectedObject = spriteSheetProxy_;

            base.ShowDialog(owner);
        }


        private void resize()
        {
            hsSpriteSheet.Top = pnSpriteSheet.ClientSize.Height - hsSpriteSheet.Height;
            hsSpriteSheet.Width = pnSpriteSheet.ClientSize.Width - vsSpriteSheet.Width;

            vsSpriteSheet.Left = pnSpriteSheet.ClientSize.Width - vsSpriteSheet.Width;
            vsSpriteSheet.Height = pnSpriteSheet.ClientSize.Height - hsSpriteSheet.Height;

            pbSpriteSheet.Width = pnSpriteSheet.Width - vsSpriteSheet.Width - 1;
            pbSpriteSheet.Height = pnSpriteSheet.Height - hsSpriteSheet.Height - 1;

            int vScrollMax = ((int)(spriteSheetProxy_.Image.Height * spriteSheetProxy_.ScaleFactor)) - pbSpriteSheet.Height;

            if (vScrollMax > 0)
            {
                vsSpriteSheet.Maximum = vScrollMax;
            }
            else
            {
                vsSpriteSheet.Maximum = vsSpriteSheet.Minimum;
            }

            int hScrollMax = ((int)(spriteSheetProxy_.Image.Width * spriteSheetProxy_.ScaleFactor)) - pbSpriteSheet.Width;

            if (hScrollMax > 0)
            {
                hsSpriteSheet.Maximum = hScrollMax;
            }
            else
            {
                hsSpriteSheet.Maximum = hsSpriteSheet.Minimum;
            }

            pbSpriteSheet.Refresh();
        }

        private void pbSpriteSheet_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            float scaleFactor = spriteSheetProxy_.ScaleFactor;
            
            // Possible exception thrown if too many large bitmaps get allocated without garbage collection occurring.
            // Force garbage collection to prevent this.
            GC.Collect();
            
            int bitmapWidth = (int)(spriteSheetProxy_.Image.Width * scaleFactor);

            int bitmapHeight = (int)(spriteSheetProxy_.Image.Height * scaleFactor);

            Bitmap bmpCanvas = new Bitmap(bitmapWidth, bitmapHeight);

            Graphics g2 = Graphics.FromImage(bmpCanvas);

            int hscrollOffset = -1 * hsSpriteSheet.Value;

            int vscrollOffset = -1 * vsSpriteSheet.Value;

            Bitmap bmpSheet = spriteSheetProxy_.Image;

            int sourceWidth = bmpSheet.Width;

            int sourceHeight = bmpSheet.Height;

            // Scale the destination by the scaling factor.
            int destinationWidth = (int)(sourceWidth * scaleFactor);

            int destinationHeight = (int)(sourceHeight * scaleFactor);

            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(hscrollOffset, vscrollOffset, destinationWidth, destinationHeight);

            g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            g2.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            g2.DrawImage(bmpSheet, destRect, 0, 0, bmpSheet.Width, bmpSheet.Height, GraphicsUnit.Pixel, null);

            int cols = spriteSheetProxy_.Columns;

            int rows = spriteSheetProxy_.Rows;

            int height = (int)(spriteSheetProxy_.CellHeight * scaleFactor);

            int width = (int)(spriteSheetProxy_.CellWidth * scaleFactor);

            Pen p = new Pen(Color.Black, 1);

            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            for (int i = 0; i <= rows; i++)
            {
                g2.DrawLine(p, hscrollOffset, (i * height) + vscrollOffset, destinationWidth + hscrollOffset, (i * height) + vscrollOffset);
            }

            for (int i = 0; i <= cols; i++)
            {
                g2.DrawLine(p, (i * width) + hscrollOffset, vscrollOffset, (i * width) + hscrollOffset, destinationHeight + vscrollOffset);
            }

            g.DrawImage(bmpCanvas, 0, 0);

            g2.Dispose();
        }

        private void pnSpriteSheet_Resize(object sender, EventArgs e)
        {
            resize();
        }

        private void SpriteSheetEditor_Load(object sender, EventArgs e)
        {
            resize();
        }

        private void vsSpriteSheet_Scroll(object sender, ScrollEventArgs e)
        {
            pbSpriteSheet.Refresh();
        }

        private void hsSpriteSheet_Scroll(object sender, ScrollEventArgs e)
        {
            pbSpriteSheet.Refresh();
        }

        private void pgSpriteSheet_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
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

            pbSpriteSheet.Refresh();
        }
    }
}
