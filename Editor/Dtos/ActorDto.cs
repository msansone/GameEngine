using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public class ActorDto : StatefulEntityDto
    {
        private bool keepRoomActive_;
        public bool KeepRoomActive
        {
            get { return keepRoomActive_; }
            set { keepRoomActive_ = value; }
        }
    }

    public class ActorDtoProxy : IActorDtoProxy
    {
        IProjectController projectController_;
        Guid actorId_;

        public ActorDtoProxy(IProjectController projectController, Guid actorId)
        {
            projectController_ = projectController;
            actorId_ = actorId;
        }

        [CategoryAttribute("(ID Settings)"),
         DescriptionAttribute("Unique Name String"),
         ParenthesizePropertyName(true)]
        public string Name
        {
            get
            {
                ActorDto actor = projectController_.GetActor(actorId_);

                return actor.Name;
            }
            set
            {
                try
                {
                    projectController_.SetActorName(actorId_, value);
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
                ActorDto actor = projectController_.GetActor(actorId_);

                return actor.Tag;
            }
            set
            {
                projectController_.SetActorTag(actorId_, value);
            }
        }
        
        public bool KeepRoomActive
        {
            get
            {
                ActorDto actor = projectController_.GetActor(actorId_);

                return actor.KeepRoomActive;
            }
            set
            {
                projectController_.SetActorKeepRoomActive(actorId_, value);
            }
        }


        [BrowsableAttribute(false)]
        public Guid Id
        {
            get
            {
                return actorId_;
            }
        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                ActorDto actor = projectController_.GetActor(actorId_);

                return actor.OwnerId;
            }
        }

        [TypeConverter(typeof(StateConverter))]
        public string InitialState
        {
            get
            {
                ActorDto actor = projectController_.GetActor(actorId_);

                Guid initialStateId = actor.InitialStateId;

                if (initialStateId == Guid.Empty)
                {
                    return string.Empty;
                }
                else
                {
                    StateDto initialState = projectController_.GetState(initialStateId);

                    if (initialState == null)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return initialState.Name;
                    }
                }
            }
            set
            {
                ActorDto actor = projectController_.GetActor(actorId_);

                Guid ownerEntityId = actor.Id;

                Guid initialStateId = projectController_.GetStateIdFromName(ownerEntityId, value);

                projectController_.SetActorInitialState(actorId_, initialStateId);
            }
        }


        public int StageBackgroundDepth
        {
            get
            {
                ActorDto actor = projectController_.GetActor(actorId_);

                return actor.StageBackgroundDepth;
            }
            set
            {
                projectController_.SetStatefulEntityStageBackgroundDepth(actorId_, value);
            }
        }


        public int StageWidth
        {
            get
            {
                ActorDto actor = projectController_.GetActor(actorId_);

                return actor.StageWidth;
            }
            set
            {
                projectController_.SetStatefulEntityStageWidth(actorId_, value);
            }
        }

        public int StageHeight
        {
            get
            {
                ActorDto actor = projectController_.GetActor(actorId_);

                return actor.StageHeight;
            }
            set
            {
                projectController_.SetStatefulEntityStageHeight(actorId_, value);
            }
        }

        public OriginLocation StageOriginLocation
        {
            get
            {
                ActorDto actor = projectController_.GetActor(actorId_);

                return actor.StageOriginLocation;
            }
            set
            {
                projectController_.SetStatefulEntityStageOriginLocation(actorId_, value);
            }
        }

        override public string ToString()
        {
            ActorDto actor = projectController_.GetActor(actorId_);

            return actor.Name;
        }
    }
}
