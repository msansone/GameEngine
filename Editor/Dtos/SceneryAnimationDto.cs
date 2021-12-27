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
    
    public class SceneryAnimationDto : BaseDto
    {
        // This is for human entry purposes only. It converts the value into a "seconds per frame"
        // float, which is what it uses in the engine itself. It stores the frames per second
        // integer value as a sort of display only property.
        public int FramesPerSecond
        {
            get { return framesPerSecond_; }
            set
            {
                framesPerSecond_ = value;

                updateInterval_ = 1.0f / framesPerSecond_; // seconds per frame. Invert FPS to get it.
            }
        }
        private int framesPerSecond_ = 10;        
        private float updateInterval_ = 1.0f / 10;        
    }

    public class SceneryAnimationDtoProxy : ISceneryAnimationDtoProxy
    {
        IProjectController projectController_;
        Guid animationId_ = Guid.Empty;
        Guid ownerId_ = Guid.Empty;

        public SceneryAnimationDtoProxy(IProjectController projectController, Guid animationId)
        {
            projectController_ = projectController;
            animationId_ = animationId;
        }

        [CategoryAttribute("(ID Settings)"),
         DescriptionAttribute("Unique Name String"),
         ParenthesizePropertyName(true)]
        public string Name
        {
            get
            {
                SceneryAnimationDto sceneryAnimation = projectController_.GetSceneryAnimation(animationId_);

                return sceneryAnimation.Name;
            }
            set
            {
                bool isValid = true;

                // Validate the name.
                if (value == string.Empty)
                {
                    System.Windows.Forms.MessageBox.Show("Animation name cannot be blank.", "Invalid Value", MessageBoxButtons.OK);

                    isValid = false;
                }
                else
                {
                    ProjectDto project = projectController_.GetProjectDto();

                    SceneryAnimationDto sceneryAnimation = projectController_.GetSceneryAnimation(animationId_);

                    for (int i = 0; i < project.SceneryAnimations[sceneryAnimation.OwnerId].Count; i++)
                    {
                        SceneryAnimationDto currentSceneryAnimation = project.SceneryAnimations[sceneryAnimation.OwnerId][i];

                        if (value.ToUpper() == currentSceneryAnimation.Name.ToUpper() && animationId_ != currentSceneryAnimation.Id)
                        {
                            isValid = false;

                            break;
                        }
                    }

                    if (isValid == false)
                    {
                        System.Windows.Forms.MessageBox.Show("Animation name is already in use.", "Name In Use", MessageBoxButtons.OK);
                    }
                }

                if (isValid == true)
                {
                    projectController_.SetSceneryAnimationName(animationId_, value);
                }
            }
        }

        public int FramesPerSecond
        {
            get
            {
                SceneryAnimationDto sceneryAnimation = projectController_.GetSceneryAnimation(animationId_);

                return sceneryAnimation.FramesPerSecond;
            }
            set
            {
                projectController_.SetSceneryAnimationFramesPerSecond(animationId_, value);
            }
        }

        [BrowsableAttribute(false)]
        public Guid Id
        {
            get
            {
                return animationId_;
            }
        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                SceneryAnimationDto animation = projectController_.GetSceneryAnimation(animationId_);

                return animation.OwnerId;
            }
        }        
    }
}
