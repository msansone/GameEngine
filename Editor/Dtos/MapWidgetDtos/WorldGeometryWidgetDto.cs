using System;

namespace FiremelonEditor2
{
    public enum WorldGeometryCollisionStyle
    {
        Solid = 0,
        Incline = 1,
        Decline = 2,
        InvertedIncline = 3,
        InvertedDecline = 4,
        OneWayTop = 5,
        OneWayBottom = 6,
        OneWayLeft = 7,
        OneWayRight = 8,
        SnapToBottom = 9,
        SnapToTop = 10,
        SnapToLeft = 11, 
        SnapToRight = 12
    }

    public class WorldGeometryWidgetDto : MapWidgetDto
    {
        public WorldGeometryWidgetDto()
        {
            Type = MapWidgetType.WorldGeometry;

            corner1_.PointChanged += new PointChangeHandler(this.WorldGeometryWidgetDto_PointChanged);
            corner2_.PointChanged += new PointChangeHandler(this.WorldGeometryWidgetDto_PointChanged);
        }

        public Point2D Corner1
        {
            get { return corner1_; }
        }
        private Point2D corner1_ = new Point2D(0, 0);

        public Point2D Corner2
        {
            get { return corner2_; }
        }
        private Point2D corner2_ = new Point2D(0, 0);

        public WorldGeometryCollisionStyle CollisionStyle
        {
            get { return collisionStyle_; }
            set { collisionStyle_ = value; }
        }
        private WorldGeometryCollisionStyle collisionStyle_ = WorldGeometryCollisionStyle.Solid;

        public WorldGeometryEdgesDto Edges
        {
            get { return edges_; }
        }
        private WorldGeometryEdgesDto edges_ = new WorldGeometryEdgesDto();

        public int SlopeRise
        {
            get { return slopeRise_; }
            set { slopeRise_ = value; }
        }
        private int slopeRise_ = 0;
        
        private void WorldGeometryWidgetDto_PointChanged(object sender, PointChangedEventArgs e)
        {
            // Recalculate the bounds
            if (Controller != null)
            {
                Rectangle bounds = Controller.GetBoundingRect();

                BoundingBox.Left = bounds.Left;
                BoundingBox.Top = bounds.Top;
                BoundingBox.Width = bounds.Width;
                BoundingBox.Height = bounds.Height;
            }
        }
    }
}
