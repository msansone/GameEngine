using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public class EntityInstanceCreationParametersDto
    {
        private Guid entityId_ = Guid.Empty;
        public Guid EntityId
        {
            get { return entityId_; }
            set { entityId_ = value; }
        }

        private Guid roomId_ = Guid.Empty;
        public Guid RoomId
        {
            get { return roomId_; }
            set { roomId_ = value; }
        }

        private Guid layerId_ = Guid.Empty;
        public Guid LayerId
        {
            get { return layerId_; }
            set { layerId_ = value; }
        }

        private Rectangle bounds_ = new Rectangle(0, 0, 0, 0);
        public Rectangle Bounds
        {
            get { return bounds_; }
        }

        private List<PropertyDto> lstProperties_ = new List<PropertyDto>();
        public List<PropertyDto> Properties
        {
            get { return lstProperties_; }
        }
    }
}
