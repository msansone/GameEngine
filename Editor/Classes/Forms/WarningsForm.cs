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
    public partial class WarningsForm : Form, IWarningsForm
    {
        private List<string> lstWarningMessages_ = new List<string>();
        private List<string> lstWarningPrimarySources_ = new List<string>();
        private List<string> lstWarningSecondarySources_ = new List<string>();
        
        IWin32Window owner_;

        public WarningsForm(IWin32Window owner)
        {
            InitializeComponent();

            owner_ = owner;
        }

        public int WarningCount
        {
            get { return lstWarningMessages_.Count; }
        }

        public void ShowDialog()
        {
            lvWarnings.Items.Clear();

            for (int i = 0; i < lstWarningMessages_.Count; i++)
            {
                ListViewItem warningItem = new ListViewItem();

                warningItem.Text = lstWarningMessages_[i];

                ListViewItem.ListViewSubItem warningRoom = new ListViewItem.ListViewSubItem();
                warningRoom.Text = lstWarningPrimarySources_[i];
                warningItem.SubItems.Add(warningRoom);

                ListViewItem.ListViewSubItem warningLayer = new ListViewItem.ListViewSubItem();
                warningLayer.Text = lstWarningSecondarySources_[i];
                warningItem.SubItems.Add(warningLayer);

                lvWarnings.Items.Add(warningItem);
            }

            base.ShowDialog(owner_);
        }

        public void ClearWarningMessages()
        {
            lstWarningMessages_.Clear();
            lstWarningPrimarySources_.Clear();
            lstWarningSecondarySources_.Clear();
        }

        public void AddWarningMessage(string message, string rootSource, string subSource)
        {
            lstWarningMessages_.Add(message);
            lstWarningPrimarySources_.Add(rootSource);
            lstWarningSecondarySources_.Add(subSource);
        }
    }

    public interface IWarningsForm
    {
        int WarningCount { get; }

        void ShowDialog();
        void Dispose();

        void ClearWarningMessages();
        void AddWarningMessage(string message, string rootSource, string subSource);
    }
}
