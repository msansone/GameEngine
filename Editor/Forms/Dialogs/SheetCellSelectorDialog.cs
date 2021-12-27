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
    using ProjectDto = ProjectDto_2_2;

    public partial class SheetCellSelectorDialog : Form, ISheetCellSelectorDialog
    {
        private Bitmap bitmapImage_;

        private IProjectController projectController_;

        private int selectedCellColumn_ = -1;

        private int selectedCellRow_ = -1;

        private SpriteSheetDto spriteSheet_;
        
        public SheetCellSelectorDialog()
        {
            InitializeComponent();
        }

        public int? CellIndex
        {
            get { return cellIndex_; }
            set { cellIndex_ = value; }
        }
        private int? cellIndex_;
        
        public void ShowDialog(IWin32Window owner, SpriteSheetDto spriteSheet, IProjectController projectController)
        {
            projectController_ = projectController;

            spriteSheet_ = spriteSheet;

            if (cellIndex_.HasValue == true)
            {
                selectedCellColumn_ = cellIndex_.Value % spriteSheet_.Columns;

                selectedCellRow_ = cellIndex_.Value / spriteSheet_.Columns;
            }
            else
            {
                selectedCellColumn_ = -1;

                selectedCellRow_ = -1;
            }

            Guid bitmapResourceId = spriteSheet_.BitmapResourceId;

            // Separate resources dto removed in 2.2 format.
            //ProjectResourcesDto resources = projectController_.GetResources();
            //BitmapResourceDto bitmapResource = resources.Bitmaps[bitmapResourceId];

            ProjectDto project = projectController_.GetProjectDto();

            BitmapResourceDto bitmapResource = projectController_.GetBitmapResource(bitmapResourceId, false);

            // findmebitmap Free the resource when the dialogue is closed. Don't store a module level bitmapImage (see next line of code)

            bitmapImage_ = bitmapResource.BitmapImage;
            
            base.ShowDialog(owner);
        }

        private void resize()
        {
            hsSpriteSheet.Top = ssCellInfo.Top - hsSpriteSheet.Height;
            hsSpriteSheet.Width = ClientSize.Width - vsSpriteSheet.Width;

            vsSpriteSheet.Left = ClientSize.Width - vsSpriteSheet.Width;
            vsSpriteSheet.Height = ClientSize.Height - hsSpriteSheet.Height - ssCellInfo.Height;

            pbSpriteSheet.Width = ClientRectangle.Width - vsSpriteSheet.Width - 1;
            pbSpriteSheet.Height = ClientRectangle.Height - hsSpriteSheet.Height - ssCellInfo.Height - 1;


            //int vScrollMax = ((int)(bitmapResource.BitmapImage.Height * spriteSheet_.ScaleFactor)) - pbSpriteSheet.Height;
            int vScrollMax = ((int)bitmapImage_.Height) - pbSpriteSheet.Height;

            if (vScrollMax > 0)
            {
                vsSpriteSheet.Maximum = vScrollMax;
            }
            else
            {
                vsSpriteSheet.Maximum = vsSpriteSheet.Minimum;
            }

            //int hScrollMax = ((int)(bitmapResource.BitmapImage.Width * spriteSheet_.ScaleFactor)) - pbSpriteSheet.Width;
            int hScrollMax = ((int)bitmapImage_.Width) - pbSpriteSheet.Width;

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

        private void SheetCellSelectorDialog_Resize(object sender, EventArgs e)
        {
            resize();
        }

        private void SheetCellSelectorDialog_Load(object sender, EventArgs e)
        {
            resize();
        }

        private void pbSpriteSheet_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            float scaleFactor = 1.0f; // spriteSheet_.ScaleFactor;

            // Possible exception thrown if too many large bitmaps get allocated without garbage collection occurring.
            // Force garbage collection to prevent this.
            GC.Collect();

            int bitmapWidth = (int)(bitmapImage_.Width * scaleFactor);

            int bitmapHeight = (int)(bitmapImage_.Height * scaleFactor);

            Bitmap bmpCanvas = new Bitmap(bitmapWidth, bitmapHeight);

            Graphics g2 = Graphics.FromImage(bmpCanvas);

            int hscrollOffset = -1 * hsSpriteSheet.Value;

            int vscrollOffset = -1 * vsSpriteSheet.Value;
            
            int sourceWidth = bitmapImage_.Width;

            int sourceHeight = bitmapImage_.Height;

            // Scale the destination by the scaling factor.
            int destinationWidth = (int)(sourceWidth * scaleFactor);

            int destinationHeight = (int)(sourceHeight * scaleFactor);

            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(hscrollOffset, vscrollOffset, destinationWidth, destinationHeight);

            g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            g2.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            g2.DrawImage(bitmapImage_, destRect, 0, 0, bitmapImage_.Width, bitmapImage_.Height, GraphicsUnit.Pixel, null);

            int cols = spriteSheet_.Columns;

            int rows = spriteSheet_.Rows;

            int height = (int)(spriteSheet_.CellHeight * scaleFactor);

            int width = (int)(spriteSheet_.CellWidth * scaleFactor);

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

            Pen pSelectedCell = new Pen(Color.Orange, 2.0f);

            int x = (selectedCellColumn_ * width) + hscrollOffset;

            int y = (selectedCellRow_ * height) + vscrollOffset;

            g2.DrawRectangle(pSelectedCell, x, y, width, height);
            
            g.DrawImage(bmpCanvas, 0, 0);
            
            g2.Dispose();
        }

        private void vsSpriteSheet_Scroll(object sender, ScrollEventArgs e)
        {
            pbSpriteSheet.Refresh();
        }

        private void hsSpriteSheet_Scroll(object sender, ScrollEventArgs e)
        {
            pbSpriteSheet.Refresh();
        }

        private void pbSpriteSheet_MouseDown(object sender, MouseEventArgs e)
        {
            int clickedRow = (e.Y + vsSpriteSheet.Value) / spriteSheet_.CellHeight;

            int clickedCol = (e.X + hsSpriteSheet.Value) / spriteSheet_.CellWidth;
            
            if (clickedRow < spriteSheet_.Rows && clickedCol < spriteSheet_.Columns)
            {
                selectedCellColumn_ = clickedCol;

                selectedCellRow_ = clickedRow;

                cellIndex_ = (clickedRow * spriteSheet_.Columns) + clickedCol;

                tsslRow.Text = "Row: " + selectedCellRow_;

                tsslColumn.Text = "Column: " + selectedCellColumn_;

                tsslCellIndex.Text = "Index: " + cellIndex_;
                
                pbSpriteSheet.Refresh();
            }
        }

        private void SheetCellSelectorDialog_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void pbSpriteSheet_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Hide();
        }
    }

    public interface ISheetCellSelectorDialog
    {
        int? CellIndex { get; }
        
        void ShowDialog(IWin32Window owner, SpriteSheetDto spriteSheet, IProjectController projectController);
    }
}
