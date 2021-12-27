using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public interface IRoomPropertiesEditor
    {
        void ShowDialog(IWin32Window owner, IProjectController projectController, Guid roomId);
        void Dispose();
    }
}
