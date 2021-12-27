using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FiremelonEditor2
{
    public class OverlayGenerator : IOverlayGenerator
    {
        #region Properties
        
        public OverlayColorScheme OverlayColorScheme
        {
            get { return overlayColorScheme_; }
            set { overlayColorScheme_ = value; }
        }
        private OverlayColorScheme overlayColorScheme_ = OverlayColorScheme.Standard;

        public Bitmap OverlayImage
        {
            get { return bmpOverlay_; }
        }
        private Bitmap bmpOverlay_ = null;
        
        #endregion

        #region Public Functions

        public void Regenerate()
        {
            generateOverlay_ = true;
        }
        private bool generateOverlay_;

        public Bitmap GenerateOverlay(int width, int height, Point2D origin)
        {
            if (generateOverlay_ == true || bmpOverlay_ == null)
            {
                // Dispose of the old overlay object.
                if (bmpOverlay_ != null)
                {
                    bmpOverlay_.Dispose();
                    bmpOverlay_ = null;
                }

                bmpOverlay_ = new Bitmap(width, height);

                generateOverlay(width, height, origin);
                
                generateOverlay_ = false;
            }

            return bmpOverlay_;
        }

        #endregion

        #region Private Functions

        private void generateOverlay(int width, int height, Point2D origin)
        {
            Graphics gOverlay = Graphics.FromImage(bmpOverlay_);

            Pen pAxes;

            switch (OverlayColorScheme)
            {

                case OverlayColorScheme.Standard:
                default:
                    
                    pAxes = new Pen(Color.Magenta, 1.0f) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
                  
                    break;
            }
            
            Point yAxis1 = new Point(origin.X, 0);

            Point yAxis2 = new Point(origin.X, height);

            Point xAxis1 = new Point(0 , origin.Y);

            Point xAxis2 = new Point(width, origin.Y);

            gOverlay.DrawLine(pAxes, yAxis1, yAxis2);

            gOverlay.DrawLine(pAxes, xAxis1, xAxis2);

            gOverlay.Dispose();
        }
        
        #endregion
    }
}
