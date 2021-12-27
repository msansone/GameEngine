using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    [Editor(typeof(WorldGeometryEdgeSelectorTypeEditor), typeof(UITypeEditor))]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class WorldGeometryEdgesDto
    {
        public bool UseBottomEdge = true;
        public bool UseLeftEdge = true;
        public bool UseRightEdge = true;
        public bool UseTopEdge = true;

        public override string ToString()
        {
            string literal = string.Empty;

            string[] values = new string[4];

            if (UseTopEdge == true)
            {
                values[0] = "Top";
            }

            if (UseRightEdge == true)
            {
                values[1] = "Right";
            }

            if (UseBottomEdge == true)
            {
                values[2] = "Bottom";
            }

            if (UseLeftEdge == true)
            {
                values[3] = "Left";
            }

            return string.Join(",", values.Where(c => !string.IsNullOrEmpty(c)));
        }
    }
}
