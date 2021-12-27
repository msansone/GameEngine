using System;
using System.Drawing;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    class SheetUtility : ISheetUtility
    {
        public void SplitImageIntoCells(BitmapResourceDto bitmap, int cols, int rows, int cellWidth, int cellHeight, float scaleFactor)
        {
            // Clear out the ImageList and repopulate it.          
            bitmap.SpriteSheetImageList.Clear();

            if (rows > 0 && cols > 0 && cellHeight > 0 && cellWidth > 0 && scaleFactor > 0)
            {
                try
                {
                    int scaledWidth = ((int)(cellWidth * scaleFactor));

                    int scaledHeight = ((int)(cellHeight * scaleFactor));

                    bitmap.SpriteSheetImageList.ImageSize = new System.Drawing.Size(scaledWidth, scaledHeight);

                    // Load in each cell.
                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < cols; j++)
                        {
                            // If the image was larger than the max ImageList image size (256), it must be broken into separate smaller images.
                            // When the ImageSize property was set, it will have calcualted how many sub-rows/columns are needed.
                            for (int k = 0; k < bitmap.SpriteSheetImageList.ImageListRows; k++)
                            {
                                for (int l = 0; l < bitmap.SpriteSheetImageList.ImageListCols; l++)
                                {
                                    int imageIndex = (i * rows) + j;

                                    Size imageSize = bitmap.SpriteSheetImageList.GetImageSize(l, k);

                                    // Copy the current image region from the source to a destination bitmap, and add it to the image list.

                                    // Create the destination bitmap and associated graphics object.
                                    System.Drawing.Bitmap bmpDest = new System.Drawing.Bitmap(imageSize.Width, imageSize.Height);
                                    System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmpDest);

                                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                                    // Copy the source subimage to the destination.

                                    //bool doScaling = true;

                                    //if (doScaling)
                                    //{
                                    System.Drawing.Rectangle source = new System.Drawing.Rectangle();
                                    source.Width = cellWidth;
                                    source.Height = cellHeight;
                                    //source.Width = (int)(cellWidth / scaleFactor);
                                    //source.Height = (int)(cellHeight / scaleFactor);

                                    // Calculate where the source subimage is located. I don't know why it needs to be negative, but it does.
                                    source.X = (int)((j * cellWidth) + (l * Globals.maxImageListWidth) / scaleFactor);
                                    source.Y = (int)((i * cellHeight) + (k * Globals.maxImageListWidth) / scaleFactor);

                                    // Scale the destination by the scaling factor.
                                    int destinationWidth = scaledWidth;

                                    int destinationHeight = scaledHeight;

                                    System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(0, 0, destinationWidth, destinationHeight);

                                    g.DrawImage(bitmap.BitmapImageWithTransparency, destRect, source.X, source.Y, source.Width, source.Height, GraphicsUnit.Pixel, null);
                                    //}
                                    //else
                                    //{
                                    //    // Calculate where the source subimage is located. I don't know why it needs to be negative, but it does.
                                    //    source.X = (-j * cellWidth) - (l * Globals.maxImageListWidth);
                                    //    source.Y = (-i * cellHeight) - (k * Globals.maxImageListWidth);

                                    //    // Copy the source subimage to the destination.
                                    //    g.DrawImageUnscaled(bitmap.BitmapImageWithTransparency, source);
                                    //}

                                    bitmap.SpriteSheetImageList.AddImage((System.Drawing.Image)bmpDest, l, k);

                                    g.Dispose();
                                }
                            }

                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error building sheet cells");
                }
            }
        }
    }
}
