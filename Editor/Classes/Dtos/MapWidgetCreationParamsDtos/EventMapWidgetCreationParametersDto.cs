using System;

namespace FiremelonEditor2
{
    class EventMapWidgetCreationParametersDto : MapWidgetCreationParametersDto
    {
        #region Constructors

        public EventMapWidgetCreationParametersDto(Guid eventId) : base(MapWidgetType.Event)
        {
            eventId_ = eventId;
        }

        public EventMapWidgetCreationParametersDto(Guid eventId, Guid selectedRoomId, Guid selectedLayerId, Rectangle bounds) : base(MapWidgetType.Event)
        {
            eventId_ = eventId;
            RoomId = selectedRoomId;
            LayerId = selectedLayerId;
            Bounds.Left = bounds.Left;
            Bounds.Top = bounds.Top;
            Bounds.Width = bounds.Width;
            Bounds.Height = bounds.Height;
        }

        #endregion

        #region Private Variables
        #endregion

        #region Properties

        public Guid EventId
        {
            get { return eventId_; }
            set { eventId_ = value; }
        }
        private Guid eventId_ = Guid.Empty;

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
