using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public partial class ColorMapControl : UserControl
    {
        public ColorMapControl(KeyValuePair<Color, Color> colorMap)
        {
            InitializeComponent();

            colorMap_ = colorMap;

            btnFromColor.BackColor = colorMap.Key;

            btnToColor.BackColor = colorMap.Value;
        }

        private KeyValuePair<Color, Color> colorMap_;

        private void btnToColor_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgColorChooser.ShowDialog();

            if (result == DialogResult.OK)
            {
                btnToColor.BackColor = dlgColorChooser.Color;
            }
        }
    }
}
