using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace FiremelonEditor2
{
    public partial class PopoutForm : DockContent, IPopoutForm
    {
        private IFiremelonEditorFactory firemelonEditorFactory_;
                
        public PopoutForm()
        {
            InitializeComponent();

            firemelonEditorFactory_ = new FiremelonEditorFactory();       
        }

        public Control ChildControl
        {
            get
            {
                return childControl_;
            }
            set
            {
                childControl_ = value;

                childControl_.Dock = DockStyle.Fill;

                this.Controls.Add(childControl_);
            }
        }
        private Control childControl_;
        
        //protected override void DefWndProc(ref Message m)
        //{
        //    const int WM_MOUSEACTIVATE = 0x21;
        //    const int MA_NOACTIVATE = 0x0003;

        //    switch (m.Msg)
        //    {
        //        case WM_MOUSEACTIVATE:
        //            m.Result = (IntPtr)MA_NOACTIVATE;
        //            return;
        //    }
        //    base.DefWndProc(ref m);
        //}        
    }

    public interface IPopoutForm
    {
        Control ChildControl { get; set; }
        
        string Text { get; set; }

        // Derived from base.
        int Width { get; set; }
        int Height { get; set; }
        int Bottom { get; }
        int Left { get; set; }
        int Top { get; set; }
        bool TopLevel { get; set; }

        DockPane Pane { get; set; }

        void Show(IWin32Window owner);
        void Show(DockPanel dockPanel, DockState dockState);
        void Show(DockPane previousPane, DockAlignment dockAlignment, double proportion);
        void Hide();
        void Close();
    }    
}
