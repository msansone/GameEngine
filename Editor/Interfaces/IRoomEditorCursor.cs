using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FiremelonEditor2
{
    public interface IRoomEditorCursor
    {
        bool GridAligned { get; }

        Point LayerNameOffset { get; set; }

        Point2D Offset { get; set; }

        Size Size { get; set; }

        void Render(Graphics g, int x, int y, int gridOffsetX, int gridOffsetY);
    }
}
