using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public interface ITileSheetEditor
    {
        void ShowDialog(IWin32Window owner, ITileSheetDtoProxy tileSheetProxy);
        void Dispose();
    }
}
