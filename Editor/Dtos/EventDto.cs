using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public class EventDto : EntityDto
    {
    }

    public class EventDtoProxy : IEventDtoProxy
    {
        IProjectController projectController_;
        Guid eventId_;

        public EventDtoProxy(IProjectController projectController, Guid eventId)
        {
            projectController_ = projectController;
            eventId_ = eventId;
        }

        [CategoryAttribute("(ID Settings)"),
         DescriptionAttribute("Unique Name String"),
         ParenthesizePropertyName(true)]
        public string Name
        {
            get
            {
                EventDto eventEntity = projectController_.GetEvent(eventId_);

                return eventEntity.Name;
            }
            set
            {
                try
                {
                    projectController_.SetEventName(eventId_, value);
                }
                catch (InvalidNameException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public string Tag
        {
            get
            {
                EventDto eventEntity = projectController_.GetEvent(eventId_);

                return eventEntity.Tag;
            }
            set
            {
                projectController_.SetEventTag(eventId_, value);
            }
        }

        [BrowsableAttribute(false)]
        public Guid Id
        {
            get
            {
                return eventId_;
            }
        }
        
        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                EventDto eventEntity = projectController_.GetEvent(eventId_);

                return eventEntity.OwnerId;
            }
        }
    }
}
