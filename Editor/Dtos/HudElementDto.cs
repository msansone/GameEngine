using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public class HudElementDto : StatefulEntityDto
    {
    }

    public class HudElementDtoProxy : IHudElementDtoProxy
    {
        IProjectController projectController_;
        Guid hudElementId_;

        public HudElementDtoProxy(IProjectController projectController, Guid hudElementId)
        {
            projectController_ = projectController;
            hudElementId_ = hudElementId;
        }

        [CategoryAttribute("(ID Settings)"),
         DescriptionAttribute("Unique Name String"),
         ParenthesizePropertyName(true)]
        public string Name
        {
            get
            {
                HudElementDto hudElement = projectController_.GetHudElement(hudElementId_);

                return hudElement.Name;
            }
            set
            {
                try
                {
                    projectController_.SetHudElementName(hudElementId_, value);
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
                HudElementDto hudElement = projectController_.GetHudElement(hudElementId_);

                return hudElement.Tag;
            }
            set
            {
                projectController_.SetHudElementTag(hudElementId_, value);
            }
        }
        
        [BrowsableAttribute(false)]
        public Guid Id
        {
            get
            {
                return hudElementId_;
            }
        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                HudElementDto hudElement = projectController_.GetHudElement(hudElementId_);

                return hudElement.OwnerId;
            }
        }

        [TypeConverter(typeof(StateConverter))]
        public string InitialState
        {
            get
            {
                HudElementDto hudElement = projectController_.GetHudElement(hudElementId_);

                Guid initialStateId = hudElement.InitialStateId;

                if (initialStateId == Guid.Empty)
                {
                    return string.Empty;
                }
                else
                {
                    StateDto initialState = projectController_.GetState(initialStateId);

                    return initialState.Name;
                }
            }
            set
            {
                HudElementDto hudElement = projectController_.GetHudElement(hudElementId_);

                Guid ownerEntityId = hudElement.Id;

                Guid initialStateId = projectController_.GetStateIdFromName(ownerEntityId, value);

                projectController_.SetHudElementInitialState(hudElementId_, initialStateId);
            }
        }

        public int StageWidth
        {
            get
            {
                HudElementDto hudElement = projectController_.GetHudElement(hudElementId_);

                return hudElement.StageWidth;
            }
            set
            {
                projectController_.SetStatefulEntityStageWidth(hudElementId_, value);
            }
        }

        public int StageHeight
        {
            get
            {
                HudElementDto hudElement = projectController_.GetHudElement(hudElementId_);

                return hudElement.StageHeight;
            }
            set
            {
                projectController_.SetStatefulEntityStageHeight(hudElementId_, value);
            }
        }

        public OriginLocation StageOriginLocation
        {
            get
            {
                HudElementDto hudElement = projectController_.GetHudElement(hudElementId_);

                return hudElement.StageOriginLocation;
            }
            set
            {
                projectController_.SetStatefulEntityStageOriginLocation(hudElementId_, value);
            }
        }
    }
}
