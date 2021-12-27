using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public enum Ownership
    {
        CLIENT = 1,
        SERVER = 2
    };

    public class EntityInstanceDto : BaseDto
    {
        private Guid entityId_ = Guid.Empty;
        public Guid EntityId
        {
            get { return entityId_; }
            set { entityId_ = value; }
        }

        private Rectangle boundingBox_ = new Rectangle(0, 0, 0, 0);
        public Rectangle BoundingBox
        {
            get { return boundingBox_; }
        }

        private List<Point2D> lstGridCells_ = new List<Point2D>();
        public List<Point2D> GridCells
        {
            get { return lstGridCells_; }
        }

        private bool acceptInput_ = false;
        public bool AcceptInput
        {
            get { return acceptInput_; }
            set { acceptInput_ = value; }
        }

        private bool attachToCamera_ = false;
        public bool AttachToCamera
        {
            get { return attachToCamera_; }
            set { attachToCamera_ = value; }
        }

        private int renderOrder_ = 0;
        public int RenderOrder
        {
            get { return renderOrder_; }
            set { renderOrder_ = value; }
        }

        private Ownership ownership_ = Ownership.CLIENT;
        public Ownership Ownership
        {
            get { return ownership_; }
            set { ownership_ = value; }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
