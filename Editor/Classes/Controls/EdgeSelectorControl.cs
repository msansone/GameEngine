using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace FiremelonEditor2
{
    public partial class EdgeSelectorControl : UserControl
    {
        public EdgeSelectorControl(WorldGeometryEdgesDto edges, IWindowsFormsEditorService editorService)
        {
            InitializeComponent();

            edges_ = edges;

            clbEdges.SetItemChecked(0, edges.UseTopEdge);
            clbEdges.SetItemChecked(1, edges.UseRightEdge);
            clbEdges.SetItemChecked(2, edges.UseBottomEdge);
            clbEdges.SetItemChecked(3, edges.UseLeftEdge);
        }
        
        private WorldGeometryEdgesDto edges_;

        private void clbEdges_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            switch (clbEdges.Items[e.Index].ToString().ToUpper())
            {
                case "TOP":

                    edges_.UseTopEdge = e.NewValue == CheckState.Checked;

                    break;

                case "RIGHT":

                    edges_.UseRightEdge = e.NewValue == CheckState.Checked;

                    break;

                case "BOTTOM":

                    edges_.UseBottomEdge = e.NewValue == CheckState.Checked;

                    break;

                case "LEFT":

                    edges_.UseLeftEdge = e.NewValue == CheckState.Checked;

                    break;
            }
        }
    }
}
