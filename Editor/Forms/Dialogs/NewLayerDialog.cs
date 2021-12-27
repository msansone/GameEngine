using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public delegate void LayerAddedHandler(object sender, LayerAddedEventArgs e);

    public partial class NewLayerDialog : Form, INewLayerDialog
    {
        public event LayerAddedHandler LayerAdded;

        private int minCols_;
        private int minRows_;

        public NewLayerDialog()
        {
            InitializeComponent();

            minCols_ = 0;
            minRows_ = 0;
        }

        private void txtCols_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        public int MinRows
        {
            set
            {
                minRows_ = value;
            }
        }

        public int MinCols
        {
            set
            {
                minCols_ = value;
            }
        }

        private void txtRows_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        protected virtual void OnLayerAdded(LayerAddedEventArgs e)
        {
            LayerAdded(this, e);
        }

        private void NewLayerDialog_Load(object sender, EventArgs e)
        {
            txtLayerName.Text = string.Empty;
            txtRows.Text = string.Empty;
            txtCols.Text = string.Empty;
            
            this.ActiveControl = txtLayerName;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            // Validate data
            bool isValidData = true;
            
            int rows = 0;
            int cols = 0;

            if (txtRows.Text.Length > 0)
            {
                rows = Convert.ToInt32(txtRows.Text);

                if (rows < minRows_)
                {
                    isValidData = false;
                    txtRows.BackColor = Color.Salmon;
                }
            }
            else
            {
                isValidData = false;
                txtRows.BackColor = Color.Salmon;
            }

            if (txtCols.Text.Length > 0)
            {
                cols = Convert.ToInt32(txtCols.Text);

                if (cols < minCols_)
                {
                    isValidData = false;
                    txtCols.BackColor = Color.Salmon;
                }

            }
            else
            {
                isValidData = false;
                txtCols.BackColor = Color.Salmon;
            }

            if (txtLayerName.Text.Length == 0)
            {
                isValidData = false;
                txtLayerName.BackColor = Color.Salmon;
            }

            if (isValidData == true)
            {
                LayerAddedEventArgs e2 = new LayerAddedEventArgs(txtLayerName.Text, cols, rows);

                OnLayerAdded(e2);

                if (e2.Cancel == true)
                {
                    System.Windows.Forms.MessageBox.Show("Layer name already exists.");
                    txtLayerName.BackColor = Color.Salmon;
                }
                else
                {
                    this.Hide();
                }
            }
        }

        private void txtLayerName_Enter(object sender, EventArgs e)
        {
            if (txtLayerName.BackColor == Color.Salmon)
            {
                txtLayerName.BackColor = Color.LightSalmon;
            }
            else
            {
                txtLayerName.BackColor = Color.Beige;
            }
        }

        private void txtLayerName_Leave(object sender, EventArgs e)
        {
            if (txtLayerName.Text == string.Empty)
            {
                txtLayerName.BackColor = Color.Salmon;
            }
            else
            {
                txtLayerName.BackColor = Color.White;
            }
        }

        private void txtRows_Enter(object sender, EventArgs e)
        {
            if (txtRows.BackColor == Color.Salmon)
            {
                txtRows.BackColor = Color.LightSalmon;
            }
            else
            {
                txtRows.BackColor = Color.Beige;
            }
        }

        private void txtRows_Leave(object sender, EventArgs e)
        {
            // Validate the data.
            if (txtRows.Text.Length == 0 || Convert.ToInt32(txtRows.Text) < minRows_)
            {
                txtRows.BackColor = Color.Salmon;
                txtRows.ForeColor = Color.DarkRed;
            }
            else
            {
                txtRows.BackColor = Color.White;
                txtRows.ForeColor = Color.Black;
            }
        }

        private void txtCols_Enter(object sender, EventArgs e)
        {
            if (txtCols.BackColor == Color.Salmon)
            {
                txtCols.BackColor = Color.LightSalmon;
            }
            else
            {
                txtCols.BackColor = Color.Beige;
            }
        }

        private void txtCols_Leave(object sender, EventArgs e)
        {
            // Validate the data.
            if (txtCols.Text.Length == 0 || Convert.ToInt32(txtCols.Text) < minCols_)
            {
                txtCols.BackColor = Color.Salmon;
                txtCols.ForeColor = Color.DarkRed;
            }
            else
            {
                txtCols.BackColor = Color.White;
                txtCols.ForeColor = Color.Black;
            }
        }

        private void NewLayerDialog_Shown(object sender, EventArgs e)
        {
            txtLayerName.Focus();

            txtLayerName.BackColor = Color.White;
            txtLayerName.ForeColor = Color.Black;

            txtCols.BackColor = Color.White;
            txtCols.ForeColor = Color.Black;

            txtRows.BackColor = Color.White;
            txtRows.ForeColor = Color.Black;
        }

        new public void ShowDialog(IWin32Window owner)
        {
            base.ShowDialog(owner);
        }
    }

    public interface INewLayerDialog
    {
        event LayerAddedHandler LayerAdded;

        int MinCols { set; }
        int MinRows { set; }

        void ShowDialog(IWin32Window owner);
    }

    public class LayerAddedEventArgs : System.EventArgs
    {
        // Fields
        private string layerName_;
        private int layerRows_;
        private int layerCols_;
        private bool cancel_;

        // Constructor
        public LayerAddedEventArgs(string layerName, int layerCols, int layerRows)
        {
            layerName_ = layerName;
            layerRows_ = layerRows;
            layerCols_ = layerCols;
            cancel_ = false;
        }

        // Properties
        public string LayerName
        {
            get { return layerName_; }
        }

        public int LayerRows
        {
            get { return layerRows_; }
        }

        public int LayerCols
        {
            get { return layerCols_; }
        }

        public bool Cancel
        {
            set
            {
                cancel_ = value;
            }
            get 
            { 
                return cancel_; 
            }
        }
    }

    public class LayerResizedEventArgs : System.EventArgs
    {
        // Fields
        private Guid layerId_;
        private int layerRows_;
        private int layerCols_;
        private bool cancel_;

        // Constructor
        public LayerResizedEventArgs(Guid layerId, int layerCols, int layerRows)
        {
            layerId_ = layerId;
            layerRows_ = layerRows;
            layerCols_ = layerCols;
            cancel_ = false;
        }

        // Properties
        public Guid LayerId
        {
            get { return layerId_; }
        }

        public int LayerRows
        {
            get { return layerRows_; }
        }

        public int LayerCols
        {
            get { return layerCols_; }
        }
    }
}
