namespace FiremelonEditor2
{
    class WorldGeometryMapWidgetCreationParametersDto : MapWidgetCreationParametersDto
    {
        public WorldGeometryMapWidgetCreationParametersDto() : base(MapWidgetType.WorldGeometry)
        {

        }

        public WorldGeometryMapWidgetCreationParametersDto(WorldGeometryWidgetDto copyFrom) : base((MapWidgetDto)copyFrom)
        {
            Corner1.X = copyFrom.Corner1.X;
            Corner1.Y = copyFrom.Corner1.Y;

            Corner2.X = copyFrom.Corner2.X;
            Corner2.Y = copyFrom.Corner2.Y;
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
    }
}
