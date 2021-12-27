namespace FiremelonEditor2
{
    public delegate void PointChangeHandler(object sender, PointChangedEventArgs e);

    public class Point2D
    {
        public event PointChangeHandler PointChanged;

        public Point2D(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public int X
        {
            get
            {
                return x_;
            }
            set
            {
                int oldX = x_;

                x_ = value;

                if (x_ != oldX)
                {
                    OnChanged(new PointChangedEventArgs());
                }
            }
        }
        private int x_;

        public int Y
        {
            get
            {
                return y_;
            }
            set
            {
                int oldY = y_;

                y_ = value;

                if (y_ != oldY)
                {
                    OnChanged(new PointChangedEventArgs());
                }
            }
        }
        private int y_;

        protected void OnChanged(PointChangedEventArgs e)
        {
            PointChanged?.Invoke(this, e);
        }
    }


    public class PointChangedEventArgs : System.EventArgs
    {
        public PointChangedEventArgs()
        {
        }        
    }
}