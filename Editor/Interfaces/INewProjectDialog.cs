using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public interface INewProjectDialog
    {
        IProjectController ProjectController { set; }

        void ShowDialog(IWin32Window owner);
    }
}
