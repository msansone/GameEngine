using ScintillaNET;
using System;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    #region Delegates

    public delegate void FindNextStringHandler(object sender, FindNextStringEventArgs e);

    #endregion

    public partial class FindReplaceDialog : Form, IFindReplaceDialog
    {
        #region Events

        public event FindNextStringHandler FindNextString;

        #endregion

        #region Constructors

        public FindReplaceDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Variables

        string findToken_ = string.Empty;

        #endregion
        
        #region Properties

        public string TokenToFind
        {
            get
            {
                return txtFind.Text;
            }
            set
            {
                txtFind.Text = value;
            }
        }

        public SearchFlags SearchFlags
        {
            get
            {
                return getSearchFlags();
            }
        }

        #endregion

        #region Public Functions

        public void Show(IWin32Window owner, string findToken)
        {
            findToken_ = findToken;

            base.Show(owner);
        }

        #endregion

        #region Private Functions

        private SearchFlags getSearchFlags()
        {
            SearchFlags searchFlags = SearchFlags.None;

            if (chkFindWholeWordOnly.Checked == true)
            {
                searchFlags |= SearchFlags.WholeWord;
            }

            if (chkMatchCase.Checked == true)
            {
                searchFlags |= SearchFlags.MatchCase;
            }

             return searchFlags;
        }

        #endregion

        #region Event Handlers

        private void btnFindNext_Click(object sender, EventArgs e)
        {
            OnFindNextString(new FindNextStringEventArgs(txtFind.Text, getSearchFlags()));
        }

        private void FindReplaceDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Don't close, it will dispose the object. Just hide it instead.
            e.Cancel = true;

            this.Hide();
        }

        private void FindReplaceDialog_Shown(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(findToken_) == false)
            {
                txtFind.Text = findToken_;
            }
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnFindNextString(new FindNextStringEventArgs(txtFind.Text, getSearchFlags()));
        }

        #endregion

        #region Event Dispatchers

        protected virtual void OnFindNextString(FindNextStringEventArgs e)
        {
            FindNextString?.Invoke(this, e);
        }

        #endregion

    }

    #region Event Arts

    public class FindNextStringEventArgs : System.EventArgs
    {
        // Fields
        private string find_;
        private SearchFlags searchFlags_;

        // Constructor
        public FindNextStringEventArgs(string find, SearchFlags searchFlags)
        {
            find_ = find;
            searchFlags_ = searchFlags;
        }

        // Properties       
        public string Find
        {
            get { return find_; }
        }

        public SearchFlags SearchFlags
        {
            get { return searchFlags_; }
        }
    }

    #endregion
}
