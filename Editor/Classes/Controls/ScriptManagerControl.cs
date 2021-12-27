using System;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public partial class ScriptManagerControl : UserControl, IScriptManagerControl
    {
        #region Constructors

        public ScriptManagerControl(IProjectController projectController)
        {
            InitializeComponent();

            projectController_ = projectController;

            projectController_.ProjectCreated += new ProjectCreateHandler(this.ScriptManagerControl_ProjectCreated);

            IFiremelonEditorFactory firemelonEditorFactory = new FiremelonEditorFactory();

            stringReplacementDialog_ = firemelonEditorFactory.NewStringReplacementDialog();

            stringReplacementDialog_.StringReplaced += new StringReplacedHandler(this.ScriptManagerControl_StringReplaced);
        }

        #endregion

        #region Private Variables

        private IProjectController projectController_;

        private IStringReplacementDialog stringReplacementDialog_;

        #endregion

        #region Private Functions
        
        private void populateListView()
        {
            lvScripts.Items.Clear();

            ProjectDto project = projectController_.GetProjectDto();

            foreach (ScriptDto script in project.Scripts.Values)
            {
                ListViewItem lviScriptPath = new ListViewItem();
                lviScriptPath.Text = script.Name;
                lvScripts.Items.Add(lviScriptPath);

                ListViewItem.ListViewSubItem lvsiScriptType = new ListViewItem.ListViewSubItem();
                lvsiScriptType.Text = script.ScriptType.ToString();
                lviScriptPath.SubItems.Add(lvsiScriptType);

                ListViewItem.ListViewSubItem lvsiScriptPathRelative = new ListViewItem.ListViewSubItem();
                lvsiScriptPathRelative.Text = script.ScriptRelativePath;
                lviScriptPath.SubItems.Add(lvsiScriptPathRelative);

                ListViewItem.ListViewSubItem lvsiScriptPathFull = new ListViewItem.ListViewSubItem();
                lvsiScriptPathFull.Text = script.ScriptPath;
                lviScriptPath.SubItems.Add(lvsiScriptPathFull);
            }
        }

        #endregion

        #region Event Handlers

        public void ScriptManagerControl_ProjectCreated(object sender, ProjectCreatedEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;

            if (e.NewProject == true)
            {
                populateListView();
            }
        }

        private void ScriptManagerControl_StringReplaced(object sender, StringReplacedEventArgs e)
        {
            int replacementCount = projectController_.ReplaceScriptFolder(e.OldValue, e.NewValue);

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
}
