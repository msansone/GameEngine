using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public class EntityClassificationDto : BaseDto
    {
    }

    // Removed in 2.1
    //class EntityClassificationDtoProxy : IEntityClassificationDtoProxy
    //{
    //    IProjectController projectController_;
    //    Guid entityClassificationId_;

    //    public EntityClassificationDtoProxy(IProjectController projectController, Guid entityClassificationId)
    //    {
    //        projectController_ = projectController;
    //        entityClassificationId_ = entityClassificationId;
    //    }

    //    [CategoryAttribute("(ID Settings)"),
    //     DescriptionAttribute("Unique Name String"),
    //     ParenthesizePropertyName(true)]
    //    public string Name
    //    {
    //        get
    //        {
    //            EntityClassificationDto entityClassification = projectController_.GetEntityClassification(entityClassificationId_);

    //            return entityClassification.Name;
    //        }
    //        set
    //        {
    //            try
    //            {
    //                projectController_.SetEntityClassificationName(entityClassificationId_, value);
    //            }
    //            catch (InvalidNameException ex)
    //            {
    //                MessageBox.Show(ex.Message);
    //            }
    //        }
    //    }

    //    [BrowsableAttribute(false)]
    //    public Guid Id
    //    {
    //        get
    //        {
    //            return entityClassificationId_;
    //        }
    //    }

    //    [BrowsableAttribute(false)]
    //    public Guid OwnerId
    //    {
    //        get
    //        {
    //            EntityClassificationDto entityClassification = projectController_.GetEntityClassification(entityClassificationId_);

    //            return entityClassification.OwnerId;
    //        }
    //    }
    //}
}
