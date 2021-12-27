using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public class FrameDto : BaseDto
    {
        private int? sheetCellIndex_;
        public int? SheetCellIndex
        {
            get { return sheetCellIndex_; }
            set { sheetCellIndex_ = value; }
        }

        private int? alphaMaskSheetCellIndex_;
        public int? AlphaMaskCellIndex
        {
            get { return alphaMaskSheetCellIndex_; }
            set { alphaMaskSheetCellIndex_ = value; }
        }
    }
    
    public class FrameDtoProxy : IFrameDtoProxy
    {
        IProjectController  projectController_;

        Guid                frameId_;
        
        public FrameDtoProxy(IProjectController projectController, Guid frameId)
        {
            projectController_ = projectController;
            frameId_ = frameId;
        }

        [BrowsableAttribute(false)]
        public IProjectController ProjectController
        {
            get { return projectController_; }
        }

        [BrowsableAttribute(false)]
        public string Name
        {
            get { return string.Empty; }
            set { /*No-op*/ }
        }


        [Editor(typeof(AlphaMaskSheetCellUiTypeEditor), typeof(UITypeEditor))]
        public int? AlphaMaskSheetCellIndex
        {
            get
            {
                FrameDto frame = projectController_.GetFrame(frameId_);
                
                return frame.AlphaMaskCellIndex;
            }
            set
            {
                projectController_.SetFrameAlphaMaskCellIndex(frameId_, value);                
            }
        }

        [Editor(typeof(SpriteSheetCellUiTypeEditor), typeof(UITypeEditor))]
        public int? SheetCellIndex
        {
            get
            {
                FrameDto frame = projectController_.GetFrame(frameId_);
                
                return frame.SheetCellIndex;
            }
            set
            {
                projectController_.SetFrameSpriteSheetCellIndex(frameId_, value);
            }
        }

        [BrowsableAttribute(false)]
        public Guid Id
        {
            get
            {
                return frameId_;
            }
        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                FrameDto frame = projectController_.GetFrame(frameId_);

                return frame.OwnerId;
            }
        }
    }

    public interface IFrameDtoProxy : IBaseDtoProxy
    {
        IProjectController ProjectController { get; }
    }
}
