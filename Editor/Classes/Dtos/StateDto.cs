using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public class StateDto : BaseDto
    {
    }

    public class StateDtoProxy : IStateDtoProxy
    {
        IProjectController projectController_;
        Guid stateId_ = Guid.Empty;

        public StateDtoProxy(IProjectController projectController, Guid stateId)
        {
            projectController_ = projectController;
            stateId_ = stateId;
        }

        [CategoryAttribute("(ID Settings)"),
         DescriptionAttribute("Unique Name String"),
         ParenthesizePropertyName(true)]
        public string Name
        {
            get
            {
                StateDto state = projectController_.GetState(stateId_);

                return state.Name;
            }
            set
            {
                bool isValid = true;

                // Validate the name.
                if (value == string.Empty)
                {
                    System.Windows.Forms.MessageBox.Show("State name cannot be blank.", "Invalid Value", MessageBoxButtons.OK);

                    isValid = false;
                }
                else
                {
                    ProjectDto project = projectController_.GetProjectDto();

                    StateDto state = projectController_.GetState(stateId_);

                    for (int i = 0; i < project.States[state.OwnerId].Count; i++)
                    {
                        StateDto currentState = project.States[state.OwnerId][i];

                        if (value.ToUpper() == currentState.Name.ToUpper() && stateId_ != currentState.Id)
                        {
                            isValid = false;
                            break;
                        }
                    }

                    if (isValid == false)
                    {
                        System.Windows.Forms.MessageBox.Show("State name is already in use.", "Name In Use", MessageBoxButtons.OK);
                    }
                }

                if (isValid == true)
                {
                    projectController_.SetStateName(stateId_, value);
                }
            }
        }

        public Point PivotPoint
        {
            get
            {
                StateDto state = projectController_.GetState(stateId_);

                StatefulEntityDto statefulEntity = projectController_.GetStatefulEntity(state.OwnerId);

                return statefulEntity.PivotPoint;
            }
            set
            {
                StateDto state = projectController_.GetState(stateId_);

                projectController_.SetStatefulEntityPivotPoint(state.OwnerId, value);
            }
        }

        public int StageWidth
        {
            get
            {
                StateDto state = projectController_.GetState(stateId_);

                StatefulEntityDto statefulEntity = projectController_.GetStatefulEntity(state.OwnerId);

                return statefulEntity.StageWidth;
            }
            set
            {
                StateDto state = projectController_.GetState(stateId_);

                projectController_.SetStatefulEntityStageWidth(state.OwnerId, value);
            }
        }

        public int StageHeight
        {
            get
            {
                StateDto state = projectController_.GetState(stateId_);

                StatefulEntityDto statefulEntity = projectController_.GetStatefulEntity(state.OwnerId);

                return statefulEntity.StageHeight;
            }
            set
            {
                StateDto state = projectController_.GetState(stateId_);

                projectController_.SetStatefulEntityStageHeight(state.OwnerId, value);
            }
        }

        public OriginLocation StageOriginLocation
        {
            get
            {
                StateDto state = projectController_.GetState(stateId_);

                StatefulEntityDto statefulEntity = projectController_.GetStatefulEntity(state.OwnerId);

                return statefulEntity.StageOriginLocation;
            }
            set
            {
                StateDto state = projectController_.GetState(stateId_);

                projectController_.SetStatefulEntityStageOriginLocation(state.OwnerId, value);
            }
        }

        [BrowsableAttribute(false)]
        public Guid Id
        {
            get
            {
                return stateId_;
            }
        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                StateDto state = projectController_.GetState(stateId_);

                return state.OwnerId;
            }
        }
    }
}
