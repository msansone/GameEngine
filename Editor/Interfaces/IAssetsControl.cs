using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiremelonEditor2
{
    public interface IAssetSelectionControl
    {
        bool IsTileObjectSelected { get; }
        IMapWidgetPropertiesControl PropertiesControl { set; }

        // Derived from base.
        bool Enabled { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        int Left { get; set; }
        int Top { get; set; }
        int TabIndex { get; set; }
        string Name { get; set; }
        System.Drawing.Size Size { get; set; }
        System.Drawing.Point Location { get; set; }

        void Dispose();
        void Refresh();
    }
}
