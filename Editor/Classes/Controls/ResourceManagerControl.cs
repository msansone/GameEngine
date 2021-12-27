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
    using System.IO;
    using ProjectDto = ProjectDto_2_2;

    public partial class ResourceManagerControl : UserControl, IResourceManagerControl
    {
        #region Constructors

        public ResourceManagerControl(IProjectController projectController)
        {
            InitializeComponent();

            projectController_ = projectController;

            projectController_.ProjectCreated += new ProjectCreateHandler(this.ResourceManagerControl_ProjectCreated);

            IFiremelonEditorFactory firemelonEditorFactory = new FiremelonEditorFactory();

            stringReplacementDialog_ = firemelonEditorFactory.NewStringReplacementDialog();

            stringReplacementDialog_.StringReplaced += new StringReplacedHandler(this.ResourceManagerControl_StringReplaced);
        }

        #endregion

        #region Private Variables

        private IProjectController projectController_;

        private IStringReplacementDialog stringReplacementDialog_;

        #endregion

        #region Private Functions

        private void populateListView()
        {
            lvResources.Items.Clear();

            ProjectDto project = projectController_.GetProjectDto();
            
            foreach (BitmapResourceDto bitmapResource in project.Bitmaps.Values)
            {
                ListViewItem lviResourceType = new ListViewItem();
                lviResourceType.Text = "Bitmap";
                lvResources.Items.Add(lviResourceType);
               
                ListViewItem.ListViewSubItem lvsiResourcePathRelative = new ListViewItem.ListViewSubItem();
                lvsiResourcePathRelative.Text = bitmapResource.RelativePath;
                lviResourceType.SubItems.Add(lvsiResourcePathRelative);

                ListViewItem.ListViewSubItem lvsiResourcePathFull = new ListViewItem.ListViewSubItem();
                lvsiResourcePathFull.Text = bitmapResource.Path;
                lviResourceType.SubItems.Add(lvsiResourcePathFull);

                ListViewItem.ListViewSubItem lvsiResourceExists = new ListViewItem.ListViewSubItem();

                if (File.Exists(bitmapResource.Path) == true)
                {
                    lvsiResourceExists.Text = "Yes";
                }
                else
                {
                    lvsiResourceExists.Text = "No";
                }

                lviResourceType.SubItems.Add(lvsiResourceExists);

                ListViewItem.ListViewSubItem lvsiResourceId = new ListViewItem.ListViewSubItem();
                lvsiResourceId.Tag = bitmapResource.Id;
                lviResourceType.SubItems.Add(lvsiResourceId);
            }

            foreach (AudioResourceDto audioResource in project.AudioData.Values)
            {
                ListViewItem lviResourceType = new ListViewItem();
                lviResourceType.Text = "Audio";
                lvResources.Items.Add(lviResourceType);
                
                ListViewItem.ListViewSubItem lvsiResourcePathRelative = new ListViewItem.ListViewSubItem();
                lvsiResourcePathRelative.Text = audioResource.RelativePath;
                lviResourceType.SubItems.Add(lvsiResourcePathRelative);

                ListViewItem.ListViewSubItem lvsiResourcePathFull = new ListViewItem.ListViewSubItem();
                lvsiResourcePathFull.Text = audioResource.Path;
                lviResourceType.SubItems.Add(lvsiResourcePathFull);

                ListViewItem.ListViewSubItem lvsiResourceExists = new ListViewItem.ListViewSubItem();

                if (File.Exists(audioResource.Path) == true)
                {
                    lvsiResourceExists.Text = "Yes";
                }
                else
                {
                    lvsiResourceExists.Text = "No";
                }

                lviResourceType.SubItems.Add(lvsiResourceExists);

                ListViewItem.ListViewSubItem lvsiResourceId = new ListViewItem.ListViewSubItem();
                lvsiResourceId.Tag = audioResource.Id;
                lviResourceType.SubItems.Add(lvsiResourceId);
            }
        }

        #endregion

        #region Event Handlers

        private void lvResources_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem selectedItem = lvResources.SelectedItems[0];

            string resourceType = selectedItem.Text;

            Guid resourceId = (Guid)(selectedItem.SubItems[4].Tag);

            string resourceFullPath = selectedItem.SubItems[2].Text;

            string input = Microsoft.VisualBasic.Interaction.InputBox("Prompt", "Title", resourceFullPath, Screen.PrimaryScreen.WorkingArea.Width / 2, Screen.PrimaryScreen.WorkingArea.Height / 2);
            
            if (File.Exists(input))
            {
                ExternalResourceDto externalResource = null;

                if (resourceType == "Bitmap")
                {
                    BitmapResourceDto bitmapResource = projectController_.GetBitmapResource(resourceId, false);

                    externalResource = bitmapResource;
                    
                }
                else if (resourceType == "Audio")
                {
                    ProjectDto project = projectController_.GetProjectDto();

                    AudioResourceDto audioResource = project.AudioData[resourceId];

                    externalResource = audioResource;
                    
                }

                externalResource.Path = input;


                UtilityFactory utilityFactory = new UtilityFactory();
                
                IUriUtility uriUtility = utilityFactory.NewUriUtility();


                externalResource.RelativePath = uriUtility.GetRelativePath(input);

                // Update the UI.
                ListViewItem.ListViewSubItem lvsiResourcePathRelative = selectedItem.SubItems[1];

                lvsiResourcePathRelative.Text = externalResource.RelativePath;

                ListViewItem.ListViewSubItem lvsiResourcePathFull = selectedItem.SubItems[2];

                lvsiResourcePathFull.Text = externalResource.Path;
            }    
        }

        public void ResourceManagerControl_ProjectCreated(object sender, ProjectCreatedEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;

            if (e.NewProject == true)
            {
                populateListView();
                
                tslbProjectFolder.Text = project.ProjectFolderFullPath;
            }
        }

        private void ResourceManagerControl_StringReplaced(object sender, StringReplacedEventArgs e)
        {
            int replacementCount = projectController_.ReplaceResourceFolder(e.OldValue, e.NewValue);

            populateListView();

            string message = replacementCount + " replacements occurred.";

            string caption = "Replacement Operation Complete";

            System.Windows.Forms.MessageBox.Show(message, caption);
        }

        private void tsbFolderReplacement_Click(object sender, EventArgs e)
        {
            stringReplacementDialog_.ShowDialog(this);
        }

        #endregion
    }

    public interface IResourceManagerControl
    {
    }
}
