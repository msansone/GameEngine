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

//using Microsoft.ReportingServices.QueryDesigners.Interop;

namespace FiremelonEditor2
{
    public partial class PaletteMapControl : UserControl
    {
        public PaletteMapControl(PaletteMap paletteMap, IWindowsFormsEditorService editorService)
        {
            InitializeComponent();
            
            paletteMap_ = paletteMap;

            // Go through each color in the palette map, and create a color map control of size 80x20.
            foreach (KeyValuePair<Color, Color> colorMap in paletteMap.ColorMap)
            {
                ColorMapControl colorMapControl = new ColorMapControl(colorMap);

                colorMapControl.Size = new Size(80, 20);

                flpnlColorMaps.Controls.Add(colorMapControl);
            }
        }

        private PaletteMap paletteMap_;

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
