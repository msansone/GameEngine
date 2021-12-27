using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public class AnimationGroupDto : BaseDto
    {
    }


    public class AnimationGroupDtoProxy : IAnimationGroupDtoProxy
    {
        IProjectController projectController_;
        Guid animationGroupId_ = Guid.Empty;
        Guid ownerId_ = Guid.Empty;

        public AnimationGroupDtoProxy(IProjectController projectController, Guid animationGroupId)
        {
            projectController_ = projectController;
            animationGroupId_ = animationGroupId;
        }

        [CategoryAttribute("(ID Settings)"),
         DescriptionAttribute("Unique Name String"),
         ParenthesizePropertyName(true)]
        public string Name
        {
            get
            {
                AnimationGroupDto animationGroup = projectController_.GetAnimationGroup(animationGroupId_);

                return animationGroup.Name;
            }
            set
            {
                bool isValid = true;

                AnimationGroupDto animationGroup = projectController_.GetAnimationGroup(animationGroupId_);

                // Validate the name.
                if (value == string.Empty)
                {
                    System.Windows.Forms.MessageBox.Show("Animation group name cannot be blank.", "Invalid Value", MessageBoxButtons.OK);

                    isValid = false;
                }
                else if (animationGroup.Name.ToUpper() == "NONE")
                {
                    System.Windows.Forms.MessageBox.Show("\"None\" animation group name cannot be changed.", "Invalid Value", MessageBoxButtons.OK);

                    isValid = false;
                }
                else
                {
                    ProjectDto project = projectController_.GetProjectDto();

                    for (int i = 0; i < project.AnimationGroups.Count; i++)
                    {
                        AnimationGroupDto currentAnimationGroup = project.AnimationGroups[i];

                        if (value.ToUpper() == currentAnimationGroup.Name.ToUpper() && animationGroupId_ != currentAnimationGroup.Id)
                        {
                            isValid = false;
                            break;
                        }
                    }

                    if (isValid == false)
                    {
                        System.Windows.Forms.MessageBox.Show("Animation group name is already in use.", "Name In Use", MessageBoxButtons.OK);
                    }
                }

                if (isValid == true)
                {
                    projectController_.SetAnimationGroupName(animationGroupId_, value);
                }
            }
        }

        [BrowsableAttribute(false)]
        public Guid Id
        {
            get
            {
                return animationGroupId_;
            }
        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                AnimationGroupDto animationGroup = projectController_.GetAnimationGroup(animationGroupId_);

                return animationGroup.OwnerId;
            }
        }        
    }
}
