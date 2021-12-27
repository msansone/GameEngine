using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public partial class ProgressForm : Form, IProgressForm
    {
        public ProgressForm()
        {
            InitializeComponent();            
        }

        public void CenterToScreen()
        {
            base.CenterToScreen();
        }

        public void SetStatus(string status)
        {
            lbStatus.Text = status;

            this.Refresh();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(255, 192, 192, 255), Color.White, 0.0f))
            {
                e.Graphics.FillRectangle(brush, panel4.ClientRectangle);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, Color.WhiteSmoke, Color.White, 0.0f))
            {
                e.Graphics.FillRectangle(brush, panel1.ClientRectangle);
            }
        }
    }

    public interface IProgressForm
    {
        void SetStatus(string status);

        // Derived from base.
        int Width { get; set; }
        int Height { get; set; }
        int Bottom { get; }
        int Left { get; set; }
        int Top { get; set; }
        bool TopLevel { get; set; }

        void CenterToScreen();
        void Close();
        void Hide();
        void Show(IWin32Window owner);
    }
}
