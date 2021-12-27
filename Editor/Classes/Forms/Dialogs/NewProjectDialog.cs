using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public partial class NewProjectDialog : Form, INewProjectDialog
    {
        private IProjectController projectController_;

        private int mapHeight_;
        private int mapWidth_;
        private int tileSize_;

        public NewProjectDialog(IProjectController projectController)
        {
            InitializeComponent();

            projectController_ = projectController;
        }

        public IProjectController ProjectController
        {
            set { projectController_ = value; }
        }

        new public void ShowDialog(IWin32Window owner)
        {
            base.ShowDialog(owner);
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (validateData() == true)
            {
                string tileSize;
                tileSize = cboTileSize.Text.Trim();
                tileSize = tileSize.Substring(0, 2);

                ProjectDto project = new ProjectDto();
                project.ProjectName = txtProjectName.Text;
                project.ProjectFolderFullPath = txtProjectFolder.Text;
                project.CameraWidth = mapWidth_;
                project.CameraHeight = mapHeight_;
                project.TileSize = Convert.ToInt32(tileSize);
                
                RoomDto initialRoom = new RoomDto();
                initialRoom.Name = txtRoomName.Text;
                
                ScriptDto script = new ScriptDto();
                script.ScriptType = ScriptType.Room;
                script.OwnerId = initialRoom.Id;
                script.Name = initialRoom.Name.Replace(" ", "");
                project.Scripts[initialRoom.Id] = script;

                int cols = Convert.ToInt32(txtCols.Text);
                int rows = Convert.ToInt32(txtRows.Text);

                LayerDto initialLayer = new LayerDto(txtLayerName.Text, cols, rows);

                initialLayer.OwnerId = initialRoom.Id;

                List<LayerDto> lstLayers = new List<LayerDto>();
                lstLayers.Add(initialLayer);

                project.Rooms.Add(initialRoom);
                project.Layers.Add(initialRoom.Id, lstLayers);

                project.InitialRoomId = initialRoom.Id;

                ScriptDto uiManagerScript = new ScriptDto();
                uiManagerScript.OwnerId = Globals.UiManagerId;
                uiManagerScript.Name = project.ProjectName + "Ui";
                uiManagerScript.ScriptType = ScriptType.UiManager;
                project.Scripts[Globals.UiManagerId] = uiManagerScript;

                ScriptDto networkHandlerScript = new ScriptDto();
                networkHandlerScript.OwnerId = Globals.NetworkHandlerId;
                networkHandlerScript.Name = project.ProjectName + "NetworkHandler";
                networkHandlerScript.ScriptType = ScriptType.NetworkHandler;
                project.Scripts[Globals.NetworkHandlerId] = networkHandlerScript;

                ScriptDto cameraScript = new ScriptDto();
                cameraScript.OwnerId = Globals.CameraScriptId;
                cameraScript.Name = "Camera";
                cameraScript.ScriptType = ScriptType.Entity;
                project.Scripts[Globals.CameraScriptId] = cameraScript;

                ScriptDto engineScript = new ScriptDto();
                engineScript.OwnerId = Globals.EngineScriptId;
                engineScript.Name = "Engine";
                engineScript.ScriptType = ScriptType.Engine;
                project.Scripts[Globals.EngineScriptId] = engineScript;

                DataFileDto jsonPanels = new DataFileDto();
                jsonPanels.OwnerId = Globals.PanelsJsonFileId;
                jsonPanels.Name = "panels";
                jsonPanels.Extension = "json";
                jsonPanels.FilePath = project.ProjectFolderFullPath;
                project.DataFiles[Globals.PanelsJsonFileId] = jsonPanels;
                
                if (string.IsNullOrEmpty(txtImportAssets.Text) == false)
                {
                    // Set the current working directory to the project folder, so relative paths 
                    // can be converted to absolute during the read operation.
                    Directory.SetCurrentDirectory(txtProjectFolder.Text);

                    FileStream fileStream = null;
                    MemoryStream stream = new MemoryStream();

                    try
                    {
                        fileStream = new FileStream(txtImportAssets.Text, FileMode.Open);

                        fileStream.CopyTo(stream);

                        Version projectVersionNumber = projectController_.ReadProjectVersionNumberFromStream(stream);

                        ProjectDto assets = projectController_.ReadProjectDtoFromStream(stream);
                        
                        projectController_.CreateNewProject(project, assets);

                        projectController_.FinalizeProject();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Failed to load assets from file " + txtImportAssets.Text);
                    }
                    finally
                    {
                        stream.Close();
                        fileStream.Close();
                    }                    
                }
                else
                {
                    projectController_.CreateNewProject(project, null);

                    projectController_.FinalizeProject();
                }

                projectController_.ChangesMade = true;

                this.Hide();
            }
        
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;

            if (txt.BackColor == Color.Salmon)
            {
                txt.BackColor = Color.LightSalmon;
            }
            else
            {
                txt.BackColor = Color.Beige;
            }
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;

            if (txt.Text == string.Empty)
            {
                txt.BackColor = Color.Salmon;
            }
            else
            {
                txt.BackColor = Color.White;
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
            string tileSize;
            tileSize = cboTileSize.Text.Trim();
            tileSize = tileSize.Substring(0, 2);

            if (txtRows.Text.Length == 0 || mapHeight_ == 0 || Convert.ToInt32(txtRows.Text) < Convert.ToInt32(Math.Ceiling(Convert.ToDouble(mapHeight_) / Convert.ToDouble(tileSize))))
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
            string tileSize;
            tileSize = cboTileSize.Text.Trim();
            tileSize = tileSize.Substring(0, 2);

            if (txtCols.Text.Length == 0 || mapWidth_ == 0 || Convert.ToInt32(txtCols.Text) < Convert.ToInt32(Math.Ceiling(Convert.ToDouble(mapWidth_) / Convert.ToDouble(tileSize))))
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

        private void txtRows_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtCols_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openTileset = new OpenFileDialog();

            openTileset.CheckFileExists = true;
            openTileset.CheckPathExists = true;
            openTileset.DefaultExt = "tset";
            openTileset.Filter = "Tilesets (*.tset)|*.tset";
            openTileset.Multiselect = false;
            openTileset.RestoreDirectory = true;

            if (openTileset.ShowDialog() == DialogResult.OK)
            {
            }
        }

        private void NewProjectDialog_Shown(object sender, EventArgs e)
        {
            // Restore the original state of the form.
            cboTileSize.SelectedIndex = 0;
            cboIngameSize.SelectedIndex = 0;

            // Get map size from combo box.
            if (cboIngameSize.Text.Trim() != "CUSTOM")
            {
                txtHeight.Enabled = false;
                txtWidth.Enabled = false;

                string[] values = cboIngameSize.Text.Trim().Split('x');

                string mapWidth = values[0];
                string mapHeight = values[1];
                
                mapHeight_ = Convert.ToInt32(mapHeight);
                mapWidth_ = Convert.ToInt32(mapWidth);

                txtHeight.Text = mapHeight;
                txtWidth.Text = mapWidth;
            }
            else
            {
                txtHeight.Enabled = true;
                txtWidth.Enabled = true;

                mapHeight_ = Convert.ToInt32(txtHeight.Text);
                mapWidth_ = Convert.ToInt32(txtWidth.Text);
            }

            // Get tilesize from combo box.
            string tileSize;
            tileSize = cboTileSize.Text.Trim();
            tileSize = tileSize.Substring(0, 2);

            tileSize_ = Convert.ToInt32(tileSize);

            // Default rows and cols to minimum
            txtCols.Text = (Math.Ceiling(Convert.ToDouble(mapWidth_) / Convert.ToDouble(tileSize))).ToString();
            txtRows.Text = (Math.Ceiling(Convert.ToDouble(mapHeight_) / Convert.ToDouble(tileSize))).ToString();

            txtCols.BackColor = Color.White;
            txtCols.ForeColor = Color.Black;

            txtRows.BackColor = Color.White;
            txtRows.ForeColor = Color.Black;

            txtLayerName.Text = "Layer 1";
            txtLayerName.BackColor = Color.White;

            txtRoomName.Text = "Room 1";
            txtRoomName.BackColor = Color.White;

            txtProjectFolder.Text = string.Empty;
            txtProjectFolder.BackColor = Color.White;

            txtProjectName.Text = string.Empty;
            txtProjectName.BackColor = Color.White;

            cboTileSize.Enabled = true;

            validateData(true);

            this.ActiveControl = txtProjectName;
        }

        private void cboTileSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Changing tilesize value. Revalidate the data.
                string tileSize;
                tileSize = cboTileSize.Text.Trim();
                tileSize = tileSize.Substring(0, 2);

                tileSize_ = Convert.ToInt32(tileSize);

                validateData(true);
            }
            catch (Exception ex)
            {
            }
        }

        private void cboTileSize_Enter(object sender, EventArgs e)
        {
            cboTileSize.BackColor = Color.Beige;
        }

        private void cboTileSize_Leave(object sender, EventArgs e)
        {
            cboTileSize.BackColor = Color.White;
        }

        private void cboIngameSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Changing map resolution value. Revalidate the data.
                // Get map size from combo box.
                if (cboIngameSize.Text.Trim() != "CUSTOM")
                {
                    txtHeight.Enabled = false;
                    txtWidth.Enabled = false;

                    string[] values = cboIngameSize.Text.Trim().Split('x');

                    string mapWidth = values[0];
                    string mapHeight = values[1];

                    mapHeight_ = Convert.ToInt32(mapHeight);
                    mapWidth_ = Convert.ToInt32(mapWidth);

                    txtHeight.Text = mapHeight;
                    txtWidth.Text = mapWidth;
                }
                else
                {
                    txtHeight.Enabled = true;
                    txtWidth.Enabled = true;

                    mapHeight_ = Convert.ToInt32(txtHeight.Text);
                    mapWidth_ = Convert.ToInt32(txtWidth.Text);
                }

                validateData(true);
            }
            catch (Exception ex)
            {
            }
        }

        private bool validateData(bool ignoreFolder = false)
        {
            bool isValid = true;
            string columns = txtCols.Text;
            string rows = txtRows.Text;

            if (columns == string.Empty)
            {
                columns = "0";
            }

            if (rows == string.Empty)
            {
                rows = "0";
            }

            if (mapWidth_ == 0 || Convert.ToInt32(columns) < Convert.ToInt32(Math.Ceiling(Convert.ToDouble(mapWidth_) / Convert.ToDouble(tileSize_))))
            {
                txtCols.BackColor = Color.Salmon;
                txtCols.ForeColor = Color.DarkRed;
                isValid = false;
            }
            else
            {
                txtCols.BackColor = Color.White;
                txtCols.ForeColor = Color.Black;
            }

            if (mapHeight_ == 0 || Convert.ToInt32(rows) < Convert.ToInt32(Math.Ceiling(Convert.ToDouble(mapHeight_) / Convert.ToDouble(tileSize_))))
            {
                txtRows.BackColor = Color.Salmon;
                txtRows.ForeColor = Color.DarkRed;
                isValid = false;
            }
            else
            {
                txtRows.BackColor = Color.White;
                txtRows.ForeColor = Color.Black;
            }

            if (txtLayerName.Text == string.Empty)
            {
                txtLayerName.BackColor = Color.Salmon;
                isValid = false;
            }
            else
            {
                txtLayerName.BackColor = Color.White;
            }

            if (txtRoomName.Text == string.Empty)
            {
                txtRoomName.BackColor = Color.Salmon;
                isValid = false;
            }
            else
            {
                txtRoomName.BackColor = Color.White;
            }

            if (ignoreFolder == false)
            {
                if (txtProjectFolder.Text == string.Empty)
                {
                    txtProjectFolder.BackColor = Color.Salmon;
                    isValid = false;
                }
                else
                {
                    txtProjectFolder.BackColor = Color.White;
                }
            }

            if (ignoreFolder == false)
            {
                if (txtProjectName.Text == string.Empty)
                {
                    txtProjectName.BackColor = Color.Salmon;
                    isValid = false;
                }
                else
                {
                    txtProjectName.BackColor = Color.White;
                }
            }

            return isValid;
        }

        private void txtHeight_Enter(object sender, EventArgs e)
        {
            if (txtHeight.Text == string.Empty || Convert.ToInt32(txtHeight.Text) == 0)
            {
                txtHeight.BackColor = Color.LightSalmon;
            }
            else
            {
                txtHeight.BackColor = Color.Beige;
            }
        }

        private void txtHeight_Leave(object sender, EventArgs e)
        {
            txtHeight.BackColor = Color.White;

            if (txtHeight.Text == string.Empty || Convert.ToInt32(txtHeight.Text) == 0)
            {
                txtHeight.BackColor = Color.Salmon;
                mapHeight_ = 0;
            }
            else
            {
                mapHeight_ = Convert.ToInt32(txtHeight.Text);
            }

            validateData();
        }

        private void txtHeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtWidth_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtWidth_Enter(object sender, EventArgs e)
        {
            if (txtWidth.Text == string.Empty || Convert.ToInt32(txtWidth.Text) == 0)
            {
                txtWidth.BackColor = Color.LightSalmon;
            }
            else
            {
                txtWidth.BackColor = Color.Beige;
            }
        }

        private void txtWidth_Leave(object sender, EventArgs e)
        {
            txtWidth.BackColor = Color.White;

            if (txtWidth.Text == string.Empty || Convert.ToInt32(txtWidth.Text) == 0)
            {
                txtWidth.BackColor = Color.Salmon;
                mapWidth_ = 0;
            }
            else
            {
                mapWidth_ = Convert.ToInt32(txtWidth.Text);
            }

            validateData();
        }

        private void txtLayerName_Enter(object sender, EventArgs e)
        {
            if (txtLayerName.Text == string.Empty)
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

        private void textboxBrowseFolders_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                txtProjectFolder.Text = folderBrowserDialog1.SelectedPath + "\\";

                txtProjectFolder.BackColor = Color.White;
                txtProjectFolder.ForeColor = Color.Black;
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

        private void btnBrowseAssets_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                txtImportAssets.Text = openFileDialog1.FileName;

                txtImportAssets.BackColor = Color.White;
                txtImportAssets.ForeColor = Color.Black;
            }
        }
    }
}