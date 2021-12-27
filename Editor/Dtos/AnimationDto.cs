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

    public enum AnimationStyle
    {
        AnimationRepeat = 0,
        AnimationSingle = 1,
        AnimationSinglePersist = 2,
        AnimationSingleEndState = 3
    };

    public class AnimationDto : BaseDto
    {
        private Guid spriteSheetId_ = Guid.Empty;
        public Guid SpriteSheet
        {
            get { return spriteSheetId_; }
            set { spriteSheetId_ = value; }
        }

        private Guid alphaMaskSheetId_ = Guid.Empty;
        public Guid AlphaMaskSheet
        {
            get { return alphaMaskSheetId_; }
            set { alphaMaskSheetId_ = value; }
        }

        // DEPRECATED - LEAVE FOR PROJECT UPGRADER
        [ObsoleteAttribute("This property is obsolete. Use the property in the AnimationSlotDto class instead.", false)]
        public AnimationStyle AnimationStyle
        {
            get { return animationStyle_; }
            set { animationStyle_ = value; }
        }
        private AnimationStyle animationStyle_ = AnimationStyle.AnimationRepeat;


        // DEPRECATED - LEAVE FOR PROJECT UPGRADER
        // This is for human entry purposes only. It converts the value into a "seconds per frame"
        // float, which is what it uses in the engine itself. It stores the frames per second
        // integer value as a sort of display only property.
        [ObsoleteAttribute("This property is obsolete. Use the property in the AnimationSlotDto class instead.", false)]
        public int FramesPerSecond
        {
            get { return framesPerSecond_; }
            set { framesPerSecond_ = value; }
        }
        private int framesPerSecond_ = 10;


        // DEPRECATED - LEAVE FOR PROJECT UPGRADER
        [ObsoleteAttribute("This property is obsolete. Use the property in the AnimationSlotDto class instead.", false)]
        public float UpdateInterval
        {
            get { return updateInterval_; }
            set { updateInterval_ = value; }
        }
        
        private float updateInterval_ = 1.0f / 10;

        private Guid groupId_ = Guid.Empty;
        public Guid GroupId
        {
            get { return groupId_; }
            set { groupId_ = value; }
        }
    }

    public class AnimationDtoProxy : IAnimationDtoProxy
    {
        IProjectController projectController_;
        Guid animationId_ = Guid.Empty;
        Guid ownerId_ = Guid.Empty;
        Guid alphaMaskSheetId_ = Guid.Empty;

        public AnimationDtoProxy(IProjectController projectController, Guid animationId)
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
                AnimationDto animation = projectController_.GetAnimation(animationId_);

                return animation.Name;
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

                    foreach (AnimationGroupDto animationGroup in project.AnimationGroups)
                    {
                        for (int i = 0; i < project.Animations[animationGroup.Id].Count; i++)
                        {
                            AnimationDto currentAnimation = project.Animations[animationGroup.Id][i];

                            if (value.ToUpper() == currentAnimation.Name.ToUpper() && animationId_ != currentAnimation.Id)
                            {
                                isValid = false;
                                break;
                            }
                        }
                    }

                    if (isValid == false)
                    {
                        System.Windows.Forms.MessageBox.Show("Animation name is already in use.", "Name In Use", MessageBoxButtons.OK);
                    }
                }

                if (isValid == true)
                {
                    projectController_.SetAnimationName(animationId_, value);
                }
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
                AnimationDto animation = projectController_.GetAnimation(animationId_);

                return animation.OwnerId;
            }
        }

        [BrowsableAttribute(false)]
        public Guid SpriteSheetId
        {
            get
            {
                AnimationDto animation = projectController_.GetAnimation(animationId_);

                return animation.SpriteSheet;               
            }
        }

        [TypeConverter(typeof(SpriteSheetConverter))]
        public string SpriteSheet
        {
            get
            {
                AnimationDto animation = projectController_.GetAnimation(animationId_);
                
                if (animation.SpriteSheet == Guid.Empty)
                {
                    return string.Empty;
                }
                else
                {
                    SpriteSheetDto spriteSheet = projectController_.GetSpriteSheet(animation.SpriteSheet);

                    return spriteSheet.Name;
                }
            }
            set
            {
                Guid spriteSheetId = projectController_.GetSpriteSheetIdFromName(value);

                projectController_.SetAnimationSpriteSheet(animationId_, spriteSheetId);
            }
        }

        [BrowsableAttribute(false)]
        public Guid AlphaMaskSheetId
        {
            get
            {
                return alphaMaskSheetId_;
            }
        }

        [TypeConverter(typeof(SpriteSheetConverter))]
        public string AlphaMaskSheet
        {
            get
            {
                AnimationDto animation = projectController_.GetAnimation(animationId_);

                alphaMaskSheetId_ = animation.AlphaMaskSheet;

                if (alphaMaskSheetId_ == Guid.Empty)
                {
                    return string.Empty;
                }
                else
                {
                    SpriteSheetDto spriteSheet = projectController_.GetSpriteSheet(alphaMaskSheetId_);

                    return spriteSheet.Name;
                }
            }
            set
            {
                Guid alphaMaskSheetId = projectController_.GetSpriteSheetIdFromName(value);

                projectController_.SetAnimationAlphaMaskSheet(animationId_, alphaMaskSheetId);
            }
        }
    }
}
