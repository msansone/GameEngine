using System;

namespace FiremelonEditor2
{
    class ActorMapWidgetCreationParametersDto : MapWidgetCreationParametersDto
    {
        #region Constructors

        public ActorMapWidgetCreationParametersDto(Guid actorId) : base(MapWidgetType.Actor)
        {
            actorId_ = actorId;
        }

        #endregion

        #region Private Variables
        #endregion

        #region Properties

        public Guid ActorId
        {
            get { return actorId_; }
            set { actorId_ = value; }
        }
        private Guid actorId_ = Guid.Empty;

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
