using System;

namespace FiremelonEditor2
{
    public class EntityWidgetDto : MapWidgetDto
    {
        #region Constructors

        public EntityWidgetDto()
        {

        }

        #endregion

        #region Private Variables
        #endregion

        #region Properties

        public bool AcceptInput
        {
            get { return acceptInput_; }
            set { acceptInput_ = value; }
        }
        bool acceptInput_ = false;

        public bool AttachToCamera
        {
            get { return attachToCamera_; }
            set { attachToCamera_ = value; }
        }
        bool attachToCamera_ = false;

        public Guid EntityId
        {
            get { return entityId_; }
            set { entityId_ = value; }
        }
        private Guid entityId_ = Guid.Empty;

        public int RenderOrder
        {
            get { return renderOrder_; }
            set { renderOrder_ = value; }
        }
        int renderOrder_ = 0;

        #endregion

        #region Public Functions

        #endregion

        #region Protected Functions
        #endregion

        #region Private Functions
        #endregion

        #region Event Handlers
        #endregion
    }
}
