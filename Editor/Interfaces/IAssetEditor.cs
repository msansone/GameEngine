using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public interface IAssetEditor
    {
        void ShowDialog(IWin32Window owner, IProjectController projectController, INameValidator nameValidator, INameGenerator nameGenerator, IExceptionHandler exceptionHandler);
        void Dispose();
    }
}
