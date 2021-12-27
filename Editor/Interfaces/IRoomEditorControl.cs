using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiremelonEditor2
{
    public interface IRoomEditorControl
    {
        //event SelectionToggleHandler SelectionToggle;
        //event EditModeChangedHandler EditModeChanged;
        event CursorChangedHandler CursorChanged;
        
        //void Clear();
        //void Initialize();
        //void SelectAll();
        //void CopySelection();
        //void PasteSelection();
        //void FillSelection();
        //void CreateTileObjectFromSelection();

        //void RefreshScrollbars();
        //void ResetCursor();
        //void ToggleGrid();
        //void ToggleOutlines();
        //void ToggleTransparentSelect();
        
        // Derived from base class
        void Dispose();
        void Refresh();

        bool IsMouseOver { get; }
        int Left { get; set; }
        int Top { get; set; }
        int Height { get; set; }
        int Width { get; set; }
        string Name { get; set; }
        int TabIndex { get; set; }
        System.Drawing.Size Size { get; set; }
        System.Drawing.Point Location { get; set; }
    }
}
