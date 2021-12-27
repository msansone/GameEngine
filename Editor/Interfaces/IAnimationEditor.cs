using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public interface IAnimationEditor
    {
        void ShowDialog(IWin32Window owner, IProjectController projectController, IAnimationDtoProxy animationProxy, bool allowAddHitbox);
        void Dispose();
    }
}
