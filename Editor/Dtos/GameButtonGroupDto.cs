using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public class GameButtonGroupDto : BaseDto
    {
    }

    public class GameButtonGroupDtoProxy : IGameButtonGroupDtoProxy
    {
        private IProjectController projectController_;
        private Guid gameButtonGroupId_;

        public GameButtonGroupDtoProxy(IProjectController projectController, Guid gameButtonGroupId)
        {
            projectController_ = projectController;
            gameButtonGroupId_ = gameButtonGroupId;
        }

        [CategoryAttribute("(ID Settings)"),
         DescriptionAttribute("Unique Name String"),
         ParenthesizePropertyName(true)]
        public string Name
        {
            get
            {
                GameButtonGroupDto gameButtonGroup = projectController_.GetGameButtonGroup(gameButtonGroupId_);

                return gameButtonGroup.Name;
            }
            set
            {
                bool isValid = true;

                // Validate the name.
                if (value == string.Empty)
                {
                    System.Windows.Forms.MessageBox.Show("Game button group name cannot be blank.", "Invalid Value", MessageBoxButtons.OK);

                    isValid = false;
                }
                else
                {
                    ProjectDto project = projectController_.GetProjectDto();

                    for (int i = 0; i < project.GameButtonGroups.Count; i++)
                    {
                        GameButtonGroupDto currentGameButtonGroup = project.GameButtonGroups[i];

                        if (value.ToUpper() == currentGameButtonGroup.Name.ToUpper() && gameButtonGroupId_ != currentGameButtonGroup.Id)
                        {
                            isValid = false;
                            break;
                        }
                    }

                    if (isValid == false)
                    {
                        System.Windows.Forms.MessageBox.Show("Game button group name is already in use.", "Name In Use", MessageBoxButtons.OK);
                    }
                }

                if (isValid == true)
                {
                    projectController_.SetGameButtonGroupName(gameButtonGroupId_, value);
                }
            }
        }

        [BrowsableAttribute(false)]
        public Guid Id
        {
            get { return gameButtonGroupId_; }
        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                GameButtonGroupDto gameButtonGroup = projectController_.GetGameButtonGroup(gameButtonGroupId_);

                return gameButtonGroup.OwnerId;
            }
        }
    }

    public interface IGameButtonGroupDtoProxy : IBaseDtoProxy
    {
    }
}
