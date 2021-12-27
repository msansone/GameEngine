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

    public class GameButtonDto : BaseDto
    {
        private Guid group_ = Guid.Empty;
        public Guid Group
        {
            get { return group_; }
            set { group_ = value; }
        }

        private string label_ = string.Empty;
        public string Label
        {
            get { return label_; }
            set { label_ = value; }
        }
    }

    public class GameButtonDtoProxy : IGameButtonDtoProxy
    {
        private IProjectController projectController_;
        private Guid gameButtonId_;
        private Guid gameButtonGroupId_ = Guid.Empty;

        public GameButtonDtoProxy(IProjectController projectController, Guid gameButtonId)
        {
            projectController_ = projectController;
            gameButtonId_ = gameButtonId;
        }

        [CategoryAttribute("(ID Settings)"),
         DescriptionAttribute("Unique Name String"),
         ParenthesizePropertyName(true)]
        public string Name
        {
            get
            {
                GameButtonDto gameButton = projectController_.GetGameButton(gameButtonId_);

                return gameButton.Name;
            }
            set
            {
                bool isValid = true;

                // Validate the name.
                if (value == string.Empty)
                {
                    System.Windows.Forms.MessageBox.Show("Game button name cannot be blank.", "Invalid Value", MessageBoxButtons.OK);

                    isValid = false;
                }
                else
                {
                    ProjectDto project = projectController_.GetProjectDto();

                    for (int i = 0; i < project.GameButtons.Count; i++)
                    {
                        GameButtonDto currentGameButton = project.GameButtons[i];

                        if (value.ToUpper() == currentGameButton.Name.ToUpper() && gameButtonId_ != currentGameButton.Id)
                        {
                            isValid = false;
                            break;
                        }
                    }

                    if (isValid == false)
                    {
                        System.Windows.Forms.MessageBox.Show("Game Button name is already in use.", "Name In Use", MessageBoxButtons.OK);
                    }
                }

                if (isValid == true)
                {
                    projectController_.SetGameButtonName(gameButtonId_, value);
                }
            }
        }

        [TypeConverter(typeof(GameButtonGroupConverter))]
        public string Group
        {
            get
            {
                GameButtonDto gameButton = projectController_.GetGameButton(gameButtonId_);

                gameButtonGroupId_ = gameButton.Group;

                if (gameButtonGroupId_ == Guid.Empty)
                {
                    return string.Empty;
                }
                else
                {
                    GameButtonGroupDto gameButtonGroup = projectController_.GetGameButtonGroup(gameButtonGroupId_);

                    return gameButtonGroup.Name;
                }
            }
            set
            {
                Guid gameButtonGroupId = projectController_.GetGameButtonGroupIdFromName(value);

                projectController_.SetGameButtonGroup(gameButtonId_, gameButtonGroupId);
            }
        }

        public string Label
        {
            get
            {
                GameButtonDto gameButton = projectController_.GetGameButton(gameButtonId_);

                return gameButton.Label;
                
            }
            set
            {
                projectController_.SetGameButtonLabel(gameButtonId_, value);
            }
        }

        [BrowsableAttribute(false)]
        public Guid Id
        {
            get { return gameButtonId_; }
        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                GameButtonDto gameButton = projectController_.GetGameButton(gameButtonId_);

                return gameButton.OwnerId;
            }
        }
    }

    public interface IGameButtonDtoProxy : IBaseDtoProxy
    {
    }
}
