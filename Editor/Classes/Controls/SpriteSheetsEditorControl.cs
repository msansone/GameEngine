using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public partial class SpriteSheetsEditorControl : UserControl, ISpriteSheetsEditorControl 
    {
        #region Constructors

        public SpriteSheetsEditorControl(IProjectController projectController)
        {
            InitializeComponent();

            projectController_ = projectController;

            firemelonEditorFactory_ = new FiremelonEditorFactory();

            ProjectDto project = projectController.GetProjectDto();

            // Initialize sprite sheets.
            foreach (SpriteSheetDto spriteSheet in project.SpriteSheets)
            {
                lbxSpriteSheets.Items.Add(spriteSheet.Name);
            }

            spriteSheetViewerControl_ = firemelonEditorFactory_.NewSpriteSheetViewerControl(projectController_);

            Control spriteSheetViewerControl = (Control)spriteSheetViewerControl_;

            spriteSheetViewerControl.Dock = DockStyle.Fill;

            scSpriteSheets.Panel2.Controls.Add(spriteSheetViewerControl);
        }

        #endregion

        #region Private Variables

        IFiremelonEditorFactory firemelonEditorFactory_;

        IProjectController projectController_;

        ISheetViewerControl spriteSheetViewerControl_;

        #endregion

        #region Public Functions

        public void AddNew()
        {
            addNewSpriteSheet();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Functions

        private void addNewSpriteSheet()
        {
            ProjectDto project = projectController_.GetProjectDto();

            int tileSize = project.TileSize;

            OpenFileDialog openSpriteSheet = new OpenFileDialog();

            openSpriteSheet.CheckFileExists = true;
            openSpriteSheet.CheckPathExists = true;
            openSpriteSheet.DefaultExt = "png";
            openSpriteSheet.Filter = "PNG Files|*.png";
            openSpriteSheet.FileName = string.Empty;
            openSpriteSheet.Multiselect = false;
            openSpriteSheet.RestoreDirectory = true;

            if (openSpriteSheet.ShowDialog() == DialogResult.OK)
            {
                int tilesheetCount = project.SpriteSheets.Count + 1;

                bool isNameValid = false;
                int counter = 0;
                string currentName = "New Sprite Sheet";

                // Find the first sequentially available name.
                while (isNameValid == false)
                {
                    isNameValid = true;

                    // The current name that is being checked for collision.
                    if (counter > 0)
                    {
                        currentName = "New Sprite Sheet " + counter.ToString();
                    }

                    // Tile sheets and sprite sheets cannot have name collisions with each other.
                    for (int j = 0; j < project.SpriteSheets.Count; j++)
                    {
                        if (currentName.ToUpper() == project.SpriteSheets[j].Name.ToUpper())
                        {
                            isNameValid = false;
                            break;
                        }
                    }

                    for (int j = 0; j < project.TileSheets.Count; j++)
                    {
                        if (currentName.ToUpper() == project.TileSheets[j].Name.ToUpper())
                        {
                            isNameValid = false;
                            break;
                        }
                    }

                    counter++;
                }

                SpriteSheetDto newSpriteSheet = projectController_.AddSpriteSheet(openSpriteSheet.FileName, currentName);

                // Add sprite sheet to list box.
                lbxSpriteSheets.Items.Add(currentName);
            }
        }

        #endregion

        #region Event Handlers

        private void lbxSpriteSheets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxSpriteSheets.SelectedIndex >= 0 && lbxSpriteSheets.SelectedIndex < lbxSpriteSheets.Items.Count)
            {
                SpriteSheetDto spriteSheet = projectController_.GetSpriteSheetByName(lbxSpriteSheets.Items[lbxSpriteSheets.SelectedIndex].ToString());

                ISpriteSheetDtoProxy spriteSheetProxy = firemelonEditorFactory_.NewSpriteSheetProxy(projectController_, spriteSheet.Id);

                if (spriteSheetViewerControl_.Sheet != null)
                {
                    SpriteSheetDto spriteSheetToUnload = projectController_.GetSpriteSheet(spriteSheetViewerControl_.Sheet.Id);
                    
                    // Unload the existing tile sheet, if it is changing.
                    Guid resourceToUnloadId = spriteSheetToUnload.BitmapResourceId;

                    projectController_.UnloadBitmapResource(resourceToUnloadId, EditorModule.SpriteSheetViewer);
                }

                pgSpriteSheet.SelectedObject = spriteSheetProxy;

                spriteSheetViewerControl_.Sheet = spriteSheetProxy;
            }
        }

        private void pgSpriteSheet_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            switch (e.ChangedItem.Label.ToUpper())
            {
                case "IMAGEPATH":
                case "SCALEFACTOR":
                case "ROWS":
                case "COLUMNS":
                case "CELLHEIGHT":
                case "CELLWIDTH":
                case "PADDING":

                    // Scroll bars likely need update and the image needs refreshed.
                    spriteSheetViewerControl_.RefreshImage();

                    break;

                case "(NAME)":

                    try
                    {
                        int oldIndex = lbxSpriteSheets.Items.IndexOf(e.OldValue.ToString());

                        lbxSpriteSheets.Items[oldIndex] = e.ChangedItem.Value.ToString();

                        lbxSpriteSheets.Sorted = false;
                        lbxSpriteSheets.Sorted = true;
                    }
                    catch (Exception ex)
                    {
                    }

                    break;
            }
        }

        #endregion
    }
}
