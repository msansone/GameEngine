using System.Drawing;

namespace FiremelonEditor2
{ 
    public abstract class RoomEditorCursor : IRoomEditorCursor
    {
        public virtual bool GridAligned => false;
    
        public Point LayerNameOffset
        {
            get { return layerNameOffset_; }
            set { layerNameOffset_ = value; }
        }
        Point layerNameOffset_ = new Point(0, 0);

        public virtual Point2D Offset
        {
            get { return offset_; }
            set { offset_ = value; }
        }
        Point2D offset_ = new Point2D(0, 0);

        public Size Size
        {
            get { return cursorSize_; }
            set { cursorSize_ = value; }
        }
        Size cursorSize_ = new Size(16, 16);

        public abstract void Render(Graphics g, int x, int y, int gridOffsetX, int gridOffsetY);
    }
}
