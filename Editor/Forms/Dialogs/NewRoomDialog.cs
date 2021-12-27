using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public delegate void NewRoomHandler(object sender, NewRoomEventArgs e);

    public partial class NewRoomDialog : Form, INewRoomDialog
    {
        #region Events

        public event NewRoomHandler NewRoom;

        #endregion

        #region Constructors

        public NewRoomDialog(IProjectController projectController, INameValidator nameValidator)
        {
            InitializeComponent();
            cameraHeight_ = 0;
            cameraWidth_ = 0;
            tileSize_ = 0;

            nameValidator_ = nameValidator;
            projectController_ = projectController;
        }

        #endregion

        #region Private Variables

        private INameValidator nameValidator_;

        private IProjectController projectController_;

        #endregion

        #region Properties

        public int CameraHeight
        {
            set
            {
                cameraHeight_ = value;
            }
        }
        private int cameraHeight_;

        public int CameraWidth
        {
            set
            {
                cameraWidth_ = value;
            }
        }
        private int cameraWidth_;

        public int TileSize
        {
            set
            {
                tileSize_ = value;
            }
        }
        private int tileSize_;

        #endregion

        #region Event Handlers

        private void txtRoomName_Enter(object sender, EventArgs e)
        {
            pbRoomNameError.Visible = false;

            if (txtRoomName.BackColor == Color.Salmon)
            {
                txtRoomName.BackColor = Color.LightSalmon;
            }
            else
            {
                txtRoomName.BackColor = Color.Beige;
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

        private void txtRows_Leave(object sender, EventArgs e)
        {
            // Validate the data.   
            int minRows = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(cameraHeight_) / Convert.ToDouble(tileSize_)));

            if (txtRows.Text.Length == 0 || Convert.ToInt32(txtRows.Text) < minRows)
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

        private void txtCols_Leave(object sender, EventArgs e)
        {
            // Validate the data.
            int minColumns = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(cameraWidth_) / Convert.ToDouble(tileSize_)));

            if (txtCols.Text.Length == 0 || Convert.ToInt32(txtCols.Text) < minColumns)
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

        private void txtLayerName_Leave(object sender, EventArgs e)
        {
            if (txtLayerName.Text == string.Empty)
            {
                txtLayerName.BackColor = Color.Salmon;
                txtLayerName.ForeColor = Color.DarkRed;
            }
            else
            {
                txtLayerName.BackColor = Color.White;
                txtLayerName.ForeColor = Color.Black;
            }
        }

        private void txtCols_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtRows_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtRoomName_Leave(object sender, EventArgs e)
        {
            if (txtRoomName.Text == string.Empty)
            {
                txtRoomName.BackColor = Color.Salmon;
                txtRoomName.ForeColor = Color.DarkRed;
            }
            else
            {
                txtRoomName.BackColor = Color.White;
                txtRoomName.ForeColor = Color.Black;
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            bool isValidData = true;

            int minColumns = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(cameraWidth_) / Convert.ToDouble(tileSize_)));
            int minRows = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(cameraHeight_) / Convert.ToDouble(tileSize_)));

            int rows = 0;
            int cols = 0;

            if (txtRows.Text.Length > 0)
            {
                rows = Convert.ToInt32(txtRows.Text);

                if (rows < minRows)
                {
                    isValidData = false;
                    txtRows.BackColor = Color.Salmon;
                    txtRows.ForeColor = Color.DarkRed;
                }
            }
            else
            {
                isValidData = false;
                txtRows.BackColor = Color.Salmon;
                txtRows.ForeColor = Color.DarkRed;
            }

            if (txtCols.Text.Length > 0)
            {
                cols = Convert.ToInt32(txtCols.Text);

                if (cols < minColumns)
                {
                    isValidData = false;
                    txtCols.BackColor = Color.Salmon;
                    txtCols.ForeColor = Color.DarkRed;
                }
            }
            else
            {
                isValidData = false;
                txtCols.BackColor = Color.Salmon;
                txtCols.ForeColor = Color.DarkRed;
            }

            if (txtLayerName.Text.Length == 0)
            {
                isValidData = false;
                txtLayerName.BackColor = Color.Salmon;
                txtLayerName.ForeColor = Color.DarkRed;
            }

            IUtilityFactory factory = new UtilityFactory();
            
            string errorMessage = string.Empty;
            
            try
            {
                nameValidator_.ValidateAssetName(Guid.Empty, projectController_.GetProjectDto(), txtRoomName.Text);
            }
            catch (InvalidNameException ex)
            {
                errorMessage = ex.Message.ToString();
            }
            
            if (errorMessage.Length > 0)
            {
                pbRoomNameError.Visible = true;

                ttRoomNameError.SetToolTip(this.pbRoomNameError, errorMessage);

                isValidData = false;

                txtRoomName.BackColor = Color.Salmon;
                txtRoomName.ForeColor = Color.DarkRed;
            }

            if (isValidData == true)
            {
                NewRoomEventArgs e2 = new NewRoomEventArgs(txtRoomName.Text, txtLayerName.Text, rows, cols);

                OnNewRoom(e2);

                if (e2.Cancel == true)
                {
                    System.Windows.Forms.MessageBox.Show("Room name already exists.");
                    txtRoomName.BackColor = Color.Salmon;
                    txtRoomName.ForeColor = Color.DarkRed;
                }
                else
                {
                    this.Hide();
                }
            }
        }

        private void NewRoomDialog_Load(object sender, EventArgs e)
        {
            txtRows.Text = string.Empty;
            txtRows.BackColor = Color.White;
            txtRows.ForeColor = Color.Black;

            txtCols.Text = string.Empty;
            txtCols.BackColor = Color.White;
            txtCols.ForeColor = Color.Black;

            txtRoomName.Text = string.Empty;
            txtRoomName.BackColor = Color.White;
            txtRoomName.ForeColor = Color.Black;

            txtLayerName.Text = "Layer 1";
            txtLayerName.BackColor = Color.White;
            txtLayerName.ForeColor = Color.Black;

            txtRoomName.Select();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        #endregion

        protected virtual void OnNewRoom(NewRoomEventArgs e)
        {
            NewRoom(this, e);
        }

        new public void ShowDialog(IWin32Window owner)
        {
            base.ShowDialog(owner);
        }
    }

    public interface INewRoomDialog
    {
        event NewRoomHandler NewRoom;

        int CameraHeight { set; }
        int CameraWidth { set; }
        int TileSize { set; }

        void ShowDialog(IWin32Window owner);
    }

    public class NewRoomEventArgs : System.EventArgs
    {
        // Fields
        private string roomName_;
        private string layerName_;
        private int layerRows_;
        private int layerCols_;
        private bool cancel_;

        // Constructor
        public NewRoomEventArgs(string roomName, string layerName, int layerRows, int layerCols)
        {
            roomName_ = roomName;
            layerName_ = layerName;
            layerRows_ = layerRows;
            layerCols_ = layerCols;
            cancel_ = false;
        }

        // Properties
        public string RoomName
        {
            get { return roomName_; }
        }

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
}
