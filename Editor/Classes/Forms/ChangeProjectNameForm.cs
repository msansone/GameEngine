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
    public delegate void ProjectNameChangedHandler(object sender, ProjectNameChangedEventArgs e);

    public partial class ChangeProjectNameForm : Form, IChangeProjectNameDialog
    {
        public ChangeProjectNameForm()
        {
            InitializeComponent();
        }

        public event ProjectNameChangedHandler ProjectNameChanged;
        
        new public void ShowDialog(IWin32Window owner)
        {
            base.ShowDialog(owner);
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            // Validate data
            bool isValidData = true;
            
            if (string.IsNullOrWhiteSpace(txtProjectName.Text))
            {
                isValidData = false;
                txtProjectName.BackColor = Color.Salmon;
            }
            
            if (isValidData == true)
            {
                ProjectNameChangedEventArgs projectNameChangedEventArgs = new ProjectNameChangedEventArgs(txtProjectName.Text);

                OnProjectNameChanged(projectNameChangedEventArgs);
                
                this.Hide();
            }
        }

        protected virtual void OnProjectNameChanged(ProjectNameChangedEventArgs e)
        {
            ProjectNameChanged(this, e);
        }

        private void txtProjectName_Enter(object sender, EventArgs e)
        {
            if (txtProjectName.BackColor == Color.Salmon)
            {
                txtProjectName.BackColor = Color.LightSalmon;
            }
            else
            {
                txtProjectName.BackColor = Color.Beige;
            }
        }

        private void txtProjectName_Leave(object sender, EventArgs e)
        {
            if (txtProjectName.Text == string.Empty)
            {
                txtProjectName.BackColor = Color.Salmon;
            }
            else
            {
                txtProjectName.BackColor = Color.White;
            }
        }

        private void txtProjectName_KeyPress(object sender, KeyPressEventArgs e)
        {
            // This value is going to be used as a folder name and in c++ classes.
            // Only allow alpha characters.
            const char delete = (char)0x08;
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != delete)
            {
                e.Handled = true;
            }
        }
    }

    public interface IChangeProjectNameDialog
    {
        event ProjectNameChangedHandler ProjectNameChanged;
        
        void ShowDialog(IWin32Window owner);
    }

    public class ProjectNameChangedEventArgs : System.EventArgs
    {
        // Constructor
        public ProjectNameChangedEventArgs(string projectName)
        {
            projectName_ = projectName;
        }

        // Properties        
        public string ProjectName
        {
            get { return projectName_; }
        }
        private string projectName_;        
    }
}
