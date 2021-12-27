using System;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    using System.ComponentModel;
    using ProjectDto = ProjectDto_2_2;

    class ProjectLauncher : IProjectLauncher
    {
        #region Constructors

        public ProjectLauncher(IProjectController projectController, IWin32Window owner)
        {
            executableName_ = Globals.executableName;

            firemelonEditorFactory_ = new FiremelonEditorFactory();

            owner_ = owner;

            projectController_ = projectController;
        }

        #endregion

        #region Private Variables

        private string executableName_;

        IFiremelonEditorFactory firemelonEditorFactory_;

        IWin32Window owner_;

        IProjectController projectController_;

        #endregion

        #region Properties

        public bool ExportScriptsOnly
        {
            get { return exportScriptsOnly_; }
            set { exportScriptsOnly_ = value; }
        }
        private bool exportScriptsOnly_ = false;
        

        public bool ShowWarnings
        {
            get { return showWarnings_; }
            set { showWarnings_ = value; }
        }
        private bool showWarnings_;
        
        #endregion

        #region Public Functions

        public void Launch()
        {
            executableName_ = Globals.executableName;

            exportProject();

            runEngine();
        }

        public void LaunchWithConsole()
        {
            executableName_ = Globals.executableWithConsoleName;

            try
            {
                exportProject();

                runEngine();
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("Failed to launch. An instance may currently be running.", "Project Launch Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #region Protected Functions
        #endregion

        #region Private Functions
        
        private void exportProject()
        {
            IProjectExporter projectExporter = firemelonEditorFactory_.NewProjectExporter(projectController_, owner_, exportScriptsOnly_);

            ProjectDto project = projectController_.GetProjectDto();

            string projectFolder = project.ProjectFolderFullPath;
            string projectName = project.ProjectName + "_Export";
            string exportPath = projectFolder + projectName;

            projectExporter.ExportProject(exportPath, showWarnings_);
        }

        public void runEngine()
        {
            ProjectDto project = projectController_.GetProjectDto();

            string folder = project.ProjectFolderFullPath;

            string exportFolder = project.ProjectName + "_Export";

            string executablePath = folder + exportFolder + "\\" + executableName_;

            try
            {

                System.Diagnostics.Process engineProcess = new System.Diagnostics.Process();


                engineProcess.StartInfo.FileName = executablePath;

                engineProcess.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(folder + exportFolder + "\\");

                engineProcess.Start();
            }
            catch (Win32Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + Environment.NewLine + "Executable Path: " + executablePath, "Error Running Project", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error Running Project", MessageBoxButtons.OK);
            }
        }


        #endregion

        #region Event Handlers
        #endregion
    }
}
