using ScintillaNET;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public interface IFindReplaceDialog
    {
        event FindNextStringHandler FindNextString;

        SearchFlags SearchFlags { get; }

        string TokenToFind { get; set; }

        bool Visible { get; set; }

        void Show(IWin32Window owner, string findToken);
    }
}
