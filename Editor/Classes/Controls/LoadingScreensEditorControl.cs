using System;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public partial class LoadingScreensEditorControl : UserControl, ILoadingScreensEditorControl
    {
        private IFiremelonEditorFactory firemelonEditorFactory_;
        
        private IProjectController projectController_;

        private INameGenerator nameGenerator_;
        
        public LoadingScreensEditorControl(IProjectController projectController, INameGenerator nameGenerator)
        {
            InitializeComponent();

            projectController_ = projectController;

            firemelonEditorFactory_ = new FiremelonEditorFactory();
            
            nameGenerator_ = nameGenerator;
        }

        public void AddNew()
        {
            addLoadingScreen();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        #region Private Functions

        private void addLoadingScreen()
        {
            ProjectDto project = projectController_.GetProjectDto();

            int loadingScreenCount = project.LoadingScreens.Count + 1;

            string name = nameGenerator_.GetNextAvailableAssetName("New Loading Screen", project);

            LoadingScreenDto newLoadingScreen = projectController_.AddLoadingScreen(name);

            // Add sprite sheet to list box.
            lbxLoadingScreens.Items.Add(newLoadingScreen.Name);
        }

        #endregion

        private void lbxLoadingScreens_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (lbxLoadingScreens.SelectedIndex >= 0 && lbxLoadingScreens.SelectedIndex < lbxLoadingScreens.Items.Count)
            {
                LoadingScreenDto loadingScreen = projectController_.GetLoadingScreenByName(lbxLoadingScreens.Items[lbxLoadingScreens.SelectedIndex].ToString());

                ILoadingScreenDtoProxy loadingScreenProxy = firemelonEditorFactory_.NewLoadingScreenProxy(projectController_, loadingScreen.Id);

                pgLoadingScreens.SelectedObject = loadingScreenProxy;
            }
        }
    }
}
