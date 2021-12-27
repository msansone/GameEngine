using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public class SpriteSheetImageList
    {
        #region Constructor

        public SpriteSheetImageList()
        {
        }

        #endregion

        #region Private Variables
        
        #endregion

        #region Properties
        
        public int Count
        {
            get
            {
                if (imageLists_.Count == 0)
                {
                    return 0;
                }
                else
                {
                    return imageLists_[0][0].Images.Count;
                }
            }
        }

        private Size imageSize_ = new Size(0, 0);
        public Size ImageSize
        {
            get { return imageSize_; }
            set
            {
                imageSize_ = value;
                
                // Max size of image in imagelist is 256x256. If either dimension is larger, we will need multiple image lists.
                imageListCols_ = Convert.ToInt32(imageSize_.Width / (Globals.maxImageListWidth + 1)) + 1;
                imageListRows_ = Convert.ToInt32(imageSize_.Height / (Globals.maxImageListHeight + 1)) + 1;

                createImageLists(imageListCols_, imageListRows_, imageSize_);
            }
        }
        
        int imageListCols_ = 0;
        public int ImageListCols
        {
            get { return imageListCols_; }
        }

        int imageListRows_ = 0;
        public int ImageListRows
        {
            get { return imageListRows_; }
        }


        private List<List<ImageList>> imageLists_ = new List<List<ImageList>>();
        public List<List<ImageList>> ImageLists
        {
            get { return imageLists_; }
        }

        #endregion

        #region Public Functions

        public void AddImage(System.Drawing.Image image, int imageListColumn, int imageListRow)
        {          
            imageLists_[imageListRow][imageListColumn].Images.Add(image);
        }

        public void Clear()
        {
            disposeImageLists();
        }

        public Size GetImageSize(int imageListColumn, int imageListRow)
        {
            return imageLists_[imageListRow][imageListColumn].ImageSize;
        }

        #endregion

        #region Private functions

        private void createImageLists(int cols, int rows, Size imageSize)
        {
            disposeImageLists();

            for (int i = 0; i < rows; i++)
            {
                // Append a row and populate its columns with new ImageList objects.
                imageLists_.Add(new List<ImageList>());
                
                for (int j = 0; j < cols; j++)
                {
                    ImageList newImageList = new ImageList();
                    newImageList.ColorDepth = ColorDepth.Depth32Bit;
                    newImageList.TransparentColor = Color.Magenta;

                    Size newImageSize = new Size(imageSize.Width, imageSize.Height);

                    // If this image has to be broken up because it is too large...
                    if (imageSize.Height > Globals.maxImageListHeight)
                    {
                        // If this is the final row, and it does not fit exactly, calculate what the height should be. Otherwise use the max height.
                        if (i == rows - 1 && imageSize.Height % Globals.maxImageListHeight > 0)
                        {
                            newImageSize.Height = imageSize.Height % Globals.maxImageListHeight;
                        }
                        else
                        {
                            newImageSize.Height = Globals.maxImageListHeight;
                        }
                    }

                    if (imageSize.Width > Globals.maxImageListWidth)
                    {
                        // If this is the final col, and it does not fit exactly calculate what the width should be. Otherwise use the max width.
                        if (j == cols - 1 && imageSize.Width % Globals.maxImageListWidth > 0)
                        {
                            newImageSize.Width = imageSize.Width % Globals.maxImageListWidth;
                        }
                        else
                        {
                            newImageSize.Width = Globals.maxImageListWidth;
                        }
                    }

                    newImageList.ImageSize = newImageSize;

                    imageLists_[i].Add(newImageList);
                }
            }
        }

        private void disposeImageLists()
        {
            foreach (List<ImageList> imageListList in imageLists_)
            {
                foreach (ImageList imageList in imageListList)
                {
                    imageList.Images.Clear();
                    imageList.Dispose();
                }

                imageListList.Clear();
            }

            imageLists_.Clear();
        }

        #endregion
    }
}
