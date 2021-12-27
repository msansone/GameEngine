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
    public delegate void StringReplacedHandler(object sender, StringReplacedEventArgs e);

    public partial class StringReplacementDialog : Form, IStringReplacementDialog
    {
        public event StringReplacedHandler StringReplaced;

        public StringReplacementDialog()
        {
            InitializeComponent();
        }

        new public void ShowDialog(IWin32Window owner)
        {
            base.ShowDialog(owner);
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            OnStringReplaced(new StringReplacedEventArgs(txtOldValue.Text, txtNewValue.Text));

            this.Hide();
        }

        protected virtual void OnStringReplaced(StringReplacedEventArgs e)
        {
            StringReplaced(this, e);
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }

    public interface IStringReplacementDialog
    {
        event StringReplacedHandler StringReplaced;
    
        void ShowDialog(IWin32Window owner);
    }

    public class StringReplacedEventArgs : System.EventArgs
    {
        // Constructor
        public StringReplacedEventArgs(string oldValue, string newValue)
        {
            oldValue_ = oldValue;

            newValue_ = newValue;

            cancel_ = false;
        }

        // Properties
        public string OldValue
        {
            get { return oldValue_; }
        }
        private string oldValue_;

        public string NewValue
        {
            get { return newValue_; }
        }
        private string newValue_;

        public bool Cancel
        {
            get { return cancel_; }
            set { cancel_ = value; }
        }
        private bool cancel_;
    }
}