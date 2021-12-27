using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpriteSheetBuilder
{
    public partial class ColorMapControl : UserControl
    {
        public ColorMapControl(Dictionary<Color, Color> colorMap, Color fromColor)
        {
            InitializeComponent();

            colorMap_ = colorMap;

            fromColor_ = fromColor;

            btnFromColor.BackColor = fromColor;

            btnToColor.BackColor = colorMap[fromColor];
        }

        private Dictionary<Color, Color> colorMap_;

        private Color fromColor_;

        private void btnToColor_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgColorChooser.ShowDialog();

            if (result == DialogResult.OK)
            {
                btnToColor.BackColor = dlgColorChooser.Color;

                colorMap_[fromColor_] = btnToColor.BackColor;
            }
        }
    }
}
