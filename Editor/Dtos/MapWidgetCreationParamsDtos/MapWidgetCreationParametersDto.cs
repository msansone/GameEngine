using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public class MapWidgetCreationParametersDto
    {
        public MapWidgetCreationParametersDto(MapWidgetType type)
        {
            type_ = type;
        }

        public MapWidgetCreationParametersDto(MapWidgetDto copyFrom)
        {
            RoomId = copyFrom.RootOwnerId;
            LayerId = copyFrom.OwnerId;
            Type = copyFrom.Type;
            Bounds.Left = copyFrom.BoundingBox.Left;
            Bounds.Top = copyFrom.BoundingBox.Top;
            Bounds.Width = copyFrom.BoundingBox.Width;
            Bounds.Height = copyFrom.BoundingBox.Height;
        }

        public virtual string BaseName
        {
            get { return Type.ToString(); }
        }

        public string Name
        {
            get { return name_; }
            set { name_ = value; }
        }
        private string name_;

        public MapWidgetType Type
        {
            get { return type_; }
            set { type_ = value; }
        }
        private MapWidgetType type_;

        public Guid RoomId
        {
            get { return roomId_; }
            set { roomId_ = value; }
        }
        private Guid roomId_ = Guid.Empty;

        public Guid LayerId
        {
            get { return layerId_; }
            set { layerId_ = value; }
        }
        private Guid layerId_ = Guid.Empty;

        public Rectangle Bounds
        {
            get { return bounds_; }
        }
        private Rectangle bounds_ = new Rectangle(0, 0, 0, 0);

        private List<PropertyDto> lstProperties_ = new List<PropertyDto>();
        public List<PropertyDto> Properties
        {
            get { return lstProperties_; }
        }

        // Position offset is used when creating a map widget that you don't want to be created at the point given.
        // For example, when you are pasting a group of map widgets at a point, they will need to be offset from the 
        // location they are pasted.
        public Point2D PositionOffset
        {
            get { return positionOffset_; }
            set { positionOffset_ = value; }
        }
        private Point2D positionOffset_ = new Point2D(0, 0);

        // The position is the location of the origin point.
        // It is needed because the top left point of the widget may not be the origin, if there is a stage origin 
        // such as the center. So the bounding box may not be the position.
        public Point2D Position
        {
            get { return position_; }
            set { position_ = value; }
        }
        private Point2D position_ = new Point2D(0, 0);

        public string SerializedDataString
        {
            get { return serializedDataString_; }
            set { serializedDataString_ = value; }
        }
        private string serializedDataString_ = String.Empty;

        //private IMapWidgetController controller_;
        //public IMapWidgetController Controller
        //{
        //    get { return controller_; }
        //    set { controller_ = value; }
        //}
    }
}
