using System;

namespace FiremelonEditor2
{
    public class TileObjectWidgetDto : MapWidgetDto
    {
        public TileObjectWidgetDto()
        {
            Type = MapWidgetType.TileObject;            
        }

        public Guid TileObjectId
        {
            get { return tileObjectId_; }
            set { tileObjectId_ = value; }
        }
        private Guid tileObjectId_ = Guid.Empty;
    }
}
