using System;

namespace FiremelonEditor2
{
    class HudElementMapWidgetCreationParametersDto : MapWidgetCreationParametersDto
    {
        #region Constructors

        public HudElementMapWidgetCreationParametersDto(Guid hudElementId) : base(MapWidgetType.HudElement)
        {
            hudElementId_ = hudElementId;
        }

        #endregion

        #region Private Variables
        #endregion

        #region Properties

        public Guid HudElementId
        {
            get { return hudElementId_; }
            set { hudElementId_ = value; }
        }
        private Guid hudElementId_ = Guid.Empty;

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
