using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FiremelonEditor2
{
    public class CheckeredBackgroundGenerator : IBackgroundGenerator
    {
        #region Properties

        public Bitmap BackgroundImage
        {
            get { return bmpBackground_; }
        }
        private Bitmap bmpBackground_ = null;
        
        public BackgroundColorScheme BackgroundColorScheme
        {
            get { return backgroundColorScheme_; }
            set { backgroundColorScheme_ = value; }
        }
        private BackgroundColorScheme backgroundColorScheme_ = BackgroundColorScheme.StandardLight;
        
        #endregion

        #region Public Functions

        public void Regenerate()
        {
            generateBackground_ = true;
        }
        private bool generateBackground_;
        
        public Bitmap GenerateBackground(int width, int height)
        {
            if (generateBackground_ == true || bmpBackground_ == null)
            {
                // Dispose of the old background object.
                if (bmpBackground_ != null)
                {
                    bmpBackground_.Dispose();
                    bmpBackground_ = null;
                }

                bmpBackground_ = new Bitmap(width, height);
                
                generateBackground(width, height);
                
                generateBackground_ = false;
            }

            return bmpBackground_;
        }

        #endregion

        #region Private Functions

        private void generateBackground(int width, int height)
        {
            Graphics gBackground = Graphics.FromImage(bmpBackground_);

            SolidBrush bTransparency1;
            SolidBrush bTransparency2;

            switch (BackgroundColorScheme)
            {
                case BackgroundColorScheme.StandardDark:

                    bTransparency1 = new SolidBrush(Color.FromArgb(44, 44, 44));
                    bTransparency2 = new SolidBrush(Color.Black);

                    break;

                case BackgroundColorScheme.Vivid:

                    bTransparency1 = new SolidBrush(Color.Magenta);
                    bTransparency2 = new SolidBrush(Color.FromArgb(105, 3, 128));

                    break;

                case BackgroundColorScheme.StandardLight:
                default:

                    bTransparency1 = new SolidBrush(Color.LightGray);
                    bTransparency2 = new SolidBrush(Color.White);

                    break;

            }

            bool toggle = true;
            int row = 0;
            int boxSize = 8;
            int sliceOffX = 0;
            int sliceOffY = 0;

            for (int j = 0; j < height + boxSize; j += boxSize)
            {
                for (int i = 0; i < width + boxSize; i += boxSize)
                {
                    sliceOffX = 0;
                    sliceOffY = 0;

                    if (i + boxSize > width)
                    {
                        sliceOffX = (i + boxSize) - width;
                    }

                    if (j + boxSize > height)
                    {
                        sliceOffY = (j + boxSize) - height;
                    }

                    if (toggle)
                    {
                        gBackground.FillRectangle(bTransparency1, i, j, boxSize - sliceOffX, boxSize - sliceOffY);
                    }
                    else
                    {
                        gBackground.FillRectangle(bTransparency2, i, j, boxSize - sliceOffX, boxSize - sliceOffY);
                    }

                    toggle = !toggle;
                }

                row++;
                toggle = (row % 2 == 0);
            }

            gBackground.Dispose();
        }
        
        #endregion
    }
}
