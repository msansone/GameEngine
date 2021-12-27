using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public class MapWidgetDto : BaseDto
    {
        private MapWidgetType type_;
        public MapWidgetType Type
        {
            get 
            { 
                return type_; 
            }
            set
            {
                type_ = value;
            }
        }

        private Rectangle boundingBox_ = new Rectangle(0, 0, 0, 0);
        public Rectangle BoundingBox
        {
            get { return boundingBox_; }
            set { boundingBox_ = value; }
        }

        private List<Point2D> lstGridCells_ = new List<Point2D>();
        public List<Point2D> GridCells
        {
            get { return lstGridCells_; }
        }

        private IMapWidgetController mapWidgetController_;
        public IMapWidgetController Controller
        {
            get
            {
                return mapWidgetController_;
            }
            set
            {
                mapWidgetController_ = value;
            }
        }

        private Point2D position_ = new Point2D(0, 0);
        public Point2D Position
        {
            get { return position_; }
        }
    }
}
