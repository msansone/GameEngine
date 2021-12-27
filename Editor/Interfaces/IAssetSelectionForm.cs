using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public interface IAssetSelectionForm
    {
        event FormHiddenHandler FormHidden;

        IAssetSelectionControl AssetSelectionControl { get; }

        IMapWidgetPropertiesControl PropertiesControl { set; }

        bool IsClosed { get; set; }

        // Derived from base.
        int Width { get; set; }
        int Height { get; set; }
        int Bottom { get; }
        int Left { get; set; }
        int Top { get; set; }
        bool TopLevel { get; set; }

        void Show(IWin32Window owner);
        void Hide();
        void Close();
    }
}
