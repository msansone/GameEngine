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
    public delegate void NameEnteredHandler(object sender, NameEnteredEventArgs e);

    public partial class EnterNameForm : Form, IEnterNameDialog
    {
        public EnterNameForm()
        {
            InitializeComponent();
        }

        public event NameEnteredHandler NameEntered;
        
        new public void ShowDialog(IWin32Window owner)
        {
            base.ShowDialog(owner);
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            // Validate data
            bool isValidData = true;

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                isValidData = false;
                txtName.BackColor = Color.Salmon;
            }

            if (isValidData == true)
            {
                NameEnteredEventArgs nameEnteredEventArgs = new NameEnteredEventArgs(txtName.Text);

                OnNameEntered(nameEnteredEventArgs);

                if (nameEnteredEventArgs.Cancel == false)
                {
                    this.Hide();
                }
                else
                {
                    txtName.BackColor = Color.Salmon;
                }
            }
        }

        protected virtual void OnNameEntered(NameEnteredEventArgs e)
        {
            NameEntered(this, e);
        }

        private void txtName_Enter(object sender, EventArgs e)
        {
            if (txtName.BackColor == Color.Salmon)
            {
                txtName.BackColor = Color.LightSalmon;
            }
            else
            {
                txtName.BackColor = Color.Beige;
            }
        }

        private void txtName_Leave(object sender, EventArgs e)
        {
            if (txtName.Text == string.Empty)
            {
                txtName.BackColor = Color.Salmon;
            }
            else
            {
                txtName.BackColor = Color.White;
            }
        }
    }

    public interface IEnterNameDialog
    {
        event NameEnteredHandler NameEntered;
        
        void ShowDialog(IWin32Window owner);
    }

    public class NameEnteredEventArgs : System.EventArgs
    {
        // Constructor
        public NameEnteredEventArgs(string name)
        {
            name_ = name;
        }

        // Properties        
        public bool Cancel
        {
            get { return cancel_; }
            set { cancel_ = value; }
        }
        private bool cancel_;

        public string Name
        {
            get { return name_; }
        }
        private string name_;
    }
}
