using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FiremelonEditor2
{
    public interface IMapWidgetController
    {
        void DeserializeFromString(string data);
        Rectangle GetBoundingRect();
        void Initialize();
        void MouseDown(System.Windows.Forms.MouseEventArgs e);
        void MouseMove();
        void MouseUp(System.Windows.Forms.MouseEventArgs e);
        void PropertyValueChanged(string name, ref object value, ref bool cancel);
        void RenderBackground(Graphics g, int x, int y);
        void Render(Graphics g, int x, int y);
        void RenderOverlay(Graphics g, Point viewOffset, bool isSelected, bool isSingularSelection, bool showOutline);
        void ResetProperties(MapWidgetProperties properties);
        string SerializeToString();
        void UpdatePosition(Point2D position);
        
        bool GridAligned { get; }
        MapWidgetDto MapWidget { set; }
    }
}
