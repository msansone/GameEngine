namespace FiremelonEditor2
{
    class DrawingUtility : IDrawingUtility
    {
        #region Constructors

        public DrawingUtility()
        {

        }

        #endregion

        #region Private Variables
        #endregion

        #region Properties
        #endregion

        #region Public Functions

        public System.Drawing.Rectangle GetRectFromPoints(Point2D p1, Point2D p2)
        {
            // Take the two corner points and determine the dimensions and position of the drawable rect.
            int left = 0;
            int top = 0;
            int right = 0;
            int bottom = 0;

            if (p1.X > p2.X)
            {
                left = p2.X;
                right = p1.X;
            }
            else
            {
                left = p1.X;
                right = p2.X;
            }

            if (p1.Y > p2.Y)
            {
                bottom = p1.Y;
                top = p2.Y;
            }
            else
            {
                bottom = p2.Y;
                top = p1.Y;
            }

            int width = right - left;
            int height = bottom - top;

            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(left, top, width, height);

            return rect;
        }


        #endregion

        #region Protected Functions
        #endregion

        #region Private Functions
        #endregion

        #region Event Handlers
        #endregion
    }
}
