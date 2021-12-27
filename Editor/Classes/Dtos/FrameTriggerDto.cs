using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public class FrameTriggerDto : BaseDto
    {
        private Guid signal_ = Guid.Empty;
        public Guid Signal
        {
            get { return signal_; }
            set { signal_ = value; }
        }
    }

    public class FrameTriggerDtoProxy : IFrameTriggerDtoProxy
    {
        IProjectController projectController_;
        Guid frameTriggerId_;
        Guid triggerSignalId_ = Guid.Empty;

        public FrameTriggerDtoProxy(IProjectController projectController, Guid frameTriggerId)
        {
            projectController_ = projectController;
            frameTriggerId_ = frameTriggerId;
        }

        [BrowsableAttribute(false)]
        public string Name
        {
            get { return string.Empty; }
            set { /*No-op*/ }
        }

        [TypeConverter(typeof(TriggerSignalConverter))]
        public string Signal
        {
            get
            {
                FrameTriggerDto frameTrigger = projectController_.GetFrameTrigger(frameTriggerId_);

                triggerSignalId_ = frameTrigger.Signal;

                if (triggerSignalId_ == Guid.Empty)
                {
                    return string.Empty;
                }
                else
                {
                    TriggerSignalDto triggerSignal = projectController_.GetTriggerSignal(triggerSignalId_);

                    return triggerSignal.Name;
                }
            }
            set
            {
                FrameTriggerDto frameTrigger = projectController_.GetFrameTrigger(frameTriggerId_);

                Guid ownerEntityId = frameTrigger.RootOwnerId;

                Guid triggerSignalId = projectController_.GetTriggerSignalIdFromName(value);

                projectController_.SetTriggerSignal(frameTriggerId_, triggerSignalId);
            }
        }

        [BrowsableAttribute(false)]
        public Guid Id
        {
            get
            {
                return frameTriggerId_;
            }
        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                FrameTriggerDto frameTrigger = projectController_.GetFrameTrigger(frameTriggerId_);

                return frameTrigger.OwnerId;
            }
        }
    }

    public interface IFrameTriggerDtoProxy : IBaseDtoProxy
    {
    }
}
