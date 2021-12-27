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

    public class HitboxIdentityDto : BaseDto
    {
    }

    public class HitboxIdentityDtoProxy : IHitboxIdentityDtoProxy
    {
        private IProjectController projectController_;
        private Guid hitboxIdentityId_;

        public HitboxIdentityDtoProxy(IProjectController projectController, Guid hitboxIdentityId)
        {
            projectController_ = projectController;
            hitboxIdentityId_ = hitboxIdentityId;
        }

        [CategoryAttribute("(ID Settings)"),
         DescriptionAttribute("Unique Name String"),
         ParenthesizePropertyName(true)]
        public string Name
        {
            get
            {
                HitboxIdentityDto hitboxIdentity = projectController_.GetHitboxIdentity(hitboxIdentityId_);

                return hitboxIdentity.Name;
            }
            set
            {
                bool isValid = true;

                HitboxIdentityDto hitboxIdentity = projectController_.GetHitboxIdentity(hitboxIdentityId_);

                Guid ownerId = hitboxIdentity.OwnerId;

                // Validate the name.
                if (value == string.Empty)
                {
                    System.Windows.Forms.MessageBox.Show("Hitbox Identity name cannot be blank.", "Invalid Value", MessageBoxButtons.OK);

                    isValid = false;
                }
                else
                {
                    ProjectDto project = projectController_.GetProjectDto();

                    for (int i = 0; i < project.HitboxIdentities.Count; i++)
                    {
                        HitboxIdentityDto currentHitboxIdentity = project.HitboxIdentities[i];

                        if (value.ToUpper() == currentHitboxIdentity.Name.ToUpper() && hitboxIdentityId_ != currentHitboxIdentity.Id)
                        {
                            isValid = false;
                            break;
                        }
                    }

                    if (isValid == false)
                    {
                        System.Windows.Forms.MessageBox.Show("Hitbox Identity name is already in use.", "Name In Use", MessageBoxButtons.OK);
                    }
                }

                if (isValid == true)
                {
                    projectController_.SetHitboxIdentityName(hitboxIdentityId_, value);
                }
            }
        }

        [BrowsableAttribute(false)]
        public Guid Id
        {
            get
            {
                return hitboxIdentityId_;
            }

        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                HitboxIdentityDto hitboxIdentity = projectController_.GetHitboxIdentity(hitboxIdentityId_);

                return hitboxIdentity.OwnerId;
            }
        }
    }
}
