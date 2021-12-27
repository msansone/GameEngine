using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public class MenuBookDto : BaseDto
    {
        private bool suspendUpdates_;
        public bool SuspendUpdates
        {
            get { return suspendUpdates_; }
            set { suspendUpdates_ = value; }
        }
    }

    //public class MenuBookDtoProxy : IMenuBookDtoProxy
    //{
    //    private IProjectController projectController_;
    //    private Guid menuBookId_;

    //    public MenuBookDtoProxy(IProjectController projectController, Guid menuBookId)
    //    {
    //        projectController_ = projectController;
    //        menuBookId_ = menuBookId;
    //    }

    //    [CategoryAttribute("(ID Settings)"),
    //     DescriptionAttribute("Unique Name String"),
    //     ParenthesizePropertyName(true)]
    //    public string Name
    //    {
    //        get
    //        {
    //            MenuBookDto menuBook = projectController_.GetMenuBook(menuBookId_);

    //            return menuBook.Name;
    //        }
    //        set
    //        {
    //            bool isValid = true;

    //            // Validate the name.
    //            if (value == string.Empty)
    //            {
    //                System.Windows.Forms.MessageBox.Show("Menu book name cannot be blank.", "Invalid Value", MessageBoxButtons.OK);

    //                isValid = false;
    //            }
    //            else
    //            {
    //                MenuBookDto menuBook = projectController_.GetMenuBook(menuBookId_);

    //                ProjectDto project = projectController_.GetProjectDto();

    //                for (int i = 0; i < project.MenuBooks.Count; i++)
    //                {
    //                    MenuBookDto currentMenuBook = project.MenuBooks[i];

    //                    if (value.ToUpper() == currentMenuBook.Name.ToUpper() && menuBookId_ != currentMenuBook.Id)
    //                    {
    //                        isValid = false;
    //                        break;
    //                    }
    //                }

    //                if (isValid == false)
    //                {
    //                    System.Windows.Forms.MessageBox.Show("Menu book name is already in use.", "Name In Use", MessageBoxButtons.OK);
    //                }
    //            }

    //            if (isValid == true)
    //            {
    //                projectController_.SetMenuBookName(menuBookId_, value);
    //            }
    //        }
    //    }

    //    public bool SuspendUpdates
    //    {
    //        get
    //        {
    //            MenuBookDto menuBook = projectController_.GetMenuBook(menuBookId_);

    //            return menuBook.SuspendUpdates;
    //        }
    //        set
    //        {
    //            projectController_.SetMenuBookSuspendUpdates(menuBookId_, value);
    //        }
    //    }

    //    [BrowsableAttribute(false)]
    //    public Guid Id
    //    {
    //        get { return menuBookId_; }
    //    }

    //    [BrowsableAttribute(false)]
    //    public Guid OwnerId
    //    {
    //        get
    //        {
    //            MenuBookDto menuBook = projectController_.GetMenuBook(menuBookId_);

    //            return menuBook.OwnerId;
    //        }
    //    }
    //}
}
