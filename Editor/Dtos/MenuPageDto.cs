using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public class MenuPageDto : BaseDto
    {
    }

    //public class MenuPageDtoProxy : IMenuPageDtoProxy
    //{
    //    private IProjectController projectController_;
    //    private Guid menuPageId_;

    //    public MenuPageDtoProxy(IProjectController projectController, Guid menuPageId)
    //    {
    //        projectController_ = projectController;
    //        menuPageId_ = menuPageId;
    //    }

    //    [CategoryAttribute("(ID Settings)"),
    //     DescriptionAttribute("Unique Name String"),
    //     ParenthesizePropertyName(true)]
    //    public string Name
    //    {
    //        get
    //        {
    //            MenuPageDto menuPage = projectController_.GetMenuPage(menuPageId_);

    //            return menuPage.Name;
    //        }
    //        set
    //        {
    //            bool isValid = true;

    //            // Validate the name.
    //            if (value == string.Empty)
    //            {
    //                System.Windows.Forms.MessageBox.Show("Menu page name cannot be blank.", "Invalid Value", MessageBoxButtons.OK);

    //                isValid = false;
    //            }
    //            else
    //            {
    //                MenuPageDto menuPage = projectController_.GetMenuPage(menuPageId_);

    //                ProjectDto project = projectController_.GetProjectDto();

    //                for (int i = 0; i < project.MenuPages.Count; i++)
    //                {
    //                    MenuPageDto currentMenuPage = project.MenuPages[i];

    //                    if (value.ToUpper() == currentMenuPage.Name.ToUpper() && menuPageId_ != currentMenuPage.Id)
    //                    {
    //                        isValid = false;
    //                        break;
    //                    }
    //                }

    //                if (isValid == false)
    //                {
    //                    System.Windows.Forms.MessageBox.Show("Menu page name is already in use.", "Name In Use", MessageBoxButtons.OK);
    //                }
    //            }

    //            if (isValid == true)
    //            {
    //                projectController_.SetMenuPageName(menuPageId_, value);
    //            }
    //        }
    //    }

    //    [BrowsableAttribute(false)]
    //    public Guid Id
    //    {
    //        get { return menuPageId_; }
    //    }

    //    [BrowsableAttribute(false)]
    //    public Guid OwnerId
    //    {
    //        get
    //        {
    //            MenuPageDto menuPage = projectController_.GetMenuPage(menuPageId_);

    //            return menuPage.OwnerId;
    //        }
    //    }
    //}
}
