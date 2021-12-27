using System;

namespace FiremelonEditor2
{
    class TileObjectMapWidgetCreationParametersDto : MapWidgetCreationParametersDto
    {
        public TileObjectMapWidgetCreationParametersDto(Guid tileObjectId) : base(MapWidgetType.TileObject)
        {
            baseName_ = string.Empty;
            tileObjectId_ = tileObjectId;
        }

        public TileObjectMapWidgetCreationParametersDto(Guid tileObjectId, string baseName) : base(MapWidgetType.TileObject)
        {
            baseName_ = baseName;
            tileObjectId_ = tileObjectId;
        }

        public TileObjectMapWidgetCreationParametersDto(TileObjectWidgetDto copyFrom) : base((MapWidgetDto)copyFrom)
        {
            TileObjectId = copyFrom.TileObjectId;
        }

        private string baseName_ = string.Empty;

        public override string BaseName
        {
            get
            {
                return "TileObject " + baseName_;
            }
        }

        public Guid TileObjectId
        {
            get { return tileObjectId_; }
            set { tileObjectId_ = value; }
        }
        private Guid tileObjectId_ = Guid.Empty;        
    }
}
