using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public partial class ResourcesForm : Form, IResourcesForm
    {
        #region Constructors

        public ResourcesForm(IProjectController projectController)
        {
            InitializeComponent();

            IFiremelonEditorFactory firemelonEditorFactory = new FiremelonEditorFactory();

            resourceManagerControl_ = firemelonEditorFactory.NewResourceManagerControl(projectController);

            ((Control)resourceManagerControl_).Dock = DockStyle.Fill;

            this.Controls.Add(((Control)resourceManagerControl_));
        }
        
        #endregion

        #region Private Variables
        
        IResourceManagerControl resourceManagerControl_;

        #endregion

        #region Public Functions

        new public void ShowDialog(IWin32Window owner)
        {
            base.ShowDialog(owner);
        }

        #endregion        
    }

    public interface IResourcesForm
    {
        // Derived from base.
        int Width { get; set; }
        int Height { get; set; }
        int Bottom { get; }
        int Left { get; set; }
        int Top { get; set; }
        bool TopLevel { get; set; }

        void Show(IWin32Window owner);
        void ShowDialog(IWin32Window owner);
        void Hide();
        void Close();
    }
}
