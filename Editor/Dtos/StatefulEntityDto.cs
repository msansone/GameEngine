using System;
using System.Drawing;

namespace FiremelonEditor2
{
    public enum OriginLocation
    {
        TopLeft = 0,
        Center = 1,
        TopMiddle = 2,
        TopRight = 3,
        MiddleLeft = 4, 
        MiddleRight = 5,
        BottomLeft = 6,
        BottomMiddle = 7,
        BottomRight = 8,
        CustomPoint = 9
    };

    public class StatefulEntityDto : EntityDto
    {
        public Guid InitialStateId
        {
            get { return initialStateId_; }
            set { initialStateId_ = value; }
        }
        private Guid initialStateId_ = Guid.Empty;

        public Point PivotPoint
        {
            get { return pivotPoint_; }
            set { pivotPoint_ = value; }
        }
        private Point pivotPoint_;

        public int StageWidth
        {
            get { return stageWidth_; }
            set
            {
                stageWidth_ = value;
                BoundRect.Width = stageWidth_;
            }
        }
        private int stageWidth_;

        public int StageHeight
        {
            get { return stageHeight_; }
            set 
            {
                stageHeight_ = value;
                BoundRect.Height = stageHeight_;
            }
        }
        private int stageHeight_;

        public OriginLocation StageOriginLocation
        {
            get { return stageOriginLocation_; }
            set { stageOriginLocation_ = value; }
        }
        private OriginLocation stageOriginLocation_ = OriginLocation.TopLeft;


        public int StageBackgroundDepth
        {
            get { return stageBackgroundDepth_; }
            set
            {
                stageBackgroundDepth_ = value;
            }
        }
        private int stageBackgroundDepth_ = 1;
    }
}
